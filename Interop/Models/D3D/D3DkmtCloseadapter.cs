using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// The D3DKMT_CLOSEADAPTER structure specifies the adapter to close.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-_d3dkmt_closeadapter
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtCloseadapter
    {
        public uint hAdapter;
    }
}
