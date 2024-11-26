namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios
{
    /// <summary>
    /// Cache associativity based on <see href="https://www.dmtf.org/dsp/DSP0134">
    /// DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.8.5</see>.
    /// </summary>
    public enum CacheAssociativity
    {
        Other = 1,
        Unknown,
        DirectMapped,
        _2Way,
        _4Way,
        FullyAssociative,
        _8Way,
        _16Way,
        _12Way,
        _24Way,
        _32Way,
        _48Way,
        _64Way,
        _20Way
    }
}
