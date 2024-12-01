namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/et-ee/windows-hardware/drivers/ddi/d3dkmthk/ne-d3dkmthk-d3dkmt_querystatistics_dma_packet_type
    /// </summary>
    internal enum D3DkmtQuerystatisticsDmaPacketType
    {
        D3DkmtClientRenderBuffer = 0,
        D3DkmtClientPagingBuffer = 1,
        D3DkmtSystemPagingBuffer = 2,
        D3DkmtSystemPreemptionBuffer = 3,
        D3DkmtDmaPacketTypeMax
    }
}
