using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Usages
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvUsages
    {
        public uint Version;
        private readonly uint _reserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public NvUsagesEntry[] Entries;
    }
}
