using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// Windows Storage
/// </summary>
internal static class WindowsStorage
{
    public static Storage.StorageInfo GetStorageInfo(string deviceId, uint driveIndex)
    {
        using SafeFileHandle handle = Kernel32.OpenDevice(deviceId);

        if (handle?.IsInvalid != false)
            return null;

        var query = new Kernel32.STORAGE_PROPERTY_QUERY { PropertyId = Kernel32.STORAGE_PROPERTY_ID.StorageDeviceProperty, QueryType = Kernel32.STORAGE_QUERY_TYPE.PropertyStandardQuery };

        if (!Kernel32.DeviceIoControl(handle,
                                      Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY,
                                      ref query,
                                      Marshal.SizeOf(query),
                                      out Kernel32.STORAGE_DEVICE_DESCRIPTOR_HEADER header,
                                      Marshal.SizeOf<Kernel32.STORAGE_DEVICE_DESCRIPTOR_HEADER>(),
                                      out _,
                                      IntPtr.Zero))
        {
            return null;
        }

        IntPtr descriptorPtr = Marshal.AllocHGlobal((int)header.Size);

        try
        {
            return Kernel32.DeviceIoControl(handle, Kernel32.IOCTL.IOCTL_STORAGE_QUERY_PROPERTY, ref query, Marshal.SizeOf(query), descriptorPtr, header.Size, out uint bytesReturned, IntPtr.Zero)
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
