// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System.IO;
using System.Linq;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Memory;

/// <summary>
/// Generic Linux Memory
/// </summary>
/// <seealso cref="Hardware" />
internal sealed class GenericLinuxMemory : Hardware
{
    private readonly Sensor _physicalMemoryAvailable;
    private readonly Sensor _physicalMemoryLoad;
    private readonly Sensor _physicalMemoryUsed;
    private readonly Sensor _virtualMemoryAvailable;
    private readonly Sensor _virtualMemoryLoad;
    private readonly Sensor _virtualMemoryUsed;

    public override HardwareType HardwareType => HardwareType.Memory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericLinuxMemory"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="settings">The settings.</param>
    public GenericLinuxMemory(string name, ISettings settings) : base(name, new Identifier("ram"), settings)
    {
        _physicalMemoryUsed = new Sensor("Memory Used", 0, SensorType.Data, this, settings);
        ActivateSensor(_physicalMemoryUsed);

        _physicalMemoryAvailable = new Sensor("Memory Available", 1, SensorType.Data, this, settings);
        ActivateSensor(_physicalMemoryAvailable);

        _physicalMemoryLoad = new Sensor("Memory", 0, SensorType.Load, this, settings);
        ActivateSensor(_physicalMemoryLoad);

        _virtualMemoryUsed = new Sensor("Virtual Memory Used", 2, SensorType.Data, this, settings);
        ActivateSensor(_virtualMemoryUsed);

        _virtualMemoryAvailable = new Sensor("Virtual Memory Available", 3, SensorType.Data, this, settings);
        ActivateSensor(_virtualMemoryAvailable);

        _virtualMemoryLoad = new Sensor("Virtual Memory", 1, SensorType.Load, this, settings);
        ActivateSensor(_virtualMemoryLoad);
    }

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override void Update()
    {
        try
        {
            string[] memoryInfo = File.ReadAllLines("/proc/meminfo");

            {
                float totalMemoryGb = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemTotal:"))) / 1024.0f / 1024.0f;
                float freeMemoryGb = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemFree:"))) / 1024.0f / 1024.0f;
                float cachedMemoryGb = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("Cached:"))) / 1024.0f / 1024.0f;

                float usedMemoryGb = totalMemoryGb - freeMemoryGb - cachedMemoryGb;

                _physicalMemoryUsed.Value = usedMemoryGb;
                _physicalMemoryAvailable.Value = totalMemoryGb;
                _physicalMemoryLoad.Value = 100.0f * (usedMemoryGb / totalMemoryGb);
            }
            {
                float totalSwapMemoryGb = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("SwapTotal"))) / 1024.0f / 1024.0f;
                float freeSwapMemoryGb = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("SwapFree"))) / 1024.0f / 1024.0f;
                float usedSwapMemoryGb = totalSwapMemoryGb - freeSwapMemoryGb;

                _virtualMemoryUsed.Value = usedSwapMemoryGb;
                _virtualMemoryAvailable.Value = totalSwapMemoryGb;
                _virtualMemoryLoad.Value = 100.0f * (usedSwapMemoryGb / totalSwapMemoryGb);
            }
        }
        catch
        {
            _physicalMemoryUsed.Value = null;
            _physicalMemoryAvailable.Value = null;
            _physicalMemoryLoad.Value = null;

            _virtualMemoryUsed.Value = null;
            _virtualMemoryAvailable.Value = null;
            _virtualMemoryLoad.Value = null;
        }
    }

    /// <summary>
    /// Gets the memory information value.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <returns></returns>
    private static long GetMemInfoValue(string line)
    {
        // Example: "MemTotal:       32849676 kB"

        string valueWithUnit = line.Split(':').Skip(1).First().Trim();
        string valueAsString = valueWithUnit.Split(' ').First();

        return long.Parse(valueAsString);
    }
}
