using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlTemperature
    {
        public int iSize;
        public int iTemperature;
    }
}
