using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// Storage info model.
/// </summary>
public class StorageInfo
{
    #region Properties

    /// <summary>
    /// Gets or sets the type of the bus.
    /// </summary>
    /// <value>
    /// The type of the bus.
    /// </value>
    public StorageBusType BusType { get; protected set; }

    /// <summary>
    /// Gets or sets the device identifier.
    /// </summary>
    /// <value>
    /// The device identifier.
    /// </value>
    public string DeviceId { get; set; }

    /// <summary>
    /// Gets or sets the size of the disk.
    /// </summary>
    /// <value>
    /// The size of the disk.
    /// </value>
    public ulong DiskSize { get; set; }

    /// <summary>
    /// Gets or sets the handle.
    /// </summary>
    /// <value>
    /// The handle.
    /// </value>
    public SafeFileHandle Handle { get; set; }

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    /// <value>
    /// The index.
    /// </value>
    public int Index { get; protected set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name => (Vendor + " " + Product).Trim();

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>
    /// The product.
    /// </value>
    public string Product { get; protected set; }

    /// <summary>
    /// Gets or sets the raw data.
    /// </summary>
    /// <value>
    /// The raw data.
    /// </value>
    public byte[] RawData { get; protected set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="StorageInfo"/> is removable.
    /// </summary>
    /// <value>
    ///   <c>true</c> if removable; otherwise, <c>false</c>.
    /// </value>
    public bool Removable { get; protected set; }

    /// <summary>
    /// Gets or sets the revision.
    /// </summary>
    /// <value>
    /// The revision.
    /// </value>
    public string Revision { get; protected set; }

    /// <summary>
    /// Gets or sets the scsi.
    /// </summary>
    /// <value>
    /// The scsi.
    /// </value>
    public string Scsi { get; set; }

    /// <summary>
    /// Gets or sets the serial.
    /// </summary>
    /// <value>
    /// The serial.
    /// </value>
    public string Serial { get; protected set; }

    /// <summary>
    /// Gets or sets the vendor.
    /// </summary>
    /// <value>
    /// The vendor.
    /// </value>
    public string Vendor { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageInfo"/> class.
    /// </summary>
    public StorageInfo() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageInfo"/> class.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="descriptorPtr">The descriptor PTR.</param>
    public StorageInfo(int index, IntPtr descriptorPtr)
    {
        var descriptor = Marshal.PtrToStructure<StorageDeviceDescriptor>(descriptorPtr);
        Index = index;
        Vendor = GetString(descriptorPtr, descriptor.VendorIdOffset, descriptor.Size);
        Product = GetString(descriptorPtr, descriptor.ProductIdOffset, descriptor.Size);
        Revision = GetString(descriptorPtr, descriptor.ProductRevisionOffset, descriptor.Size);
        Serial = GetString(descriptorPtr, descriptor.SerialNumberOffset, descriptor.Size);
        BusType = descriptor.BusType;
        Removable = descriptor.RemovableMedia;
        RawData = new byte[descriptor.Size];
        Marshal.Copy(descriptorPtr, RawData, 0, RawData.Length);
    }
    
    #endregion

    #region Methods

    /// <summary>
    /// Gets the string.
    /// </summary>
    /// <param name="descriptorPtr">The descriptor PTR.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="size">The size.</param>
    /// <returns></returns>
    public static string GetString(IntPtr descriptorPtr, uint offset, uint size) =>
        offset > 0 && offset < size ? Marshal.PtrToStringAnsi(IntPtr.Add(descriptorPtr, (int)offset))?.Trim() : string.Empty;

    #endregion
}
