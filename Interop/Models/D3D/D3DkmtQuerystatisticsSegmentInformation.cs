using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_segment_information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct D3DkmtQuerystatisticsSegmentInformation
    {
        public ulong CommitLimit;
        public ulong BytesCommitted;
        public ulong BytesResident;
        public D3DkmtQuerystatisticsMemory Memory;
        public uint Aperture; // boolean
        // D3DKMT_QUERYSTATISTICS_SEGMENT_PREFERENCE_MAX
        public fixed ulong TotalBytesEvictedByPriority[5];
        public ulong SystemMemoryEndAddress;
        public D3DkmtQuerystatisticsSegmentInformationPowerFlags PowerFlags;
        public fixed ulong Reserved[6];
    }
}
