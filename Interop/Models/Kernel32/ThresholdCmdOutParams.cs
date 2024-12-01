using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ThresholdCmdOutParams
    {
        public const int MaxDriveAttributes = 512;

        public uint cBufferSize;
        public DriverStatus DriverStatus;
        public byte Version;
        public byte Reserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxDriveAttributes)]
        public SmartThreshold[] Thresholds;
    }
}
