using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlodnPerformanceStatus
    {
        public int iCoreClock;
        public int iMemoryClock;
        public int iDCEFClock;
        public int iGFXClock;
        public int iUVDClock;
        public int iVCEClock;
        public int iGPUActivityPercent;
        public int iCurrentCorePerformanceLevel;
        public int iCurrentMemoryPerformanceLevel;
        public int iCurrentDCEFPerformanceLevel;
        public int iCurrentGFXPerformanceLevel;
        public int iUVDPerformanceLevel;
        public int iVCEPerformanceLevel;
        public int iCurrentBusSpeed;
        public int iCurrentBusLanes;
        public int iMaximumBusLanes;
        public int iVDDC;
        public int iVDDCI;
    }
}
