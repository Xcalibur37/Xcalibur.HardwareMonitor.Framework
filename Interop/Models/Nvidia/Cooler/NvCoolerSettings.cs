using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Cooler
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvCoolerSettings
    {
        public const int MAX_COOLERS_PER_GPU = 20;

        public uint Version;
        public uint Count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_COOLERS_PER_GPU)]
        public NvCooler[] Cooler;
    }
}
