namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu;

/// <summary>
/// GPU Base
/// </summary>
/// <seealso cref="Xcalibur.HardwareMonitor.Framework.Hardware.Hardware" />
public abstract class GpuBase : Hardware
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GpuBase" /> class.
    /// </summary>
    /// <param name="name">Component name.</param>
    /// <param name="identifier">Identifier that will be assigned to the device. Based on <see cref="Identifier" /></param>
    /// <param name="settings">Additional settings passed by the <see cref="IComputer" />.</param>
    protected GpuBase(string name, Identifier identifier, ISettings settings) : base(name, identifier, settings) { }

    /// <summary>
    /// Gets the device identifier.
    /// </summary>
    public abstract string DeviceId { get; }
}