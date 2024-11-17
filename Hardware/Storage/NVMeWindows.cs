﻿




using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

internal class NVMeWindows : INVMeDrive
{
    //windows generic driver nvme access

    public SafeHandle Identify(StorageInfo storageInfo)
    {
        return IdentifyDevice(storageInfo);
    }

    public bool IdentifyController(SafeHandle hDevice, out Kernel32.NVME_IDENTIFY_CONTROLLER_DATA data)
    {
        data = Kernel32.CreateStruct<Kernel32.NVME_IDENTIFY_CONTROLLER_DATA>();
        if (hDevice?.IsInvalid != false)
            return false;

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
        IntPtr buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY, buffer, length, buffer, length, out _, IntPtr.Zero);
        if (validTransfer)
        {
            //map NVME_IDENTIFY_CONTROLLER_DATA to nptwb.Buffer
            IntPtr offset = Marshal.OffsetOf<Kernel32.STORAGE_QUERY_BUFFER>(nameof(Kernel32.STORAGE_QUERY_BUFFER.Buffer));
            var newPtr = IntPtr.Add(buffer, offset.ToInt32());
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

    public bool HealthInfoLog(SafeHandle hDevice, out Kernel32.NVME_HEALTH_INFO_LOG data)
    {
        data = Kernel32.CreateStruct<Kernel32.NVME_HEALTH_INFO_LOG>();
        if (hDevice?.IsInvalid != false)
            return false;

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
        IntPtr buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY, buffer, length, buffer, length, out _, IntPtr.Zero);
        if (validTransfer)
        {
            //map NVME_HEALTH_INFO_LOG to nptwb.Buffer
            IntPtr offset = Marshal.OffsetOf<Kernel32.STORAGE_QUERY_BUFFER>(nameof(Kernel32.STORAGE_QUERY_BUFFER.Buffer));
            var newPtr = IntPtr.Add(buffer, offset.ToInt32());
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

    public static SafeHandle IdentifyDevice(StorageInfo storageInfo)
    {
        SafeFileHandle handle = Kernel32.OpenDevice(storageInfo.DeviceId);
        if (handle?.IsInvalid != false)
            return null;

        Kernel32.STORAGE_QUERY_BUFFER nptwb = Kernel32.CreateStruct<Kernel32.STORAGE_QUERY_BUFFER>();
        nptwb.ProtocolSpecific.ProtocolType = Kernel32.STORAGE_PROTOCOL_TYPE.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)Kernel32.STORAGE_PROTOCOL_NVME_DATA_TYPE.NVMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = (uint)Kernel32.STORAGE_PROTOCOL_NVME_PROTOCOL_DATA_REQUEST_VALUE.NVMeIdentifyCnsController;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf<Kernel32.STORAGE_PROTOCOL_SPECIFIC_DATA>();
        nptwb.ProtocolSpecific.ProtocolDataLength = (uint)nptwb.Buffer.Length;
        nptwb.PropertyId = Kernel32.STORAGE_PROPERTY_ID.StorageAdapterProtocolSpecificProperty;
        nptwb.QueryType = Kernel32.STORAGE_QUERY_TYPE.PropertyStandardQuery;

        int length = Marshal.SizeOf<Kernel32.STORAGE_QUERY_BUFFER>();
        IntPtr buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(nptwb, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(handle, Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY, buffer, length, buffer, length, out _, IntPtr.Zero);
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
