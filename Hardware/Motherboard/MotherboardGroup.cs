// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

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