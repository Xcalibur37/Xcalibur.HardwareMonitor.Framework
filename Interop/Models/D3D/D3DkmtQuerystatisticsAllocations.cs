using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsAllocations
    {
        public D3DkmtQuerystatisticsCounter Created;
        public D3DkmtQuerystatisticsCounter Destroyed;
        public D3DkmtQuerystatisticsCounter Opened;
        public D3DkmtQuerystatisticsCounter Closed;
        public D3DkmtQuerystatisticsCounter MigratedSuccess;
        public D3DkmtQuerystatisticsCounter MigratedFail;
        public D3DkmtQuerystatisticsCounter MigratedAbandoned;
    }
}
