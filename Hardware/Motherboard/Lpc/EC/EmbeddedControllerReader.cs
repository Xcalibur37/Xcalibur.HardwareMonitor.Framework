// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// All Rights Reserved.

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Embedded Controller Reader
/// </summary>
/// <param name="ecIO">The ec io.</param>
/// <param name="register">The register.</param>
/// <returns></returns>
public delegate float EmbeddedControllerReader(IEmbeddedControllerIo ecIo, ushort register);