namespace Xcalibur.HardwareMonitor.Framework.Interop.Models
{
    /// <summary>
    /// A handle to the service. This handle is returned by the OpenService or CreateService function.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winsvc/nf-winsvc-controlservice
    /// </summary>
    internal enum ServiceControl : uint
    {
        ServiceControlStop = 1,
        ServiceControlPause = 2,
        ServiceControlContinue = 3,
        ServiceControlInterrogate = 4,
        ServiceControlShutdown = 5,
        ServiceControlParamchange = 6,
        ServiceControlNetbindadd = 7,
        ServiceControlNetbindremove = 8,
        ServiceControlNetbindenable = 9,
        ServiceControlNetbinddisable = 10,
        ServiceControlDeviceevent = 11,
        ServiceControlHardwareprofilechange = 12,
        ServiceControlPowerevent = 13,
        ServiceControlSessionchange = 14
    }
}
