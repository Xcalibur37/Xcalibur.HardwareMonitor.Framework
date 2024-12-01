using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_packet_information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsPacketInformation
    {
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket;
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket1;
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket2;
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket3;
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket4;
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket5;
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket6;
        public D3DkmtQuerystatisticsQueuePacketTypeInformation QueuePacket7;

        public D3DkmtQuerystatisticsDmaPacketTypeInformation DmaPacket;
        public D3DkmtQuerystatisticsDmaPacketTypeInformation DmaPacket1;
        public D3DkmtQuerystatisticsDmaPacketTypeInformation DmaPacket2;
        public D3DkmtQuerystatisticsDmaPacketTypeInformation DmaPacket3;
    }
}
