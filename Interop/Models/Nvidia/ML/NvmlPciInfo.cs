using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.ML
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NvmlPciInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string busId;

        public uint domain;

        public uint bus;

        public uint device;

        public ushort pciVendorId;

        public ushort pciDeviceId;

        public uint pciSubSystemId;

        public uint reserved0;
        public uint reserved1;
        public uint reserved2;
        public uint reserved3;
    }
}
