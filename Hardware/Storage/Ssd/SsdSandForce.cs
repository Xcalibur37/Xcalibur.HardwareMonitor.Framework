using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Ssd;

/// <summary>
/// SSD: SandForce
/// </summary>
/// <seealso cref="AtaStorage" />
[NamePrefix(""), RequireSmart(0xAB), RequireSmart(0xB1)]
internal class SsdSandForce : AtaStorage
{
    /// <summary>
    /// The smart attributes
    /// </summary>
    private static readonly IReadOnlyList<SmartAttribute> _smartAttributes = new List<SmartAttribute>
    {
        new(0x01, SmartNames.RawReadErrorRate),
        new(0x05, SmartNames.RetiredBlockCount, RawToInt),
        new(0x09, SmartNames.PowerOnHours, RawToInt),
        new(0x0C, SmartNames.PowerCycleCount, RawToInt),
        new(0xAB, SmartNames.ProgramFailCount, RawToInt),
        new(0xAC, SmartNames.EraseFailCount, RawToInt),
        new(0xAE, SmartNames.UnexpectedPowerLossCount, RawToInt),
        new(0xB1, SmartNames.WearRangeDelta, RawToInt),
        new(0xB5, SmartNames.AlternativeProgramFailCount, RawToInt),
        new(0xB6, SmartNames.AlternativeEraseFailCount, RawToInt),
        new(0xBB, SmartNames.UncorrectableErrorCount, RawToInt),
        new(0xC2,
            SmartNames.Temperature,
            (_, value, p) => value + (p?[0].Value ?? 0),
            SensorType.Temperature,
            0,
            SmartNames.Temperature,
            true,
            new[] { new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0) }),
        new(0xC3, SmartNames.UnrecoverableEcc),
        new(0xC4, SmartNames.ReallocationEventCount, RawToInt),
        new(0xE7, SmartNames.RemainingLife, null, SensorType.Level, 0, SmartNames.RemainingLife),
        new(0xE9, SmartNames.ControllerWritesToNand, RawToInt, SensorType.Data, 0, SmartNames.ControllerWritesToNand),
        new(0xEA, SmartNames.HostWritesToController, RawToInt, SensorType.Data, 1, SmartNames.HostWritesToController),
        new(0xF1, SmartNames.HostWrites, RawToInt, SensorType.Data, 1, SmartNames.HostWrites),
        new(0xF2, SmartNames.HostReads, RawToInt, SensorType.Data, 2, SmartNames.HostReads)
    };

    private readonly Sensor _writeAmplification;

    /// <summary>
    /// Initializes a new instance of the <see cref="SsdSandForce"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="smart">The smart.</param>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <param name="index">The index.</param>
    /// <param name="settings">The settings.</param>
    public SsdSandForce(StorageInfo storageInfo, ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
        : base(storageInfo, smart, name, firmwareRevision, "ssd", index, _smartAttributes, settings)
    {
        _writeAmplification = new Sensor("Write Amplification", 1, SensorType.Factor, this, settings);
    }

    /// <summary>
    /// Updates the additional sensors.
    /// </summary>
    /// <param name="values">The values.</param>
    protected override void UpdateAdditionalSensors(Interop.Models.Kernel32.SmartAttribute[] values)
    {
        float? controllerWritesToNand = null;
        float? hostWritesToController = null;
        foreach (Interop.Models.Kernel32.SmartAttribute value in values)
        {
            if (value.Id == 0xE9)
            {
                controllerWritesToNand = RawToInt(value.RawValue, value.CurrentValue, null);
            }

            if (value.Id == 0xEA)
            {
                hostWritesToController = RawToInt(value.RawValue, value.CurrentValue, null);
            }
        }

        if (!controllerWritesToNand.HasValue || !hostWritesToController.HasValue) return;
        _writeAmplification.Value = hostWritesToController.Value > 0
            ? controllerWritesToNand.Value / hostWritesToController.Value
            : 0;

        ActivateSensor(_writeAmplification);
    }
}
