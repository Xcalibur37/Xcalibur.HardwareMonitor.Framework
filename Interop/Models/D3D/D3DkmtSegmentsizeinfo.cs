using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// The D3DKMT_SEGMENTSIZEINFO structure describes the size, in bytes, of memory and aperture segments.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-_d3dkmt_segmentsizeinfo
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtSegmentsizeinfo
    {
        public ulong DedicatedVideoMemorySize;
        public ulong DedicatedSystemMemorySize;
        public ulong SharedSystemMemorySize;
    }
}
