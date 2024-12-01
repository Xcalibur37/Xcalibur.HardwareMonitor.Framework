using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsRenaming
    {
        public uint NbAllocationsRenamed;
        public uint NbAllocationsShrinked;
        public uint NbRenamedBuffer;
        public uint MaxRenamingListLength;
        public uint NbFailuresDueToRenamingLimit;
        public uint NbFailuresDueToCreateAllocation;
        public uint NbFailuresDueToOpenAllocation;
        public uint NbFailuresDueToLowResource;
        public uint NbFailuresDueToNonRetiredLimit;
    }
}
