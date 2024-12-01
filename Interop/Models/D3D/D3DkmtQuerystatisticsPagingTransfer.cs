using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatstics_paging_transfer
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsPagingTransfer
    {
        public ulong BytesFilled;
        public ulong BytesDiscarded;
        public ulong BytesMappedIntoAperture;
        public ulong BytesUnmappedFromAperture;
        public ulong BytesTransferredFromMdlToMemory;
        public ulong BytesTransferredFromMemoryToMdl;
        public ulong BytesTransferredFromApertureToMemory;
        public ulong BytesTransferredFromMemoryToAperture;
    }
}
