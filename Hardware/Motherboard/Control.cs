namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard;

/// <summary>
/// Control
/// </summary>
internal class Control
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
    /// Initializes a new instance of the <see cref="Control"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="index">The index.</param>
    public Control(string name, int index)
    {
        Name = name;
        Index = index;
    }
}
