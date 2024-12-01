using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.NT
{
    /// <summary>
    /// Describes a local identifier for an adapter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Luid
    {
        public readonly uint LowPart;
        public readonly int HighPart;
    }
}
