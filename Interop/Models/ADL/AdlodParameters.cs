using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlodParameters
    {
        public int iSize;
        public int iNumberOfPerformanceLevels;
        public int iActivityReportingSupported;
        public int iDiscretePerformanceLevels;
        public int iReserved;
        public AdlodParameterRange sEngineClock;
        public AdlodParameterRange sMemoryClock;
        public AdlodParameterRange sVddc;
    }
}
