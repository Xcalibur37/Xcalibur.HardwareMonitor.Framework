using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Cooler
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvCooler
    {
        public int Type;
        public int Controller;
        public int DefaultMin;
        public int DefaultMax;
        public int CurrentMin;
        public int CurrentMax;
        public int CurrentLevel;
        public int DefaultPolicy;
        public int CurrentPolicy;
        public int Target;
        public int ControlType;
        public int Active;
    }
}
