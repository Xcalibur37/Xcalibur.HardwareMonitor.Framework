using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.NT;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// NTDLL is a dynamic link library (DLL) that acts as the user-mode interface to the Windows kernel.
/// https://learn.microsoft.com/en-us/windows-hardware/drivers/kernel/libraries-and-headers
/// </summary>
internal class NtDll
{
    private const string DllName = "ntdll.dll";

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern int NtQuerySystemInformation(SystemInformationClass systemInformationClass, [Out] SystemProcessorPerformanceInformation[] systemInformation, int systemInformationLength, out int returnLength);

    [DllImport(DllName)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern int NtQuerySystemInformation(SystemInformationClass systemInformationClass, [Out] SystemProcessorIdleInformation[] systemInformation, int systemInformationLength, out int returnLength);
}
