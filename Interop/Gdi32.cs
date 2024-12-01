using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

internal static class Gdi32
{
    internal const string DllName = "Gdi32.dll";

    [DllImport(DllName, ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint D3DKMTCloseAdapter(ref D3DkmtCloseadapter closeAdapter);

    [DllImport(DllName, ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint D3DKMTOpenAdapterFromDeviceName(ref D3DkmtOpenadapterfromdevicename openAdapterFromDeviceName);

    [DllImport(DllName, ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint D3DKMTQueryAdapterInfo(ref D3DkmtQueryadapterinfo queryAdapterInfo);

    [DllImport(DllName, ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern uint D3DKMTQueryStatistics(ref D3DkmtQuerystatistics queryStatistics);
}