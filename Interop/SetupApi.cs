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
    private const string DllName = "SetupAPI.dll";
    internal const int DigcfDeviceinterface = 0x00000010;
    internal const int DigcfPresent = 0x00000002;
    internal const int ErrorInsufficientBuffer = 122;
    internal const int ErrorNoMoreItems = 259;
    internal static Guid GuidDeviceBattery = new(0x72631e54, 0x78A4, 0x11d0, 0xbc, 0xf7, 0x00, 0xaa, 0x00, 0xb7, 0xb3, 0x2a);
    internal static readonly IntPtr InvalidHandleValue = new(-1);

    /// <summary>
    /// The SetupDiGetClassDevs function returns a handle to a device information set that contains requested device information
    /// elements for a local computer.
    /// https://learn.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetclassdevsw
    /// </summary>
    /// <param name="classGuid">The class unique identifier.</param>
    /// <param name="enumerator">The enumerator.</param>
    /// <param name="hwndParent">The HWND parent.</param>
    /// <param name="flags">The flags.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, uint flags);

    /// <summary>
    /// The SetupDiEnumDeviceInterfaces function enumerates the device interfaces that are contained in a device information set.
    /// https://learn.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdienumdeviceinterfaces
    /// </summary>
    /// <param name="deviceInfoSet">The device information set.</param>
    /// <param name="deviceInfoData">The device information data.</param>
    /// <param name="interfaceClassGuid">The interface class unique identifier.</param>
    /// <param name="memberIndex">Index of the member.</param>
    /// <param name="deviceInterfaceData">The device interface data.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiEnumDeviceInterfaces
    (
        IntPtr deviceInfoSet, 
        IntPtr deviceInfoData, 
        ref Guid interfaceClassGuid, 
        uint memberIndex, 
        ref SpDeviceInterfaceData deviceInterfaceData);

    /// <summary>
    /// The SetupDiGetDeviceInterfaceDetail function returns details about a device interface.
    /// https://learn.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdigetdeviceinterfacedetaila
    /// </summary>
    /// <param name="deviceInfoSet">The device information set.</param>
    /// <param name="deviceInterfaceData">The device interface data.</param>
    /// <param name="deviceInterfaceDetailData">The device interface detail data.</param>
    /// <param name="deviceInterfaceDetailDataSize">Size of the device interface detail data.</param>
    /// <param name="requiredSize">Size of the required.</param>
    /// <param name="deviceInfoData">The device information data.</param>
    /// <returns></returns>
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

    /// <summary>
    /// The SetupDiDestroyDeviceInfoList function deletes a device information set and frees all associated memory.
    /// https://learn.microsoft.com/en-us/windows/win32/api/setupapi/nf-setupapi-setupdidestroydeviceinfolist
    /// </summary>
    /// <param name="deviceInfoSet">The device information set.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);
}
