namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

/// <summary>
/// Control Sensor: Interface
/// </summary>
public interface IControlSensor
{
    /// <summary>
    /// Gets the control mode.
    /// </summary>
    /// <value>
    /// The control mode.
    /// </value>
    ControlSensorMode ControlMode { get; }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    Identifier Identifier { get; }

    /// <summary>
    /// Gets the maximum software value.
    /// </summary>
    /// <value>
    /// The maximum software value.
    /// </value>
    float MaxSoftwareValue { get; }

    /// <summary>
    /// Gets the minimum software value.
    /// </summary>
    /// <value>
    /// The minimum software value.
    /// </value>
    float MinSoftwareValue { get; }

    /// <summary>
    /// Gets the sensor.
    /// </summary>
    /// <value>
    /// The sensor.
    /// </value>
    ISensor Sensor { get; }

    /// <summary>
    /// Gets the software value.
    /// </summary>
    /// <value>
    /// The software value.
    /// </value>
    float SoftwareValue { get; }

    /// <summary>
    /// Sets the default.
    /// </summary>
    void SetDefault();

    /// <summary>
    /// Sets the software.
    /// </summary>
    /// <param name="value">The value.</param>
    void SetSoftware(float value);
}