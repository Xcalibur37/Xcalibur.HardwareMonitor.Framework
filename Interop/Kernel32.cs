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
    public const int ErrorServiceAlreadyRunning = unchecked((int)0x80070420);

    public const int ErrorServiceExists = unchecked((int)0x80070431);

    public const uint BatteryUnknownTime = 0xFFFFFFFF;
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

    /// <summary>
    /// Create a instance from a struct with zero initialized memory arrays
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

    public static SafeFileHandle OpenDevice(string devicePath)
    {
        SafeFileHandle hDevice = CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);
        if (hDevice.IsInvalid || hDevice.IsClosed)
        {
            hDevice = null;
        }

        return hDevice;
    }

    [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);

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

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LocalAlloc(uint uFlags, ulong uBytes);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LocalFree(IntPtr hMem);

    [DllImport(DllName, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern void RtlZeroMemory(IntPtr destination, int length);

    [DllImport(DllName, SetLastError = false)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern void RtlCopyMemory(IntPtr destination, IntPtr source, uint length);

    [DllImport(DllName, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport(DllName, ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr GetProcAddress(IntPtr module, string methodName);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr module);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr GetCurrentThread();

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern ushort GetActiveProcessorGroupCount();

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool SetThreadGroupAffinity(IntPtr thread, ref GroupAffinity groupAffinity, out GroupAffinity previousGroupAffinity);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, Mem flAllocationType, Page flProtect);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, Mem dwFreeType);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern bool DeviceIoControl
    (
        SafeFileHandle device,
        IoControlCode ioControlCode,
        [MarshalAs(UnmanagedType.AsAny)] [In] object inBuffer,
        uint inBufferSize,
        [MarshalAs(UnmanagedType.AsAny)] [Out] object outBuffer,
        uint nOutBufferSize,
        out uint bytesReturned,
        IntPtr overlapped);

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

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int EnumSystemFirmwareTables(Provider firmwareTableProviderSignature, IntPtr firmwareTableBuffer, int bufferSize);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern int GetSystemFirmwareTable(Provider firmwareTableProviderSignature, int firmwareTableId, IntPtr firmwareTableBuffer, int bufferSize);
}
