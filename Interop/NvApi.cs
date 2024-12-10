using System;
using System.Runtime.InteropServices;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Cooler;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Display;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.DynamicPStates;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Fans;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.GPU;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Power;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Thermal;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Usages;

namespace Xcalibur.HardwareMonitor.Framework.Interop;
/// <summary>
/// NVAPI provides you the ability to query and manipulate the way that 3D Vision Automatic works on behalf of your application;
/// along with allowing you access to important internal variables.
/// https://docs.nvidia.com/gameworks/content/technologies/desktop/nv3dva_using_nvapi.htm
/// </summary>
internal static class NvApi
{
    public const int MAX_CLOCKS_PER_GPU = 0x120;
    public const int MAX_COOLERS_PER_GPU = 20;
    public const int MAX_FAN_CONTROLLER_ITEMS = 32;
    public const int MAX_FAN_COOLERS_STATUS_ITEMS = 32;
    public const int MAX_GPU_PUBLIC_CLOCKS = 32;
    public const int MAX_GPU_UTILIZATIONS = 8;
    public const int MAX_MEMORY_VALUES_PER_GPU = 5;
    public const int MAX_PHYSICAL_GPUS = 64;
    public const int MAX_POWER_TOPOLOGIES = 4;
    public const int MAX_THERMAL_SENSORS_PER_GPU = 3;
    public const int MAX_USAGES_PER_GPU = 8;

    public const int SHORT_STRING_MAX = 64;
    public const int THERMAL_SENSOR_RESERVED_COUNT = 8;
    public const int THERMAL_SENSOR_TEMPERATURE_COUNT = 32;

    private const string DllName = "nvapi.dll";
    private const string DllName64 = "nvapi64.dll";

    public static readonly NvAPI_EnumNvidiaDisplayHandleDelegate NvAPI_EnumNvidiaDisplayHandle;
    public static readonly NvAPI_EnumPhysicalGPUsDelegate NvAPI_EnumPhysicalGPUs;
    public static readonly NvAPI_GetDisplayDriverVersionDelegate NvAPI_GetDisplayDriverVersion;
    public static readonly NvAPI_GetPhysicalGPUsFromDisplayDelegate NvAPI_GetPhysicalGPUsFromDisplay;
    public static readonly NvAPI_GPU_ClientFanCoolersGetControlDelegate NvAPI_GPU_ClientFanCoolersGetControl;
    public static readonly NvAPI_GPU_ClientFanCoolersGetStatusDelegate NvAPI_GPU_ClientFanCoolersGetStatus;
    public static readonly NvAPI_GPU_ClientFanCoolersSetControlDelegate NvAPI_GPU_ClientFanCoolersSetControl;
    public static readonly NvAPI_GPU_ClientPowerTopologyGetStatusDelegate NvAPI_GPU_ClientPowerTopologyGetStatus;
    public static readonly NvAPI_GPU_GetAllClockFrequenciesDelegate NvAPI_GPU_GetAllClockFrequencies;
    public static readonly NvAPI_GPU_GetAllClocksDelegate NvAPI_GPU_GetAllClocks;
    public static readonly NvAPI_GPU_GetBusIdDelegate NvAPI_GPU_GetBusId;
    public static readonly NvAPI_GPU_GetCoolerSettingsDelegate NvAPI_GPU_GetCoolerSettings;
    public static readonly NvAPI_GPU_GetDynamicPstatesInfoExDelegate NvAPI_GPU_GetDynamicPstatesInfoEx;
    public static readonly NvAPI_GPU_GetMemoryInfoDelegate NvAPI_GPU_GetMemoryInfo;
    public static readonly NvAPI_GPU_GetPCIIdentifiersDelegate NvAPI_GPU_GetPCIIdentifiers;
    public static readonly NvAPI_GPU_GetTachReadingDelegate NvAPI_GPU_GetTachReading;
    public static readonly NvAPI_GPU_GetThermalSettingsDelegate NvAPI_GPU_GetThermalSettings;
    public static readonly NvAPI_GPU_GetUsagesDelegate NvAPI_GPU_GetUsages;
    public static readonly NvAPI_GPU_SetCoolerLevelsDelegate NvAPI_GPU_SetCoolerLevels;
    public static readonly NvAPI_GPU_GetThermalSensorsDelegate NvAPI_GPU_ThermalGetSensors;

