namespace Xcalibur.HardwareMonitor.Framework.Interop.Models
{
    /// <summary>
    /// Access Rights for a Service
    /// </summary>
    internal enum ServiceAccessMask : uint
    {
        ServiceQueryConfig = 0x00001,
        ServiceChangeConfig = 0x00002,
        ServiceQueryStatus = 0x00004,
        ServiceEnumerateDependents = 0x00008,
        ServiceStart = 0x00010,
        ServiceStop = 0x00020,
        ServicePauseContinue = 0x00040,
        ServiceInterrogate = 0x00080,
        ServiceUserDefinedControl = 0x00100,
        ServiceAllAccess = 0xF01FF
    }
}
