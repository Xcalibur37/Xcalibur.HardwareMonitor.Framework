namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios
{
    /// <summary>
    /// System wake-up type based on <see href="https://www.dmtf.org/dsp/DSP0134">
    /// DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.2.2</see>.
    /// </summary>
    public enum SystemWakeUp
    {
        Reserved,
        Other,
        Unknown,
        ApmTimer,
        ModemRing,
        LanRemote,
        PowerSwitch,
        PciPme,
        AcPowerRestored
    }
}
