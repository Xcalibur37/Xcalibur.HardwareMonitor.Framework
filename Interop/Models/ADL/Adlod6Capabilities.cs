using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Adlod6Capabilities
    {
        public int iCapabilities;
        public int iSupportedStates;
        public int iNumberOfPerformanceLevels;
        public AdlodParameterRange sEngineClockRange;
        public AdlodParameterRange sMemoryClockRange;
        public int iExtValue;
        public int iExtMask;
    }
}
