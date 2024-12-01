using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.FTDI
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FtHandle
    {
        private readonly IntPtr _handle;
    }
}
