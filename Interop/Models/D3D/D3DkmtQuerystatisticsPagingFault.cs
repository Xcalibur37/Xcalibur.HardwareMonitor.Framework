using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatstics_paging_fault
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsPagingFault
    {
        public D3DkmtQuerystatisticsCounter Faults;
        public D3DkmtQuerystatisticsCounter FaultsFirstTimeAccess;
        public D3DkmtQuerystatisticsCounter FaultsReclaimed;
        public D3DkmtQuerystatisticsCounter FaultsMigration;
        public D3DkmtQuerystatisticsCounter FaultsIncorrectResource;
        public D3DkmtQuerystatisticsCounter FaultsLostContent;
        public D3DkmtQuerystatisticsCounter FaultsEvicted;
        public D3DkmtQuerystatisticsCounter AllocationsMEM_RESET;
        public D3DkmtQuerystatisticsCounter AllocationsUnresetSuccess;
        public D3DkmtQuerystatisticsCounter AllocationsUnresetFail;

        public uint AllocationsUnresetSuccessRead;
        public uint AllocationsUnresetFailRead;

        public D3DkmtQuerystatisticsCounter Evictions;
        public D3DkmtQuerystatisticsCounter EvictionsDueToPreparation;
        public D3DkmtQuerystatisticsCounter EvictionsDueToLock;
        public D3DkmtQuerystatisticsCounter EvictionsDueToClose;
        public D3DkmtQuerystatisticsCounter EvictionsDueToPurge;
        public D3DkmtQuerystatisticsCounter EvictionsDueToSuspendCPUAccess;
    }
}
