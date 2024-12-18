﻿using System.Diagnostics;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.Gigabyte;

/// <summary>
/// ITE 879X EC I/O Port
/// </summary>
internal class IT879xEcIoPort
{
    #region Fields

    private const long WAIT_TIMEOUT = 1000L;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the register port.
    /// </summary>
    /// <value>
    /// The register port.
    /// </value>
    public ushort RegisterPort { get; }

    /// <summary>
    /// Gets the value port.
    /// </summary>
    /// <value>
    /// The value port.
    /// </value>
    public ushort ValuePort { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IT879xEcIoPort"/> class.
    /// </summary>
    /// <param name="registerPort">The register port.</param>
    /// <param name="valuePort">The value port.</param>
    public IT879xEcIoPort(ushort registerPort, ushort valuePort)
    {
        RegisterPort = registerPort;
        ValuePort = valuePort;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Reads the specified offset.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool Read(ushort offset, out byte value)
    {
        if (Init(0xB0, offset)) return ReadFromValue(out value);
        value = 0;
        return false;
    }

    /// <summary>
    /// Writes the specified offset.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool Write(ushort offset, byte value) => Init(0xB1, offset) && WriteToValue(value);

    /// <summary>
    /// Initializes the specified command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="offset">The offset.</param>
    /// <returns></returns>
    private bool Init(byte command, ushort offset)
    {
        if (!WriteToRegister(command)) return false;
        if (!WriteToValue((byte)(offset >> 8 & 0xFF))) return false;
        if (!WriteToValue((byte)(offset & 0xFF))) return false;
        return true;
    }

    /// <summary>
    /// Writes to register.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private bool WriteToRegister(byte value)
    {
        if (!WaitIbe()) return false;
        Ring0.WriteIoPort(RegisterPort, value);
        return true;
    }

    /// <summary>
    /// Writes to value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private bool WriteToValue(byte value)
    {
        if (!WaitIbe()) return false;
        Ring0.WriteIoPort(ValuePort, value);
        return true;
    }

    /// <summary>
    /// Reads from value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private bool ReadFromValue(out byte value)
    {
        if (WaitObf())
        {
            value = Ring0.ReadIoPort(ValuePort);
            return true;
        }

        value = 0;
        return false;
    }

    /// <summary>
    /// Waits the ibe.
    /// </summary>
    /// <returns></returns>
    private bool WaitIbe()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            while ((Ring0.ReadIoPort(RegisterPort) & 2) != 0)
            {
                if (stopwatch.ElapsedMilliseconds <= WAIT_TIMEOUT) continue;
                return false;
            }
            return true;
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    /// <summary>
    /// Waits the obf.
    /// </summary>
    /// <returns></returns>
    private bool WaitObf()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            while ((Ring0.ReadIoPort(RegisterPort) & 1) == 0)
            {
                if (stopwatch.ElapsedMilliseconds <= WAIT_TIMEOUT) continue;
                return false;
            }
            return true;
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    #endregion
}
