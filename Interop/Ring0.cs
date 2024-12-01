using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// Driver with access at kernel level.
/// </summary>
internal static class Ring0
{
    public const uint InvalidPciAddress = 0xFFFFFFFF;

    private const uint OlsType = 40000;

    public static readonly IoControlCode IoctlOlsGetRefcount = new(OlsType, 0x801, Access.Any);
    public static readonly IoControlCode IoctlOlsReadMsr = new(OlsType, 0x821, Access.Any);
    public static readonly IoControlCode IoctlOlsWriteMsr = new(OlsType, 0x822, Access.Any);
    public static readonly IoControlCode IoctlOlsReadIoPortByte = new(OlsType, 0x833, Access.Read);
    public static readonly IoControlCode IoctlOlsWriteIoPortByte = new(OlsType, 0x836, Access.Write);
    public static readonly IoControlCode IoctlOlsReadPciConfig = new(OlsType, 0x851, Access.Read);
    public static readonly IoControlCode IoctlOlsWritePciConfig = new(OlsType, 0x852, Access.Write);
    public static readonly IoControlCode IoctlOlsReadMemory = new(OlsType, 0x841, Access.Read);
}