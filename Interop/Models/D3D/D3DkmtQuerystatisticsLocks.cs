using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsLocks
    {
        public uint NbLocks;
        public uint NbLocksWaitFlag;
        public uint NbLocksDiscardFlag;
        public uint NbLocksNoOverwrite;
        public uint NbLocksNoReadSync;
        public uint NbLocksLinearization;
        public uint NbComplexLocks;
    }
}
