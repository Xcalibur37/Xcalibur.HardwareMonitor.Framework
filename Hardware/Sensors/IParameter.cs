﻿namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

/// <summary>
/// Abstract object that represents additional parameters included in <see cref="ISensor"/>.
/// </summary>
public interface IParameter : IElement
{
    /// <summary>
    /// Gets a parameter default value defined by library.
    /// </summary>
    float DefaultValue { get; }

    /// <summary>
    /// Gets a parameter description defined by library.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets a unique parameter ID that represents its location.
    /// </summary>
    Identifier Identifier { get; }

    /// <summary>
    /// Gets or sets information whether the given <see cref="IParameter"/> is the default for <see cref="ISensor"/>.
    /// </summary>
    bool IsDefault { get; set; }

    /// <summary>
    /// Gets a parameter name defined by library.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the sensor that is the data container for the given parameter.
    /// </summary>
    ISensor Sensor { get; }

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    float Value { get; set; }
}