namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Thermal
{
    internal enum NvThermalTarget
    {
        None = 0,
        Gpu = 1,
        Memory = 2,
        PowerSupply = 4,
        Board = 8,
        VisualComputingBoard = 9,
        VisualComputingInlet = 10,
        VisualComputingOutlet = 11,
        All = 15,
        Unknown = -1
    }
}
