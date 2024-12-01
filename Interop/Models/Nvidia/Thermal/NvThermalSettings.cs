using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Thermal
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvThermalSettings
    {
        public const int MAX_THERMAL_SENSORS_PER_GPU = 3;

        public uint Version;
        public uint Count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_THERMAL_SENSORS_PER_GPU)]
        public NvSensor[] Sensor;
    }
}
