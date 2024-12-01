using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    /// <summary>
    /// Structure containing information to start power management logging.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct AdlpmLogStartOutput
    {
        /// Pointer to memory address containing logging data
        [FieldOffset(0)] public IntPtr pLoggingAddress;
        [FieldOffset(0)] public ulong ptrLoggingAddress;
    }
}
