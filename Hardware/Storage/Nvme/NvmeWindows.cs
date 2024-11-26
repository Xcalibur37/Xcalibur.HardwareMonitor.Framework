using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;

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
    public bool IdentifyController(SafeHandle hDevice, out Kernel32.NVME_IDENTIFY_CONTROLLER_DATA data)
    {
        data = Kernel32.CreateStruct<Kernel32.NVME_IDENTIFY_CONTROLLER_DATA>();
        if (hDevice?.IsInvalid != false) return false;

        bool result = false;
        Kernel32.STORAGE_QUERY_BUFFER nptwb = Kernel32.CreateStruct<Kernel32.STORAGE_QUERY_BUFFER>();
        nptwb.ProtocolSpecific.ProtocolType = Kernel32.STORAGE_PROTOCOL_TYPE.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)Kernel32.STORAGE_PROTOCOL_NVME_DATA_TYPE.NVMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = (uint)Kernel32.STORAGE_PROTOCOL_NVME_PROTOCOL_DATA_REQUEST_VALUE.NVMeIdentifyCnsController;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<Kernel32.STORAGE_PROTOCOL_SPECIFIC_DATA>();
        nptwb.ProtocolSpecific.ProtocolDataLength = (uint)nptwb.Buffer.Length;
        nptwb.PropertyId = Kernel32.STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty;
        nptwb.QueryType = Kernel32.STORAGE_QUERY_TYPE.PropertyStandardQuery;

        int length = Marshal.SizeOf<Kernel32.STORAGE_QUERY_BUFFER>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            // Map NVME_IDENTIFY_CONTROLLER_DATA to nptwb.Buffer
            nint offset = Marshal.OffsetOf<Kernel32.STORAGE_QUERY_BUFFER>(nameof(Kernel32.STORAGE_QUERY_BUFFER.Buffer));
            var newPtr = nint.Add(buffer, offset.ToInt32());
            data = Marshal.PtrToStructure<Kernel32.NVME_IDENTIFY_CONTROLLER_DATA>(newPtr);
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
    public bool HealthInfoLog(SafeHandle hDevice, out Kernel32.NVME_HEALTH_INFO_LOG data)
    {
        data = Kernel32.CreateStruct<Kernel32.NVME_HEALTH_INFO_LOG>();
        if (hDevice?.IsInvalid != false) return false;

        bool result = false;
        Kernel32.STORAGE_QUERY_BUFFER nptwb = Kernel32.CreateStruct<Kernel32.STORAGE_QUERY_BUFFER>();
        nptwb.ProtocolSpecific.ProtocolType = Kernel32.STORAGE_PROTOCOL_TYPE.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)Kernel32.STORAGE_PROTOCOL_NVME_DATA_TYPE.NVMeDataTypeLogPage;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = (uint)Kernel32.NVME_LOG_PAGES.NVME_LOG_PAGE_HEALTH_INFO;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<Kernel32.STORAGE_PROTOCOL_SPECIFIC_DATA>();
        nptwb.ProtocolSpecific.ProtocolDataLength = (uint)nptwb.Buffer.Length;
        nptwb.PropertyId = Kernel32.STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty;
        nptwb.QueryType = Kernel32.STORAGE_QUERY_TYPE.PropertyStandardQuery;

        int length = Marshal.SizeOf<Kernel32.STORAGE_QUERY_BUFFER>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            // Map NVME_HEALTH_INFO_LOG to nptwb.Buffer
            nint offset = Marshal.OffsetOf<Kernel32.STORAGE_QUERY_BUFFER>(nameof(Kernel32.STORAGE_QUERY_BUFFER.Buffer));
            var newPtr = nint.Add(buffer, offset.ToInt32());
            data = Marshal.PtrToStructure<Kernel32.NVME_HEALTH_INFO_LOG>(newPtr);
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

        Kernel32.STORAGE_QUERY_BUFFER nptwb = Kernel32.CreateStruct<Kernel32.STORAGE_QUERY_BUFFER>();
        nptwb.ProtocolSpecific.ProtocolType = Kernel32.STORAGE_PROTOCOL_TYPE.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)Kernel32.STORAGE_PROTOCOL_NVME_DATA_TYPE.NVMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = (uint)Kernel32.STORAGE_PROTOCOL_NVME_PROTOCOL_DATA_REQUEST_VALUE.NVMeIdentifyCnsController;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<Kernel32.STORAGE_PROTOCOL_SPECIFIC_DATA>();
        nptwb.ProtocolSpecific.ProtocolDataLength = (uint)nptwb.Buffer.Length;
        nptwb.PropertyId = Kernel32.STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty;
        nptwb.QueryType = Kernel32.STORAGE_QUERY_TYPE.PropertyStandardQuery;

        int length = Marshal.SizeOf<Kernel32.STORAGE_QUERY_BUFFER>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(handle, Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            Marshal.FreeHGlobal(buffer);
        }
        else
        {
            Marshal.FreeHGlobal(buffer);
            handle.Close();
            handle = null;
        }

        return handle;
    }
}
