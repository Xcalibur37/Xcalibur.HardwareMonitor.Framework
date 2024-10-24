// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.Intel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu;

internal class IntelGpuGroup : IGroup
{
    private readonly List<Hardware> _hardware = new();

    public IntelGpuGroup(List<IntelCpu> intelCpus, ISettings settings)
    {
        if (!Software.OperatingSystem.IsUnix && intelCpus?.Count > 0)
        {

            string[] ids = D3DDisplayDevice.GetDeviceIdentifiers();

            for (int i = 0; i < ids.Length; i++)
            {
                string deviceId = ids[i];
                bool isIntel = deviceId.IndexOf("VEN_8086", StringComparison.Ordinal) != -1;

                if (isIntel && D3DDisplayDevice.GetDeviceInfoByIdentifier(deviceId, out D3DDisplayDevice.D3DDeviceInfo deviceInfo))
                {
                    if (deviceInfo.Integrated)
                    {
                        // It may seem strange to only use the first cpu here, but in-case we have a multi cpu system with integrated graphics (does that exist?),
                        // we would pick up the multiple device identifiers above and would add one instance for each CPU.
                        _hardware.Add(new IntelIntegratedGpu(intelCpus[0], deviceId, deviceInfo, settings));
                    }
                }
            }
        }
    }

    public IReadOnlyList<IHardware> Hardware => _hardware;

    public void Close()
    {
        foreach (Hardware gpu in _hardware)
            gpu.Close();
    }
}