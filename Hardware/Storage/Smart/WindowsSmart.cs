using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;

/// <summary>
/// Windows Smart
/// </summary>
/// <seealso cref="ISmart" />
internal class WindowsSmart : ISmart
{
    #region Fields

    private readonly int _driveNumber;
    private readonly SafeHandle _handle;

    #endregion

    #region Properties

    /// <summary>
    /// Returns true if ... is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid => !_handle.IsInvalid && !_handle.IsClosed;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsSmart" /> class.
    /// </summary>
    /// <param name="number">The number.</param>
    public WindowsSmart(int number)
    {
        _driveNumber = number;
        _handle = Kernel32.CreateFile(@$"\\.\PhysicalDrive{number}", FileAccess.ReadWrite, FileShare.ReadWrite,
            nint.Zero, FileMode.Open, FileAttributes.Normal, nint.Zero);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public void Close()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Close();
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected void Dispose(bool disposing)
    {
        if (!disposing || _handle.IsClosed) return;
        _handle.Close();
    }

    /// <summary>
    /// Enables the smart.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">WindowsSmart</exception>
    public bool EnableSmart()
    {
        if (_handle.IsClosed)
        {
            throw new ObjectDisposedException(nameof(WindowsSmart));
        }

        var parameter = new SendCmdInParams
        {
            bDriveNumber = (byte)_driveNumber,
            irDriveRegs =
            {
                bFeaturesReg = SmartFeatures.EnableSmart,
                bCylLowReg = Kernel32.SmartLbaMid,
                bCylHighReg = Kernel32.SmartLbaHi,
                bCommandReg = AtaCommand.AtaSmart
            }
        };

        return Kernel32.DeviceIoControl(
            _handle,
            Dfp.DFP_SEND_DRIVE_COMMAND,
            ref parameter,
            Marshal.SizeOf(parameter),
            out SendCmdOutParams _,
            Marshal.SizeOf<SendCmdOutParams>(),
            out _,
            nint.Zero);
    }

    /// <summary>
    /// Reads the smart data.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">WindowsSmart</exception>
    public Interop.Models.Kernel32.SmartAttribute[] ReadSmartData()
    {
        if (_handle.IsClosed)
        {
            throw new ObjectDisposedException(nameof(WindowsSmart));
        }

        var parameter = new SendCmdInParams
        {
            bDriveNumber = (byte)_driveNumber,
            irDriveRegs = {
                bFeaturesReg = SmartFeatures.SmartReadData,
                bCylLowReg = Kernel32.SmartLbaMid,
                bCylHighReg = Kernel32.SmartLbaHi,
                bCommandReg = AtaCommand.AtaSmart
            }
        };

        var isValid = Kernel32.DeviceIoControl(_handle, Dfp.DFP_RECEIVE_DRIVE_DATA, ref parameter, Marshal.SizeOf(parameter),
                                                out AttributeCmdOutParams result, Marshal.SizeOf<AttributeCmdOutParams>(), out _, nint.Zero);
        return isValid ? result.Attributes : [];
    }

    /// <summary>
    /// Reads the smart thresholds.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">WindowsSmart</exception>
    public SmartThreshold[] ReadSmartThresholds()
    {
        if (_handle.IsClosed)
        {
            throw new ObjectDisposedException(nameof(WindowsSmart));
        }

        var parameter = new SendCmdInParams
        {
            bDriveNumber = (byte)_driveNumber,
            irDriveRegs = {
                bFeaturesReg = SmartFeatures.ReadThresholds,
                bCylLowReg = Kernel32.SmartLbaMid,
                bCylHighReg = Kernel32.SmartLbaHi,
                bCommandReg = AtaCommand.AtaSmart
            }
        };

        bool isValid = Kernel32.DeviceIoControl(_handle, Dfp.DFP_RECEIVE_DRIVE_DATA, ref parameter, Marshal.SizeOf(parameter),
                                                out ThresholdCmdOutParams result, Marshal.SizeOf<ThresholdCmdOutParams>(), out _, nint.Zero);

        return isValid ? result.Thresholds : [];
    }

    /// <summary>
    /// Reads the name and firmware revision.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">WindowsSmart</exception>
    public bool ReadNameAndFirmwareRevision(out string name, out string firmwareRevision)
    {
        if (_handle.IsClosed)
        {
            throw new ObjectDisposedException(nameof(WindowsSmart));
        }

        var parameter = new SendCmdInParams
        {
            bDriveNumber = (byte)_driveNumber,
            irDriveRegs = { bCommandReg = AtaCommand.AtaIdentifyDevice }
        };

        bool valid = Kernel32.DeviceIoControl(_handle, Dfp.DFP_RECEIVE_DRIVE_DATA, ref parameter, Marshal.SizeOf(parameter),
                                              out IdentifyCmdOutParams result, Marshal.SizeOf<IdentifyCmdOutParams>(), out _, nint.Zero);

        if (!valid)
        {
            name = null;
            firmwareRevision = null;
            return false;
        }

        name = GetString(result.Identify.ModelNumber);
        firmwareRevision = GetString(result.Identify.FirmwareRevision);
        return true;
    }

    /// <summary>
    /// Reads Smart health status of the drive
    /// </summary>
    /// <returns>True, if drive is healthy; False, if unhealthy; Null, if it cannot be read</returns>
    public bool? ReadSmartHealth()
    {
        if (_handle.IsClosed)
        {
            throw new ObjectDisposedException(nameof(WindowsSmart));
        }

        var parameter = new SendCmdInParams
        {
            bDriveNumber = (byte)_driveNumber,
            irDriveRegs = {
                bFeaturesReg = SmartFeatures.ReturnSmartStatus,
                bCylLowReg = Kernel32.SmartLbaMid,
                bCylHighReg = Kernel32.SmartLbaHi,
                bCommandReg = AtaCommand.AtaSmart
            }
        };

        bool isValid = Kernel32.DeviceIoControl(_handle, Dfp.DFP_SEND_DRIVE_COMMAND, ref parameter, Marshal.SizeOf(parameter),
                                                out StatusCmdOutParams result, Marshal.SizeOf<StatusCmdOutParams>(), out _, nint.Zero);
        if (!isValid) return null;

        return result.irDriveRegs.bCylHighReg switch
        {
            // reference: https://github.com/smartmontools/smartmontools/blob/master/smartmontools/atacmds.cpp
            Kernel32.SmartLbaHi when Kernel32.SmartLbaMid == result.irDriveRegs.bCylLowReg => true,
            Kernel32.SmartLbaHiExceeded when Kernel32.SmartLbaMidExceeded == result.irDriveRegs.bCylLowReg =>
                false,
            _ => null
        };
    }

    /// <summary>
    /// Gets the string.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <returns></returns>
    private static string GetString(IReadOnlyList<byte> bytes)
    {
        char[] chars = new char[bytes.Count];
        for (int i = 0; i < bytes.Count; i += 2)
        {
            chars[i] = (char)bytes[i + 1];
            chars[i + 1] = (char)bytes[i];
        }
        return new string(chars).Trim(' ', '\0');
    }

    #endregion
}
