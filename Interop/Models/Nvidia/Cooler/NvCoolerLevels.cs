using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Level;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Cooler
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvCoolerLevels
    {
        public const int MAX_COOLERS_PER_GPU = 20;

        public uint Version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_COOLERS_PER_GPU)]
        public NvLevel[] Levels;
    }
}
