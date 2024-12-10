namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.Intel
{
    /// <summary>
    /// Intel Constants
    /// </summary>
    internal class IntelConstants
    {
        internal const uint Ia32PackageThermStatus = 0x1B1;
        internal const uint Ia32PerfStatus = 0x0198;
        internal const uint Ia32TemperatureTarget = 0x01A2;
        internal const uint Ia32ThermStatusMsr = 0x019C;

        internal const uint MsrDramEnergyStatus = 0x619;
        internal const uint MsrPkgEnergyStatus = 0x611;
        internal const uint MsrPlatformInfo = 0xCE;
        internal const uint MsrPp0EnergyStatus = 0x639;
        internal const uint MsrPp1EnergyStatus = 0x641;
        internal const uint MsrPlatformEnergyStatus = 0x64D;
        internal const uint MsrRaplPowerUnit = 0x606;
    }
}
