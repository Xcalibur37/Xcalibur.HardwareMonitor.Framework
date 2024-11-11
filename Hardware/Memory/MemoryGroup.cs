// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using Xcalibur.Extensions.V2;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Memory;

/// <summary>
/// Memory Group
/// </summary>
/// <seealso cref="Xcalibur.HardwareMonitor.Framework.Hardware.IGroup" />
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
        _hardware = new Hardware[] { Software.OperatingSystem.IsUnix ? new GenericLinuxMemory("Generic Memory", settings) : new GenericWindowsMemory("Generic Memory", settings) };
    }

    /// <summary>
    /// Close open devices.
    /// </summary>
    public void Close()
    {
        _hardware.Apply(x => x.Close());
    }
}
