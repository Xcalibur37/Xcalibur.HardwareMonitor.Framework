namespace Xcalibur.HardwareMonitor.Framework.Hardware;

/// <summary>
/// Hardware Changed: Interface
/// </summary>
internal interface IHardwareChanged
{
    /// <summary>
    /// Occurs when [hardware added].
    /// </summary>
    event HardwareEventHandler HardwareAdded;

    /// <summary>
    /// Occurs when [hardware removed].
    /// </summary>
    event HardwareEventHandler HardwareRemoved;
}