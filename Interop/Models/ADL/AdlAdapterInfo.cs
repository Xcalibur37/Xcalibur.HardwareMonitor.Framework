using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    /// <summary>
    /// ADL Adapter Info
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlAdapterInfo
    {
        public const int AdlMaxPath = 256;

        public int Size;
        public int AdapterIndex;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = AdlMaxPath)]
        public string UDID;

        public int BusNumber;
        public int DeviceNumber;
        public int FunctionNumber;
        public int VendorID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = AdlMaxPath)]
        public string AdapterName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = AdlMaxPath)]
        public string DisplayName;

        public int Present;
        public int Exist;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = AdlMaxPath)]
        public string DriverPath;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = AdlMaxPath)]
        public string DriverPathExt;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = AdlMaxPath)]
        public string PNPString;

        public int OSDisplayIndex;
    }
}
