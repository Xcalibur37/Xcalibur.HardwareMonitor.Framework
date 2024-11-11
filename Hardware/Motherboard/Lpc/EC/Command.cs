namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC
{
    /// <summary>
    /// EC Command
    /// </summary>
    internal enum Command : byte
    {
        Read = 0x80, // RD_EC
        Write = 0x81, // WR_EC
        BurstEnable = 0x82, // BE_EC
        BurstDisable = 0x83, // BD_EC
        Query = 0x84 // QR_EC
    }
}
