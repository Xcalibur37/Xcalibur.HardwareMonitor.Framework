using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// Windows Storage
/// </summary>
internal static class WindowsStorage
{
    public static StorageInfo GetStorageInfo(string deviceId, uint driveIndex)
    {
        using SafeFileHandle handle = Kernel32.OpenDevice(deviceId);

        if (handle?.IsInvalid != false)
            return null;

        var query = new StoragePropertyQuery { PropertyId = StoragePropertyId.StorageDeviceProperty, QueryType = StorageQueryType.PropertyStandardQuery };

        if (!Kernel32.DeviceIoControl(handle,
                                      IoCtl.IOCTL_STORAGE_QUERY_PROPERTY,
                                      ref query,
                                      Marshal.SizeOf(query),
                                      out StorageDeviceDescriptorHeader header,
                                      Marshal.SizeOf<StorageDeviceDescriptorHeader>(),
                                      out _,
                                      IntPtr.Zero))
        {
            return null;
        }

        IntPtr descriptorPtr = Marshal.AllocHGlobal((int)header.Size);

        try
        {
            return Kernel32.DeviceIoControl(handle, IoCtl.IOCTL_STORAGE_QUERY_PROPERTY, ref query, Marshal.SizeOf(query), descriptorPtr, header.Size, out uint bytesReturned, IntPtr.Zero)
                ? new StorageInfo((int)driveIndex, descriptorPtr)
                : null;
        }
        finally
        {
            Marshal.FreeHGlobal(descriptorPtr);
        }
    }

    /// <summary>
    /// Gets the logical drives.
    /// </summary>
    /// <param name="driveIndex">Index of the drive.</param>
    /// <returns></returns>
    public static string[] GetLogicalDrives(int driveIndex)
    {
        var list = new List<string>();

        #pragma warning disable CA1416
        try
        {
            using var s = new ManagementObjectSearcher("root\\CIMV2", 
                $"SELECT * FROM Win32_DiskPartition WHERE DiskIndex={driveIndex}");

            foreach (ManagementBaseObject o in s.Get())
            {
                if (o is not ManagementObject dp) continue;
                foreach (ManagementBaseObject ld in dp.GetRelated("Win32_LogicalDisk"))
                {
                    list.Add(((string)ld["Name"]).TrimEnd(':'));
                }
            }
        }
        catch
        {
            // Ignored.
        }
        #pragma warning restore CA1416

        return list.ToArray();
    }
}
