using System.Diagnostics.CodeAnalysis;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Source Nuvoton 67XXD
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SourceNct67XXD : byte
    {
        SYSTIN = 1,
        CPUTIN = 2,
        AUXTIN0 = 3,
        AUXTIN1 = 4,
        AUXTIN2 = 5,
        AUXTIN3 = 6,
        AUXTIN4 = 7,
        SMBUSMASTER0 = 8,
        SMBUSMASTER1 = 9,
        TSENSOR = 10,
        PECI_0 = 16,
        PECI_1 = 17,
        PCH_CHIP_CPU_MAX_TEMP = 18,
        PCH_CHIP_TEMP = 19,
        PCH_CPU_TEMP = 20,
        PCH_MCH_TEMP = 21,
        AGENT0_DIMM0 = 22,
        AGENT0_DIMM1 = 23,
        AGENT1_DIMM0 = 24,
        AGENT1_DIMM1 = 25,
        BYTE_TEMP0 = 26,
        BYTE_TEMP1 = 27,
        PECI_0_CAL = 28,
        PECI_1_CAL = 29,
        VIRTUAL_TEMP = 31
    }
}
