using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlFanSpeedInfo
    {
        public int iSize;
        public int iFlags;
        public int iMinPercent;
        public int iMaxPercent;
        public int iMinRPM;
        public int iMaxRPM;
    }
}
