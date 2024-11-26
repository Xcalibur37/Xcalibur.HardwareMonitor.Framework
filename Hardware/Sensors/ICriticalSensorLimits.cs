namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors
{
    /// <summary>
    /// Abstract object that stores information about the critical limits of <see cref="ISensor"/>.
    /// </summary>
    public interface ICriticalSensorLimits
    {
        /// <summary>
        /// Critical upper limit of <see cref="ISensor"/> value.
        /// </summary>
        float? CriticalHighLimit { get; }

        /// <summary>
        /// Critical lower limit of <see cref="ISensor"/> value.
        /// </summary>
        float? CriticalLowLimit { get; }
    }
}
