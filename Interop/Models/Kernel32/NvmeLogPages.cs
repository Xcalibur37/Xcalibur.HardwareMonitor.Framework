namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    public enum NvmeLogPages
    {
        NvmeLogPageErrorInfo = 0x01,
        NvmeLogPageHealthInfo = 0x02,
        NvmeLogPageFirmwareSlotInfo = 0x03,
        NvmeLogPageChangedNamespaceList = 0x04,
        NvmeLogPageCommandEffects = 0x05,
        NvmeLogPageDeviceSelfTest = 0x06,
        NvmeLogPageTelemetryHostInitiated = 0x07,
        NvmeLogPageTelemetryCtlrInitiated = 0x08,
        NvmeLogPageReservationNotification = 0x80,
        NvmeLogPageSanitizeStatus = 0x81
    }
}
