// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// All Rights Reserved.

using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Embedded Controller I/O - Interface
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IEmbeddedControllerIo : IDisposable
{
    /// <summary>
    /// Reads the specified registers.
    /// </summary>
    /// <param name="registers">The registers.</param>
    /// <param name="data">The data.</param>
    void Read(ushort[] registers, byte[] data);
}