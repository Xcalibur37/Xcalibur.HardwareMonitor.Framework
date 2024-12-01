namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    internal enum AdlodnTemperatureType
    {
        // This typed is named like this in the documentation but for some reason AMD failed to include it...
        // Yet it seems these correspond with ADL_PMLOG_TEMPERATURE_xxx.
        Edge = 1,
        Mem = 2,
        Vrvddc = 3,
        Vrmvdd = 4,
        Liquid = 5,
        Plx = 6,
        Hotspot = 7
    }
}
