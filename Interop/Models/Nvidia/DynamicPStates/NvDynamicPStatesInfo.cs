using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.DynamicPStates
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvDynamicPStatesInfo
    {
        public const int MAX_GPU_UTILIZATIONS = 8;

        public uint Version;
        public uint Flags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_GPU_UTILIZATIONS)]
        public NvDynamicPState[] Utilizations;
    }
}
