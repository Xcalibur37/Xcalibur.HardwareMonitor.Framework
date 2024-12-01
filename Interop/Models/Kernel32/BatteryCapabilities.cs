using System;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [Flags]
    public enum BatteryCapabilities : uint
    {
        BATTERY_CAPACITY_RELATIVE = 0x40000000,
        BATTERY_IS_SHORT_TERM = 0x20000000,
        BATTERY_SET_CHARGE_SUPPORTED = 0x00000001,
        BATTERY_SET_DISCHARGE_SUPPORTED = 0x00000002,
        BATTERY_SYSTEM_BATTERY = 0x80000000
    }
}
