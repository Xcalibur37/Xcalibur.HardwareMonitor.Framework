using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.GPU
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvGpuClockFrequenciesDomain
    {
        private readonly uint _isPresent;
        public uint Frequency;

        public bool IsPresent => (_isPresent & 1) != 0;
    }
}
