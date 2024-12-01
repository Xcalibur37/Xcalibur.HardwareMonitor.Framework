using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
        /// <summary>
    /// Structure containing information related power management logging.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlpmLogSupportInfo
    {
        internal const int AdlPmlogMaxSensors = 256;

        /// list of sensors defined by ADL_PMLOG_SENSORS
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = AdlPmlogMaxSensors)]
        public ushort[] usSensors;

        /// Reserved
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] iReserved;
    }
}
