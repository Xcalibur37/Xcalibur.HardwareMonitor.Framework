using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Each KMTQUERYADAPTERINFOTYPE value correlates to a specific piece of adapter information being retrieved by D3DKMQueryAdapterInfo.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmthk/ne-d3dkmthk-_kmtqueryadapterinfotype
    /// </summary>
    internal enum Kmtqueryadapterinfotype
    {
        KmtqaitypeUmdriverprivate = 0,
        KmtqaitypeUmdrivername = 1,
        KmtqaitypeUmopenglinfo = 2,
        KmtqaitypeGetsegmentsize = 3,
        KmtqaitypeAdapterguid = 4,
        KmtqaitypeFlipqueueinfo = 5,
        KmtqaitypeAdapteraddress = 6,
        KmtqaitypeSetworkingsetinfo = 7,
        KmtqaitypeAdapterregistryinfo = 8,
        KmtqaitypeCurrentdisplaymode = 9,
        KmtqaitypeModelist = 10,
        KmtqaitypeCheckdriverupdatestatus = 11,
        KmtqaitypeVirtualaddressinfo = 12,
        KmtqaitypeDriverversion = 13,
        KmtqaitypeAdaptertype = 15,
        KmtqaitypeOutputduplcontextscount = 16,
        KmtqaitypeWddm12Caps = 17,
        KmtqaitypeUmdDriverVersion = 18,
        KmtqaitypeDirectflipSupport = 19,
        KmtqaitypeMultiplaneoverlaySupport = 20,
        KmtqaitypeDlistDriverName = 21,
        KmtqaitypeWddm13Caps = 22,
        KmtqaitypeMultiplaneoverlayHudSupport = 23,
        KmtqaitypeWddm20Caps = 24,
        KmtqaitypeNodemetadata = 25,
        KmtqaitypeCpdrivername = 26,
        KmtqaitypeXbox = 27,
        KmtqaitypeIndependentflipSupport = 28,
        KmtqaitypeMiracastcompaniondrivername = 29,
        KmtqaitypePhysicaladaptercount = 30,
        KmtqaitypePhysicaladapterdeviceids = 31,
        KmtqaitypeDrivercapsExt = 32,
        KmtqaitypeQueryMiracastDriverType = 33,
        KmtqaitypeQueryGpummuCaps = 34,
        KmtqaitypeQueryMultiplaneoverlayDecodeSupport = 35,
        KmtqaitypeQueryHwProtectionTeardownCount = 36,
        KmtqaitypeQueryIsbaddriverforhwprotectiondisabled = 37,
        KmtqaitypeMultiplaneoverlaySecondarySupport = 38,
        KmtqaitypeIndependentflipSecondarySupport = 39,
        KmtqaitypePanelfitterSupport = 40,
        KmtqaitypePhysicaladapterpnpkey = 41,
        KmtqaitypeGetsegmentgroupsize = 42,
        KmtqaitypeMpo3DdiSupport = 43,
        KmtqaitypeHwdrmSupport = 44,
        KmtqaitypeMpokernelcapsSupport = 45,
        KmtqaitypeMultiplaneoverlayStretchSupport = 46,
        KmtqaitypeGetDeviceVidpnOwnershipInfo = 47,
        KmtqaitypeQueryregistry = 48,
        KmtqaitypeKmdDriverVersion = 49,
        KmtqaitypeBlocklistKernel = 50,
        KmtqaitypeBlocklistRuntime = 51,
        KmtqaitypeAdapterguidRender = 52,
        KmtqaitypeAdapteraddressRender = 53,
        KmtqaitypeAdapterregistryinfoRender = 54,
        KmtqaitypeCheckdriverupdatestatusRender = 55,
        KmtqaitypeDriverversionRender = 56,
        KmtqaitypeAdaptertypeRender = 57,
        KmtqaitypeWddm12CapsRender = 58,
        KmtqaitypeWddm13CapsRender = 59,
        KmtqaitypeQueryAdapterUniqueGuid = 60,
        KmtqaitypeNodeperfdata = 61,
        KmtqaitypeAdapterperfdata = 62,
        KmtqaitypeAdapterperfdataCaps = 63,
        KmtquitypeGpuversion = 64,
        KmtqaitypeDriverDescription = 65,
        KmtqaitypeDriverDescriptionRender = 66,
        KmtqaitypeScanoutCaps = 67,
        KmtqaitypeDisplayUmdrivername = 71,
        KmtqaitypeParavirtualizationRender = 68,
        KmtqaitypeServicename = 69,
        KmtqaitypeWddm27Caps = 70,
        KmtqaitypeTrackedworkloadSupport = 72
    }
}
