using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.GPU
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvGpuClockFrequencies
    {
        public const int MAX_GPU_PUBLIC_CLOCKS = 32;

        public uint Version;
        private readonly uint _reserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_GPU_PUBLIC_CLOCKS)]
        public NvGpuClockFrequenciesDomain[] Clocks;
    }
}
