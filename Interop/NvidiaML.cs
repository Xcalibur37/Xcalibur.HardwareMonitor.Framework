using System;
using System.IO;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.ML;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// Nvidia Management Library
/// A C-based API for monitoring and managing various states of the NVIDIA GPU devices.
/// It provides a direct-access to the queries and commands exposed via nvidia-smi.
/// The runtime version of NVML ships with the NVIDIA display driver, and the SDK provides
/// the appropriate header, stub libraries and sample applications.
/// 
/// Each new version of NVML is backwards compatible and is intended to be a platform for
/// building 3rd party applications.
/// </summary>
internal static class NvidiaMl
{
    private const string LinuxDllName = "nvidia-ml";
    private const string WindowsDllName = "nvml.dll";

    private static readonly object _syncRoot = new();

    private static IntPtr _windowsDll;

    private static WindowsNvmlGetHandleDelegate _windowsNvmlDeviceGetHandleByIndex;
    private static WindowsNvmlGetHandleByPciBusIdDelegate _windowsNvmlDeviceGetHandleByPciBusId;
    private static WindowsNvmlDeviceGetPcieThroughputDelegate _windowsNvmlDeviceGetPcieThroughputDelegate;
    private static WindowsNvmlDeviceGetPciInfo _windowsNvmlDeviceGetPciInfo;
    private static WindowsNvmlGetPowerUsageDelegate _windowsNvmlDeviceGetPowerUsage;
    private static WindowsNvmlDelegate _windowsNvmlInit;
    private static WindowsNvmlDelegate _windowsNvmlShutdown;

    private delegate NvmlReturn WindowsNvmlDelegate();

    private delegate NvmlReturn WindowsNvmlGetHandleDelegate(int index, out NvmlDevice device);

    private delegate NvmlReturn WindowsNvmlGetHandleByPciBusIdDelegate([MarshalAs(UnmanagedType.LPStr)] string pciBusId, out NvmlDevice device);

    private delegate NvmlReturn WindowsNvmlGetPowerUsageDelegate(NvmlDevice device, out int power);

    private delegate NvmlReturn WindowsNvmlDeviceGetPcieThroughputDelegate(NvmlDevice device, NvmlPcieUtilCounter counter, out uint value);

    private delegate NvmlReturn WindowsNvmlDeviceGetPciInfo(NvmlDevice device, ref NvmlPciInfo pci);

    public static bool IsAvailable { get; private set; }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <returns></returns>
    public static bool Initialize()
    {
        lock (_syncRoot)
        {
            if (IsAvailable) return true;
            if (Software.OperatingSystem.IsUnix)
            {
                try
                {
                    IsAvailable = nvmlInit() == NvmlReturn.Success;
                }
                catch (DllNotFoundException)
                {
                    // Do nothing
                }
                catch (EntryPointNotFoundException)
                {
                    try
                    {
                        IsAvailable = nvmlInitLegacy() == NvmlReturn.Success;
                    }
                    catch (EntryPointNotFoundException)
                    {
                        // Do nothing
                    }
                }
            }
            else if (IsNvmlCompatibleWindowsVersion())
            {
                // Attempt to load the Nvidia Management Library from the
                // windows standard search order for applications. This will
                // help installations that either have the library in
                // %windir%/system32 or provide their own library
                _windowsDll = Kernel32.LoadLibrary(WindowsDllName);

                // If there is no dll in the path, then attempt to load it
                // from program files
                if (_windowsDll == IntPtr.Zero)
                {
                    string programFilesDirectory = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
                    string dllPath = Path.Combine(programFilesDirectory, @"NVIDIA Corporation\NVSMI", WindowsDllName);

                    _windowsDll = Kernel32.LoadLibrary(dllPath);
                }

                IsAvailable = (_windowsDll != IntPtr.Zero) && InitialiseDelegates() && (_windowsNvmlInit() == NvmlReturn.Success);
            }

            return IsAvailable;
        }
    }

