using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// NVMe: Samsung
/// https://github.com/hiyohiyo/CrystalDiskInfo
/// https://github.com/hiyohiyo/CrystalDiskInfo/blob/master/AtaSmart.cpp
/// </summary>
/// <seealso cref="INvmeDrive" />
internal class NvmeSamsung : INvmeDrive
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
        ScsiPassThroughWithBuffers buffers = Kernel32.CreateStruct<ScsiPassThroughWithBuffers>();

        buffers.Spt.Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
        buffers.Spt.PathId = 0;
        buffers.Spt.TargetId = 0;
        buffers.Spt.Lun = 0;
        buffers.Spt.SenseInfoLength = 24;
        buffers.Spt.DataTransferLength = (uint)buffers.DataBuf.Length;
        buffers.Spt.TimeOutValue = 2;
        buffers.Spt.DataBufferOffset = Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.DataBuf));
        buffers.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.SenseBuf));
        buffers.Spt.CdbLength = 16;
        buffers.Spt.Cdb[0] = 0xB5; // SECURITY PROTOCOL IN
        buffers.Spt.Cdb[1] = 0xFE; // Samsung Protocol
        buffers.Spt.Cdb[3] = 6; // Log Data
        buffers.Spt.Cdb[8] = 0; // Transfer Length
        buffers.Spt.Cdb[9] = 0x40; // Transfer Length
        buffers.Spt.DataIn = (byte)ScsiIoCtlData.ScsiIoctlDataOut;
        buffers.DataBuf[0] = 2;
        buffers.DataBuf[4] = 0xff;
        buffers.DataBuf[5] = 0xff;
        buffers.DataBuf[6] = 0xff;
        buffers.DataBuf[7] = 0xff;

        int length = Marshal.SizeOf<ScsiPassThroughWithBuffers>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(buffers, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IOCTL_SCSI_PASS_THROUGH, buffer, length, buffer, length, out _, nint.Zero);
        Marshal.FreeHGlobal(buffer);

        if (!validTransfer) return false;
        //read data from samsung SSD
        buffers = Kernel32.CreateStruct<ScsiPassThroughWithBuffers>();
        buffers.Spt.Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
        buffers.Spt.PathId = 0;
        buffers.Spt.TargetId = 0;
        buffers.Spt.Lun = 0;
        buffers.Spt.SenseInfoLength = 24;
        buffers.Spt.DataTransferLength = (uint)buffers.DataBuf.Length;
        buffers.Spt.TimeOutValue = 2;
        buffers.Spt.DataBufferOffset = Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.DataBuf));
        buffers.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.SenseBuf));
        buffers.Spt.CdbLength = 16;
        buffers.Spt.Cdb[0] = 0xA2; // SECURITY PROTOCOL IN
        buffers.Spt.Cdb[1] = 0xFE; // Samsung Protocol
        buffers.Spt.Cdb[3] = 6; // Log Data
        buffers.Spt.Cdb[8] = 2; // Transfer Length (high)
        buffers.Spt.Cdb[9] = 0; // Transfer Length (low)
        buffers.Spt.DataIn = (byte)ScsiIoCtlData.ScsiIoctlDataIn;

        length = Marshal.SizeOf<ScsiPassThroughWithBuffers>();
        buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(buffers, buffer, false);

        validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IOCTL_SCSI_PASS_THROUGH, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            nint offset = Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.DataBuf));
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
        ScsiPassThroughWithBuffers buffers = Kernel32.CreateStruct<ScsiPassThroughWithBuffers>();

        buffers.Spt.Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
        buffers.Spt.PathId = 0;
        buffers.Spt.TargetId = 0;
        buffers.Spt.Lun = 0;
        buffers.Spt.SenseInfoLength = 24;
        buffers.Spt.DataTransferLength = (uint)buffers.DataBuf.Length;
        buffers.Spt.TimeOutValue = 2;
        buffers.Spt.DataBufferOffset = Marshal.OffsetOf(typeof(ScsiPassThroughWithBuffers), nameof(ScsiPassThroughWithBuffers.DataBuf));
        buffers.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf(typeof(ScsiPassThroughWithBuffers), nameof(ScsiPassThroughWithBuffers.SenseBuf));
        buffers.Spt.CdbLength = 16;
        buffers.Spt.Cdb[0] = 0xB5; // SECURITY PROTOCOL IN
        buffers.Spt.Cdb[1] = 0xFE; // Samsung Protocol
        buffers.Spt.Cdb[3] = 5; // Identify
        buffers.Spt.Cdb[8] = 0; // Transfer Length
        buffers.Spt.Cdb[9] = 0x40; // Transfer Length
        buffers.Spt.DataIn = (byte)ScsiIoCtlData.ScsiIoctlDataOut;
        buffers.DataBuf[0] = 1;

        int length = Marshal.SizeOf<ScsiPassThroughWithBuffers>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(buffers, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IOCTL_SCSI_PASS_THROUGH, buffer, length, buffer, length, out _, nint.Zero);
        Marshal.FreeHGlobal(buffer);

        if (!validTransfer) return false;
        //read data from samsung SSD
        buffers = Kernel32.CreateStruct<ScsiPassThroughWithBuffers>();
        buffers.Spt.Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
        buffers.Spt.PathId = 0;
        buffers.Spt.TargetId = 0;
        buffers.Spt.Lun = 0;
        buffers.Spt.SenseInfoLength = 24;
        buffers.Spt.DataTransferLength = (uint)buffers.DataBuf.Length;
        buffers.Spt.TimeOutValue = 2;
        buffers.Spt.DataBufferOffset = Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.DataBuf));
        buffers.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.SenseBuf));
        buffers.Spt.CdbLength = 16;
        buffers.Spt.Cdb[0] = 0xA2; // SECURITY PROTOCOL IN
        buffers.Spt.Cdb[1] = 0xFE; // Samsung Protocol
        buffers.Spt.Cdb[3] = 5; // Identify
        buffers.Spt.Cdb[8] = 2; // Transfer Length (high)
        buffers.Spt.Cdb[9] = 0; // Transfer Length (low)
        buffers.Spt.DataIn = (byte)ScsiIoCtlData.ScsiIoctlDataIn;

        length = Marshal.SizeOf<ScsiPassThroughWithBuffers>();
        buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(buffers, buffer, false);

        validTransfer = Kernel32.DeviceIoControl(hDevice, IoCtl.IOCTL_SCSI_PASS_THROUGH, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer && buffers.DataBuf.Any(x => x != 0))
        {
            nint offset = Marshal.OffsetOf<ScsiPassThroughWithBuffers>(nameof(ScsiPassThroughWithBuffers.DataBuf));
            nint newPtr = nint.Add(buffer, offset.ToInt32());
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
    /// Identifies the device.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <returns></returns>
    public static SafeHandle IdentifyDevice(StorageInfo storageInfo)
    {
        SafeFileHandle handle = Kernel32.OpenDevice(storageInfo.DeviceId);
        if (handle?.IsInvalid != false) return null;

        ScsiPassThroughWithBuffers buffers = Kernel32.CreateStruct<ScsiPassThroughWithBuffers>();

        buffers.Spt.Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
        buffers.Spt.PathId = 0;
        buffers.Spt.TargetId = 0;
        buffers.Spt.Lun = 0;
        buffers.Spt.SenseInfoLength = 24;
        buffers.Spt.DataTransferLength = (uint)buffers.DataBuf.Length;
        buffers.Spt.TimeOutValue = 2;
        buffers.Spt.DataBufferOffset = Marshal.OffsetOf(typeof(ScsiPassThroughWithBuffers), nameof(ScsiPassThroughWithBuffers.DataBuf));
        buffers.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf(typeof(ScsiPassThroughWithBuffers), nameof(ScsiPassThroughWithBuffers.SenseBuf));
        buffers.Spt.CdbLength = 16;
        buffers.Spt.Cdb[0] = 0xB5; // SECURITY PROTOCOL IN
        buffers.Spt.Cdb[1] = 0xFE; // Samsung Protocol
        buffers.Spt.Cdb[3] = 5; // Identify
        buffers.Spt.Cdb[8] = 0; // Transfer Length
        buffers.Spt.Cdb[9] = 0x40;
        buffers.Spt.DataIn = (byte)ScsiIoCtlData.ScsiIoctlDataOut;
        buffers.DataBuf[0] = 1;

        int length = Marshal.SizeOf<ScsiPassThroughWithBuffers>();
        nint buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(buffers, buffer, false);
        bool validTransfer = Kernel32.DeviceIoControl(handle, IoCtl.IOCTL_SCSI_PASS_THROUGH, buffer, length, buffer, length, out _, nint.Zero);
        Marshal.FreeHGlobal(buffer);

        if (!validTransfer) return handle;
        //read data from samsung SSD
        buffers = Kernel32.CreateStruct<ScsiPassThroughWithBuffers>();
        buffers.Spt.Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
        buffers.Spt.PathId = 0;
        buffers.Spt.TargetId = 0;
        buffers.Spt.Lun = 0;
        buffers.Spt.SenseInfoLength = 24;
        buffers.Spt.DataTransferLength = (uint)buffers.DataBuf.Length;
        buffers.Spt.TimeOutValue = 2;
        buffers.Spt.DataBufferOffset = Marshal.OffsetOf(typeof(ScsiPassThroughWithBuffers), nameof(ScsiPassThroughWithBuffers.DataBuf));
        buffers.Spt.SenseInfoOffset = (uint)Marshal.OffsetOf(typeof(ScsiPassThroughWithBuffers), nameof(ScsiPassThroughWithBuffers.SenseBuf));
        buffers.Spt.CdbLength = 16;
        buffers.Spt.Cdb[0] = 0xA2; // SECURITY PROTOCOL IN
        buffers.Spt.Cdb[1] = 0xFE; // Samsung Protocol
        buffers.Spt.Cdb[3] = 5; // Identify
        buffers.Spt.Cdb[8] = 2; // Transfer Length
        buffers.Spt.Cdb[9] = 0;
        buffers.Spt.DataIn = (byte)ScsiIoCtlData.ScsiIoctlDataIn;

        length = Marshal.SizeOf<ScsiPassThroughWithBuffers>();
        buffer = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(buffers, buffer, false);

        validTransfer = Kernel32.DeviceIoControl(handle, IoCtl.IOCTL_SCSI_PASS_THROUGH, buffer, length, buffer, length, out _, nint.Zero);
        if (validTransfer)
        {
            var result = Marshal.PtrToStructure<ScsiPassThroughWithBuffers>(buffer);

            if (result.DataBuf.All(x => x == 0))
            {
                handle.Close();
                handle = null;
            }
        }
        else
        {
            handle.Close();
            handle = null;
        }

        Marshal.FreeHGlobal(buffer);

        return handle;
    }
}
