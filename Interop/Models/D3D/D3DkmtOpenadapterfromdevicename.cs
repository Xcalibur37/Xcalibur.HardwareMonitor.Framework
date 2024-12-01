using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.NT;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// The D3DKMT_OPENADAPTERFROMDEVICENAME structure describes the mapping of the given name of a device to a graphics
    /// adapter handle and monitor output.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-_d3dkmt_openadapterfromdevicename
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtOpenadapterfromdevicename
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDeviceName;

        public uint hAdapter;
        public Luid AdapterLuid;
    }
}
