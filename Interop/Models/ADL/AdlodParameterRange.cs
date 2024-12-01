using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlodParameterRange
    {
        public int iMin;
        public int iMax;
        public int iStep;
    }
}
