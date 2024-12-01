namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.FTDI
{
    internal enum FtStatus
    {
        FtOk,
        FtInvalidHandle,
        FtDeviceNotFound,
        FtDeviceNotOpened,
        FtIoError,
        FtInsufficientResources,
        FtInvalidParameter,
        FtInvalidBaudRate,
        FtDeviceNotOpenedForErase,
        FtDeviceNotOpenedForWrite,
        FtFailedToWriteDevice,
        FtEepromReadFailed,
        FtEepromWriteFailed,
        FtEepromEraseFailed,
        FtEepromNotPresent,
        FtEepromNotProgrammed,
        FtInvalidArgs,
        FtOtherError
    }
}
