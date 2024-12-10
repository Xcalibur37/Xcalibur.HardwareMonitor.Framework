using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel.Models
{
    /// <summary>
    /// Write PCI Config Input
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct WritePciConfigInput
    {
        public uint PciAddress;
        public uint RegAddress;
        public uint Value;
    }
}
