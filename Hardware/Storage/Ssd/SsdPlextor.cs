using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Ssd;

/// <summary>
/// SSD: Plextor
/// </summary>
/// <seealso cref="AtaStorage" />
[NamePrefix("PLEXTOR")]
internal class SsdPlextor : AtaStorage
{
    private static readonly IReadOnlyList<SmartAttribute> _smartAttributes = new List<SmartAttribute>
    {
        new(0x09, SmartNames.PowerOnHours, RawToInt),
        new(0x0C, SmartNames.PowerCycleCount, RawToInt),
        new(0xF1, SmartNames.HostWrites, RawToGb, SensorType.Data, 0, SmartNames.HostWrites),
        new(0xF2, SmartNames.HostReads, RawToGb, SensorType.Data, 1, SmartNames.HostReads)
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="SsdPlextor"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="smart">The smart.</param>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <param name="index">The index.</param>
    /// <param name="settings">The settings.</param>
    public SsdPlextor(StorageInfo storageInfo, ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
        : base(storageInfo, smart, name, firmwareRevision, "ssd", index, _smartAttributes, settings) { }

    /// <summary>
    /// Raws to gb.
    /// </summary>
    /// <param name="rawValue">The raw value.</param>
    /// <param name="value">The value.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    private static float RawToGb(byte[] rawValue, byte value, IReadOnlyList<IParameter> parameters) => 
        RawToInt(rawValue, value, parameters) / 32;
}
