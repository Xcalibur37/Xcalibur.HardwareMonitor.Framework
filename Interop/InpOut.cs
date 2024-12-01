using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// InpOut32 is a windows DLL and Driver to give direct access to hardware ports.
/// </summary>
internal class InpOut
{
    public delegate IntPtr MapPhysToLinDelegate(IntPtr pbPhysAddr, uint dwPhysSize, out IntPtr pPhysicalMemoryHandle);

    public delegate bool UnmapPhysicalMemoryDelegate(IntPtr physicalMemoryHandle, IntPtr pbLinAddr);

    [DllImport("inpout.dll", EntryPoint = "MapPhysToLin", CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr MapPhysToLin(IntPtr pbPhysAddr, uint dwPhysSize, out IntPtr pPhysicalMemoryHandle);

    [DllImport("inpout.dll", EntryPoint = "UnmapPhysicalMemory", CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnmapPhysicalMemory(IntPtr physicalMemoryHandle, IntPtr pbLinAddr);
}