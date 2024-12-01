using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
/// <summary>
/// This header is used by Device and Driver Installation Reference.
/// https://learn.microsoft.com/en-us/windows/win32/api/cfgmgr32/
/// </summary>
internal static class CfgMgr32
{
    internal const uint CM_GET_DEVICE_INTERFACE_LIST_PRESENT = 0;
    internal const int CR_SUCCESS = 0;

    internal const string DllName = "CfgMgr32.dll";
    internal static Guid GUID_DISPLAY_DEVICE_ARRIVAL = new("1CA05180-A699-450A-9A0C-DE4FBE3DDD89");

    /// <summary>
    /// The CM_Get_Device_Interface_List_Size function retrieves the buffer size that must be passed to the
    /// CM_Get_Device_Interface_List function.
    /// https://learn.microsoft.com/en-us/windows/win32/api/cfgmgr32/nf-cfgmgr32-cm_get_device_interface_list_sizea
    /// </summary>
    /// <param name="size">The size.</param>
    /// <param name="interfaceClassGuid">The interface class unique identifier.</param>
    /// <param name="deviceID">The device identifier.</param>
    /// <param name="flags">The flags.</param>
    /// <returns></returns>
    [DllImport(DllName, CharSet = CharSet.Unicode)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint CM_Get_Device_Interface_List_Size(out uint size, ref Guid interfaceClassGuid, string deviceID, uint flags);

    /// <summary>
    /// The CM_Get_Device_Interface_List function retrieves a list of device interface instances that belong to a
    /// specified device interface class.
    /// https://learn.microsoft.com/en-us/windows/win32/api/cfgmgr32/nf-cfgmgr32-cm_get_device_interface_lista
    /// </summary>
    /// <param name="interfaceClassGuid">The interface class unique identifier.</param>
    /// <param name="deviceID">The device identifier.</param>
    /// <param name="buffer">The buffer.</param>
    /// <param name="bufferLength">Length of the buffer.</param>
    /// <param name="flags">The flags.</param>
    /// <returns></returns>
    [DllImport(DllName, CharSet = CharSet.Unicode)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint CM_Get_Device_Interface_List(ref Guid interfaceClassGuid, string deviceID, char[] buffer, uint bufferLength, uint flags);
}