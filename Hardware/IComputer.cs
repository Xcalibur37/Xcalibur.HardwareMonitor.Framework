using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware;

/// <summary>
/// Handler that will trigger the actions assigned to it when the event occurs.
/// </summary>
/// <param name="hardware">Component returned to the assigned action(s).</param>
public delegate void HardwareEventHandler(IHardware hardware);

/// <summary>
/// Basic abstract with methods for the class which can store all hardware and decides which devices are to be checked and updated.
/// </summary>
public interface IComputer : IElement
{
    /// <summary>
    /// Gets a list of all known <see cref="IHardware" />.
    /// <para>Can be updated by <see cref="IVisitor" />.</para>
    /// </summary>
    /// <returns>List of all enabled devices.</returns>
    IList<IHardware> Hardware { get; }

    /// <summary>
    /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Battery" /> devices should be enabled and updated.
    /// </summary>
    /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
    bool IsBatteryEnabled { get; }

    /// <summary>
    /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Cpu" /> devices should be enabled and updated.
    /// </summary>
    /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
    bool IsCpuEnabled { get; }

    /// <summary>
    /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.GpuAmd" /> or <see cref="HardwareType.GpuNvidia" /> devices should be enabled and updated.
    /// </summary>
    /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
    bool IsGpuEnabled { get; }

    /// <summary>
    /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Memory" /> devices should be enabled and updated.
    /// </summary>
    /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
    bool IsMemoryEnabled { get; }

    /// <summary>
    /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Motherboard" /> devices should be enabled and updated.
    /// </summary>
    /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
    bool IsMotherboardEnabled { get; }

    /// <summary>
    /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Network" /> devices should be enabled and updated.
    /// </summary>
    /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
    bool IsNetworkEnabled { get; }

    /// <summary>
    /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Storage" /> devices should be enabled and updated.
    /// </summary>
    /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
    bool IsStorageEnabled { get; }

    /// <summary>
    /// Triggered when a new <see cref="IHardware" /> is registered.
    /// </summary>
    event HardwareEventHandler HardwareAdded;

    /// <summary>
    /// Triggered when a <see cref="IHardware" /> is removed.
    /// </summary>
    event HardwareEventHandler HardwareRemoved;
}