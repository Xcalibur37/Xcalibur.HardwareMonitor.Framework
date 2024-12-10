using System;
using System.Collections.Generic;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.Intel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.D3D;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.Intel;

/// <summary>
/// Intel GPU Group
/// </summary>
/// <seealso cref="IGroup" />
internal class IntelGpuGroup : IGroup
{
    private readonly List<Hardware> _hardware = [];

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _hardware;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntelGpuGroup"/> class.
    /// </summary>
    /// <param name="intelCpus">The intel cpus.</param>
    /// <param name="settings">The settings.</param>
    public IntelGpuGroup(IntelCpu[] intelCpus, ISettings settings)
    {
        if (Software.OperatingSystem.IsUnix || intelCpus.Length == 0) return;
        foreach (var deviceId in D3dDisplayDevice.GetDeviceIdentifiers())
        {
            var isIntel = deviceId.IndexOf(GpuConstants.IntelManagementEngineInterface, StringComparison.Ordinal) != -1;
            if (!isIntel || 
                !D3dDisplayDevice.GetDeviceInfoByIdentifier(deviceId, out var deviceInfo) || 
                !deviceInfo.Integrated) continue;

            // It may seem strange to only use the first cpu here, but in-case we have a multi cpu system with integrated graphics (does that exist?),
            // we would pick up the multiple device identifiers above and would add one instance for each CPU.
            _hardware.Add(new IntelIntegratedGpu(intelCpus[0], deviceId, deviceInfo, settings));
        }
    }
    
    /// <summary>
    /// Stop updating this group in the future.
    /// </summary>
    public void Close()
    {
        _hardware.Apply(x => x.Close());
    }
}