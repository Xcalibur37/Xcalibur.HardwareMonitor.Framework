namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Source Nuvoton 67XXD
    /// </summary>
    public enum SourceNct67XXD : byte
    {
        SysTin = 1,
        CpuTin = 2,
        AuxTin0 = 3,
        AuxTin1 = 4,
        AuxTin2 = 5,
        AuxTin3 = 6,
        AuxTin4 = 7,
        SmBusMaster0 = 8,
        SmBusMaster1 = 9,
        Sensor = 10,
        Peci0 = 16,
        Peci1 = 17,
        PchChipCpuMaxTemp = 18,
        PchChipTemp = 19,
        PchCpuTemp = 20,
        PchMchTemp = 21,
        Agent0Dimm0 = 22,
        Agent0Dimm1 = 23,
        Agent1Dimm0 = 24,
        Agent1Dimm1 = 25,
        ByteTemp0 = 26,
        ByteTemp1 = 27,
        Peci0Cal = 28,
        Peci1Cal = 29,
        VirtualTemp = 31
    }
}
