using System;
using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

/// <summary>
/// Stores information about the read values and the time in which they were collected.
/// </summary>
public interface ISensor : IElement
{
    /// <summary>
    /// Gets the control.
    /// </summary>
    /// <value>
    /// The control.
    /// </value>
    IControl Control { get; }

    /// <summary>
    /// <inheritdoc cref="IHardware"/>
    /// </summary>
    IHardware Hardware { get; }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    Identifier Identifier { get; }

    /// <summary>
    /// Gets the unique identifier of this sensor for a given <see cref="IHardware"/>.
    /// </summary>
    int Index { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is default hidden.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is default hidden; otherwise, <c>false</c>.
    /// </value>
    bool IsDefaultHidden { get; }

    /// <summary>
    /// Gets a maximum value recorded for the given sensor.
    /// </summary>
    float? Max { get; }

    /// <summary>
    /// Gets a minimum value recorded for the given sensor.
    /// </summary>
    float? Min { get; }

    /// <summary>
    /// Gets or sets a sensor name.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <value>
    /// The parameters.
    /// </value>
    IReadOnlyList<IParameter> Parameters { get; }

    /// <summary>
    /// <inheritdoc cref="Sensors.SensorType"/>
    /// </summary>
    SensorType SensorType { get; }

    /// <summary>
    /// Gets the last recorded value for the given sensor.
    /// </summary>
    float? Value { get; }

    /// <summary>
    /// Gets a list of recorded values for the given sensor.
    /// </summary>
    IEnumerable<SensorValue> Values { get; }

    /// <summary>
    /// Gets or sets the values time window.
    /// </summary>
    /// <value>
    /// The values time window.
    /// </value>
    TimeSpan ValuesTimeWindow { get; set; }

    /// <summary>
    /// Resets a value stored in <see cref="Min"/>.
    /// </summary>
    void ResetMin();

    /// <summary>
    /// Resets a value stored in <see cref="Max"/>.
    /// </summary>
    void ResetMax();

    /// <summary>
    /// Clears the values stored in <see cref="Values"/>.
    /// </summary>
    void ClearValues();
}
