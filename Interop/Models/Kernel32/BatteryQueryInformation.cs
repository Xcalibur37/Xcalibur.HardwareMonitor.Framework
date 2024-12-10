using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    /// <summary>
    /// Contains battery query information. This structure is used with the IOCTL_BATTERY_QUERY_INFORMATION control
    /// code to specify the type of information to return.
    /// https://learn.microsoft.com/en-us/windows/win32/power/battery-query-information-str
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BatteryQueryInformation
    {
        /// <summary>
        /// The current battery tag for the battery. Only information for a battery matching the tag can be returned.
        /// Whenever this value does not match the battery's current tag, the IOCTL request will be completed with ERROR_FILE_NOT_FOUND.
        /// </summary>
        public uint BatteryTag;

        /// <summary>
        /// The level of the battery information being queried. The data returned by the IOCTL depends on this value.
        /// </summary>
        public BatteryQueryInformationLevel InformationLevel;

        /// <summary>
        /// This member is used only if InformationLevel is BatteryEstimatedTime.
        /// </summary>
        public uint AtRate;
    }
}
