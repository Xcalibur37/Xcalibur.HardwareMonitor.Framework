﻿





using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

internal abstract class StorageInfo
{
    public Kernel32.STORAGE_BUS_TYPE BusType { get; protected set; }

    public string DeviceId { get; set; }

    public ulong DiskSize { get; set; }

    public SafeFileHandle Handle { get; set; }

    public int Index { get; protected set; }

    public string Name => (Vendor + " " + Product).Trim();

    public string Product { get; protected set; }

    public byte[] RawData { get; protected set; }

    public bool Removable { get; protected set; }

    public string Revision { get; protected set; }

    public string Scsi { get; set; }

    public string Serial { get; protected set; }

    public string Vendor { get; protected set; }
}