    private static readonly NvAPI_GetInterfaceVersionStringDelegate _nvAPI_GetInterfaceVersionString;
    private static readonly NvAPI_GPU_GetFullNameDelegate _nvAPI_GPU_GetFullName;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_EnumNvidiaDisplayHandleDelegate(int thisEnum, ref NvDisplayHandle displayHandle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_EnumPhysicalGPUsDelegate([Out] NvPhysicalGpuHandle[] gpuHandles, out int gpuCount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GetDisplayDriverVersionDelegate(NvDisplayHandle displayHandle, [In, Out] ref NvDisplayDriverVersion nvDisplayDriverVersion);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GetInterfaceVersionStringDelegate(StringBuilder version);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GetPhysicalGPUsFromDisplayDelegate(NvDisplayHandle displayHandle, [Out] NvPhysicalGpuHandle[] gpuHandles, out uint gpuCount);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_ClientFanCoolersGetControlDelegate(NvPhysicalGpuHandle gpuHandle, ref NvFanCoolerControl control);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_ClientFanCoolersGetStatusDelegate(NvPhysicalGpuHandle gpuHandle, ref NvFanCoolersStatus fanCoolersStatus);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_ClientFanCoolersSetControlDelegate(NvPhysicalGpuHandle gpuHandle, ref NvFanCoolerControl control);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_ClientPowerTopologyGetStatusDelegate(NvPhysicalGpuHandle gpuHandle, ref NvPowerTopology powerTopology);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetAllClockFrequenciesDelegate(NvPhysicalGpuHandle gpuHandle, ref NvGpuClockFrequencies clockFrequencies);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetAllClocksDelegate(NvPhysicalGpuHandle gpuHandle, ref NvClocks nvClocks);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetBusIdDelegate(NvPhysicalGpuHandle gpuHandle, out uint busId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetCoolerSettingsDelegate(NvPhysicalGpuHandle gpuHandle, NvCoolerTarget coolerTarget, ref NvCoolerSettings NvCoolerSettings);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetDynamicPstatesInfoExDelegate(NvPhysicalGpuHandle gpuHandle, ref NvDynamicPStatesInfo nvPStates);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetMemoryInfoDelegate(NvDisplayHandle displayHandle, ref NvMemoryInfo nvMemoryInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetPCIIdentifiersDelegate(NvPhysicalGpuHandle gpuHandle, out uint deviceId, out uint subSystemId, out uint revisionId, out uint extDeviceId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetTachReadingDelegate(NvPhysicalGpuHandle gpuHandle, out int value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetThermalSensorsDelegate(NvPhysicalGpuHandle gpuHandle, ref NvThermalSensors nvThermalSensors);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetThermalSettingsDelegate(NvPhysicalGpuHandle gpuHandle, int sensorIndex, ref NvThermalSettings NvThermalSettings);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_GetUsagesDelegate(NvPhysicalGpuHandle gpuHandle, ref NvUsages nvUsages);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate NvStatus NvAPI_GPU_SetCoolerLevelsDelegate(NvPhysicalGpuHandle gpuHandle, int coolerIndex, ref NvCoolerLevels NvCoolerLevels);


    /// <summary>
    /// Gets a value indicating whether this instance is available.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
    /// </value>
    public static bool IsAvailable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NvApi"/> class.
    /// </summary>
    static NvApi()
    {
        NvAPI_InitializeDelegate nvApiInitialize;

        try
        {
            if (!DllExists()) return;
            GetDelegate(0x0150E828, out nvApiInitialize);
        }
        catch (Exception e) when (e is DllNotFoundException or ArgumentNullException or EntryPointNotFoundException or BadImageFormatException)
        {
            return;
        }

        if (nvApiInitialize() != NvStatus.OK) return;
        GetDelegate(0xE3640A56, out NvAPI_GPU_GetThermalSettings);
        GetDelegate(0xCEEE8E9F, out _nvAPI_GPU_GetFullName);
        GetDelegate(0x9ABDD40D, out NvAPI_EnumNvidiaDisplayHandle);
        GetDelegate(0x34EF9506, out NvAPI_GetPhysicalGPUsFromDisplay);
        GetDelegate(0xE5AC921F, out NvAPI_EnumPhysicalGPUs);
        GetDelegate(0x5F608315, out NvAPI_GPU_GetTachReading);
        GetDelegate(0x1BD69F49, out NvAPI_GPU_GetAllClocks);
        GetDelegate(0x60DED2ED, out NvAPI_GPU_GetDynamicPstatesInfoEx);
        GetDelegate(0x189A1FDF, out NvAPI_GPU_GetUsages);
        GetDelegate(0xDA141340, out NvAPI_GPU_GetCoolerSettings);
        GetDelegate(0x891FA0AE, out NvAPI_GPU_SetCoolerLevels);
        GetDelegate(0x774AA982, out NvAPI_GPU_GetMemoryInfo);
        GetDelegate(0xF951A4D1, out NvAPI_GetDisplayDriverVersion);
        GetDelegate(0x01053FA5, out _nvAPI_GetInterfaceVersionString);
        GetDelegate(0x2DDFB66E, out NvAPI_GPU_GetPCIIdentifiers);
        GetDelegate(0x1BE0B8E5, out NvAPI_GPU_GetBusId);
        GetDelegate(0x35AED5E8, out NvAPI_GPU_ClientFanCoolersGetStatus);
        GetDelegate(0xDCB616C3, out NvAPI_GPU_GetAllClockFrequencies);
        GetDelegate(0x814B209F, out NvAPI_GPU_ClientFanCoolersGetControl);
        GetDelegate(0xA58971A5, out NvAPI_GPU_ClientFanCoolersSetControl);
        GetDelegate(0x0EDCF624E, out NvAPI_GPU_ClientPowerTopologyGetStatus);
        GetDelegate(0x65FE3AAD, out NvAPI_GPU_ThermalGetSensors);

        IsAvailable = true;
    }

    public static NvStatus NvAPI_GPU_GetFullName(NvPhysicalGpuHandle gpuHandle, out string name)
    {
        StringBuilder builder = new(SHORT_STRING_MAX);
        NvStatus status = _nvAPI_GPU_GetFullName?.Invoke(gpuHandle, builder) ?? NvStatus.FunctionNotFound;

        name = builder.ToString();
        return status;
    }

    public static NvStatus NvAPI_GetInterfaceVersionString(out string version)
    {
        StringBuilder builder = new(SHORT_STRING_MAX);
        NvStatus status = _nvAPI_GetInterfaceVersionString?.Invoke(builder) ?? NvStatus.FunctionNotFound;

        version = builder.ToString();
        return status;
    }
    
    public static bool DllExists()
    {
        IntPtr module = Kernel32.LoadLibrary(Environment.Is64BitProcess ? DllName64 : DllName);
        if (module == IntPtr.Zero)
        {
            return false;
        }

        Kernel32.FreeLibrary(module);
        return true;
    }

    internal static int MAKE_NVAPI_VERSION<T>(int ver) => Marshal.SizeOf<T>() | (ver << 16);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate NvStatus NvAPI_InitializeDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate NvStatus NvAPI_GPU_GetFullNameDelegate(NvPhysicalGpuHandle gpuHandle, StringBuilder name);

    [DllImport(DllName, EntryPoint = "nvapi_QueryInterface", CallingConvention = CallingConvention.Cdecl, PreserveSig = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern IntPtr NvAPI32_QueryInterface(uint interfaceId);

    [DllImport(DllName64, EntryPoint = "nvapi_QueryInterface", CallingConvention = CallingConvention.Cdecl, PreserveSig = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern IntPtr NvAPI64_QueryInterface(uint interfaceId);

    private static void GetDelegate<T>(uint id, out T newDelegate) where T : class
    {
        IntPtr ptr = Environment.Is64BitProcess ? NvAPI64_QueryInterface(id) : NvAPI32_QueryInterface(id);
        newDelegate = ptr != IntPtr.Zero ? Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T : null;
    }
}
