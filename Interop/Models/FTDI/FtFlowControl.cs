namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.FTDI
{
    internal enum FtFlowControl : ushort
    {
        FtFlowDtrDsr = 512,
        FtFlowNone = 0,
        FtFlowRtsCts = 256,
        FtFlowXonXoff = 1024
    }
}
