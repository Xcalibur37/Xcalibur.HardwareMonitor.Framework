// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu;

internal class NvidiaGroup : IGroup
{
    private readonly List<Hardware> _hardware = new();

    public NvidiaGroup(ISettings settings)
    {
        if (!NvApi.IsAvailable)
            return;

        NvApi.NvPhysicalGpuHandle[] handles = new NvApi.NvPhysicalGpuHandle[NvApi.MAX_PHYSICAL_GPUS];
        if (NvApi.NvAPI_EnumPhysicalGPUs == null)
        {
            return;
        }

        NvApi.NvStatus status = NvApi.NvAPI_EnumPhysicalGPUs(handles, out int count);
        if (status != NvApi.NvStatus.OK)
        {
            return;
        }

        IDictionary<NvApi.NvPhysicalGpuHandle, NvApi.NvDisplayHandle> displayHandles = new Dictionary<NvApi.NvPhysicalGpuHandle, NvApi.NvDisplayHandle>();
        if (NvApi.NvAPI_EnumNvidiaDisplayHandle != null && NvApi.NvAPI_GetPhysicalGPUsFromDisplay != null)
        {
            status = NvApi.NvStatus.OK;
            int i = 0;
            while (status == NvApi.NvStatus.OK)
            {
                NvApi.NvDisplayHandle displayHandle = new();
                status = NvApi.NvAPI_EnumNvidiaDisplayHandle(i, ref displayHandle);
                i++;

                if (status == NvApi.NvStatus.OK)
                {
                    NvApi.NvPhysicalGpuHandle[] handlesFromDisplay = new NvApi.NvPhysicalGpuHandle[NvApi.MAX_PHYSICAL_GPUS];
                    if (NvApi.NvAPI_GetPhysicalGPUsFromDisplay(displayHandle, handlesFromDisplay, out uint countFromDisplay) == NvApi.NvStatus.OK)
                    {
                        for (int j = 0; j < countFromDisplay; j++)
                        {
                            if (!displayHandles.ContainsKey(handlesFromDisplay[j]))
                                displayHandles.Add(handlesFromDisplay[j], displayHandle);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            displayHandles.TryGetValue(handles[i], out NvApi.NvDisplayHandle displayHandle);
            _hardware.Add(new NvidiaGpu(i, handles[i], displayHandle, settings));
        }
    }

    public IReadOnlyList<IHardware> Hardware => _hardware;

    public void Close()
    {
        foreach (Hardware gpu in _hardware)
            gpu.Close();

        NvidiaML.Close();
    }
}