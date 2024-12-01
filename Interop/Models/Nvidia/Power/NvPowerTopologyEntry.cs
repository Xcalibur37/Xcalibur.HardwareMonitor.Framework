using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Power
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvPowerTopologyEntry
    {
        public NvPowerTopologyDomain Domain;
        private readonly uint _reserved;
        public uint PowerUsage;
        private readonly uint _reserved1;
    }
}
