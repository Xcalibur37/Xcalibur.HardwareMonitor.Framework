using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// The D3DKMT_QUERYADAPTERINFO structure retrieves various information that describes the adapter.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-_d3dkmt_queryadapterinfo
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQueryadapterinfo
    {
        public uint hAdapter;
        public Kmtqueryadapterinfotype Type;
        public IntPtr pPrivateDriverData;
        public int PrivateDriverDataSize;
    }
}
