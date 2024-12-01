using System;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// Advapi32.dll is an advanced Windows 32 base API DLL file; it is an API services library that supports security and registry calls.
/// https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2003/cc738291(v=ws.10)
/// </summary>
internal class AdvApi32
{
    private const string DllName = "advapi32.dll";

    [DllImport(DllName, SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, ScManagerAccessMask dwDesiredAccess);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool CloseServiceHandle(IntPtr hSCObject);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern IntPtr CreateService
    (
        IntPtr hSCManager,
        string lpServiceName,
        string lpDisplayName,
        ServiceAccessMask dwDesiredAccess,
        ServiceType dwServiceType,
        ServiceStart dwStartType,
        ServiceError dwErrorControl,
        string lpBinaryPathName,
        string lpLoadOrderGroup,
        string lpdwTagId,
        string lpDependencies,
        string lpServiceStartName,
        string lpPassword);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, ServiceAccessMask dwDesiredAccess);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeleteService(IntPtr hService);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool StartService(IntPtr hService, uint dwNumServiceArgs, string[] lpServiceArgVectors);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool ControlService(IntPtr hService, ServiceControl dwControl, ref ServiceStatus lpServiceStatus);
}
