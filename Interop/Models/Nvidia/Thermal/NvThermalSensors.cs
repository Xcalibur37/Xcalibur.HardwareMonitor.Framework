using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Thermal
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct NvThermalSensors
    {
        public const int THERMAL_SENSOR_RESERVED_COUNT = 8;
        public const int THERMAL_SENSOR_TEMPERATURE_COUNT = 32;

        internal uint Version;
        internal uint Mask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = THERMAL_SENSOR_RESERVED_COUNT)]
        internal int[] Reserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = THERMAL_SENSOR_TEMPERATURE_COUNT)]
        internal int[] Temperatures;
    }
}
