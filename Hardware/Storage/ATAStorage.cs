﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;
using Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Ssd;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// ATA storage
/// </summary>
/// <seealso cref="AbstractStorage" />
public abstract class AtaStorage : AbstractStorage
{
    #region Fields

    // array of all hard drive types, matching type is searched in this order
    private static readonly Type[] _hddTypes = 
        [
            typeof(SsdPlextor), 
            typeof(SsdIntel), 
            typeof(SsdSandForce), 
            typeof(SsdIndilinx), 
            typeof(SsdSamsung), 
            typeof(SsdMicron), 
            typeof(GenericHardDisk)
    ];

    private IDictionary<SmartAttribute, Sensor> _sensors;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the SMART data.
    /// </summary>
    public ISmart Smart { get; }

    /// <summary>
    /// Gets the SMART attributes.
    /// </summary>
    public IReadOnlyList<SmartAttribute> SmartAttributes { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AtaStorage"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="smart">The smart.</param>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="index">The index.</param>
    /// <param name="smartAttributes">The smart attributes.</param>
    /// <param name="settings">The settings.</param>
    internal AtaStorage(StorageInfo storageInfo, ISmart smart, string name, string firmwareRevision, string id, int index, IReadOnlyList<SmartAttribute> smartAttributes, ISettings settings)
        : base(storageInfo, name, firmwareRevision, id, index, settings)
    {
        Smart = smart;
        if (smart.IsValid)
        {
            smart.EnableSmart();
        }

        SmartAttributes = smartAttributes;
        CreateSensors();
    }

    #endregion

    #region Methods
    
    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override void Close()
    {
        Smart.Close();
        base.Close();
    }

    /// <summary>
    /// Creates the instance.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="settings">The settings.</param>
    /// <returns></returns>
    internal static AbstractStorage CreateInstance(StorageInfo storageInfo, ISettings settings)
    {
        using ISmart smart = new WindowsSmart(storageInfo.Index);
        string name = null;
        string firmwareRevision = null;
        Interop.Models.Kernel32.SmartAttribute[] smartAttributes = [];

        if (smart.IsValid)
        {
            var nameValid = smart.ReadNameAndFirmwareRevision(out name, out firmwareRevision);
            var smartEnabled = smart.EnableSmart();

            if (smartEnabled)
            {
                smartAttributes = smart.ReadSmartData();
            }

            if (!nameValid)
            {
                name = null;
                firmwareRevision = null;
            }
        }
        else
        {
            string[] logicalDrives = WindowsStorage.GetLogicalDrives(storageInfo.Index);
            if (logicalDrives == null || logicalDrives.Length == 0)
            {
                smart.Close();
                return null;
            }

            bool hasNonZeroSizeDrive = false;
            foreach (var logicalDrive in logicalDrives)
            {
                try
                {
                    var driveInfo = new DriveInfo(logicalDrive);
                    if (driveInfo.TotalSize <= 0) continue;
                    hasNonZeroSizeDrive = true;
                    break;
                }
                catch (ArgumentException)
                {
                    // Do nothing
                }
                catch (IOException)
                {
                    // Do nothing
                }
                catch (UnauthorizedAccessException)
                {
                    // Do nothing
                }
            }

            if (!hasNonZeroSizeDrive)
            {
                smart.Close();
                return null;
            }
        }

        if (string.IsNullOrEmpty(name))
        {
            name = string.IsNullOrEmpty(storageInfo.Name) ? "Generic Hard Disk" : storageInfo.Name;
        }

        if (string.IsNullOrEmpty(firmwareRevision))
        {
            firmwareRevision = string.IsNullOrEmpty(storageInfo.Revision) ? "Unknown" : storageInfo.Revision;
        }

        foreach (var type in _hddTypes)
        {
            // get the array of the required SMART attributes for the current type

            // check if all required attributes are present
            bool allAttributesFound = true;

            if (type.GetCustomAttributes(typeof(RequireSmartAttribute), true) is RequireSmartAttribute[] requiredAttributes)
            {
                foreach (var requireAttribute in requiredAttributes)
                {
                    bool attributeFound = false;

                    foreach (var value in smartAttributes)
                    {
                        if (value.Id != requireAttribute.AttributeId) continue;
                        attributeFound = true;
                        break;
                    }

                    if (attributeFound) continue;
                    allAttributesFound = false;
                    break;
                }
            }

            // if an attribute is missing, then try the next type
            if (!allAttributesFound) continue;

            // check if there is a matching name prefix for this type
            if (type.GetCustomAttributes(typeof(NamePrefixAttribute), true) is not NamePrefixAttribute[] namePrefixes) continue;
            foreach (NamePrefixAttribute prefix in namePrefixes)
            {
                if (!name.StartsWith(prefix.Prefix, StringComparison.InvariantCulture)) continue;
                const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                return Activator.CreateInstance(type, flags, null, 
                    [storageInfo, smart, name, firmwareRevision, storageInfo.Index, settings], null) as AtaStorage;
            }
        }

        // no matching type has been found
        smart.Close();
        return null;
    }

    /// <summary>
    /// Raw value to int.
    /// </summary>
    /// <param name="raw">The raw.</param>
    /// <param name="value">The value.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    protected static float RawToInt(byte[] raw, byte value, IReadOnlyList<IParameter> parameters)
        => (raw[3] << 24) | (raw[2] << 16) | (raw[1] << 8) | raw[0];

    /// <summary>
    /// Creates the sensors.
    /// </summary>
    protected sealed override void CreateSensors()
    {
        _sensors = new Dictionary<SmartAttribute, Sensor>();

        if (Smart.IsValid)
        {
            byte[] smartIds = Smart.ReadSmartData().Select(x => x.Id).ToArray();

            // unique attributes by SensorType and SensorChannel.
            IEnumerable<SmartAttribute> smartAttributes = 
                SmartAttributes
                    .Where(x => x.SensorType.HasValue && smartIds.Contains(x.Id))
                    .GroupBy(x => new { x.SensorType.Value, x.SensorChannel })
                    .Select(x => x.First());

            _sensors = smartAttributes.ToDictionary(
                attribute => attribute,
                attribute => new Sensor(
                    attribute.SensorName,
                    attribute.SensorChannel,
                    attribute.DefaultHiddenSensor,
                    attribute.SensorType.GetValueOrDefault(),
                    this,
                    attribute.ParameterDescriptions,
                    Settings));

            // Activate sensors
            _sensors.Apply(x => ActivateSensor(x.Value));
        }

        base.CreateSensors();
    }

    /// <summary>
    /// Updates the additional sensors.
    /// </summary>
    /// <param name="values">The values.</param>
    protected virtual void UpdateAdditionalSensors(Interop.Models.Kernel32.SmartAttribute[] values) { }

    /// <summary>
    /// Updates the sensors.
    /// </summary>
    protected override void UpdateSensors()
    {
        if (!Smart.IsValid) return;
        var smartAttributes = Smart.ReadSmartData();

        foreach (var keyValuePair in _sensors)
        {
            SmartAttribute attribute = keyValuePair.Key;
            foreach (var value in smartAttributes)
            {
                if (value.Id != attribute.Id) continue;
                var sensor = keyValuePair.Value;
                sensor.Value = attribute.ConvertValue(value, sensor.Parameters);
            }
        }

        UpdateAdditionalSensors(smartAttributes);
    }

    #endregion
}