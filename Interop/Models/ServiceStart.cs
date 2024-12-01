namespace Xcalibur.HardwareMonitor.Framework.Interop.Models
{
    /// <summary>
    /// The service start options.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winsvc/nf-winsvc-createservicea
    /// </summary>
    internal enum ServiceStart : uint
    {
        ServiceBootStart = 0,
        ServiceSystemStart = 1,
        ServiceAutoStart = 2,
        ServiceDemandStart = 3,
        ServiceDisabled = 4
    }
}
