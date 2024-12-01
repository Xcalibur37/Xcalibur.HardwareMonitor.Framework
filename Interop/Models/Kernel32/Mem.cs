using System;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [Flags]
    public enum Mem : uint
    {
        MemCommit = 0x1000,
        MemReserve = 0x2000,
        MemDecommit = 0x4000,
        MemRelease = 0x8000,
        MemReset = 0x80000,
        MemLargePages = 0x20000000,
        MemPhysical = 0x400000,
        MemTopDown = 0x100000,
        MemWriteWatch = 0x200000
    }
}
