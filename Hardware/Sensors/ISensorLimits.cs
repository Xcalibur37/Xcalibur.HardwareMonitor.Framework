namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

/// <summary>
/// Abstract object that stores information about the limits of <see cref="ISensor"/>.
/// </summary>
public interface ISensorLimits
{
    /// <summary>
    /// Upper limit of <see cref="ISensor"/> value.
    /// </summary>
    float? HighLimit { get; }

    /// <summary>
    /// Lower limit of <see cref="ISensor"/> value.
    /// </summary>
    float? LowLimit { get; }
}