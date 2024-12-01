using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct D3DkmtNodemetadata
    {
        public uint NodeOrdinalAndAdapterIndex;
        public DxgkNodemetadata NodeData;
    }
}
