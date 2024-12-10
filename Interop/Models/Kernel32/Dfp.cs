namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    /// <summary>
    /// For displaying the details of hard drives in a command window
    /// </summary>
    public enum Dfp : uint
    {
        DfpGetVersion = 0x00074080,
        DfpSendDriveCommand = 0x0007c084,
        DfpReceiveDriveData = 0x0007c088
    }
}
