using System.Collections.Generic;
using Xcalibur.Extensions.V2;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Memory;

/// <summary>
/// Memory Group
/// </summary>
/// <seealso cref="IGroup" />
internal class MemoryGroup : IGroup
{
    private readonly Hardware[] _hardware;

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _hardware;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryGroup"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public MemoryGroup(ISettings settings)
    {
        _hardware = [
            Software.OperatingSystem.IsUnix 
                ? new GenericLinuxMemory(MemoryConstants.GenericMemory, settings) 
                : new GenericWindowsMemory(MemoryConstants.GenericMemory, settings)
        ];
    }

    /// <summary>
    /// Close open devices.
    /// </summary>
    public void Close()
    {
        _hardware.Apply(x => x.Close());
    }
}
