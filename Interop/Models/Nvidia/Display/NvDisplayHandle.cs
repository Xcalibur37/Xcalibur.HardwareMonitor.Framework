using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Display
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NvDisplayHandle
    {
        private readonly IntPtr ptr;
    }
}
