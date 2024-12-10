using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel.Models
{
    /// <summary>
    /// Write I/O Port Input
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct WriteIoPortInput
    {
        public uint PortNumber;
        public byte Value;
    }
}
