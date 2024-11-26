using System.Collections.Generic;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using GetSensorValue = Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme.NvmeHelper.GetSensorValue;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// NVME: Generic
/// </summary>
/// <seealso cref="AbstractStorage" />
public sealed class NvmeGeneric : AbstractStorage
{
    #region Fields

    private const ulong Scale = 1000000;
    private const ulong Units = 512;
    private readonly NvmeInfo _info;
    private readonly List<NvmeSensor> _sensors = [];

    #endregion

    #region Properties

    /// <summary>
    /// Gets the SMART data.
    /// </summary>
    public NvmeSmart Smart { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NvmeGeneric"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="info">The information.</param>
    /// <param name="index">The index.</param>
    /// <param name="settings">The settings.</param>
    private NvmeGeneric(StorageInfo storageInfo, NvmeInfo info, int index, ISettings settings)
        : base(storageInfo, info.Model, info.Revision, "nvme", index, settings)
    {
        Smart = new NvmeSmart(storageInfo);
        _info = info;
        CreateSensors();
    }

    #endregion

    #region Methods

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override void Close()
    {
        Smart?.Close();
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
        NvmeInfo nvmeInfo = GetDeviceInfo(storageInfo);
        return nvmeInfo == null ? null : new NvmeGeneric(storageInfo, nvmeInfo, storageInfo.Index, settings);
    }

    /// <summary>
    /// Creates the sensors.
    /// </summary>
    protected override void CreateSensors()
    {
        NvmeHealthInfo log = Smart.GetHealthInfo();
        if (log != null)
        {
            AddSensor("Temperature", 0, false, SensorType.Temperature, health => health.Temperature);
            AddSensor("Available Spare", 1, false, SensorType.Level, health => health.AvailableSpare);
            AddSensor("Available Spare Threshold", 2, false, SensorType.Level, health => health.AvailableSpareThreshold);
            AddSensor("Percentage Used", 3, false, SensorType.Level, health => health.PercentageUsed);
            AddSensor("Data Read", 4, false, SensorType.Data, health => UnitsToData(health.DataUnitRead));
            AddSensor("Data Written", 5, false, SensorType.Data, health => UnitsToData(health.DataUnitWritten));

            int sensorIdx = 6;
            for (int i = 0; i < log.TemperatureSensors.Length; i++)
            {
                int idx = i;
                if (log.TemperatureSensors[idx] <= short.MinValue) continue;
                AddSensor($"Temperature {idx + 1}", sensorIdx, false,
                    SensorType.Temperature, health => health.TemperatureSensors[idx]);
                sensorIdx++;
            }
        }

        base.CreateSensors();
    }

    /// <summary>
    /// Updates the sensors.
    /// </summary>
    protected override void UpdateSensors()
    {
        NvmeHealthInfo health = Smart.GetHealthInfo();
        if (health == null) return;
        _sensors.Apply(x => x.Update(health));
    }

    /// <summary>
    /// Adds the sensor.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="index">The index.</param>
    /// <param name="defaultHidden">if set to <c>true</c> [default hidden].</param>
    /// <param name="sensorType">Type of the sensor.</param>
    /// <param name="getValue">The get value.</param>
    private void AddSensor(string name, int index, bool defaultHidden, SensorType sensorType, GetSensorValue getValue)
    {
        var sensor = new NvmeSensor(name, index, defaultHidden, sensorType, this, Settings, getValue)
        {
            Value = 0
        };
        ActivateSensor(sensor);
        _sensors.Add(sensor);
    }

    /// <summary>
    /// Gets the device information.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <returns></returns>
    private static NvmeInfo GetDeviceInfo(StorageInfo storageInfo)
    {
        var smart = new NvmeSmart(storageInfo);
        return smart.GetInfo();
    }

    /// <summary>
    /// Converts a unit to data.
    /// Notes: one unit is 512 * 1000 bytes, return in GB (not GiB)
    /// </summary>
    /// <param name="u">The u.</param>
    /// <returns></returns>
    private static float UnitsToData(ulong u) => (float)(Units * u) / Scale;

    #endregion
}
