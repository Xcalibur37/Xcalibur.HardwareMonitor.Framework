// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.Gigabyte;

/// <summary>
/// Gigabyte Controller: Interface
/// </summary>
internal interface IGigabyteController
{
    /// <summary>
    /// Enables the specified enabled.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <returns></returns>
    bool Enable(bool enabled);

    /// <summary>
    /// Restores this instance.
    /// </summary>
    void Restore();
}
