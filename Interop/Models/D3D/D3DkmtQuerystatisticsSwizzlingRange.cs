using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsSwizzlingRange
    {
        public uint NbRangesAcquired;
        public uint NbRangesReleased;
    }
}
