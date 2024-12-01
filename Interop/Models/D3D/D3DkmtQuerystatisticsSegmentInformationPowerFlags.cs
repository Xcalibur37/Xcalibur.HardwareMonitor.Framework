using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct D3DkmtQuerystatisticsSegmentInformationPowerFlags
    {
        [FieldOffset(0)]
        public ulong PreservedDuringStandby;

        [FieldOffset(1)]
        public ulong PreservedDuringHibernate;

        [FieldOffset(2)]
        public ulong PartiallyPreservedDuringHibernate;

        [FieldOffset(3)]
        public ulong Reserved;
    }
}
