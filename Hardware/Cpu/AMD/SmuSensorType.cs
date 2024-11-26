using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD
{
    /// <summary>
    /// SMU Sensor Type
    /// </summary>
    public struct SmuSensorType
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name;

        /// <summary>
        /// The type
        /// </summary>
        public SensorType Type;

        /// <summary>
        /// The scale
        /// </summary>
        public float Scale;
    }
}
