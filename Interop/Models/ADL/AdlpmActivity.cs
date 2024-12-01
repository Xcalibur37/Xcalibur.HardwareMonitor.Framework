using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlpmActivity
    {
        public int iSize;
        public int iEngineClock;
        public int iMemoryClock;
        public int iVddc;
        public int iActivityPercent;
        public int iCurrentPerformanceLevel;
        public int iCurrentBusSpeed;
        public int iCurrentBusLanes;
        public int iMaximumBusLanes;
        public int iReserved;
    }
}
