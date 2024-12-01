using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct D3DkmtQuerystatisticsQueryElement
    {
        [FieldOffset(0)]
        public D3DkmtQuerystatisticsQuerySegment QuerySegment;

        [FieldOffset(0)]
        public D3DkmtQuerystatisticsQuerySegment QueryProcessSegment;

        [FieldOffset(0)]
        public D3DkmtQuerystatisticsQueryNode QueryNode;

        [FieldOffset(0)]
        public D3DkmtQuerystatisticsQueryNode QueryProcessNode;

        [FieldOffset(0)]
        public D3DkmtQuerystatisticsQueryVidpnsource QueryVidPnSource;

        [FieldOffset(0)]
        public D3DkmtQuerystatisticsQueryVidpnsource QueryProcessVidPnSource;
    }
}
