using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

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
    public bool HealthInfoLog(SafeHandle hDevice, out NvmeHealthInfoLog data)
    {
        data = Kernel32.CreateStruct<NvmeHealthInfoLog>();
        if (hDevice?.IsInvalid != false) return false;

        bool result = false;

        var passThrough = Kernel32.CreateStruct<NvmePassThroughIoctl>();
        passThrough.srb.HeaderLenght = (uint)Marshal.SizeOf<SrbIoControl>();
        passThrough.srb.Signature = Encoding.ASCII.GetBytes(Kernel32.IntelNvMeMiniPortSignature2);
        passThrough.srb.Timeout = 10;
        passThrough.srb.ControlCode = Kernel32.NvmePassThroughSrbIoCode;
        passThrough.srb.ReturnCode = 0;
        passThrough.srb.Length = (uint)Marshal.SizeOf<NvmePassThroughIoctl>() - (uint)Marshal.SizeOf<SrbIoControl>();
        passThrough.NVMeCmd[0] = (uint)StorageProtocolNvmeDataType.NvMeDataTypeLogPage; // GetLogPage
        passThrough.NVMeCmd[1] = 0xFFFFFFFF; // address
        passThrough.NVMeCmd[10] = 0x007f0002; // uint cdw10 = 0x000000002 | (((size / 4) - 1) << 16);
        passThrough.Direction = NvmeDirection.NvmeFromDevToHost;
        passThrough.QueueId = 0;
        passThrough.DataBufferLen = (uint)passThrough.DataBuffer.Length;
        passThrough.MetaDataLen = 0;
        passThrough.ReturnBufferLen = (uint)Marshal.SizeOf<NvmePassThroughIoctl>();

        int length = Marshal.SizeOf<NvmePassThroughIoctl>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(passThrough, buffer, false);

        bool validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IoctlScsiMiniport, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            nint offset = Marshal.OffsetOf<NvmePassThroughIoctl>(nameof(NvmePassThroughIoctl.DataBuffer));
            nint newPtr = nint.Add(buffer, offset.ToInt32());
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
    public bool IdentifyController(SafeHandle hDevice, out NvmeIdentifyControllerData data)
    {
        data = Kernel32.CreateStruct<NvmeIdentifyControllerData>();
        if (hDevice?.IsInvalid != false) return false;

        bool result = false;

        var passThrough = Kernel32.CreateStruct<NvmePassThroughIoctl>();
        passThrough.srb.HeaderLenght = (uint)Marshal.SizeOf<SrbIoControl>();
        passThrough.srb.Signature = Encoding.ASCII.GetBytes(Kernel32.IntelNvMeMiniPortSignature2);
        passThrough.srb.Timeout = 10;
        passThrough.srb.ControlCode = Kernel32.NvmePassThroughSrbIoCode;
        passThrough.srb.ReturnCode = 0;
        passThrough.srb.Length = (uint)Marshal.SizeOf<NvmePassThroughIoctl>() - (uint)Marshal.SizeOf<SrbIoControl>();
        passThrough.NVMeCmd = new uint[16];
        passThrough.NVMeCmd[0] = 6; //identify
        passThrough.NVMeCmd[10] = 1; //return to host
        passThrough.Direction = NvmeDirection.NvmeFromDevToHost;
        passThrough.QueueId = 0;
        passThrough.DataBufferLen = (uint)passThrough.DataBuffer.Length;
        passThrough.MetaDataLen = 0;
        passThrough.ReturnBufferLen = (uint)Marshal.SizeOf<NvmePassThroughIoctl>();

        int length = Marshal.SizeOf<NvmePassThroughIoctl>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(passThrough, buffer, false);

        bool validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IoctlScsiMiniport, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            nint offset = Marshal.OffsetOf<NvmePassThroughIoctl>(nameof(NvmePassThroughIoctl.DataBuffer));
            nint newPtr = nint.Add(buffer, offset.ToInt32());
            int finalSize = Marshal.SizeOf<NvmeIdentifyControllerData>();
            nint ptr = Marshal.AllocHGlobal(Marshal.SizeOf<NvmeIdentifyControllerData>());
            Kernel32.RtlZeroMemory(ptr, finalSize);
            int len = Math.Min(finalSize, passThrough.DataBuffer.Length);
            Kernel32.RtlCopyMemory(ptr, newPtr, (uint)len);
            Marshal.FreeHGlobal(buffer);

            data = Marshal.PtrToStructure<NvmeIdentifyControllerData>(ptr);
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

        var passThrough = Kernel32.CreateStruct<NvmePassThroughIoctl>();
        passThrough.srb.HeaderLenght = (uint)Marshal.SizeOf<SrbIoControl>();
        passThrough.srb.Signature = Encoding.ASCII.GetBytes(Kernel32.IntelNvMeMiniPortSignature2);
        passThrough.srb.Timeout = 10;
        passThrough.srb.ControlCode = Kernel32.NvmePassThroughSrbIoCode;
        passThrough.srb.ReturnCode = 0;
        passThrough.srb.Length = (uint)Marshal.SizeOf<NvmePassThroughIoctl>() - (uint)Marshal.SizeOf<SrbIoControl>();
        passThrough.NVMeCmd = new uint[16];
        passThrough.NVMeCmd[0] = 6; //identify
        passThrough.NVMeCmd[10] = 1; //return to host
        passThrough.Direction = NvmeDirection.NvmeFromDevToHost;
        passThrough.QueueId = 0;
        passThrough.DataBufferLen = (uint)passThrough.DataBuffer.Length;
        passThrough.MetaDataLen = 0;
        passThrough.ReturnBufferLen = (uint)Marshal.SizeOf<NvmePassThroughIoctl>();

        int length = Marshal.SizeOf<NvmePassThroughIoctl>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(passThrough, buffer, false);

        bool validTransfer = Kernel32.DeviceIoControl(handle, IoCtl.IoctlScsiMiniport, buffer, length, buffer, length, out _, nint.Zero);
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
