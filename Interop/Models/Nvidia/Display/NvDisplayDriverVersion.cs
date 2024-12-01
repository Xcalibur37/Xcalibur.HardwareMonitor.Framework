using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Display
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvDisplayDriverVersion
    {
        public const int SHORT_STRING_MAX = 64;

        public uint Version;
        public uint DriverVersion;
        public uint BldChangeListNum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SHORT_STRING_MAX)]
        public string BuildBranch;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SHORT_STRING_MAX)]
        public string Adapter;
    }
}
