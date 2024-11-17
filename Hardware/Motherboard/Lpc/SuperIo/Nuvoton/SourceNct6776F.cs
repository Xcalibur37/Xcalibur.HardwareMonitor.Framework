using System.Diagnostics.CodeAnalysis;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Source Nuvoton 6776F
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SourceNct6776F : byte
    {
        SYSTIN = 1,
        CPUTIN = 2,
        AUXTIN = 3,
        PECI_0 = 12
    }
}
