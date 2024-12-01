using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_adapter_information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsAdapterInformation
    {
        public uint NbSegments;
        public uint NodeCount;
        public uint VidPnSourceCount;

        public uint VSyncEnabled;
        public uint TdrDetectedCount;

        public long ZeroLengthDmaBuffers;
        public ulong RestartedPeriod;

        public D3DkmtQuerystatisticsReferenceDmaBuffer ReferenceDmaBuffer;
        public D3DkmtQuerystatisticsRenaming Renaming;
        public D3DkmtQuerystatisticsPrepration Preparation;
        public D3DkmtQuerystatisticsPagingFault PagingFault;
        public D3DkmtQuerystatisticsPagingTransfer PagingTransfer;
        public D3DkmtQuerystatisticsSwizzlingRange SwizzlingRange;
        public D3DkmtQuerystatisticsLocks Locks;
        public D3DkmtQuerystatisticsAllocations Allocations;
        public D3DkmtQuerystatisticsTerminations Terminations;

        private readonly ulong Reserved;
        private readonly ulong Reserved1;
        private readonly ulong Reserved2;
        private readonly ulong Reserved3;
        private readonly ulong Reserved4;
        private readonly ulong Reserved5;
        private readonly ulong Reserved6;
        private readonly ulong Reserved7;
    }
}
