using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    ///  https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_process_segment_information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct D3DkmtQuerystatisticsProcessSegmentInformation
    {
        public ulong BytesCommitted;
        public ulong MaximumWorkingSet;
        public ulong MinimumWorkingSet;

        public uint NbReferencedAllocationEvictedInPeriod;

        public D3DkmtQuerystatisticsVideoMemory VideoMemory;
        public D3DkmtQuerystatisticsProcessSegmentPolicy _Policy;

        public fixed ulong Reserved[8];
    }
}
