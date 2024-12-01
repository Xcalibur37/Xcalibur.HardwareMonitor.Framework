using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Specifies the type of display device that the graphics adapter supports.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-_d3dkmt_adaptertype
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtAdaptertype
    {
        public D3DkmtAdaptertypeFlags Value;
    }
}
