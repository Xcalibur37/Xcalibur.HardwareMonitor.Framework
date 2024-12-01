using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SendCmdOutParams
    {
        public uint cBufferSize;
        public DriverStatus DriverStatus;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] bBuffer;
    }
}
