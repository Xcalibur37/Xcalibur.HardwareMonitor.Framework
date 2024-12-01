using System;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.NT;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// D3DKMT_QUERYSTATISTICS is reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatistics
    {
        public D3DkmtQuerystatisticsType Type;
        public Luid AdapterLuid;
        public IntPtr ProcessHandle;
        public D3DkmtQuerystatisticsResult QueryResult;
        public D3DkmtQuerystatisticsQueryElement QueryElement;
    }
}
