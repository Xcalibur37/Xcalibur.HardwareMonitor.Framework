namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios
{
    /// <summary>
    /// Memory type.
    /// </summary>
    public enum MemoryType
    {
        Other = 0x01,
        Unknown = 0x02,
        DRAM = 0x03,
        EDRAM = 0x04,
        VRAM = 0x05,
        SRAM = 0x06,
        RAM = 0x07,
        ROM = 0x08,
        FLASH = 0x09,
        EEPROM = 0x0a,
        FEPROM = 0x0b,
        EPROM = 0x0c,
        CDRAM = 0x0d,
        _3DRAM = 0x0e,
        SDRAM = 0x0f,
        SGRAM = 0x10,
        RDRAM = 0x11,
        DDR = 0x12,
        DDR2 = 0x13,
        DDR2_FBDIMM = 0x14,
        DDR3 = 0x18,
        FBD2 = 0x19,
        DDR4 = 0x1a,
        LPDDR = 0x1b,
        LPDDR2 = 0x1c,
        LPDDR3 = 0x1d,
        LPDDR4 = 0x1e,
        LogicalNonVolatileDevice = 0x1f,
        HBM = 0x20,
        HBM2 = 0x21,
        DDR5 = 0x22,
        LPDDR5 = 0x23
    }
}
