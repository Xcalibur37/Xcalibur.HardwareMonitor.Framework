namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

/// <summary>
/// Fan
/// </summary>
internal class Fan
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
    /// Initializes a new instance of the <see cref="Fan"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="index">The index.</param>
    public Fan(string name, int index)
    {
        Name = name;
        Index = index;
    }
}