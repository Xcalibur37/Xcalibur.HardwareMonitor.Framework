using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios
{
    /// <summary>
    /// Processor characteristics based on <see href="https://www.dmtf.org/dsp/DSP0134">
    /// DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.5.9</see>.
    /// </summary>
    [Flags]
    public enum ProcessorCharacteristics
    {
        None = 0,
        _64BitCapable = 1,
        MultiCore = 2,
        HardwareThread = 4,
        ExecuteProtection = 8,
        EnhancedVirtualization = 16,
        PowerPerformanceControl = 32,
        _128BitCapable = 64
    }
}
