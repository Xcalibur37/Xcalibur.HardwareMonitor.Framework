using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL;

namespace Xcalibur.HardwareMonitor.Framework.Interop;

/// <summary>
/// AMD ADL Interop
/// </summary>
internal static class AtiAdlxx
{
    public const int AdlDlFanctrlFlagUserDefinedSpeed = 1;

    public const int AdlDlFanctrlSpeedTypePercent = 1;
    public const int AdlDlFanctrlSpeedTypeRpm = 2;

    public const int AdlDlFanctrlSupportsPercentRead = 1;
    public const int AdlDlFanctrlSupportsPercentWrite = 2;
    public const int AdlDlFanctrlSupportsRpmRead = 4;
    public const int AdlDlFanctrlSupportsRpmWrite = 8;

    public const int AdlDriverOk = 0;
    public const int AdlFalse = 0;

    public const int AdlMaxAdapters = 40;
    public const int AdlMaxDevicename = 32;
    public const int AdlMaxDisplays = 40;
    public const int AdlMaxGlsyncPortLeds = 8;
    public const int AdlMaxGlsyncPorts = 8;
    public const int AdlMaxNumDisplaymodes = 1024;
    public const int AdlMaxPath = 256;
    public const int AdlTrue = 1;

    public const int AtiVendorId = 0x1002;

    internal const int AdlPmlogMaxSensors = 256;

    internal const string DllName = "atiadlxx.dll";

    public static Context ContextAlloc = Marshal.AllocHGlobal;

    // create a Main_Memory_Alloc delegate and keep it alive
    public static AdlMainMemoryAllocDelegate MainMemoryAlloc = Marshal.AllocHGlobal;

    public delegate IntPtr AdlMainMemoryAllocDelegate(int size);

    public delegate IntPtr Context(int size);

    /// <summary>
    /// This function retrieves the current Overdrive parameters for a specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive5_ODParameters_Get(IntPtr context, int adapterIndex, out AdlodParameters parameters);

    /// <summary>
    /// This function retrieves current power management-related activity for a specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="iAdapterIndex">Index of the i adapter.</param>
    /// <param name="activity">The activity.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive5_CurrentActivity_Get(IntPtr context, int iAdapterIndex, ref AdlpmActivity activity);

    /// <summary>
    /// This function retrieves thermal controller temperature information for a specified adapter and thermal controller.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="thermalControllerIndex">Index of the thermal controller.</param>
    /// <param name="temperature">The temperature.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive5_Temperature_Get(IntPtr context, int adapterIndex, int thermalControllerIndex, ref AdlTemperature temperature);

    /// <summary>
    /// This function retrieves the current temperature for a specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="iTemperatureType">Type of the i temperature.</param>
    /// <param name="temp">The temporary.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_OverdriveN_Temperature_Get(IntPtr context, int adapterIndex, AdlodnTemperatureType iTemperatureType, ref int temp);

    /// <summary>
    /// This function retrieves the reported fan speed from a specified thermal controller.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="thermalControllerIndex">Index of the thermal controller.</param>
    /// <param name="fanSpeedValue">The fan speed value.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive5_FanSpeed_Get(IntPtr context, int adapterIndex, int thermalControllerIndex, ref AdlFanSpeedValue fanSpeedValue);

    /// <summary>
    /// his function retrieves the fan speed reporting capability for a specified adapter and thermal controller.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="thermalControllerIndex">Index of the thermal controller.</param>
    /// <param name="fanSpeedInfo">The fan speed information.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive5_FanSpeedInfo_Get(IntPtr context, int adapterIndex, int thermalControllerIndex, ref AdlFanSpeedInfo fanSpeedInfo);

    /// <summary>
    /// This function sets the current fan speed for a specified adapter and thermal controller to the default fan speed.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="thermalControllerIndex">Index of the thermal controller.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive5_FanSpeedToDefault_Set(IntPtr context, int adapterIndex, int thermalControllerIndex);

    /// <summary>
    /// This function sets the fan speed for a specified adapter and thermal controller.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="thermalControllerIndex">Index of the thermal controller.</param>
    /// <param name="fanSpeedValue">The fan speed value.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive5_FanSpeed_Set(IntPtr context, int adapterIndex, int thermalControllerIndex, ref AdlFanSpeedValue fanSpeedValue);

    /// <summary>
    /// This function retrieves the current OD performance for a specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="performanceStatus">The performance status.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_OverdriveN_PerformanceStatus_Get(IntPtr context, int adapterIndex, out AdlodnPerformanceStatus performanceStatus);

    /// <summary>
    /// This function retrieves current power management capabilities for a specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="supported">The supported.</param>
    /// <param name="enabled">The enabled.</param>
    /// <param name="version">The version.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive_Caps(IntPtr context, int adapterIndex, ref int supported, ref int enabled, ref int version);

