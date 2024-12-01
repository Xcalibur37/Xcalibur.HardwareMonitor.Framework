using System.Collections.Generic;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Display;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.GPU;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.Nvidia;

/// <summary>
/// Nvidia GPU Group
/// </summary>
/// <seealso cref="IGroup" />
internal class NvidiaGroup : IGroup
{
    private readonly List<Hardware> _hardware = [];

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _hardware;

    /// <summary>
    /// Initializes a new instance of the <see cref="NvidiaGroup"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public NvidiaGroup(ISettings settings)
    {
        if (!NvApi.IsAvailable) return;

        // Get physical GPU's
        var handles = new NvPhysicalGpuHandle[NvApi.MAX_PHYSICAL_GPUS];
        if (NvApi.NvAPI_EnumPhysicalGPUs == null) return;

        // Get status
        NvStatus status = NvApi.NvAPI_EnumPhysicalGPUs(handles, out int count);
        if (status != NvStatus.OK) return;

        // Get display handles
        var displayHandles = new Dictionary<NvPhysicalGpuHandle, NvDisplayHandle>();
        if (NvApi.NvAPI_EnumNvidiaDisplayHandle != null && NvApi.NvAPI_GetPhysicalGPUsFromDisplay != null)
        {
            status = NvStatus.OK;
            int i = 0;

            // Evaluate display handles
            while (status == NvStatus.OK)
            {
                // Current display handle
                NvDisplayHandle displayHandle = new();
                status = NvApi.NvAPI_EnumNvidiaDisplayHandle(i, ref displayHandle);
                i++;

                // If not "OK" skip
                if (status != NvStatus.OK) continue;

                // Display handles
                var handlesFromDisplay = new NvPhysicalGpuHandle[NvApi.MAX_PHYSICAL_GPUS];
                if (NvApi.NvAPI_GetPhysicalGPUsFromDisplay(displayHandle, handlesFromDisplay, out uint countFromDisplay) != NvStatus.OK) continue;
                for (int j = 0; j < countFromDisplay; j++)
                {
                    if (displayHandles.ContainsKey(handlesFromDisplay[j])) continue;
                    displayHandles.Add(handlesFromDisplay[j], displayHandle);
                }
            }
        }

        // Add each Nvidia GPU
        for (int i = 0; i < count; i++)
        {
            displayHandles.TryGetValue(handles[i], out NvDisplayHandle displayHandle);
            _hardware.Add(new NvidiaGpu(i, handles[i], displayHandle, settings));
        }
    }

    /// <summary>
    /// Close open devices.
    /// </summary>
    public void Close()
    {
        _hardware.Apply(x => x.Close());
        NvidiaMl.Close();
    }
}