namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios
{
    /// <summary>
    /// Processor type based on <see href="https://www.dmtf.org/dsp/DSP0134">
    /// DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.5.1</see>.
    /// </summary>
    public enum ProcessorType
    {
        Other = 1,
        Unknown,
        CentralProcessor,
        MathProcessor,
        DspProcessor,
        VideoProcessor
    }
}
