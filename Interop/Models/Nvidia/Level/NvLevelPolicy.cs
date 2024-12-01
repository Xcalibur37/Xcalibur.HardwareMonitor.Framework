namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Level
{
    public enum NvLevelPolicy : uint
    {
        None = 0,
        Manual = 1,
        Performance = 2,
        TemperatureDiscrete = 4,
        TemperatureContinuous = 8,
        Silent = 16,
        Auto = 32
    }
}
