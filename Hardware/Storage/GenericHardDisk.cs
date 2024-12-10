using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// Generic Hard Disk
/// </summary>
/// <seealso cref="AtaStorage" />
[NamePrefix("")]
public class GenericHardDisk : AtaStorage
{
    private static readonly List<SmartAttribute> _smartAttributes =
    [
        new(0x01, SmartNames.ReadErrorRate),
        new(0x02, SmartNames.ThroughputPerformance),
        new(0x03, SmartNames.SpinUpTime),
        new(0x04, SmartNames.StartStopCount, RawToInt),
        new(0x05, SmartNames.ReallocatedSectorsCount),
        new(0x06, SmartNames.ReadChannelMargin),
        new(0x07, SmartNames.SeekErrorRate),
        new(0x08, SmartNames.SeekTimePerformance),
        new(0x09, SmartNames.PowerOnHours, RawToInt),
        new(0x0A, SmartNames.SpinRetryCount),
        new(0x0B, SmartNames.RecalibrationRetries),
        new(0x0C, SmartNames.PowerCycleCount, RawToInt),
        new(0x0D, SmartNames.SoftReadErrorRate),
        new(0xAA, SmartNames.Unknown),
        new(0xAB, SmartNames.Unknown),
        new(0xAC, SmartNames.Unknown),
        new(0xB7, SmartNames.SataDownshiftErrorCount, RawToInt),
        new(0xB8, SmartNames.EndToEndError),
        new(0xB9, SmartNames.HeadStability),
        new(0xBA, SmartNames.InducedOpVibrationDetection),
        new(0xBB, SmartNames.ReportedUncorrectableErrors, RawToInt),
        new(0xBC, SmartNames.CommandTimeout, RawToInt),
        new(0xBD, SmartNames.HighFlyWrites),
        new(0xBF, SmartNames.GSenseErrorRate),
        new(0xC0, SmartNames.EmergencyRetractCycleCount),
        new(0xC1, SmartNames.LoadCycleCount),
        new(0xC3, SmartNames.HardwareEccRecovered),
        new(0xC4, SmartNames.ReallocationEventCount),
        new(0xC5, SmartNames.CurrentPendingSectorCount),
        new(0xC6, SmartNames.UncorrectableSectorCount),
        new(0xC7, SmartNames.UltraDmaCrcErrorCount),
        new(0xC8, SmartNames.WriteErrorRate),
        new(0xCA, SmartNames.DataAddressMarkErrors),
        new(0xCB, SmartNames.RunOutCancel),
        new(0xCC, SmartNames.SoftEccCorrection),
        new(0xCD, SmartNames.ThermalAsperityRate),
        new(0xCE, SmartNames.FlyingHeight),
        new(0xCF, SmartNames.SpinHighCurrent),
        new(0xD0, SmartNames.SpinBuzz),
        new(0xD1, SmartNames.OfflineSeekPerformance),
        new(0xD3, SmartNames.VibrationDuringWrite),
        new(0xD4, SmartNames.ShockDuringWrite),
        new(0xDC, SmartNames.DiskShift),
        new(0xDD, SmartNames.AlternativeGSenseErrorRate),
        new(0xDE, SmartNames.LoadedHours),
        new(0xDF, SmartNames.LoadUnloadRetryCount),
        new(0xE0, SmartNames.LoadFriction),
        new(0xE1, SmartNames.LoadUnloadCycleCount),
        new(0xE2, SmartNames.LoadInTime),
        new(0xE3, SmartNames.TorqueAmplificationCount),
        new(0xE4, SmartNames.PowerOffRetractCycle),
        new(0xE6, SmartNames.GmrHeadAmplitude),
        new(0xE8, SmartNames.EnduranceRemaining),
        new(0xE9, SmartNames.PowerOnHours),
        new(0xF0, SmartNames.HeadFlyingHours),
        new(0xF1, SmartNames.TotalLbasWritten),
        new(0xF2, SmartNames.TotalLbasRead),
        new(0xFA, SmartNames.ReadErrorRetryRate),
        new(0xFE, SmartNames.FreeFallProtection),
        new(0xC2, SmartNames.Temperature, (r, _, p) => r[0] + (p?[0].Value ?? 0),
            SensorType.Temperature, 0, SmartNames.Temperature),

        new(0xE7, SmartNames.Temperature, (r, _, p) => r[0] + (p?[0].Value ?? 0),
            SensorType.Temperature, 0, SmartNames.Temperature),

        new(0xBE, SmartNames.TemperatureDifferenceFrom100, (r, _, p) => r[0] + (p?[0].Value ?? 0),
            SensorType.Temperature, 0, SmartNames.Temperature)
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericHardDisk"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="smart">The smart.</param>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <param name="index">The index.</param>
    /// <param name="settings">The settings.</param>
    internal GenericHardDisk(StorageInfo storageInfo, ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
        : base(storageInfo, smart, name, firmwareRevision, "hdd", index, _smartAttributes, settings) { }
}
