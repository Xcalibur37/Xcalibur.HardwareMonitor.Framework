using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatstics_prepration
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsPrepration
    {
        public uint BroadcastStall;
        public uint NbDMAPrepared;
        public uint NbDMAPreparedLongPath;
        public uint ImmediateHighestPreparationPass;
        public D3DkmtQuerystatisticsCounter AllocationsTrimmed;
    }
}
