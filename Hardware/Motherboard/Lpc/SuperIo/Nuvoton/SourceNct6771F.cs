using System.Diagnostics.CodeAnalysis;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Source Nuvoton 6771F
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SourceNct6771F : byte
    {
        SYSTIN = 1,
        CPUTIN = 2,
        AUXTIN = 3,
        PECI_0 = 5
    }
}
