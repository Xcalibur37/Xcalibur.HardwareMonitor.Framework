using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DriverStatus
    {
        public byte bDriverError;
        public byte bIDEError;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] Reserved;
    }
}
