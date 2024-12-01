using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.NT;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_process_node_information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct D3DkmtQuerystatisticsProcessNodeInformation
    {
        public LargeInteger RunningTime; // 100ns
        public uint ContextSwitch;
        private readonly D3DkmtQuerystatisticsPreemptionInformation PreemptionStatistics;
        private readonly D3DkmtQuerystatisticsPacketInformation PacketStatistics;
        private fixed ulong Reserved[8];
    }
}
