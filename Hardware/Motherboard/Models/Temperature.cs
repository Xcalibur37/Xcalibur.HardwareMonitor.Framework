namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

/// <summary>
/// Temperature
/// </summary>
internal class Temperature
{
    /// <summary>
    /// Gets the index.
    /// </summary>
    /// <value>
    /// The index.
    /// </value>
    public int Index { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Temperature"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="index">The index.</param>
    public Temperature(string name, int index)
    {
        Name = name;
        Index = index;
    }
}