using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel.Models
{
    /// <summary>
    /// Write MSR Input
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct WriteMsrInput
    {
        public uint Register;
        public ulong Value;
    }
}
