namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC
{
    /// <summary>
    /// EC Status
    /// </summary>
    internal enum Status : byte
    {
        OutputBufferFull = 0x01, // EC_OBF
        InputBufferFull = 0x02, // EC_IBF
        Command = 0x08, // CMD
        BurstMode = 0x10, // BURST
        SciEventPending = 0x20, // SCI_EVT
        SmiEventPending = 0x40 // SMI_EVT
    }
}
