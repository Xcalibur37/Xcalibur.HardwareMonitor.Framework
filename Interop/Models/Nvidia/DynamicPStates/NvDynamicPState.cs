using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.DynamicPStates
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvDynamicPState
    {
        public bool IsPresent;
        public int Percentage;
    }
}
