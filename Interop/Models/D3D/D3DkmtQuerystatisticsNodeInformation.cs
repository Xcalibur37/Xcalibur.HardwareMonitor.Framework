using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_node_information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct D3DkmtQuerystatisticsNodeInformation
    {
        public D3DkmtQuerystatisticsProcessNodeInformation GlobalInformation; // global

        public D3DkmtQuerystatisticsProcessNodeInformation SystemInformation; // system thread

        //public UInt32 NodeId; // Win10
        public fixed ulong Reserved[8];
    }
}
