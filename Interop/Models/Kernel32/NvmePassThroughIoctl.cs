using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NvmePassThroughIoctl
    {
        public SrbIoControl srb;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public uint[] VendorSpecific;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public uint[] NVMeCmd;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] CplEntry;

        public NvmeDirection Direction;
        public uint QueueId;
        public uint DataBufferLen;
        public uint MetaDataLen;
        public uint ReturnBufferLen;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] DataBuffer;
    }
}
