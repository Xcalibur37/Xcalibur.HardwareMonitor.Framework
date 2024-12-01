using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.NT
{
    /// <summary>
    /// Represents a 64-bit signed integer value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    internal struct LargeInteger
    {
        [FieldOffset(0)]
        public long QuadPart;

        [FieldOffset(0)]
        public uint LowPart;

        [FieldOffset(4)]
        public int HighPart;
    }
}
