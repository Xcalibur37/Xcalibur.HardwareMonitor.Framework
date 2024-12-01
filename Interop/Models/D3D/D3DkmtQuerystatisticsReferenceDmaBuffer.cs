using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsReferenceDmaBuffer
    {
        public uint NbCall;
        public uint NbAllocationsReferenced;
        public uint MaxNbAllocationsReferenced;
        public uint NbNULLReference;
        public uint NbWriteReference;
        public uint NbRenamedAllocationsReferenced;
        public uint NbIterationSearchingRenamedAllocation;
        public uint NbLockedAllocationReferenced;
        public uint NbAllocationWithValidPrepatchingInfoReferenced;
        public uint NbAllocationWithInvalidPrepatchingInfoReferenced;
        public uint NbDMABufferSuccessfullyPrePatched;
        public uint NbPrimariesReferencesOverflow;
        public uint NbAllocationWithNonPreferredResources;
        public uint NbAllocationInsertedInMigrationTable;
    }
}
