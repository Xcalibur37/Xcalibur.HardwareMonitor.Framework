using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmartAttribute
    {
        public byte Id;
        public short Flags;
        public byte CurrentValue;
        public byte WorstValue;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] RawValue;

        public byte Reserved;
    }
}
