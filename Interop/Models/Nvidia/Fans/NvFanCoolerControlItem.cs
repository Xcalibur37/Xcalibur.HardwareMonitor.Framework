using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Fans
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvFanCoolerControlItem
    {
        public uint CoolerId;
        public uint Level;
        public NvFanControlMode ControlMode;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U4)]
        private readonly uint[] _reserved;
    }
}
