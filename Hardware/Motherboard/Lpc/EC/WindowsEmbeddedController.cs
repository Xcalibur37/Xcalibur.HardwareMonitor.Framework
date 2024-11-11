// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// All Rights Reserved.

using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Windows Embedded Controller.
/// </summary>
/// <seealso cref="EmbeddedControllerBase" />
public class WindowsEmbeddedController : EmbeddedControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsEmbeddedController"/> class.
    /// </summary>
    /// <param name="sources">The sources.</param>
    /// <param name="settings">The settings.</param>
    public WindowsEmbeddedController(IEnumerable<EmbeddedControllerSource> sources, ISettings settings) : 
        base(sources, settings) { }

    /// <summary>
    /// Acquires the io interface.
    /// </summary>
    /// <returns></returns>
    protected override IEmbeddedControllerIo AcquireIoInterface() => new WindowsEmbeddedControllerIo();
}