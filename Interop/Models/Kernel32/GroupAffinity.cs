using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    /// <summary>
    /// Represents a processor group-specific affinity, such as the affinity of a thread.
    /// https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-group_affinity
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct GroupAffinity
    {
        public UIntPtr Mask;

        [MarshalAs(UnmanagedType.U2)]
        public ushort Group;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
        public ushort[] Reserved;
    }
}
