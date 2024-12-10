namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    /// <summary>
    /// The DeviceIoControl function provides a device input and output control (IOCTL) interface through which an application
    /// can communicate directly with a device driver.
    /// https://learn.microsoft.com/en-us/windows/win32/devio/device-input-and-output-control-ioctl-
    /// </summary>
    public enum IoCtl : uint
    {

        /// <summary>
        /// Allows an application to send almost any SCSI command to a target device with restrictions.
        /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddscsi/ni-ntddscsi-ioctl_scsi_pass_through
        /// </summary>
        IoctlScsiPassThrough = 0x04d004,

        /// <summary>
        /// Sends a special control function to a host bus adapter-specific (HBA) miniport driver. Results vary, depending on
        /// the particular miniport driver to which this request is forwarded. If the caller specifies a nonzero Length, either
        /// the input or output buffer must be at least (sizeof(SRB_IO_CONTROL) + DataBufferLength)).
        /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddscsi/ni-ntddscsi-ioctl_scsi_miniport
        /// </summary>
        IoctlScsiMiniport = 0x04d008,

        /// <summary>
        /// Allows an application to send almost any SCSI command to a target device with restrictions.
        /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddscsi/ni-ntddscsi-ioctl_scsi_pass_through_direct
        /// </summary>
        IoctlScsiPassThroughDirect = 0x04d014,
        
        /// <summary>
        /// Returns the address information, such as the target ID (TID) and the logical unit number (LUN) of a particular SCSI
        /// target. A legacy class driver can issue this request to the port driver to obtain the address of its device.
        /// On Windows 10 version 1809 and later versions, a legacy class driver can issue this request to obtain the address of
        /// its adapter.
        /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddscsi/ni-ntddscsi-ioctl_scsi_get_address
        /// </summary>
        IoctlScsiGetAddress = 0x41018,
        
        /// <summary>
        /// Enables performance counters that provide disk performance information.
        /// https://learn.microsoft.com/en-us/windows/win32/api/winioctl/ni-winioctl-ioctl_disk_performance
        /// </summary>
        IoctlDiskPerformance = 0x70020,
        
        /// <summary>
        /// Windows applications can use this control code to return the properties of a storage device or adapter.
        /// The request indicates the kind of information to retrieve, such as the inquiry data for a device or the
        /// capabilities and limitations of an adapter. IOCTL_STORAGE_QUERY_PROPERTY can also be used to determine whether
        /// the port driver supports a particular property or which fields in the property descriptor can be modified with
        /// a subsequent change-property request.
        /// https://learn.microsoft.com/en-us/windows/win32/api/winioctl/ni-winioctl-ioctl_storage_query_property
        /// </summary>
        IoctlStorageQueryProperty = 0x2D1400,

        /// <summary>
        /// Retrieves the battery's current tag.
        /// </summary>
        IoctlBatteryQueryTag = 0x294040,
        
        /// <summary>
        /// Retrieves a variety of information for the battery.
        /// </summary>
        IoctlBatteryQueryInformation = 0x294044,
        
        /// <summary>
        /// Retrieves the current status for the battery.
        /// </summary>
        IoctlBatteryQueryStatus = 0x29404C
    }
}
