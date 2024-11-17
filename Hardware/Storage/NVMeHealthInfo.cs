﻿




using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

public abstract class NVMeHealthInfo
{
    public byte AvailableSpare { get; protected set; }

    public byte AvailableSpareThreshold { get; protected set; }

    public ulong ControllerBusyTime { get; protected set; }

    public uint CriticalCompositeTemperatureTime { get; protected set; }

    public Kernel32.NVME_CRITICAL_WARNING CriticalWarning { get; protected set; }

    public ulong DataUnitRead { get; protected set; }

    public ulong DataUnitWritten { get; protected set; }

    public ulong ErrorInfoLogEntryCount { get; protected set; }

    public ulong HostReadCommands { get; protected set; }

    public ulong HostWriteCommands { get; protected set; }

    public ulong MediaErrors { get; protected set; }

    public byte PercentageUsed { get; protected set; }

    public ulong PowerCycle { get; protected set; }

    public ulong PowerOnHours { get; protected set; }

    public short Temperature { get; protected set; }

    public short[] TemperatureSensors { get; protected set; }

    public ulong UnsafeShutdowns { get; protected set; }

    public uint WarningCompositeTemperatureTime { get; protected set; }
}