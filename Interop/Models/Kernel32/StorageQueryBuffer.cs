using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct StorageQueryBuffer
    {
        public StoragePropertyId PropertyId;
        public StorageQueryType QueryType;
        public StorageProtocolSpecificData ProtocolSpecific;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] Buffer;
    }
}
