using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct StorageDeviceDescriptorHeader
    {
        public uint Version;
        public uint Size;
    }
}
