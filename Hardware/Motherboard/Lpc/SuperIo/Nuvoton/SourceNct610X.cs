using System.Diagnostics.CodeAnalysis;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Source Nuvoton 610X
    /// </summary>
    public enum SourceNct610X : byte
    {
        SysTin = 1,
        CpuTin = 2,
        AuxTin = 3,
        Peci0 = 12
    }
}
