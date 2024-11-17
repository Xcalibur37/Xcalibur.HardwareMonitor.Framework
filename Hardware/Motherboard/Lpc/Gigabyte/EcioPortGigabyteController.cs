using System;
using System.Threading;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.Gigabyte;

/// <summary>
/// EC I/O Port Gigabyte Controller
/// </summary>
/// <seealso cref="IGigabyteController" />
internal class EcIoPortGigabyteController : IGigabyteController
{
    #region Fields

    private const ushort ControllerVersionOffset = 0x00;
    private const ushort ControllerEnableRegister = 0x47;
    private const ushort ControllerFanControlArea = 0x900;
    private const ushort EcIoRegisterPort = 0x3F4;
    private const ushort EcIoValuePort = 0x3F0;
    private readonly IT879xEcIoPort _port;
    private bool? _initialState;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EcIoPortGigabyteController"/> class.
    /// </summary>
    /// <param name="port">The port.</param>
    private EcIoPortGigabyteController(IT879xEcIoPort port)
    {
        _port = port;
    }

    #endregion

    #region Methods

    /// <summary>
    /// TryCreate controller.
    /// </summary>
    /// <returns></returns>
    public static EcIoPortGigabyteController TryCreate()
    {
        IT879xEcIoPort port = new(EcIoRegisterPort, EcIoValuePort);

        // Check compatibility by querying its version.
        if (!port.Read(ControllerFanControlArea + ControllerVersionOffset, out byte majorVersion) || majorVersion != 1)
        {
            return null;
        }

        return new EcIoPortGigabyteController(port);
    }

    /// <summary>
    /// Enables the specified enabled.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <returns></returns>
    public bool Enable(bool enabled)
    {
        ushort offset = ControllerFanControlArea + ControllerEnableRegister;

        if (!_port.Read(offset, out byte bCurrent)) return false;
        var current = Convert.ToBoolean(bCurrent);
        _initialState ??= current;

        if (current == enabled) return true;
        if (!_port.Write(offset, Convert.ToByte(enabled))) return false;

        // Allow the system to catch up.
        Thread.Sleep(500);
        return true;
    }

    /// <summary>
    /// Restores this instance.
    /// </summary>
    public void Restore()
    {
        if (_initialState.HasValue)
        {
            Enable(_initialState.Value);
        }
    }

    #endregion
}
