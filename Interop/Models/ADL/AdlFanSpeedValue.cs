using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlFanSpeedValue
    {
        public int iSize;
        public int iSpeedType;
        public int iFanSpeed;
        public int iFlags;
    }
}
