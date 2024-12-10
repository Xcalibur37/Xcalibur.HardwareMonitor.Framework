using System.Diagnostics.CodeAnalysis;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Source Nuvoton 6776F
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SourceNct6776F : byte
    {
        SysTin = 1,
        CpuTin = 2,
        AuxTin = 3,
        Peci0 = 12
    }
}
