using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.FTDI
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FtDeviceInfoNode
    {
        public uint Flags;
        public FtDevice Type;
        public uint ID;
        public uint LocId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SerialNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Description;

        public FtHandle Handle;
    }
}
