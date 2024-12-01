namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    /// <summary>
    /// For displaying the details of hard drives in a command window
    /// </summary>
    public enum Dfp : uint
    {
        DFP_GET_VERSION = 0x00074080,
        DFP_SEND_DRIVE_COMMAND = 0x0007c084,
        DFP_RECEIVE_DRIVE_DATA = 0x0007c088
    }
}
