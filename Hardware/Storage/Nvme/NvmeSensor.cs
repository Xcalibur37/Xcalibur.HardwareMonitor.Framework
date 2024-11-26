using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using GetSensorValue = Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme.NvmeHelper.GetSensorValue;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme
{
    /// <summary>
    /// NVME Sensor
    /// </summary>
    /// <seealso cref="Sensor" />
    internal class NvmeSensor : Sensor
    {
        private readonly GetSensorValue _getValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NvmeSensor"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="index">The index.</param>
        /// <param name="defaultHidden">if set to <c>true</c> [default hidden].</param>
        /// <param name="sensorType">Type of the sensor.</param>
        /// <param name="hardware">The hardware.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="getValue">The get value.</param>
        public NvmeSensor(string name, int index, bool defaultHidden, SensorType sensorType, Hardware hardware, ISettings settings, GetSensorValue getValue)
            : base(name, index, defaultHidden, sensorType, hardware, null, settings)
        {
            _getValue = getValue;
        }

        /// <summary>
        /// Updates the specified health.
        /// </summary>
        /// <param name="health">The health.</param>
        public void Update(NvmeHealthInfo health)
        {
            float v = _getValue(health);
            if (SensorType == SensorType.Temperature && v is < -1000 or > 1000) return;
            Value = v;
        }
    }
}
