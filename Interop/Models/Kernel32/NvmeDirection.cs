using System;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [Flags]
    public enum NvmeDirection : uint
    {
        NvmeFromHostToDev = 1,
        NvmeFromDevToHost = 2,
        NvmeBiDirection = NvmeFromDevToHost | NvmeFromHostToDev
    }
}
