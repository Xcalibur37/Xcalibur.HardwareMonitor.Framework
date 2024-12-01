using System;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [Flags]
    internal enum D3DkmtAdaptertypeFlags : uint
    {
        RenderSupported = 0,
        DisplaySupported = 1,
        SoftwareDevice = 2,
        PostDevice = 4,
        HybridDiscrete = 8,
        HybridIntegrated = 16,
        IndirectDisplayDevice = 32,
        Paravirtualized = 64,
        AcgSupported = 128,
        SupportSetTimingsFromVidPn = 256,
        Detachable = 512,
        ComputeOnly = 1024,
        Prototype = 2045
    }
}
