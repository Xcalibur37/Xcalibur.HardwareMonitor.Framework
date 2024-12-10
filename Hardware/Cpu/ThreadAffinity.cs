using System;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu;

internal static class ThreadAffinity
{
    /// <summary>
    /// Initializes static members of the <see cref="ThreadAffinity" /> class.
    /// </summary>
    static ThreadAffinity()
    {
        ProcessorGroupCount = Software.OperatingSystem.IsUnix ? 1 : Kernel32.GetActiveProcessorGroupCount();
        if (ProcessorGroupCount >= 1) return;
        ProcessorGroupCount = 1;
    }

    /// <summary>
    /// Gets the processor group count.
    /// </summary>
    public static int ProcessorGroupCount { get; }

    /// <summary>
    /// Sets the processor group affinity for the current thread.
    /// </summary>
    /// <param name="affinity">The processor group affinity.</param>
    /// <returns>The previous processor group affinity.</returns>
    public static GroupAffinity Set(GroupAffinity affinity)
    {
        if (affinity == GroupAffinity.Undefined)
        {
            return GroupAffinity.Undefined;
        }

        // Unix only
        if (Software.OperatingSystem.IsUnix)
        {
            if (affinity.Group > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(affinity));
            }

            // Get processor affinity
            ulong result = 0;
            if (LibC.sched_getaffinity(0, 8, ref result) != 0)
            {
                return GroupAffinity.Undefined;
            }

            // Set processor affinity
            ulong mask = affinity.Mask;
            return LibC.sched_setaffinity(0, 8, ref mask) != 0
                ? GroupAffinity.Undefined
                : new GroupAffinity(0, result);
        }

        ulong maxValue = IntPtr.Size == 8 ? ulong.MaxValue : uint.MaxValue;
        if (affinity.Mask > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(affinity));
        }

        // Processor group affinity
        var groupAffinity = new Interop.Models.Kernel32.GroupAffinity
        {
            Group = affinity.Group, 
            Mask = (UIntPtr)affinity.Mask
        };
        IntPtr currentThread = Kernel32.GetCurrentThread();
        return Kernel32.SetThreadGroupAffinity(currentThread, ref groupAffinity, out Interop.Models.Kernel32.GroupAffinity previousGroupAffinity)
            ? new GroupAffinity(previousGroupAffinity.Group, (ulong)previousGroupAffinity.Mask)
            : GroupAffinity.Undefined;
    }
}
