using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlpmLogData
    {
        internal const int AdlPmlogMaxSensors = 256;

        /// Structure version
        public uint ulVersion;

        /// Current driver sample rate
        public uint ulActiveSampleRate;

        /// Timestamp of last update
        public ulong ulLastUpdated;

        // 2D array of sensor and values -- unsigned int ulValues[ADL_PMLOG_MAX_SUPPORTED_SENSORS][2]
        // the nested array will be accessed like a single dimension array
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = AdlPmlogMaxSensors * 2)]
        public uint[] ulValues;

        /// Reserved
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public uint[] ulReserved;
    }
}
