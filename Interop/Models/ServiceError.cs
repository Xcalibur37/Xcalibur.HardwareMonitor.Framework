namespace Xcalibur.HardwareMonitor.Framework.Interop.Models
{
    /// <summary>
    /// The severity of the error, and action taken, if this service fails to start.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winsvc/nf-winsvc-createservicea
    /// </summary>
    internal enum ServiceError : uint
    {
        ServiceErrorIgnore = 0,
        ServiceErrorNormal = 1,
        ServiceErrorSevere = 2,
        ServiceErrorCritical = 3
    }
}
