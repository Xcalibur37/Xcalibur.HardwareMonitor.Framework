using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlpmLogDataOutput
    {
        internal const int AdlPmlogMaxSensors = 256;

        public int size;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = AdlPmlogMaxSensors)]
        public AdlSingleSensorData[] sensors;
    }
}
