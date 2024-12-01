using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Thermal
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvSensor
    {
        public NvThermalController Controller;
        public uint DefaultMinTemp;
        public uint DefaultMaxTemp;
        public uint CurrentTemp;
        public NvThermalTarget Target;
    }
}