    /// <summary>
    /// This function retrieves the current Overdrive capabilities for a specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="lpOdCapabilities">The lp od capabilities.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive6_Capabilities_Get(IntPtr context, int adapterIndex, ref Adlod6Capabilities lpOdCapabilities);

    /// <summary>
    /// Function returns the current power of the specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="powerType">Type of the power.</param>
    /// <param name="currentValue">The current value.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Overdrive6_CurrentPower_Get(IntPtr context, int adapterIndex, AdlodnCurrentPowerType powerType, ref int currentValue);

    /// <summary>
    /// Function to initialize the ADL2 interface and to obtain client's context handle.
    /// Clients can use ADL2 versions of ADL APIs to assure that there is no interference with other ADL clients that are running
    /// in the same process.Such clients have to call ADL2_Main_Control_Create first to obtain ADL_CONTEXT_HANDLE instance that has
    /// to be passed to each subsequent ADL2 call and finally destroyed using ADL2_Main_Control_Destroy. ADL initialized using
    /// ADL2_Main_Control_Create will not enforce serialization of ADL API executions by multiple threads.Multiple threads will
    /// be allowed to enter to ADL at the same time.Note that ADL library is not guaranteed to be thread-safe.Client that calls
    /// ADL2_Main_Control_Create have to provide its own mechanism for ADL calls serialization.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <param name="connectedAdapters">The connected adapters.</param>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Main_Control_Create(AdlMainMemoryAllocDelegate callback, int connectedAdapters, ref IntPtr context);

    /// <summary>
    /// Destroy client's ADL context. Clients can use ADL2 versions of ADL APIs to assure that there is no
    /// interference with other ADL clients that are running in the same process and to assure that ADL APIs
    /// are thread-safe.Such clients have to call ADL2_Main_Control_Create first to obtain ADL_CONTEXT_HANDLE
    /// instance that has to be passed to each subsequent ADL2 call and finally destroyed using
    /// ADL2_Main_Control_Destroy.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Main_Control_Destroy(IntPtr context);

    /// <summary>
    /// This function retrieves the Overdrive8 current settings for a specified adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="aDlpmLogDataOutput">a DLPM log data output.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_New_QueryPMLogData_Get(IntPtr context, int adapterIndex, ref AdlpmLogDataOutput aDlpmLogDataOutput);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_FrameMetrics_Caps(IntPtr context, int adapterIndex, ref int supported);

    /// <summary>
    /// Get frame metrics monitoring Data on GPU (identified by adapter id). Will detect if in crossfire and Get data from all slaves as well.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="displayIndex">The display index.</param>
    /// <param name="fps">The FPS.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_FrameMetrics_Get(IntPtr context, int adapterIndex, int displayIndex, ref float fps);

    /// <summary>
    /// Start frame metrics monitoring on GPU (identified by adapter id). Will detect if in crossfire and start all slaves as well.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="displayIndex">The display index.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_FrameMetrics_Start(IntPtr context, int adapterIndex, int displayIndex);

    /// <summary>
    /// Stop frame metrics monitoring on GPU (identified by adapter id). Will detect if in crossfire and stop all slaves as well.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="displayIndex">The display index.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_FrameMetrics_Stop(IntPtr context, int adapterIndex, int displayIndex);

