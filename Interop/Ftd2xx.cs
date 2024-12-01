using System;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.FTDI;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// FTDI Chip D2XX Drivers
/// </summary>
internal static class Ftd2Xx
{
    private const string DllName = "Ftd2xx.dll";

    /// <summary>
    /// Checks if the DLL exists.
    /// </summary>
    /// <returns></returns>
    public static bool DllExists()
    {
        IntPtr module = Kernel32.LoadLibrary(DllName);
        if (module == IntPtr.Zero) return false;
        Kernel32.FreeLibrary(module);
        return true;
    }

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_CreateDeviceInfoList(out uint numDevices);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_GetDeviceInfoList([Out] FtDeviceInfoNode[] deviceInfoNodes, ref uint length);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_Open(int device, out FtHandle handle);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_Close(FtHandle handle);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_SetBaudRate(FtHandle handle, uint baudRate);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_SetDataCharacteristics(FtHandle handle, byte wordLength, byte stopBits, byte parity);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_SetFlowControl(FtHandle handle, FtFlowControl flowControl, byte xon, byte xoff);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_SetTimeouts(FtHandle handle, uint readTimeout, uint writeTimeout);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_Write(FtHandle handle, byte[] buffer, uint bytesToWrite, out uint bytesWritten);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_Purge(FtHandle handle, FtPurge mask);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_GetStatus(FtHandle handle, out uint amountInRxQueue, out uint amountInTxQueue, out uint eventStatus);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_Read(FtHandle handle, [Out] byte[] buffer, uint bytesToRead, out uint bytesReturned);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern FtStatus FT_ReadByte(FtHandle handle, out byte buffer, uint bytesToRead, out uint bytesReturned);

    /// <summary>
    /// Writes the specified handle.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    public static FtStatus Write(FtHandle handle, byte[] buffer)
    {
        FtStatus status = FT_Write(handle, buffer, (uint)buffer.Length, out uint bytesWritten);
        return bytesWritten != buffer.Length ? FtStatus.FtFailedToWriteDevice : status;
    }

    /// <summary>
    /// Bytes to read.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    public static int BytesToRead(FtHandle handle)
    {
        return FT_GetStatus(handle, out uint amountInRxQueue, out uint _, out uint _) == FtStatus.FtOk
            ? (int)amountInRxQueue
            : 0;
    }

    /// <summary>
    /// Reads the byte.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public static byte ReadByte(FtHandle handle)
    {
        FtStatus status = FT_ReadByte(handle, out byte buffer, 1, out uint bytesReturned);
        return status != FtStatus.FtOk || bytesReturned != 1 ? throw new InvalidOperationException() : buffer;
    }

    /// <summary>
    /// Reads the specified handle.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <param name="buffer">The buffer.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    public static void Read(FtHandle handle, byte[] buffer)
    {
        FtStatus status = FT_Read(handle, buffer, (uint)buffer.Length, out uint bytesReturned);
        if (status == FtStatus.FtOk && bytesReturned == buffer.Length) return;
        throw new InvalidOperationException();
    }
}