using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors
{
    /// <summary>
    /// Stores the read value and the time in which it was recorded.
    /// </summary>
    public struct SensorValue
    {
        /// <param name="value"><see cref="Value"/> of the sensor.</param>
        /// <param name="time">The time code during which the <see cref="Value"/> was recorded.</param>
        public SensorValue(float value, DateTime time)
        {
            Value = value;
            Time = time;
        }

        /// <summary>
        /// Gets the value of the sensor
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Gets the time code during which the <see cref="Value"/> was recorded.
        /// </summary>
        public DateTime Time { get; }
    }
}
