using System;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// Windows NT Setup and Device Installer services DLL.
/// https://learn.microsoft.com/en-us/windows/win32/api/setupapi/
/// </summary>
internal class SetupApi
{
    internal const int DigcfDeviceinterface = 0x00000010;
    internal const int DigcfPresent = 0x00000002;
    internal const int ErrorInsufficientBuffer = 122;
    internal const int ErrorNoMoreItems = 259;

    private const string DllName = "SetupAPI.dll";
    internal static Guid GuidDeviceBattery = new(0x72631e54, 0x78A4, 0x11d0, 0xbc, 0xf7, 0x00, 0xaa, 0x00, 0xb7, 0xb3, 0x2a);
    internal static readonly IntPtr InvalidHandleValue = new(-1);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, uint flags);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiEnumDeviceInterfaces
        (IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, uint memberIndex, ref SpDeviceInterfaceData deviceInterfaceData);

    [DllImport(DllName, SetLastError = true, EntryPoint = "SetupDiGetDeviceInterfaceDetailW", CharSet = CharSet.Unicode)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiGetDeviceInterfaceDetail
    (
        IntPtr deviceInfoSet,
        in SpDeviceInterfaceData deviceInterfaceData,
        [Out, Optional] IntPtr deviceInterfaceDetailData,
        uint deviceInterfaceDetailDataSize,
        out uint requiredSize,
        IntPtr deviceInfoData = default);

    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);
}
