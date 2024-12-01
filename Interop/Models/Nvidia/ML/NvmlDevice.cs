using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.ML
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NvmlDevice
    {
        public IntPtr Handle;
    }
}
