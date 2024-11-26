﻿using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct ReadPciConfigInput
    {
        public uint PciAddress;
        public uint RegAddress;
    }
}
