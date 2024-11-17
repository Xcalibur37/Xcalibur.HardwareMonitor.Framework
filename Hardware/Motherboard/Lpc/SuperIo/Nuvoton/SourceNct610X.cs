using System.Diagnostics.CodeAnalysis;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Source Nuvoton 610X
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SourceNct610X : byte
    {
        SYSTIN = 1,
        CPUTIN = 2,
        AUXTIN = 3,
        PECI_0 = 12
    }
}
