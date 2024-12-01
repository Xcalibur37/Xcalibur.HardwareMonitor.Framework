using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// The DXGK_NODEMETADATA_FLAGS structure describes the capabilities of an engine on a GPU node.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmdt/ns-d3dkmdt-_dxgk_nodemetadata_flags
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DxgkNodemetadataFlags
    {
        public uint Value;
    }
}
