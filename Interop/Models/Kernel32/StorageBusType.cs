using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    public enum StorageBusType
    {
        BusTypeUnknown = 0x00,
        BusTypeScsi,
        BusTypeAtapi,
        BusTypeAta,
        BusType1394,
        BusTypeSsa,
        BusTypeFibre,
        BusTypeUsb,
        BusTypeRaid,
        BusTypeiScsi,
        BusTypeSas,
        BusTypeSata,
        BusTypeSd,
        BusTypeMmc,
        BusTypeVirtual,
        BusTypeFileBackedVirtual,
        BusTypeSpaces,
        BusTypeNvme,
        BusTypeScm,
        BusTypeMax,
        BusTypeMaxReserved = 0x7F
    }
}
