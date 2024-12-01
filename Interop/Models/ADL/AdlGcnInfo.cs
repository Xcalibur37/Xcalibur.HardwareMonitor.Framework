using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    /// <summary>
    /// Structure containing information about components of ASIC GCN architecture.
    /// Elements of GCN info are compute units, number of Tex(Texture filtering units) , number of ROPs(render back-ends).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AdlGcnInfo
    {
        public int CuCount; //Number of compute units on the ASIC.
        public int TexCount; //Number of texture mapping units.
        public int RopCount; //Number of Render backend Units.

        // see GCNFamilies enum, references:
        //        https://gitlab.freedesktop.org/mesa/mesa/-/blob/main/src/amd/addrlib/src/amdgpu_asic_addr.h
        //        https://github.com/torvalds/linux/blob/master/include/uapi/drm/amdgpu_drm.h
        public int ASICFamilyId;
        public int ASICRevisionId;
    }
}
