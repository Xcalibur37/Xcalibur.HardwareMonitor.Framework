﻿using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Embedded Controller Source
/// </summary>
public class EmbeddedControllerSource
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Gets the register.
    /// </summary>
    /// <value>
    /// The register.
    /// </value>
    public ushort Register { get; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    /// <value>
    /// The size.
    /// </value>
    public byte Size { get; }

    /// <summary>
    /// Gets the factor.
    /// </summary>
    /// <value>
    /// The factor.
    /// </value>
    public float Factor { get; }

    /// <summary>
    /// Gets the blank.
    /// </summary>
    /// <value>
    /// The blank.
    /// </value>
    public int Blank { get; }

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public SensorType Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddedControllerSource"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="type">The type.</param>
    /// <param name="register">The register.</param>
    /// <param name="size">The size.</param>
    /// <param name="factor">The factor.</param>
    /// <param name="blank">The blank.</param>
    public EmbeddedControllerSource(string name, SensorType type, ushort register, byte size = 1, float factor = 1.0f, int blank = int.MaxValue)
    {
        Name = name;
        Register = register;
        Size = size;
        Type = type;
        Factor = factor;
        Blank = blank;
    }
}