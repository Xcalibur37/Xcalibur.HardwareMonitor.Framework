using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// Interop: Kernel32
/// </summary>
public class Kernel32
{
    #region Fields

    public const uint BatteryUnknownTime = 0xFFFFFFFF;
    
    public const int ErrorServiceAlreadyRunning = unchecked((int)0x80070420);
    public const int ErrorServiceExists = unchecked((int)0x80070431);

    public const string IntelNvMeMiniPortSignature1 = "NvmeMini";
    public const string IntelNvMeMiniPortSignature2 = "IntelNvm";

    public const uint Lptr = 0x0000 | 0x0040;

    public const int MaxDriveAttributes = 512;
    public const uint NvmePassThroughSrbIoCode = 0xe0002000;
    public const byte SmartLbaHi = 0xC2;
    public const byte SmartLbaHiExceeded = 0x2C;
    public const byte SmartLbaMid = 0x4F;
    public const byte SmartLbaMidExceeded = 0xF4;

    private const string DllName = "kernel32.dll";

    #endregion

    #region Methods

    /// <summary>
    /// Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream,
    /// directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.
    /// The function returns a handle that can be used to access the file or device for various types of I/O depending
    /// on the file or device and the flags and attributes specified.
    /// https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea
    /// </summary>
    /// <param name="lpFileName">Name of the lp file.</param>
    /// <param name="dwDesiredAccess">The dw desired access.</param>
    /// <param name="dwShareMode">The dw share mode.</param>
    /// <param name="lpSecurityAttributes">The lp security attributes.</param>
    /// <param name="dwCreationDisposition">The dw creation disposition.</param>
    /// <param name="dwFlagsAndAttributes">The dw flags and attributes.</param>
    /// <param name="hTemplateFile">The h template file.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern SafeFileHandle CreateFile
    (
        [MarshalAs(UnmanagedType.LPTStr)] string lpFileName,
        [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess,
        [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
        IntPtr lpSecurityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
        [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    /// <summary>
    /// Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory,
    /// physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe. The function returns a
    /// handle that can be used to access the file or device for various types of I/O depending on the file or device and the
    /// flags and attributes specified.
    /// https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea
    /// </summary>
    /// <param name="lpFileName">Name of the lp file.</param>
    /// <param name="dwDesiredAccess">The dw desired access.</param>
    /// <param name="dwShareMode">The dw share mode.</param>
    /// <param name="lpSecurityAttributes">The lp security attributes.</param>
    /// <param name="dwCreationDisposition">The dw creation disposition.</param>
    /// <param name="dwFlagsAndAttributes">The dw flags and attributes.</param>
    /// <param name="hTemplateFile">The h template file.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr CreateFile
    (
        string lpFileName,
        uint dwDesiredAccess,
        FileShare dwShareMode,
        IntPtr lpSecurityAttributes,
        FileMode dwCreationDisposition,
        FileAttributes dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    /// <summary>
    /// Create an instance from a struct with zero initialized memory arrays
    /// no need to init every inner array with the correct sizes
    /// </summary>
    /// <typeparam name="T">type of struct that is needed</typeparam>
    /// <returns></returns>
    public static T CreateStruct<T>()
    {
        int size = Marshal.SizeOf<T>();
        IntPtr ptr = Marshal.AllocHGlobal(size);
        RtlZeroMemory(ptr, size);
        T result = Marshal.PtrToStructure<T>(ptr);
        Marshal.FreeHGlobal(ptr);
        return result;
    }

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        Dfp dwIoControlCode,
        ref SendCmdInParams lpInBuffer,
        int nInBufferSize,
        out AttributeCmdOutParams lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        Dfp dwIoControlCode,
        ref SendCmdInParams lpInBuffer,
        int nInBufferSize,
        out ThresholdCmdOutParams lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        Dfp dwIoControlCode,
        ref SendCmdInParams lpInBuffer,
        int nInBufferSize,
        out SendCmdOutParams lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        Dfp dwIoControlCode,
        ref SendCmdInParams lpInBuffer,
        int nInBufferSize,
        out IdentifyCmdOutParams lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        Dfp dwIoControlCode,
        ref SendCmdInParams lpInBuffer,
        int nInBufferSize,
        out StatusCmdOutParams lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        IoCtl dwIoControlCode,
        ref StoragePropertyQuery lpInBuffer,
        int nInBufferSize,
        out StorageDeviceDescriptorHeader lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        IoCtl dwIoControlCode,
        ref StoragePropertyQuery lpInBuffer,
        int nInBufferSize,
        IntPtr lpOutBuffer,
        uint nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        IoCtl dwIoControlCode,
        IntPtr lpInBuffer,
        int nInBufferSize,
        IntPtr lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeHandle hDevice,
        IoCtl dwIoControlCode,
        IntPtr lpInBuffer,
        int nInBufferSize,
        out DiskPerformance lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeFileHandle hDevice,
        IoCtl dwIoControlCode,
        ref BatteryQueryInformation lpInBuffer,
        int nInBufferSize,
        ref BatteryInformation lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeFileHandle hDevice,
        IoCtl dwIoControlCode,
        ref uint lpInBuffer,
        int nInBufferSize,
        ref uint lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeFileHandle hDevice,
        IoCtl dwIoControlCode,
        ref BatteryWaitStatus lpInBuffer,
        int nInBufferSize,
        ref BatteryStatus lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeFileHandle hDevice,
        IoCtl dwIoControlCode,
        ref BatteryQueryInformation lpInBuffer,
        int nInBufferSize,
        IntPtr lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="dwIoControlCode">The dw io control code.</param>
    /// <param name="lpInBuffer">The lp in buffer.</param>
    /// <param name="nInBufferSize">Size of the n in buffer.</param>
    /// <param name="lpOutBuffer">The lp out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="lpBytesReturned">The lp bytes returned.</param>
    /// <param name="lpOverlapped">The lp overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeviceIoControl
    (
        SafeFileHandle hDevice,
        IoCtl dwIoControlCode,
        ref BatteryQueryInformation lpInBuffer,
        int nInBufferSize,
        ref uint lpOutBuffer,
        int nOutBufferSize,
        out uint lpBytesReturned,
        IntPtr lpOverlapped);

    /// <summary>
    /// Sends a control code directly to a specified device driver, causing the corresponding device to perform the corresponding operation.
    /// https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
    /// </summary>
    /// <param name="device">The device.</param>
    /// <param name="ioControlCode">The io control code.</param>
    /// <param name="inBuffer">The in buffer.</param>
    /// <param name="inBufferSize">Size of the in buffer.</param>
    /// <param name="outBuffer">The out buffer.</param>
    /// <param name="nOutBufferSize">Size of the n out buffer.</param>
    /// <param name="bytesReturned">The bytes returned.</param>
    /// <param name="overlapped">The overlapped.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool DeviceIoControl
    (
        SafeFileHandle device,
        IoControlCode ioControlCode,
        [MarshalAs(UnmanagedType.AsAny)][In] object inBuffer,
        uint inBufferSize,
        [MarshalAs(UnmanagedType.AsAny)][Out] object outBuffer,
        uint nOutBufferSize,
        out uint bytesReturned,
        IntPtr overlapped);

    /// <summary>
    /// Enumerates all system firmware tables of the specified type.
    /// https://learn.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-enumsystemfirmwaretables
    /// </summary>
    /// <param name="firmwareTableProviderSignature">The firmware table provider signature.</param>
    /// <param name="firmwareTableBuffer">The firmware table buffer.</param>
    /// <param name="bufferSize">Size of the buffer.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int EnumSystemFirmwareTables(Provider firmwareTableProviderSignature, IntPtr firmwareTableBuffer, int bufferSize);

    /// <summary>
    /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
    /// When the reference count reaches zero, the module is unloaded from the address space of the calling process
    /// and the handle is no longer valid.
    /// https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-freelibrary
    /// </summary>
    /// <param name="module">The module.</param>
    /// <returns></returns>
    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr module);


    /// <summary>
    /// Returns the number of active processor groups in the system.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-getactiveprocessorgroupcount
    /// </summary>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern ushort GetActiveProcessorGroupCount();

    /// <summary>
    /// Retrieves a pseudo handle for the calling thread.
    /// https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getcurrentthread
    /// </summary>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr GetCurrentThread();

    /// <summary>
    /// Retrieves the address of an exported function (also known as a procedure) or variable from the specified dynamic-link library (DLL).
    /// https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getprocaddress
    /// </summary>
    /// <param name="module">The module.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <returns></returns>
    [DllImport(DllName, ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr GetProcAddress(IntPtr module, string methodName);

    /// <summary>
    /// Retrieves the specified firmware table from the firmware table provider.
    /// https://learn.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-getsystemfirmwaretable
    /// </summary>
    /// <param name="firmwareTableProviderSignature">The firmware table provider signature.</param>
    /// <param name="firmwareTableId">The firmware table identifier.</param>
    /// <param name="firmwareTableBuffer">The firmware table buffer.</param>
    /// <param name="bufferSize">Size of the buffer.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int GetSystemFirmwareTable(Provider firmwareTableProviderSignature, int firmwareTableId, IntPtr firmwareTableBuffer, int bufferSize);

    /// <summary>
    /// Retrieves information about the system's current usage of both physical and virtual memory.
    /// https://learn.microsoft.com/en-us/windows/win32/api/sysinfoapi/nf-sysinfoapi-globalmemorystatusex
    /// </summary>
    /// <param name="lpBuffer">The lp buffer.</param>
    /// <returns></returns>
    [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);

    /// <summary>
    /// Allocates the specified number of bytes from the heap.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-localalloc
    /// </summary>
    /// <param name="uFlags">The u flags.</param>
    /// <param name="uBytes">The u bytes.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LocalAlloc(uint uFlags, ulong uBytes);

    /// <summary>
    /// Frees the specified local memory object and invalidates its handle.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-localfree
    /// </summary>
    /// <param name="hMem">The h memory.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LocalFree(IntPtr hMem);

    /// <summary>
    /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
    /// https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibrarya
    /// </summary>
    /// <param name="lpFileName">Name of the lp file.</param>
    /// <returns></returns>
    [DllImport(DllName, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LoadLibrary(string lpFileName);

    /// <summary>
    /// Opens the device.
    /// </summary>
    /// <param name="devicePath">The device path.</param>
    /// <returns></returns>
    public static SafeFileHandle OpenDevice(string devicePath)
    {
        SafeFileHandle hDevice = CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);
        if (hDevice.IsInvalid || hDevice.IsClosed)
        {
            hDevice = null;
        }

        return hDevice;
    }

    /// <summary>
    /// The RtlCopyMemory routine copies the contents of a source memory block to a destination memory block.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/nf-wdm-rtlcopymemory
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="source">The source.</param>
    /// <param name="length">The length.</param>
    [DllImport(DllName, SetLastError = false)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern void RtlCopyMemory(IntPtr destination, IntPtr source, uint length);

    /// <summary>
    /// The RtlZeroMemory routine fills a block of memory with zeros, given a pointer to the block and the length, in bytes, to be filled.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/nf-wdm-rtlzeromemory
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="length">The length.</param>
    [DllImport(DllName, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern void RtlZeroMemory(IntPtr destination, int length);

    /// <summary>
    /// Sets the processor group affinity for the specified thread.
    /// https://learn.microsoft.com/en-us/windows/win32/api/processtopologyapi/nf-processtopologyapi-setthreadgroupaffinity
    /// </summary>
    /// <param name="thread">The thread.</param>
    /// <param name="groupAffinity">The group affinity.</param>
    /// <param name="previousGroupAffinity">The previous group affinity.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool SetThreadGroupAffinity(IntPtr thread, ref GroupAffinity groupAffinity, out GroupAffinity previousGroupAffinity);

    /// <summary>
    /// Reserves, commits, or changes the state of a region of pages in the virtual address space of the calling process.
    /// Memory allocated by this function is automatically initialized to zero.
    /// https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualalloc
    /// </summary>
    /// <param name="lpAddress">The lp address.</param>
    /// <param name="dwSize">Size of the dw.</param>
    /// <param name="flAllocationType">Type of the fl allocation.</param>
    /// <param name="flProtect">The fl protect.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, Mem flAllocationType, Page flProtect);

    /// <summary>
    /// Releases, decommits, or releases and decommits a region of pages within the virtual address space of the calling process.
    /// https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualfree
    /// </summary>
    /// <param name="lpAddress">The lp address.</param>
    /// <param name="dwSize">Size of the dw.</param>
    /// <param name="dwFreeType">Type of the dw free.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, Mem dwFreeType);

    #endregion
}
