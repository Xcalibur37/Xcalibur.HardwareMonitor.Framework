namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Cooler
{
    internal enum NvCoolerTarget
    {
        None = 0,
        Gpu,
        Memory,
        PowerSupply = 4,
        All = 7 // This cooler cools all of the components related to its target gpu.
    }
}
