using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BatteryQueryInformation
    {
        public uint BatteryTag;
        public BatteryQueryInformationLevel InformationLevel;
        public uint AtRate;
    }
}
