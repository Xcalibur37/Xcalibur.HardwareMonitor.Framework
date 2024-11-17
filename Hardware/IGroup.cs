using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware;

/// <summary>
/// A group of devices from one category in one list.
/// </summary>
internal interface IGroup
{
    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware"/> in a given group.
    /// </summary>
    IReadOnlyList<IHardware> Hardware { get; }
    
    /// <summary>
    /// Close open devices.
    /// </summary>
    void Close();
}