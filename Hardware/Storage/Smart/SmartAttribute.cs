using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;

/// <summary>
/// Attribute: Smart
/// </summary>
public class SmartAttribute
{
    private readonly RawValueConversion _rawValueConversion;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rawValue">The raw value.</param>
    /// <param name="value">The value.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    public delegate float RawValueConversion(byte[] rawValue, byte value, IReadOnlyList<IParameter> parameters);

    /// <summary>
    /// Gets a value indicating whether [default hidden sensor].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [default hidden sensor]; otherwise, <c>false</c>.
    /// </value>
    public bool DefaultHiddenSensor { get; }

    /// <summary>
    /// Gets a value indicating whether this instance has raw value conversion.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has raw value conversion; otherwise, <c>false</c>.
    /// </value>
    public bool HasRawValueConversion => _rawValueConversion != null;

    /// <summary>
    /// Gets the SMART identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public byte Id { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Gets the parameter descriptions.
    /// </summary>
    /// <value>
    /// The parameter descriptions.
    /// </value>
    public ParameterDescription[] ParameterDescriptions { get; }

    /// <summary>
    /// Gets the sensor channel.
    /// </summary>
    /// <value>
    /// The sensor channel.
    /// </value>
    public int SensorChannel { get; }

    /// <summary>
    /// Gets the name of the sensor.
    /// </summary>
    /// <value>
    /// The name of the sensor.
    /// </value>
    public string SensorName { get; }

    /// <summary>
    /// Gets the type of the sensor.
    /// </summary>
    /// <value>
    /// The type of the sensor.
    /// </value>
    public SensorType? SensorType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartAttribute" /> class.
    /// </summary>
    /// <param name="id">The SMART id of the attribute.</param>
    /// <param name="name">The name of the attribute.</param>
    public SmartAttribute(byte id, string name) : 
        this(id, name, null, null, 0, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartAttribute" /> class.
    /// </summary>
    /// <param name="id">The SMART id of the attribute.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="rawValueConversion">
    /// A delegate for converting the raw byte
    /// array into a value (or null to use the attribute value).
    /// </param>
    public SmartAttribute(byte id, string name, RawValueConversion rawValueConversion) : 
        this(id, name, rawValueConversion, null, 0, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartAttribute"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="rawValueConversion">The raw value conversion.</param>
    /// <param name="sensorType">Type of the sensor.</param>
    /// <param name="sensorChannel">The sensor channel.</param>
    /// <param name="sensorName">Name of the sensor.</param>
    /// <param name="defaultHiddenSensor">if set to <c>true</c> [default hidden sensor].</param>
    /// <param name="parameterDescriptions">The parameter descriptions.</param>
    public SmartAttribute(byte id, string name, RawValueConversion rawValueConversion, SensorType? sensorType, int sensorChannel, string sensorName, bool defaultHiddenSensor = false, ParameterDescription[] parameterDescriptions = null)
    {
        Id = id;
        Name = name;
        _rawValueConversion = rawValueConversion;
        SensorType = sensorType;
        SensorChannel = sensorChannel;
        SensorName = sensorName;
        DefaultHiddenSensor = defaultHiddenSensor;
        ParameterDescriptions = parameterDescriptions;
    }

    /// <summary>
    /// Converts the value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    internal float ConvertValue(Interop.Models.Kernel32.SmartAttribute value, IReadOnlyList<IParameter> parameters) => 
        _rawValueConversion?.Invoke(value.RawValue, value.CurrentValue, parameters) ?? value.CurrentValue;
}