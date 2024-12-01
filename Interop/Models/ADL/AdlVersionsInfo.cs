using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlVersionsInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DriverVer;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CatalystVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CatalystWebLink;
    }
}
