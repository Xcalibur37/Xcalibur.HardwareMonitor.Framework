using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct StoragePropertyQuery
    {
        public StoragePropertyId PropertyId;
        public StorageQueryType QueryType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] AdditionalParameters;
    }
}
