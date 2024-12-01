using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Usages
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvUsagesEntry
    {
        public uint IsPresent;
        public uint Percentage;
        private readonly uint _reserved1;
        private readonly uint _reserved2;
    }
}
