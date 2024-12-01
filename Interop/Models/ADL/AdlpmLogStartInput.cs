using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    /// <summary>
    /// Structure containing information to start power management logging.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlpmLogStartInput
    {
        internal const int AdlPmlogMaxSensors = 256;

        /// list of sensors defined by ADL_PMLOG_SENSORS
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = AdlPmlogMaxSensors)]
        public ushort[] usSensors;

        /// Sample rate in milliseconds
        public uint ulSampleRate;

        /// Reserved
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public int[] iReserved;
    }
}
