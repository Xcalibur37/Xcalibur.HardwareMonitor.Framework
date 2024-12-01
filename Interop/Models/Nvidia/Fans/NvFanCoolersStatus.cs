using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Fans
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvFanCoolersStatus
    {
        public const int MAX_FAN_COOLERS_STATUS_ITEMS = 32;

        public uint Version;
        public uint Count;

        public ulong Reserved1;
        public ulong Reserved2;
        public ulong Reserved3;
        public ulong Reserved4;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_FAN_COOLERS_STATUS_ITEMS)]
        internal NvFanCoolersStatusItem[] Items;
    }
}
