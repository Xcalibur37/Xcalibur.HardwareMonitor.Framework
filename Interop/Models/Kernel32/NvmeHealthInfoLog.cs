using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
public struct NvmeHealthInfoLog
{
    /// <summary>
    /// This field indicates critical warnings for the state of the  controller.
    /// Each bit corresponds to a critical warning type; multiple bits may be set.
    /// </summary>
    public byte CriticalWarning;

    /// <summary>
    /// Composite Temperature:  Contains the temperature of the overall device (controller and NVM included) in units of Kelvin.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public byte[] CompositeTemp;

    /// <summary>
    /// Available Spare:  Contains a normalized percentage (0 to 100%) of the remaining spare capacity available
    /// </summary>
    public byte AvailableSpare;

    /// <summary>
    /// Available Spare Threshold:  When the Available Spare falls below the threshold indicated in this field,
    /// an asynchronous event completion may occur. The value is indicated as a normalized percentage (0 to 100%).
    /// </summary>
    public byte AvailableSpareThreshold;

    /// <summary>
    /// Percentage Used:  Contains a vendor specific estimate of the percentage of NVM subsystem life used based on
    /// the actual usage and the manufacturer’s prediction of NVM life. A value of 100 indicates that the estimated endurance of
    /// the NVM in the NVM subsystem has been consumed, but may not indicate an NVM subsystem failure. The value is allowed to exceed 100.
    /// </summary>
    public byte PercentageUsed;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
    public byte[] Reserved1;

    /// <summary>
    /// Data Units Read:  Contains the number of 512 byte data units the host has read from the controller;
    /// this value does not include metadata. This value is reported in thousands
    /// (i.e., a value of 1 corresponds to 1000 units of 512 bytes read) and is rounded up.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] DataUnitRead;

    /// <summary>
    /// Data Units Written:  Contains the number of 512 byte data units the host has written to the controller;
    /// this value does not include metadata. This value is reported in thousands
    /// (i.e., a value of 1 corresponds to 1000 units of 512 bytes written) and is rounded up.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] DataUnitWritten;

    /// <summary>
    /// Host Read Commands:  Contains the number of read commands completed by the controller.
    /// For the NVM command set, this is the number of Compare and Read commands.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] HostReadCommands;

    /// <summary>
    /// Host Write Commands:  Contains the number of write commands completed by the controller.
    /// For the NVM command set, this is the number of Write commands.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] HostWriteCommands;

    /// <summary>
    /// Controller Busy Time:  Contains the amount of time the controller is busy with I/O commands.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] ControllerBusyTime;

    /// <summary>
    /// Power Cycles:  Contains the number of power cycles.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] PowerCycles;

    /// <summary>
    /// Power On Hours:  Contains the number of power-on hours.
    /// This does not include time that the controller was powered and in a low power state condition.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] PowerOnHours;

    /// <summary>
    /// Unsafe Shutdowns:  Contains the number of unsafe shutdowns.
    /// This count is incremented when a shutdown notification is not received prior to loss of power.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] UnsafeShutdowns;

    /// <summary>
    /// Media Errors:  Contains the number of occurrences where the controller detected an unrecoverable data integrity error.
    /// Errors such as uncorrectable ECC, CRC checksum failure, or LBA tag mismatch are included in this field.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] MediaAndDataIntegrityErrors;

    /// <summary>
    /// Number of Error Information Log Entries:  Contains the number of Error Information log entries over the life of the controller
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] NumberErrorInformationLogEntries;

    /// <summary>
    /// Warning Composite Temperature Time:  Contains the amount of time in minutes that the controller is operational and the Composite Temperature is greater than or equal to the Warning Composite
    /// Temperature Threshold.
    /// </summary>
    public uint WarningCompositeTemperatureTime;

    /// <summary>
    /// Critical Composite Temperature Time:  Contains the amount of time in minutes that the controller is operational and the Composite Temperature is greater than the Critical Composite Temperature
    /// Threshold.
    /// </summary>
    public uint CriticalCompositeTemperatureTime;

    /// <summary>
    /// Contains the current temperature reported by temperature sensor 1-8.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public ushort[] TemperatureSensor;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 296)]
    public byte[] Reserved2;
}
}
