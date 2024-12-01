using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Fans
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvFanCoolerControl
    {
        public const int MAX_FAN_CONTROLLER_ITEMS = 32;

        public uint Version;
        private readonly uint _reserved;
        public uint Count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U4)]
        private readonly uint[] _reserved2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_FAN_CONTROLLER_ITEMS)]
        public NvFanCoolerControlItem[] Items;
    }
}
