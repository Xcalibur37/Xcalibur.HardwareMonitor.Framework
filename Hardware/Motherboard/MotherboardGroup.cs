





using System.Collections.Generic;
using Xcalibur.Extensions.V2;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard;

internal class MotherboardGroup : IGroup
{
    private readonly Motherboard[] _motherboards;

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _motherboards;

    /// <summary>
    /// Initializes a new instance of the <see cref="MotherboardGroup"/> class.
    /// </summary>
    /// <param name="smbios">The smbios.</param>
    /// <param name="settings">The settings.</param>
    public MotherboardGroup(SMBios smbios, ISettings settings)
    {
        _motherboards = new Motherboard[1];
        _motherboards[0] = new Motherboard(smbios, settings);
    }

    /// <summary>
    /// Close open devices.
    /// </summary>
    public void Close()
    {
        _motherboards.Apply(x => x.Close());
    }
}