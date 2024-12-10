using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel.Models
{
    /// <summary>
    /// Read PCI Config Input
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ReadPciConfigInput
    {
        public uint PciAddress;
        public uint RegAddress;
    }
}
