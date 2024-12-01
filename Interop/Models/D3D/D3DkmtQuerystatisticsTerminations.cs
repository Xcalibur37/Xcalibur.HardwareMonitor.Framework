using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsTerminations
    {
        public D3DkmtQuerystatisticsCounter TerminatedShared;
        public D3DkmtQuerystatisticsCounter TerminatedNonShared;
        public D3DkmtQuerystatisticsCounter DestroyedShared;
        public D3DkmtQuerystatisticsCounter DestroyedNonShared;
    }
}
