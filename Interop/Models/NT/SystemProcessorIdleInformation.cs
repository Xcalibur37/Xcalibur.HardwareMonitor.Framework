using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.NT
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemProcessorIdleInformation
    {
        public long IdleTime;
        public long C1Time;
        public long C2Time;
        public long C3Time;
        public uint C1Transitions;
        public uint C2Transitions;
        public uint C3Transitions;
        public uint Padding;
    }
}
