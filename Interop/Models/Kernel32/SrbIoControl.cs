using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SrbIoControl
    {
        public uint HeaderLenght;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Signature;

        public uint Timeout;
        public uint ControlCode;
        public uint ReturnCode;
        public uint Length;
    }
}
