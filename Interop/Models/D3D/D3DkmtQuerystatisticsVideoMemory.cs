using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_video_memory
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsVideoMemory
    {
        public uint AllocsCommitted;
        public D3DkmtQuerystatisticsCounter AllocsResidentIn0;
        public D3DkmtQuerystatisticsCounter AllocsResidentIn1;
        public D3DkmtQuerystatisticsCounter AllocsResidentIn2;
        public D3DkmtQuerystatisticsCounter AllocsResidentIn3;
        public D3DkmtQuerystatisticsCounter AllocsResidentIn4;
        public D3DkmtQuerystatisticsCounter AllocsResidentInNonPreferred;
        public ulong TotalBytesEvictedDueToPreparation;
    }
}
