using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// Windows generic driver nvme access
/// </summary>
/// <seealso cref="INvmeDrive" />
internal class NvmeWindows : INvmeDrive
{
    /// <summary>
    /// Identifies the specified storage information.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <returns></returns>
    public SafeHandle Identify(StorageInfo storageInfo) => IdentifyDevice(storageInfo);

    /// <summary>
    /// Identifies the controller.
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public bool IdentifyController(SafeHandle hDevice, out NvmeIdentifyControllerData data)
    {
        data = Kernel32.CreateStruct<NvmeIdentifyControllerData>();
        if (hDevice?.IsInvalid != false) return false;

        bool result = false;
        var nptwb = Kernel32.CreateStruct<StorageQueryBuffer>();
        nptwb.ProtocolSpecific.ProtocolType = StorageProtocolType.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)StorageProtocolNvmeDataType.NvMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = (uint)StorageProtocolNvmeProtocolDataRequestValue.NvMeIdentifyCnsController;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<StorageProtocolSpecificData>();
        nptwb.ProtocolSpecific.ProtocolDataLength = (uint)nptwb.Buffer.Length;
        nptwb.PropertyId = StoragePropertyId.StorageAdapterProtocolSpecificProperty;
        nptwb.QueryType = StorageQueryType.PropertyStandardQuery;

        int length = Marshal.SizeOf<StorageQueryBuffer>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IoctlStorageQueryProperty, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            // Map NVME_IDENTIFY_CONTROLLER_DATA to nptwb.Buffer
            nint offset = Marshal.OffsetOf<StorageQueryBuffer>(nameof(StorageQueryBuffer.Buffer));
            var newPtr = nint.Add(buffer, offset.ToInt32());
            data = Marshal.PtrToStructure<NvmeIdentifyControllerData>(newPtr);
            Marshal.FreeHGlobal(buffer);
            result = true;
        }
        else
        {
            Marshal.FreeHGlobal(buffer);
        }

        return result;
    }

    /// <summary>
    /// Health information log.
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public bool HealthInfoLog(SafeHandle hDevice, out NvmeHealthInfoLog data)
    {
        data = Kernel32.CreateStruct<NvmeHealthInfoLog>();
        if (hDevice?.IsInvalid != false) return false;

        bool result = false;
        var nptwb = Kernel32.CreateStruct<StorageQueryBuffer>();
        nptwb.ProtocolSpecific.ProtocolType = StorageProtocolType.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)StorageProtocolNvmeDataType.NvMeDataTypeLogPage;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = (uint)NvmeLogPages.NvmeLogPageHealthInfo;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<StorageProtocolSpecificData>();
        nptwb.ProtocolSpecific.ProtocolDataLength = (uint)nptwb.Buffer.Length;
        nptwb.PropertyId = StoragePropertyId.StorageAdapterProtocolSpecificProperty;
        nptwb.QueryType = StorageQueryType.PropertyStandardQuery;

        int length = Marshal.SizeOf<StorageQueryBuffer>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IoctlStorageQueryProperty, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            // Map NVME_HEALTH_INFO_LOG to nptwb.Buffer
            nint offset = Marshal.OffsetOf<StorageQueryBuffer>(nameof(StorageQueryBuffer.Buffer));
            var newPtr = nint.Add(buffer, offset.ToInt32());
            data = Marshal.PtrToStructure<NvmeHealthInfoLog>(newPtr);
            Marshal.FreeHGlobal(buffer);
            result = true;
        }
        else
        {
            Marshal.FreeHGlobal(buffer);
        }

        return result;
    }

    /// <summary>
    /// Identifies the device.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <returns></returns>
    public static SafeHandle IdentifyDevice(StorageInfo storageInfo)
    {
        SafeFileHandle handle = Kernel32.OpenDevice(storageInfo.DeviceId);
        if (handle?.IsInvalid != false) return null;

        var nptwb = Kernel32.CreateStruct<StorageQueryBuffer>();
        nptwb.ProtocolSpecific.ProtocolType = StorageProtocolType.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)StorageProtocolNvmeDataType.NvMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = (uint)StorageProtocolNvmeProtocolDataRequestValue.NvMeIdentifyCnsController;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<StorageProtocolSpecificData>();
        nptwb.ProtocolSpecific.ProtocolDataLength = (uint)nptwb.Buffer.Length;
        nptwb.PropertyId = StoragePropertyId.StorageAdapterProtocolSpecificProperty;
        nptwb.QueryType = StorageQueryType.PropertyStandardQuery;

        int length = Marshal.SizeOf<StorageQueryBuffer>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);

        bool validTransfer = Kernel32.DeviceIoControl(handle, IoCtl.IoctlStorageQueryProperty, buffer, length, buffer, length, out _, nint.Zero);
        Marshal.FreeHGlobal(buffer);
        if (validTransfer) return handle;
        handle.Close();
        handle.Dispose();
        return null;
    }
}
