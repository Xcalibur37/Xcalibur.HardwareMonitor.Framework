using System;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models
{
    /// <summary>
    /// Access Rights for the Service Control Manager
    /// </summary>
    [Flags]
    internal enum ScManagerAccessMask : uint
    {
        ScManagerConnect = 0x00001,
        ScManagerCreateService = 0x00002,
        ScManagerEnumerateService = 0x00004,
        ScManagerLock = 0x00008,
        ScManagerQueryLockStatus = 0x00010,
        ScManagerModifyBootConfig = 0x00020,
        ScManagerAllAccess = 0xF003F
    }
}
