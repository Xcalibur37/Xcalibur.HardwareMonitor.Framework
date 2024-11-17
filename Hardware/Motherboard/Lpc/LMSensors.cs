using System;
using System.Collections.Generic;
using System.IO;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo;

// ReSharper disable once InconsistentNaming

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc;

/// <summary>
/// LM Sensors
/// </summary>
internal class LMSensors
{
    private const string HwMonPath = "/sys/class/hwmon/";
    private readonly List<ISuperIo> _superIOs = [];

    /// <summary>
    /// Gets the super io.
    /// </summary>
    /// <value>
    /// The super io.
    /// </value>
    public IReadOnlyList<ISuperIo> SuperIo => _superIOs;

    /// <summary>
    /// Initializes a new instance of the <see cref="LMSensors"/> class.
    /// </summary>
    public LMSensors()
    {
        if (!Directory.Exists(HwMonPath)) return;

        foreach (string basePath in Directory.GetDirectories(HwMonPath))
        {
            foreach (var devicePath in new[] { "/device", string.Empty })
            {
                var path = basePath + devicePath;
                string name = null;

                try
                {
                    using StreamReader reader = new(path + "/name");
                    name = reader.ReadLine();
                }
                catch (IOException)
                {
                    // Do nothing
                }

                if (name is null) return;
                Enum.Parse<Chip>(name.ToUpper());
                _superIOs.Add(new LmChip(Chip.ATK0110, path));
            }
        }
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public void Close()
    {
        foreach (ISuperIo superIo in _superIOs)
        {
            if (superIo is not LmChip lmChip) continue;
            lmChip.Close();
        }
    }
}
