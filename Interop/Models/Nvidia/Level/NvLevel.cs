using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Level
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvLevel
    {
        public int Level;
        public NvLevelPolicy Policy;
    }
}
