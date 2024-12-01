namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_dma_packet_type_information
    /// </summary>
    internal struct D3DkmtQuerystatisticsDmaPacketTypeInformation
    {
        public uint PacketSubmitted;
        public uint PacketCompleted;
        public uint PacketPreempted;
        public uint PacketFaulted;
    }
}
