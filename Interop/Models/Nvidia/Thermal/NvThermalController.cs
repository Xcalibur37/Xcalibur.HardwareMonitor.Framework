namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Thermal
{
    internal enum NvThermalController
    {
        None = 0,
        GpuInternal,
        Adm1032,
        Max6649,
        Max1617,
        Lm99,
        Lm89,
        Lm64,
        Adt7473,
        SbMax6649,
        VBiosEvt,
        OS,
        Unknown = -1
    }
}
