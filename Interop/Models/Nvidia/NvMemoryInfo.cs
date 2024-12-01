using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvMemoryInfo
    {
        public uint Version;

        public uint DedicatedVideoMemory;

        public uint AvailableDedicatedVideoMemory;

        public uint SystemVideoMemory;

        public uint SharedSystemMemory;

        public uint CurrentAvailableDedicatedVideoMemory;
    }
}
