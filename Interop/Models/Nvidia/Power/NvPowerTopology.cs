using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Power
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvPowerTopology
    {
        public const int MAX_POWER_TOPOLOGIES = 4;

        public int Version;
        public uint Count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_POWER_TOPOLOGIES, ArraySubType = UnmanagedType.Struct)]
        public NvPowerTopologyEntry[] Entries;
    }
}