    /// <summary>
    /// This function implements a call to retrieve power management logging support.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="pPmLogSupportInfo">The p pm log support information.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_PMLog_Support_Get(IntPtr context, int adapterIndex,
                                                                  ref AdlpmLogSupportInfo pPmLogSupportInfo);

    /// <summary>
    /// This function implements a call to start power management logging.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="pPmLogStartInput">The p pm log start input.</param>
    /// <param name="pPmLogStartOutput">The p pm log start output.</param>
    /// <param name="device">The device.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_PMLog_Start(IntPtr context, int adapterIndex,
                                                            ref AdlpmLogStartInput pPmLogStartInput,
                                                            ref AdlpmLogStartOutput pPmLogStartOutput,
                                                            uint device);

    /// <summary>
    /// This function implements a call to stop power management logging.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="device">The device.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_PMLog_Stop(IntPtr context, int adapterIndex, uint device);

    /// <summary>
    /// This function create the device. Adds MGPU support over legacy functions.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="device">The device.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Device_PMLog_Device_Create(IntPtr context, int adapterIndex, ref uint device);

    /// <summary>
    /// This function destroy the device. Adds MGPU support over legacy functions.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="device">The device.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Device_PMLog_Device_Destroy(IntPtr context, uint device);

    /// <summary>
    /// For given ASIC returns information about components of ASIC GCN architecture. Such as number of compute units,
    /// number of Tex (Texture filtering units) , number of ROPs (render back-ends).
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="gcnInfo">The GCN information.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_GcnAsicInfo_Get(IntPtr context, int adapterIndex, ref AdlGcnInfo gcnInfo);

    /// <summary>
    /// Function to retrieve the number of OS-known adapters. This function retrieves the number of
    /// graphics adapters recognized by the OS(OS-known adapters). OS-known adapters can include adapters
    /// that are physically present in the system(logical adapters) as well as ones that no longer present
    /// in the system but are still recognized by the OS.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="numAdapters">The number adapters.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_NumberOfAdapters_Get(IntPtr context, ref int numAdapters);

    /// <summary>
    /// Retrieves all OS-known adapter information. This function retrieves the adapter information of all OS-known
    /// adapters in the system.OS-known adapters can include adapters that are physically present in the system
    /// (logical adapters) as well as ones that are no longer present in the system but are still recognized by the OS.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterInfo">The adapter information.</param>
    /// <param name="size">The size.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_AdapterInfo_Get(IntPtr context, IntPtr adapterInfo, int size);

    /// <summary>
    /// Function to get the unique identifier of an adapter. This function retrieves the unique identifier of a
    /// specified adapter. The adapter ID is a unique value and will be used to determine what other controllers
    /// share the same adapter. The desktop will use this to find which HDCs are associated with an adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="adapterId">The adapter identifier.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_ID_Get(IntPtr context, int adapterIndex, out int adapterId);

    /// <summary>
    /// The function is used to check if the adapter associated with iAdapterIndex is active which has enabled desktop mapped on it.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_Active_Get(IntPtr context, int adapterIndex, out int status);

    /// <summary>
    /// The function is used to get Dedicated VRAM usge by calling MS Counter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="iVramUsageInMb">The i vram usage in mb.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_DedicatedVRAMUsage_Get(IntPtr context, int adapterIndex, out int iVramUsageInMb);

    /// <summary>
    /// This function retrieves the memory information for a specified graphics adapter.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="memoryInfo">The memory information.</param>
    /// <returns></returns>
    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static extern AdlStatus ADL2_Adapter_MemoryInfoX4_Get(IntPtr context, int adapterIndex, out AdlMemoryInfoX4 memoryInfo);

    /// <summary>
    /// Determines if the ADL method exists.
    /// </summary>
    /// <param name="adlMethod">The adl method.</param>
    /// <returns></returns>
    public static bool ADL_Method_Exists(string adlMethod)
    {
        IntPtr module = Kernel32.LoadLibrary(DllName);
        if (module == IntPtr.Zero) return false;
        bool result = Kernel32.GetProcAddress(module, adlMethod) != IntPtr.Zero;
        Kernel32.FreeLibrary(module);
        return result;
    }

    /// <summary>
    /// This function retrieves the adapter information of all OS-known adapters in the system. OS-known adapters
    /// can include adapters that are physically present in the system (logical adapters) as well as ones that are
    /// no longer present in the system but are still recognized by the OS.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="info">The information.</param>
    /// <returns></returns>
    
    public static AdlStatus ADL2_Adapter_AdapterInfo_Get(ref IntPtr context, AdlAdapterInfo[] info)
    {
        int elementSize = Marshal.SizeOf(typeof(AdlAdapterInfo));
        int size = info.Length * elementSize;
        IntPtr ptr = Marshal.AllocHGlobal(size);
        AdlStatus result = ADL2_Adapter_AdapterInfo_Get(context, ptr, size);
        for (int i = 0; i < info.Length; i++)
        {
            info[i] = (AdlAdapterInfo)Marshal.PtrToStructure((IntPtr)((long)ptr + (i * elementSize)), typeof(AdlAdapterInfo))!;
        }

        Marshal.FreeHGlobal(ptr);

        // the ADLAdapterInfo.VendorID field reported by ADL is wrong on
        // Windows systems (parse error), so we fix this here
        for (int i = 0; i < info.Length; i++)
        {
            // try Windows UDID format
            var match = Regex.Match(info[i].UDID, "PCI_VEN_([A-Fa-f0-9]{1,4})&.*");
            if (match.Success && match.Groups.Count == 2)
            {
                info[i].VendorID = Convert.ToInt32(match.Groups[1].Value, 16);
                continue;
            }

            // if above failed, try Unix UDID format
            match = Regex.Match(info[i].UDID, "[0-9]+:[0-9]+:([0-9]+):[0-9]+:[0-9]+");
            if (match.Success && match.Groups.Count == 2)
            {
                info[i].VendorID = Convert.ToInt32(match.Groups[1].Value, 10);
            }
        }

        return result;
    }

    /// <summary>
    /// Uses the pm log for family.
    /// </summary>
    /// <param name="familyId">The family identifier.</param>
    /// <returns></returns>
    public static bool UsePmLogForFamily(int familyId) => familyId >= (int)GcnFamilies.FamilyAi;
}
