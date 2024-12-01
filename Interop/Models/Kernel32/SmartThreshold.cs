using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmartThreshold
    {
        public byte Id;
        public byte Threshold;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] Reserved;
    }
}