    /// <summary>
    /// Determines whether [is NVML compatible windows version].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is NVML compatible windows version]; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsNvmlCompatibleWindowsVersion() => 
        Software.OperatingSystem.Is64Bit && 
        (Environment.OSVersion.Version.Major > 6 || 
            (Environment.OSVersion.Version.Major == 6 && 
             Environment.OSVersion.Version.Minor >= 1));

    /// <summary>
    /// Initialises the delegates.
    /// </summary>
    /// <returns></returns>
    private static bool InitialiseDelegates()
    {
        IntPtr nvmlInit = Kernel32.GetProcAddress(_windowsDll, "nvmlInit_v2");

        if (nvmlInit != IntPtr.Zero)
        {
            _windowsNvmlInit = (WindowsNvmlDelegate)Marshal.GetDelegateForFunctionPointer(nvmlInit, typeof(WindowsNvmlDelegate));
        }
        else
        {
            nvmlInit = Kernel32.GetProcAddress(_windowsDll, "nvmlInit");
            if (nvmlInit != IntPtr.Zero)
            {
                _windowsNvmlInit = (WindowsNvmlDelegate)Marshal.GetDelegateForFunctionPointer(nvmlInit, typeof(WindowsNvmlDelegate));
            }
            else
            {
                return false;
            }
        }

        IntPtr nvmlShutdown = Kernel32.GetProcAddress(_windowsDll, "nvmlShutdown");
        if (nvmlShutdown != IntPtr.Zero)
        {
            _windowsNvmlShutdown = (WindowsNvmlDelegate)Marshal.GetDelegateForFunctionPointer(nvmlShutdown, typeof(WindowsNvmlDelegate));
        }
        else
        {
            return false;
        }

        IntPtr nvmlGetHandle = Kernel32.GetProcAddress(_windowsDll, "nvmlDeviceGetHandleByIndex_v2");
        if (nvmlGetHandle != IntPtr.Zero)
        {
            _windowsNvmlDeviceGetHandleByIndex = (WindowsNvmlGetHandleDelegate)Marshal.GetDelegateForFunctionPointer(nvmlGetHandle, typeof(WindowsNvmlGetHandleDelegate));
        }
        else
        {
            nvmlGetHandle = Kernel32.GetProcAddress(_windowsDll, "nvmlDeviceGetHandleByIndex");
            if (nvmlGetHandle != IntPtr.Zero)
            {
                _windowsNvmlDeviceGetHandleByIndex = (WindowsNvmlGetHandleDelegate)Marshal.GetDelegateForFunctionPointer(nvmlGetHandle, typeof(WindowsNvmlGetHandleDelegate));
            }
            else
            {
                return false;
            }
        }

        IntPtr nvmlGetPowerUsage = Kernel32.GetProcAddress(_windowsDll, "nvmlDeviceGetPowerUsage");
        if (nvmlGetPowerUsage != IntPtr.Zero)
        {
            _windowsNvmlDeviceGetPowerUsage = (WindowsNvmlGetPowerUsageDelegate)Marshal.GetDelegateForFunctionPointer(nvmlGetPowerUsage, typeof(WindowsNvmlGetPowerUsageDelegate));
        }
        else
        {
            return false;
        }

        IntPtr nvmlGetPcieThroughput = Kernel32.GetProcAddress(_windowsDll, "nvmlDeviceGetPcieThroughput");
        if (nvmlGetPcieThroughput != IntPtr.Zero)
        {
            _windowsNvmlDeviceGetPcieThroughputDelegate = (WindowsNvmlDeviceGetPcieThroughputDelegate)Marshal.GetDelegateForFunctionPointer(nvmlGetPcieThroughput, typeof(WindowsNvmlDeviceGetPcieThroughputDelegate));
        }
        else
        {
            return false;
        }

        IntPtr nvmlGetHandlePciBus = Kernel32.GetProcAddress(_windowsDll, "nvmlDeviceGetHandleByPciBusId_v2");
        if (nvmlGetHandlePciBus != IntPtr.Zero)
        {
            _windowsNvmlDeviceGetHandleByPciBusId = (WindowsNvmlGetHandleByPciBusIdDelegate)Marshal.GetDelegateForFunctionPointer(nvmlGetHandlePciBus, typeof(WindowsNvmlGetHandleByPciBusIdDelegate));
        }
        else
        {
            return false;
        }

        IntPtr nvmlDeviceGetPciInfo = Kernel32.GetProcAddress(_windowsDll, "nvmlDeviceGetPciInfo_v2");
        if (nvmlDeviceGetPciInfo != IntPtr.Zero)
        {
            _windowsNvmlDeviceGetPciInfo = (WindowsNvmlDeviceGetPciInfo)Marshal.GetDelegateForFunctionPointer(nvmlDeviceGetPciInfo, typeof(WindowsNvmlDeviceGetPciInfo));
        }
        else
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public static void Close()
    {
        lock (_syncRoot)
        {
            if (!IsAvailable) return;
            if (Software.OperatingSystem.IsUnix)
            {
                nvmlShutdown();
            }
            else if (_windowsDll != IntPtr.Zero)
            {
                _windowsNvmlShutdown();
                Kernel32.FreeLibrary(_windowsDll);
            }

            IsAvailable = false;
        }
    }

    /// <summary>
    /// Get handle by index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public static NvmlDevice? NvmlDeviceGetHandleByIndex(int index)
    {
        if (!IsAvailable) return null;
        NvmlDevice nvmlDevice;
        if (Software.OperatingSystem.IsUnix)
        {
            try
            {
                if (nvmlDeviceGetHandleByIndex(index, out nvmlDevice) == NvmlReturn.Success)
                {
                    return nvmlDevice;
                }
            }
            catch (EntryPointNotFoundException)
            {
                if (nvmlDeviceGetHandleByIndexLegacy(index, out nvmlDevice) == NvmlReturn.Success)
                {
                    return nvmlDevice;
                }
            }
        }
        else
        {
            try
            {
                if (_windowsNvmlDeviceGetHandleByIndex(index, out nvmlDevice) == NvmlReturn.Success)
                {
                    return nvmlDevice;
                }
            }
            catch
            {
                // Do nothing
            }
        }

        return null;
    }

    /// <summary>
    /// Get handle by PCI bus identifier.
    /// </summary>
    /// <param name="pciBusId">The pci bus identifier.</param>
    /// <returns></returns>
    public static NvmlDevice? NvmlDeviceGetHandleByPciBusId(string pciBusId)
    {
        if (!IsAvailable) return null;
        NvmlDevice nvmlDevice;
        if (Software.OperatingSystem.IsUnix)
        {
            return nvmlDeviceGetHandleByPciBusId(pciBusId, out nvmlDevice) == NvmlReturn.Success
                ? nvmlDevice
                : null;
        }
        else
        {
            try
            {
                if (_windowsNvmlDeviceGetHandleByPciBusId(pciBusId, out nvmlDevice) == NvmlReturn.Success)
                {
                    return nvmlDevice;
                }
            }
            catch
            {
                // Do nothing
            }
        }

        return null;
    }

    /// <summary>
    /// Get power usage.
    /// </summary>
    /// <param name="nvmlDevice">The NVML device.</param>
    /// <returns></returns>
    public static int? NvmlDeviceGetPowerUsage(NvmlDevice nvmlDevice)
    {
        if (!IsAvailable) return null;
        int powerUsage;
        if (Software.OperatingSystem.IsUnix)
        {
            if (nvmlDeviceGetPowerUsage(nvmlDevice, out powerUsage) == NvmlReturn.Success)
            {
                return powerUsage;
            }
        }
        else
        {
            try
            {
                if (_windowsNvmlDeviceGetPowerUsage(nvmlDevice, out powerUsage) == NvmlReturn.Success)
                {
                    return powerUsage;
                }
            }
            catch
            {
                // Do nothing
            }
        }

        return null;
    }

    /// <summary>
    /// Gets PCI-E throughput.
    /// </summary>
    /// <param name="nvmlDevice">The NVML device.</param>
    /// <param name="counter">The counter.</param>
    /// <returns></returns>
    public static uint? NvmlDeviceGetPcieThroughput(NvmlDevice nvmlDevice, NvmlPcieUtilCounter counter)
    {
        if (!IsAvailable) return null;
        uint pcieThroughput;
        if (Software.OperatingSystem.IsUnix)
        {
            if (nvmlDeviceGetPcieThroughput(nvmlDevice, counter, out pcieThroughput) == NvmlReturn.Success)
            {
                return pcieThroughput;
            }
        }
        else
        {
            try
            {
                if (_windowsNvmlDeviceGetPcieThroughputDelegate(nvmlDevice, counter, out pcieThroughput) == NvmlReturn.Success)
                {
                    return pcieThroughput;
                }
            }
            catch
            {
                // Do nothing
            }
        }

        return null;
    }

    /// <summary>
    /// Get PCI information.
    /// </summary>
    /// <param name="nvmlDevice">The NVML device.</param>
    /// <returns></returns>
    public static NvmlPciInfo? NvmlDeviceGetPciInfo(NvmlDevice nvmlDevice)
    {
        if (!IsAvailable) return null;
        var pci = new NvmlPciInfo();

        if (Software.OperatingSystem.IsUnix)
        {
            if (nvmlDeviceGetPciInfo(nvmlDevice, ref pci) == NvmlReturn.Success)
            {
                return pci;
            }
        }
        else
        {
            try
            {
                if (_windowsNvmlDeviceGetPciInfo(nvmlDevice, ref pci) == NvmlReturn.Success)
                {
                    return pci;
                }
            }
            catch
            {
                // Do nothing
            }
        }

        return null;
    }

    [DllImport(LinuxDllName, EntryPoint = "nvmlInit_v2", ExactSpelling = true)]
    private static extern NvmlReturn nvmlInit();

    [DllImport(LinuxDllName, EntryPoint = "nvmlInit", ExactSpelling = true)]
    private static extern NvmlReturn nvmlInitLegacy();

    [DllImport(LinuxDllName, EntryPoint = "nvmlShutdown", ExactSpelling = true)]
    private static extern NvmlReturn nvmlShutdown();

    [DllImport(LinuxDllName, EntryPoint = "nvmlDeviceGetHandleByIndex_v2", ExactSpelling = true)]
    private static extern NvmlReturn nvmlDeviceGetHandleByIndex(int index, out NvmlDevice device);

    [DllImport(LinuxDllName, EntryPoint = "nvmlDeviceGetHandleByPciBusId_v2", ExactSpelling = true)]
    private static extern NvmlReturn nvmlDeviceGetHandleByPciBusId([MarshalAs(UnmanagedType.LPStr)] string pciBusId, out NvmlDevice device);

    [DllImport(LinuxDllName, EntryPoint = "nvmlDeviceGetHandleByIndex", ExactSpelling = true)]
    private static extern NvmlReturn nvmlDeviceGetHandleByIndexLegacy(int index, out NvmlDevice device);

    [DllImport(LinuxDllName, EntryPoint = "nvmlDeviceGetPowerUsage", ExactSpelling = true)]
    private static extern NvmlReturn nvmlDeviceGetPowerUsage(NvmlDevice device, out int power);

    [DllImport(LinuxDllName, EntryPoint = "nvmlDeviceGetPcieThroughput", ExactSpelling = true)]
    private static extern NvmlReturn nvmlDeviceGetPcieThroughput(NvmlDevice device, NvmlPcieUtilCounter counter, out uint value);

    [DllImport(LinuxDllName, EntryPoint = "nvmlDeviceGetPciInfo_v2")]
    private static extern NvmlReturn nvmlDeviceGetPciInfo(NvmlDevice device, ref NvmlPciInfo pci);
}
