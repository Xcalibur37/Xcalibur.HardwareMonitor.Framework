using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc;

namespace Xcalibur.HardwareMonitor.Framework.Hardware;

/// <summary>
/// Abstract object that stores information about a device. All sensors are available as an array of <see cref="Sensors"/>.
/// <para>
/// Can contain <see cref="SubHardware"/>.
/// Type specified in <see cref="HardwareType"/>.
/// </para>
/// </summary>
public interface IHardware : IElement
{
    #region Properties
    
    /// <summary>
    /// <inheritdoc cref="Framework.Hardware.HardwareType"/>
    /// </summary>
    HardwareType HardwareType { get; }

    /// <summary>
    /// Gets a unique hardware ID that represents its location.
    /// </summary>
    Identifier Identifier { get; }

    /// <summary>
    /// Gets or sets device name.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets the device that is the parent of the current hardware. For example, the motherboard is the parent of SuperIO.
    /// </summary>
    IHardware Parent { get; }

    /// <summary>
    /// Gets rarely changed hardware properties that can't be represented as sensors.
    /// </summary>
    IDictionary<string, string> Properties { get; }

    /// <summary>
    /// Gets an array of all sensors such as <see cref="SensorType.Temperature"/>, <see cref="SensorType.Clock"/>, <see cref="SensorType.Load"/> etc.
    /// </summary>
    ISensor[] Sensors { get; }

    /// <summary>
    /// Gets child devices, e.g. <see cref="LpcIo"/> of the <see cref="Motherboard"/>.
    /// </summary>
    IHardware[] SubHardware { get; }

    #endregion

    #region Methods
    
    /// <summary>
    /// Refreshes the information stored in <see cref="Sensors"/> array.
    /// </summary>
    void Update();

    #endregion

    #region Events

    /// <summary>
    /// An <see langword="event"/> that will be triggered when a new sensor appears.
    /// </summary>
    event SensorEventHandler SensorAdded;

    /// <summary>
    /// An <see langword="event"/> that will be triggered when one of the sensors is removed.
    /// </summary>
    event SensorEventHandler SensorRemoved;

    #endregion
}
