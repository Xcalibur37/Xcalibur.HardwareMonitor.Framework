using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// The DXGK_NODEMETADATA structure describes an engine on a GPU node.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmdt/ns-d3dkmdt-_dxgk_nodemetadata
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct DxgkNodemetadata
    {
        public DxgkEngineType EngineType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FriendlyName;

        public DxgkNodemetadataFlags Flags;
        public byte GpuMmuSupported;
        public byte IoMmuSupported;
    }
}
