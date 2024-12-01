using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IdentifyCmdOutParams
    {
        public uint cBufferSize;
        public DriverStatus DriverStatus;
        public IdentifyData Identify;
    }
}
