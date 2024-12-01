namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ne-d3dkmthk-d3dkmt_querystatistics_type
    /// </summary>
    internal enum D3DkmtQuerystatisticsType
    {
        D3DkmtQuerystatisticsAdapter,
        D3DkmtQuerystatisticsProcess,
        D3DkmtQuerystatisticsProcessAdapter,
        D3DkmtQuerystatisticsSegment,
        D3DkmtQuerystatisticsProcessSegment,
        D3DkmtQuerystatisticsNode,
        D3DkmtQuerystatisticsProcessNode,
        D3DkmtQuerystatisticsVidpnsource,
        D3DkmtQuerystatisticsProcessVidpnsource,
        D3DkmtQuerystatisticsProcessSegmentGroup,
        D3DkmtQuerystatisticsPhysicalAdapter
    }
}
