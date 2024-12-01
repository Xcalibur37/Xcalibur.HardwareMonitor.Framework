namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    public enum StorageProtocolType
    {
        ProtocolTypeUnknown = 0x00,
        ProtocolTypeScsi,
        ProtocolTypeAta,
        ProtocolTypeNvme,
        ProtocolTypeSd,
        ProtocolTypeProprietary = 0x7E,
        ProtocolTypeMaxReserved = 0x7F
    }
}
