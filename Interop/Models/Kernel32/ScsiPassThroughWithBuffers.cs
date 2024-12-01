using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScsiPassThroughWithBuffers
    {
        public ScsiPassThrough Spt;

        public uint Filler;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] SenseBuf;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] DataBuf;
    }
}
