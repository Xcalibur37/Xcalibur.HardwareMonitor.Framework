using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlMemoryInfoX4
    {
        public const int AdlMaxPath = 256;

        /// Memory size in bytes.
        public long iMemorySize;

        /// Memory type in string.
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = AdlMaxPath)]
        public string strMemoryType;

        /// Highest default performance level Memory bandwidth in Mbytes/s
        public long iMemoryBandwidth;

        /// HyperMemory size in bytes.
        public long iHyperMemorySize;

        /// Invisible Memory size in bytes.
        public long iInvisibleMemorySize;

        /// Visible Memory size in bytes.
        public long iVisibleMemorySize;

        /// Vram vendor ID
        public long iVramVendorRevId;

        /// Memory Bandiwidth that is calculated and finalized on the driver side, grab and go.
        public long iMemoryBandwidthX2;

        /// Memory Bit Rate that is calculated and finalized on the driver side, grab and go.
        public long iMemoryBitRateX2;
    }
}
