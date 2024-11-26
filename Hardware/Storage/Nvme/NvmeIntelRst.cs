




using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// NVMe: Intel RST (raid) nvme access
/// </summary>
/// <seealso cref="INvmeDrive" />
internal class NvmeIntelRst : INvmeDrive
{
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

        Kernel32.NVME_PASS_THROUGH_IOCTL passThrough = Kernel32.CreateStruct<Kernel32.NVME_PASS_THROUGH_IOCTL>();
        passThrough.srb.HeaderLenght = (uint)Marshal.SizeOf<Kernel32.SRB_IO_CONTROL>();
        passThrough.srb.Signature = Encoding.ASCII.GetBytes(Kernel32.IntelNVMeMiniPortSignature2);
        passThrough.srb.Timeout = 10;
        passThrough.srb.ControlCode = Kernel32.NVME_PASS_THROUGH_SRB_IO_CODE;
        passThrough.srb.ReturnCode = 0;
        passThrough.srb.Length = (uint)Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>() - (uint)Marshal.SizeOf<Kernel32.SRB_IO_CONTROL>();
        passThrough.NVMeCmd[0] = (uint)Kernel32.STORAGE_PROTOCOL_NVME_DATA_TYPE.NVMeDataTypeLogPage; // GetLogPage
        passThrough.NVMeCmd[1] = 0xFFFFFFFF; // address
        passThrough.NVMeCmd[10] = 0x007f0002; // uint cdw10 = 0x000000002 | (((size / 4) - 1) << 16);
        passThrough.Direction = Kernel32.NVME_DIRECTION.NVME_FROM_DEV_TO_HOST;
        passThrough.QueueId = 0;
        passThrough.DataBufferLen = (uint)passThrough.DataBuffer.Length;
        passThrough.MetaDataLen = 0;
        passThrough.ReturnBufferLen = (uint)Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>();

        int length = Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(passThrough, buffer, false);

        bool validTransfer = Kernel32.DeviceIoControl(hDevice, Kernel32.IOCTL.IOCTL_SCSI_MINIPORT, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            nint offset = Marshal.OffsetOf<Kernel32.NVME_PASS_THROUGH_IOCTL>(nameof(Kernel32.NVME_PASS_THROUGH_IOCTL.DataBuffer));
            nint newPtr = nint.Add(buffer, offset.ToInt32());
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
    /// Identifies the specified storage information.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <returns></returns>
    public SafeHandle Identify(StorageInfo storageInfo) => NvmeWindows.IdentifyDevice(storageInfo);

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

        Kernel32.NVME_PASS_THROUGH_IOCTL passThrough = Kernel32.CreateStruct<Kernel32.NVME_PASS_THROUGH_IOCTL>();
        passThrough.srb.HeaderLenght = (uint)Marshal.SizeOf<Kernel32.SRB_IO_CONTROL>();
        passThrough.srb.Signature = Encoding.ASCII.GetBytes(Kernel32.IntelNVMeMiniPortSignature2);
        passThrough.srb.Timeout = 10;
        passThrough.srb.ControlCode = Kernel32.NVME_PASS_THROUGH_SRB_IO_CODE;
        passThrough.srb.ReturnCode = 0;
        passThrough.srb.Length = (uint)Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>() - (uint)Marshal.SizeOf<Kernel32.SRB_IO_CONTROL>();
        passThrough.NVMeCmd = new uint[16];
        passThrough.NVMeCmd[0] = 6; //identify
        passThrough.NVMeCmd[10] = 1; //return to host
        passThrough.Direction = Kernel32.NVME_DIRECTION.NVME_FROM_DEV_TO_HOST;
        passThrough.QueueId = 0;
        passThrough.DataBufferLen = (uint)passThrough.DataBuffer.Length;
        passThrough.MetaDataLen = 0;
        passThrough.ReturnBufferLen = (uint)Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>();

        int length = Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(passThrough, buffer, false);

        bool validTransfer = Kernel32.DeviceIoControl(hDevice, Kernel32.IOCTL.IOCTL_SCSI_MINIPORT, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            nint offset = Marshal.OffsetOf<Kernel32.NVME_PASS_THROUGH_IOCTL>(nameof(Kernel32.NVME_PASS_THROUGH_IOCTL.DataBuffer));
            nint newPtr = nint.Add(buffer, offset.ToInt32());
            int finalSize = Marshal.SizeOf<Kernel32.NVME_IDENTIFY_CONTROLLER_DATA>();
            nint ptr = Marshal.AllocHGlobal(Marshal.SizeOf<Kernel32.NVME_IDENTIFY_CONTROLLER_DATA>());
            Kernel32.RtlZeroMemory(ptr, finalSize);
            int len = Math.Min(finalSize, passThrough.DataBuffer.Length);
            Kernel32.RtlCopyMemory(ptr, newPtr, (uint)len);
            Marshal.FreeHGlobal(buffer);

            data = Marshal.PtrToStructure<Kernel32.NVME_IDENTIFY_CONTROLLER_DATA>(ptr);
            Marshal.FreeHGlobal(ptr);
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
        SafeFileHandle handle = Kernel32.OpenDevice(storageInfo.Scsi);
        if (handle?.IsInvalid != false) return null;

        Kernel32.NVME_PASS_THROUGH_IOCTL passThrough = Kernel32.CreateStruct<Kernel32.NVME_PASS_THROUGH_IOCTL>();
        passThrough.srb.HeaderLenght = (uint)Marshal.SizeOf<Kernel32.SRB_IO_CONTROL>();
        passThrough.srb.Signature = Encoding.ASCII.GetBytes(Kernel32.IntelNVMeMiniPortSignature2);
        passThrough.srb.Timeout = 10;
        passThrough.srb.ControlCode = Kernel32.NVME_PASS_THROUGH_SRB_IO_CODE;
        passThrough.srb.ReturnCode = 0;
        passThrough.srb.Length = (uint)Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>() - (uint)Marshal.SizeOf<Kernel32.SRB_IO_CONTROL>();
        passThrough.NVMeCmd = new uint[16];
        passThrough.NVMeCmd[0] = 6; //identify
        passThrough.NVMeCmd[10] = 1; //return to host
        passThrough.Direction = Kernel32.NVME_DIRECTION.NVME_FROM_DEV_TO_HOST;
        passThrough.QueueId = 0;
        passThrough.DataBufferLen = (uint)passThrough.DataBuffer.Length;
        passThrough.MetaDataLen = 0;
        passThrough.ReturnBufferLen = (uint)Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>();

        int length = Marshal.SizeOf<Kernel32.NVME_PASS_THROUGH_IOCTL>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(passThrough, buffer, false);

        bool validTransfer = Kernel32.DeviceIoControl(handle, Kernel32.IOCTL.IOCTL_SCSI_MINIPORT, buffer, length, buffer, length, out _, nint.Zero);
        Marshal.FreeHGlobal(buffer);

        if (validTransfer) { }
        else
        {
            handle.Close();
            handle = null;
        }
        return handle;
    }
}
