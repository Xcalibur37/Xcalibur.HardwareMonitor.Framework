using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_result
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct D3DkmtQuerystatisticsResult
    {
        [FieldOffset(0), MarshalAs(UnmanagedType.Struct)]
        public D3DkmtQuerystatisticsAdapterInformation AdapterInformation;

        [FieldOffset(0), MarshalAs(UnmanagedType.Struct)]
        public D3DkmtQuerystatisticsSegmentInformation SegmentInformation;

        [FieldOffset(0), MarshalAs(UnmanagedType.Struct)]
        public D3DkmtQuerystatisticsProcessSegmentInformation ProcessSegmentInformation;

        [FieldOffset(0), MarshalAs(UnmanagedType.Struct)]
        public D3DkmtQuerystatisticsNodeInformation NodeInformation;

        // D3DKMT_QUERYSTATISTICS_PROCESS_INFORMATION ProcessInformation;
        // D3DKMT_QUERYSTATISTICS_PROCESS_NODE_INFORMATION ProcessNodeInformation;
        // D3DKMT_QUERYSTATISTICS_PHYSICAL_ADAPTER_INFORMATION PhysAdapterInformation;
        // D3DKMT_QUERYSTATISTICS_SEGMENT_INFORMATION_V1 SegmentInformationV1; // WIN7
        // D3DKMT_QUERYSTATISTICS_VIDPNSOURCE_INFORMATION VidPnSourceInformation;
        // D3DKMT_QUERYSTATISTICS_PROCESS_ADAPTER_INFORMATION ProcessAdapterInformation;
        // D3DKMT_QUERYSTATISTICS_PROCESS_VIDPNSOURCE_INFORMATION ProcessVidPnSourceInformation;
        // D3DKMT_QUERYSTATISTICS_PROCESS_SEGMENT_GROUP_INFORMATION ProcessSegmentGroupInformation;
    }
}
