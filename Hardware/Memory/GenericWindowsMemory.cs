using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Memory;

/// <summary>
/// Generic Windows Memory
/// </summary>
/// <seealso cref="Hardware" />
internal sealed class GenericWindowsMemory : Hardware
{
    private readonly Sensor _physicalMemoryAvailable;
    private readonly Sensor _physicalMemoryLoad;
    private readonly Sensor _physicalMemoryUsed;
    private readonly Sensor _virtualMemoryAvailable;
    private readonly Sensor _virtualMemoryLoad;
    private readonly Sensor _virtualMemoryUsed;

    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.Memory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericWindowsMemory"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="settings">The settings.</param>
    public GenericWindowsMemory(string name, ISettings settings) : base(name, new Identifier(HardwareConstants.RamIdentifier), settings)
    {
        _physicalMemoryUsed = new Sensor(MemoryConstants.MemoryUsed, 0, SensorType.Data, this, settings);
        ActivateSensor(_physicalMemoryUsed);

        _physicalMemoryAvailable = new Sensor(MemoryConstants.MemoryAvailable, 1, SensorType.Data, this, settings);
        ActivateSensor(_physicalMemoryAvailable);

        _physicalMemoryLoad = new Sensor(MemoryConstants.Memory, 0, SensorType.Load, this, settings);
        ActivateSensor(_physicalMemoryLoad);

        _virtualMemoryUsed = new Sensor(MemoryConstants.VirtualMemoryUsed, 2, SensorType.Data, this, settings);
        ActivateSensor(_virtualMemoryUsed);

        _virtualMemoryAvailable = new Sensor(MemoryConstants.VirtualMemoryAvailable, 3, SensorType.Data, this, settings);
        ActivateSensor(_virtualMemoryAvailable);

        _virtualMemoryLoad = new Sensor(MemoryConstants.VirtualMemory, 1, SensorType.Load, this, settings);
        ActivateSensor(_virtualMemoryLoad);
    }

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override void Update()
    {
        MemoryStatusEx status = new() { dwLength = (uint)Marshal.SizeOf<MemoryStatusEx>() };

        if (!Kernel32.GlobalMemoryStatusEx(ref status)) return;

        _physicalMemoryUsed.Value = (float)(status.ullTotalPhys - status.ullAvailPhys) / (1024 * 1024 * 1024);
        _physicalMemoryAvailable.Value = (float)status.ullAvailPhys / (1024 * 1024 * 1024);
        _physicalMemoryLoad.Value = 100.0f - ((100.0f * status.ullAvailPhys) / status.ullTotalPhys);

        _virtualMemoryUsed.Value = (float)(status.ullTotalPageFile - status.ullAvailPageFile) / (1024 * 1024 * 1024);
        _virtualMemoryAvailable.Value = (float)status.ullAvailPageFile / (1024 * 1024 * 1024);
        _virtualMemoryLoad.Value = 100.0f - ((100.0f * status.ullAvailPageFile) / status.ullTotalPageFile);
    }
}