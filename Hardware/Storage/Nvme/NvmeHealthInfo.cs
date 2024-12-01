using System;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// NVME Health Information
/// </summary>
public class NvmeHealthInfo
{
    #region Properties

    /// <summary>
    /// Gets or sets the available spare.
    /// </summary>
    /// <value>
    /// The available spare.
    /// </value>
    public byte AvailableSpare { get; protected set; }

    /// <summary>
    /// Gets or sets the available spare threshold.
    /// </summary>
    /// <value>
    /// The available spare threshold.
    /// </value>
    public byte AvailableSpareThreshold { get; protected set; }

    /// <summary>
    /// Gets or sets the controller busy time.
    /// </summary>
    /// <value>
    /// The controller busy time.
    /// </value>
    public ulong ControllerBusyTime { get; protected set; }

    /// <summary>
    /// Gets or sets the critical composite temperature time.
    /// </summary>
    /// <value>
    /// The critical composite temperature time.
    /// </value>
    public uint CriticalCompositeTemperatureTime { get; protected set; }

    /// <summary>
    /// Gets or sets the critical warning.
    /// </summary>
    /// <value>
    /// The critical warning.
    /// </value>
    public NvmeCriticalWarning CriticalWarning { get; protected set; }

    /// <summary>
    /// Gets or sets the data unit read.
    /// </summary>
    /// <value>
    /// The data unit read.
    /// </value>
    public ulong DataUnitRead { get; protected set; }

    /// <summary>
    /// Gets or sets the data unit written.
    /// </summary>
    /// <value>
    /// The data unit written.
    /// </value>
    public ulong DataUnitWritten { get; protected set; }

    /// <summary>
    /// Gets or sets the error information log entry count.
    /// </summary>
    /// <value>
    /// The error information log entry count.
    /// </value>
    public ulong ErrorInfoLogEntryCount { get; protected set; }

    /// <summary>
    /// Gets or sets the host read commands.
    /// </summary>
    /// <value>
    /// The host read commands.
    /// </value>
    public ulong HostReadCommands { get; protected set; }

    /// <summary>
    /// Gets or sets the host write commands.
    /// </summary>
    /// <value>
    /// The host write commands.
    /// </value>
    public ulong HostWriteCommands { get; protected set; }

    /// <summary>
    /// Gets or sets the media errors.
    /// </summary>
    /// <value>
    /// The media errors.
    /// </value>
    public ulong MediaErrors { get; protected set; }

    /// <summary>
    /// Gets or sets the percentage used.
    /// </summary>
    /// <value>
    /// The percentage used.
    /// </value>
    public byte PercentageUsed { get; protected set; }

    /// <summary>
    /// Gets or sets the power cycle.
    /// </summary>
    /// <value>
    /// The power cycle.
    /// </value>
    public ulong PowerCycle { get; protected set; }

    /// <summary>
    /// Gets or sets the power on hours.
    /// </summary>
    /// <value>
    /// The power on hours.
    /// </value>
    public ulong PowerOnHours { get; protected set; }

    /// <summary>
    /// Gets or sets the temperature.
    /// </summary>
    /// <value>
    /// The temperature.
    /// </value>
    public short Temperature { get; protected set; }

    /// <summary>
    /// Gets or sets the temperature sensors.
    /// </summary>
    /// <value>
    /// The temperature sensors.
    /// </value>
    public short[] TemperatureSensors { get; protected set; }

    /// <summary>
    /// Gets or sets the unsafe shutdowns.
    /// </summary>
    /// <value>
    /// The unsafe shutdowns.
    /// </value>
    public ulong UnsafeShutdowns { get; protected set; }

    /// <summary>
    /// Gets or sets the warning composite temperature time.
    /// </summary>
    /// <value>
    /// The warning composite temperature time.
    /// </value>
    public uint WarningCompositeTemperatureTime { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NvmeHealthInfo"/> class.
    /// </summary>
    /// <param name="log">The log.</param>
    public NvmeHealthInfo(NvmeHealthInfoLog log)
    {
        CriticalWarning = (NvmeCriticalWarning)log.CriticalWarning;
        Temperature = NvmeHelper.KelvinToCelsius(log.CompositeTemp);
        AvailableSpare = log.AvailableSpare;
        AvailableSpareThreshold = log.AvailableSpareThreshold;
        PercentageUsed = log.PercentageUsed;
        DataUnitRead = BitConverter.ToUInt64(log.DataUnitRead, 0);
        DataUnitWritten = BitConverter.ToUInt64(log.DataUnitWritten, 0);
        HostReadCommands = BitConverter.ToUInt64(log.HostReadCommands, 0);
        HostWriteCommands = BitConverter.ToUInt64(log.HostWriteCommands, 0);
        ControllerBusyTime = BitConverter.ToUInt64(log.ControllerBusyTime, 0);
        PowerCycle = BitConverter.ToUInt64(log.PowerCycles, 0);
        PowerOnHours = BitConverter.ToUInt64(log.PowerOnHours, 0);
        UnsafeShutdowns = BitConverter.ToUInt64(log.UnsafeShutdowns, 0);
        MediaErrors = BitConverter.ToUInt64(log.MediaAndDataIntegrityErrors, 0);
        ErrorInfoLogEntryCount = BitConverter.ToUInt64(log.NumberErrorInformationLogEntries, 0);
        WarningCompositeTemperatureTime = log.WarningCompositeTemperatureTime;
        CriticalCompositeTemperatureTime = log.CriticalCompositeTemperatureTime;
        TemperatureSensors = new short[log.TemperatureSensor.Length];

        for (int i = 0; i < TemperatureSensors.Length; i++)
        {
            TemperatureSensors[i] = NvmeHelper.KelvinToCelsius(log.TemperatureSensor[i]);
        }
    }

    #endregion
}