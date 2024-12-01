namespace Xcalibur.HardwareMonitor.Framework.Interop.Models
{
    /// <summary>
    /// The service type.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winsvc/nf-winsvc-createservicea
    /// </summary>
    internal enum ServiceType : uint
    {
        ServiceDriver = 0x0000000B,
        ServiceWin32 = 0x00000030,
        ServiceAdapter = 0x00000004,
        ServiceFileSystemDriver = 0x00000002,
        ServiceKernelDriver = 0x00000001,
        ServiceRecognizerDriver = 0x00000008,
        ServiceWin32OwnProcess = 0x00000010,
        ServiceWin32ShareProcess = 0x00000020,
        ServiceUserOwnProcess = 0x00000050,
        ServiceUserShareProcess = 0x00000060,
        ServiceInteractiveProcess = 0x00000100
    }
}
