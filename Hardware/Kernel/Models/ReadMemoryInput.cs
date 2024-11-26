using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ReadMemoryInput
    {
        public ulong Address;
        public uint UnitSize;
        public uint Count;
    }
}
