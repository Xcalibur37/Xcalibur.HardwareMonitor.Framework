namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage
{
    /// <summary>
    /// Abstract Storage - Performance Value
    /// </summary>
    internal class AbstractPerformanceValue
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public double Result { get; private set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        private ulong Time { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        private ulong Value { get; set; }

        /// <summary>
        /// Updates the specified value.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="valBase">The value base.</param>
        public void Update(ulong val, ulong valBase)
        {
            ulong diffValue = val - Value;
            ulong diffTime = valBase - Time;

            Value = val;
            Time = valBase;
            Result = 100.0 / diffTime * diffValue;

            //sometimes it is possible that diff_value > diff_timebase
            //limit result to 100%, this is because timing issues during read from pcie controller an latency between IO operation
            if (!(Result > 100)) return;
            Result = 100;
        }
    }
}
