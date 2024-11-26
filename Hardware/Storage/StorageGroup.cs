using System;
using System.Collections.Generic;
using System.Management;
using Xcalibur.Extensions.V2;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

#pragma warning disable CA1416 // Validate platform compatibility

/// <summary>
/// Storage Group
/// </summary>
/// <seealso cref="IGroup" />
internal class StorageGroup : IGroup
{
    private readonly List<AbstractStorage> _hardware = [];

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _hardware;

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageGroup"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public StorageGroup(ISettings settings)
    {
        if (Software.OperatingSystem.IsUnix) return;
        Dictionary<uint, List<(uint, ulong)>> storageSpaceDiskToPhysicalDiskMap = GetStorageSpaceDiskToPhysicalDiskMap();
        AddHardware(settings, storageSpaceDiskToPhysicalDiskMap);
    }

    /// <summary>
    /// Close open devices.
    /// </summary>
    public void Close()
    {
        _hardware.Apply(x => x.Close());
    }

    /// <summary>
    /// Adds the hardware.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="storageSpaceDiskToPhysicalDiskMap">The storage space disk to physical disk map.</param>
    private void AddHardware(ISettings settings, Dictionary<uint, List<(uint, ulong)>> storageSpaceDiskToPhysicalDiskMap)
    {
        try
        {
            // https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-diskdrive
            using var diskDriveSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive") { Options = { Timeout = TimeSpan.FromSeconds(10) } };

            foreach (ManagementBaseObject diskDrive in diskDriveSearcher.Get())
            {
                string deviceId = (string)diskDrive.Properties["DeviceId"].Value; // is \\.\PhysicalDrive0..n
                uint index = Convert.ToUInt32(diskDrive.Properties["Index"].Value);
                ulong diskSize = Convert.ToUInt64(diskDrive.Properties["Size"].Value);
                int scsi = Convert.ToInt32(diskDrive.Properties["SCSIPort"].Value);

                if (deviceId == null) continue;
                var instance = AbstractStorage.CreateInstance(deviceId, index, diskSize, scsi, settings);
                if (instance != null)
                {
                    _hardware.Add(instance);
                }

                if (!storageSpaceDiskToPhysicalDiskMap.TryGetValue(index, out var value)) continue;
                foreach ((uint, ulong) physicalDisk in value)
                {
                    var physicalDiskInstance = AbstractStorage.CreateInstance(@$"\\.\PHYSICALDRIVE{physicalDisk.Item1}", physicalDisk.Item1, physicalDisk.Item2, scsi, settings);
                    if (physicalDiskInstance == null) continue;
                    _hardware.Add(physicalDiskInstance);
                }
            }
        }
        catch
        {
            // Ignored.
        }
    }

    /// <summary>
    /// Maps each StorageSpace to the PhysicalDisks it is composed of.
    /// </summary>
    private static Dictionary<uint, List<(uint, ulong)>> GetStorageSpaceDiskToPhysicalDiskMap()
    {
        var diskToPhysicalDisk = new Dictionary<uint, List<(uint, ulong)>>();

        if (!Software.OperatingSystem.IsWindows8OrGreater)
        {
            return diskToPhysicalDisk;
        }

        try
        {
            ManagementScope scope = new(@"\root\Microsoft\Windows\Storage");

            // https://learn.microsoft.com/en-us/previous-versions/windows/desktop/stormgmt/msft-disk
            // Lists all the disks visible to your system, the output is the same as Win32_DiskDrive.
            // If you're using a storage Space, the "hidden" disks which compose your storage space will not be listed.
            using var diskSearcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT * FROM MSFT_Disk"));

            foreach (ManagementBaseObject disk in diskSearcher.Get())
            {
                try
                {
                    List<(uint, ulong)> map = MapDiskToPhysicalDisk(disk, scope);
                    if (map.Count > 0)
                        diskToPhysicalDisk[(uint)disk["Number"]] = map;
                }
                catch
                {
                    // Ignored.
                }
            }
        }
        catch
        {
            // Ignored.
        }

        return diskToPhysicalDisk;
    }

    /// <summary>
    /// Maps a disk to a physical disk.
    /// </summary>
    /// <param name="disk">The disk.</param>
    /// <param name="scope">The scope.</param>
    private static List<(uint, ulong)> MapDiskToPhysicalDisk(ManagementBaseObject disk, ManagementScope scope)
    {
        var map = new List<(uint, ulong)>();

        // https://learn.microsoft.com/en-us/previous-versions/windows/desktop/stormgmt/msft-virtualdisk
        // Maps the current Disk to its corresponding VirtualDisk. If the current Disk is not a storage space, it does not have a corresponding VirtualDisk.
        // Each Disk maps to one or zero VirtualDisk.
        using var toVirtualDisk = new ManagementObjectSearcher(scope, new ObjectQuery(FollowAssociationQuery("MSFT_Disk", (string)disk["ObjectId"], "MSFT_VirtualDiskToDisk")));

        foreach (ManagementBaseObject virtualDisk in toVirtualDisk.Get())
        {
            // https://learn.microsoft.com/en-us/previous-versions/windows/desktop/stormgmt/msft-physicaldisk
            // Maps the current VirtualDisk to the PhysicalDisk it is composed of.
            // Each VirtualDisk maps to one or more PhysicalDisk.

            using var toPhysicalDisk = new ManagementObjectSearcher(
                scope,
                new ObjectQuery(FollowAssociationQuery("MSFT_VirtualDisk",
                                                       (string)virtualDisk["ObjectId"],
                                                       "MSFT_VirtualDiskToPhysicalDisk")));

            foreach (ManagementBaseObject physicalDisk in toPhysicalDisk.Get())
            {
                ulong physicalDiskSize = (ulong)physicalDisk["Size"];

                if (!uint.TryParse((string)physicalDisk["DeviceId"], out uint physicalDiskId)) continue;
                map.Add((physicalDiskId, physicalDiskSize));
            }
        }

        return map;
    }

    /// <summary>
    /// Follows the association query.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="objectId">The object identifier.</param>
    /// <param name="associationClass">The association class.</param>
    /// <returns></returns>
    private static string FollowAssociationQuery(string source, string objectId, string associationClass) => 
        @$"ASSOCIATORS OF {{{source}.ObjectId=""{objectId
            .Replace(@"\", @"\\")
            .Replace(@"""", @"\""")}""}} WHERE AssocClass = {associationClass}";
}
