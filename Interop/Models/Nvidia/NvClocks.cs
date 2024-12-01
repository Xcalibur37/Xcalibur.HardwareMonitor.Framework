using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvClocks
    {
        public const int MAX_CLOCKS_PER_GPU = 0x120;

        public uint Version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CLOCKS_PER_GPU)]
        public uint[] Clock;
    }
}
