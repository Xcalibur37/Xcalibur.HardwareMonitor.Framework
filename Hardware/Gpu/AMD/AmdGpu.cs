using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.D3d;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.AMD;

/// <summary>
/// AMD GPU
/// </summary>
/// <seealso cref="GpuBase" />
internal sealed class AmdGpu : GpuBase
{
    #region Fields

    private AtiAdlxx.ADLAdapterInfo _adapterInfo;
    private AtiAdlxx.ADLPMLogStartOutput _adlPmLogStartOutput;
    private AtiAdlxx.ADLPMLogSupportInfo _adlPmLogSupportInfo;
    private AtiAdlxx.ADLGcnInfo _adlGcnInfo;
    private IntPtr _context;
    private Sensor _controlSensor;
    private Sensor _coreClock;
    private Sensor _coreLoad;
    private Sensor _coreVoltage;
    private int _currentOverdriveApiLevel;
    private string _d3dDeviceId;
    private uint _device;
    private Sensor _fan;
    private Control _fanControl;
    private bool _frameMetricsStarted;
    private Sensor _fullscreenFps;
    private Sensor _gpuDedicatedMemoryUsage;
    private Sensor _gpuDedicatedMemoryFree;
    private Sensor _gpuDedicatedMemoryTotal;
    private Sensor[] _gpuNodeUsage;
    private DateTime[] _gpuNodeUsagePrevTick;
    private long[] _gpuNodeUsagePrevValue;
    private Sensor _gpuSharedMemoryUsage;
    private Sensor _gpuSharedMemoryFree;
    private Sensor _gpuSharedMemoryTotal;
    private Sensor _memoryClock;
    private Sensor _memoryLoad;
    private Sensor _memoryVoltage;
    private Sensor _memoryTotal;
    private Sensor _memoryUsed;
    private Sensor _memoryFree;
    private bool _overdriveApiSupported;
    private bool _pmLogStarted;
    private Sensor _powerCore;
    private Sensor _powerPpt;
    private Sensor _powerSoC;
    private Sensor _powerTotal;
    private Sensor _socClock;
    private Sensor _socVoltage;
    private Sensor _temperatureCore;
    private Sensor _temperatureHotSpot;
    private Sensor _temperatureLiquid;
    private Sensor _temperatureMemory;
    private Sensor _temperatureMvdd;
    private Sensor _temperaturePlx;
    private Sensor _temperatureSoC;
    private Sensor _temperatureVddc;
    private readonly ushort _pmLogSampleRate = 1000;
    private bool _overdrive8LogExists;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the bus number.
    /// </summary>
    /// <value>
    /// The bus number.
    /// </value>
    public int BusNumber { get; }

    /// <inheritdoc />
    public override string DeviceId => _adapterInfo.PNPString;

    /// <summary>
    /// Gets the device number.
    /// </summary>
    /// <value>
    /// The device number.
    /// </value>
    public int DeviceNumber { get; }

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.GpuAmd;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AmdGpu"/> class.
    /// </summary>
    /// <param name="amdContext">The amd context.</param>
    /// <param name="adapterInfo">The adapter information.</param>
    /// <param name="gcnInfo">The GCN information.</param>
    /// <param name="settings">The settings.</param>
    public AmdGpu(IntPtr amdContext, AtiAdlxx.ADLAdapterInfo adapterInfo, AtiAdlxx.ADLGcnInfo gcnInfo, ISettings settings)
        : base(adapterInfo.AdapterName.Trim(), new Identifier("gpu-amd",
            adapterInfo.AdapterIndex.ToString(CultureInfo.InvariantCulture)), settings)
    {
        _context = amdContext;
        _adlGcnInfo = gcnInfo;
        _adapterInfo = adapterInfo;
        BusNumber = adapterInfo.BusNumber;
        DeviceNumber = adapterInfo.DeviceNumber;

        // Create sensors
        CreateSensors();
        
        // Update
        Update();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Close()
    {
        _fanControl.ControlModeChanged -= ControlModeChanged;
        _fanControl.SoftwareControlValueChanged -= SoftwareControlValueChanged;

        if (_fanControl.ControlMode != ControlMode.Undefined)
        {
            SetDefaultFanSpeed();
        }

        if (_frameMetricsStarted)
        {
            AtiAdlxx.ADL2_Adapter_FrameMetrics_Stop(_context, _adapterInfo.AdapterIndex, 0);
        }

        if (_pmLogStarted && _device != 0)
        {
            AtiAdlxx.ADL2_Adapter_PMLog_Stop(_context, _adapterInfo.AdapterIndex, _device);
        }

        if (_device != 0)
        {
            AtiAdlxx.ADL2_Device_PMLog_Device_Destroy(_context, _device);
        }

        base.Close();
    }

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override void Update()
    {
        UpdateD3dSensors();
        UpdateLoadSensors();
        UpdateFpsSensors();
        UpdateTemperatureSensors();
        UpdateFanSensors();
        UpdatePowerSensors();

        if (!_overdriveApiSupported) return;
        GetOd5CurrentActivity();
    }

    /// <summary>
    /// Software control value changed.
    /// </summary>
    /// <param name="control">The control.</param>
    private void SoftwareControlValueChanged(IControl control)
    {
        if (control.ControlMode != ControlMode.Software) return;
        AtiAdlxx.ADLFanSpeedValue fanSpeedValue = new()
        {
            iSpeedType = AtiAdlxx.ADL_DL_FANCTRL_SPEED_TYPE_PERCENT,
            iFlags = AtiAdlxx.ADL_DL_FANCTRL_FLAG_USER_DEFINED_SPEED,
            iFanSpeed = (int)control.SoftwareValue
        };
        AtiAdlxx.ADL2_Overdrive5_FanSpeed_Set(_context, _adapterInfo.AdapterIndex, 0, ref fanSpeedValue);
    }

    /// <summary>
    /// Controls the mode changed.
    /// </summary>
    /// <param name="control">The control.</param>
    private void ControlModeChanged(IControl control)
    {
        switch (control.ControlMode)
        {
            case ControlMode.Undefined:
                return;
            case ControlMode.Default:
                SetDefaultFanSpeed();
                break;
            case ControlMode.Software:
                SoftwareControlValueChanged(control);
                break;
            default:
                return;
        }
    }

    /// <summary>
    /// Sets the default fan speed.
    /// </summary>
    private void SetDefaultFanSpeed()
    {
        AtiAdlxx.ADL2_Overdrive5_FanSpeedToDefault_Set(_context, _adapterInfo.AdapterIndex, 0);
    }

    /// <summary>
    /// Creates the sensors.
    /// </summary>
    private void CreateSensors()
    {
        CreateTemperatureSensors();
        CreateClockSensors();
        CreateFanSensors();
        CreateVoltageSensors();
        CreateLoadSensors();
        CreateControlSensors();

        int frameMetricsSupported = 0;
        CreateFpsSensors(ref frameMetricsSupported);
        CreatePowerSensors(ref frameMetricsSupported);
        CreateD3dSensors();
    }

    /// <summary>
    /// Create temperature sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateTemperatureSensors()
    {
        _temperatureCore = new Sensor("GPU Core", 0, SensorType.Temperature, this, Settings);
        _temperatureMemory = new Sensor("GPU Memory", 1, SensorType.Temperature, this, Settings);
        _temperatureVddc = new Sensor("GPU VR VDDC", 2, SensorType.Temperature, this, Settings);
        _temperatureMvdd = new Sensor("GPU VR MVDD", 3, SensorType.Temperature, this, Settings);
        _temperatureSoC = new Sensor("GPU VR SoC", 4, SensorType.Temperature, this, Settings);
        _temperatureLiquid = new Sensor("GPU Liquid", 5, SensorType.Temperature, this, Settings);
        _temperaturePlx = new Sensor("GPU PLX", 6, SensorType.Temperature, this, Settings);
        _temperatureHotSpot = new Sensor("GPU Hot Spot", 7, SensorType.Temperature, this, Settings);
    }

    /// <summary>
    /// Update temperature sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateTemperatureSensors()
    {
        if (!_overdriveApiSupported) return;
        GetOd5Temperature(_temperatureCore);

        if (_currentOverdriveApiLevel < 7) return;
        GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType.EDGE, _temperatureCore, -256, 0.001, false);
        GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType.MEM, _temperatureMemory);
        GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType.VRVDDC, _temperatureVddc);
        GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType.VRMVDD, _temperatureMvdd);
        GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType.LIQUID, _temperatureLiquid);
        GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType.PLX, _temperaturePlx);
        GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType.HOTSPOT, _temperatureHotSpot);
    }

    /// <summary>
    /// Create clock sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateClockSensors()
    {
        _coreClock = new Sensor("GPU Core", 0, SensorType.Clock, this, Settings);
        _socClock = new Sensor("GPU SoC", 1, SensorType.Clock, this, Settings);
        _memoryClock = new Sensor("GPU Memory", 2, SensorType.Clock, this, Settings);
    }

    /// <summary>
    /// Create fan sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateFanSensors()
    {
        _fan = new Sensor("GPU Fan", 0, SensorType.Fan, this, Settings);
    }

    /// <summary>
    /// Update fan sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateFanSensors()
    {
        if (!_overdriveApiSupported) return;
        GetOd5FanSpeed(AtiAdlxx.ADL_DL_FANCTRL_SPEED_TYPE_RPM, _fan);
        GetOd5FanSpeed(AtiAdlxx.ADL_DL_FANCTRL_SPEED_TYPE_PERCENT, _controlSensor);
    }

    /// <summary>
    /// Create voltage sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateVoltageSensors()
    {
        _coreVoltage = new Sensor("GPU Core", 0, SensorType.Voltage, this, Settings);
        _memoryVoltage = new Sensor("GPU Memory", 1, SensorType.Voltage, this, Settings);
        _socVoltage = new Sensor("GPU SoC", 2, SensorType.Voltage, this, Settings);
    }

    /// <summary>
    /// Create load sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateLoadSensors()
    {
        _coreLoad = new Sensor("GPU Core", 0, SensorType.Load, this, Settings);
        _memoryLoad = new Sensor("GPU Memory", 1, SensorType.Load, this, Settings);
        _memoryUsed = new Sensor("GPU Memory Used", 0, SensorType.SmallData, this, Settings);
        _memoryFree = new Sensor("GPU Memory Free", 1, SensorType.SmallData, this, Settings);
        _memoryTotal = new Sensor("GPU Memory Total", 2, SensorType.SmallData, this, Settings);

        // Memory total
        if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Adapter_MemoryInfoX4_Get)) &&
            AtiAdlxx.ADL2_Adapter_MemoryInfoX4_Get(_context, _adapterInfo.AdapterIndex, out var memoryInfo) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            _memoryTotal.Value = memoryInfo.iMemorySize / 1024 / 1024;
            ActivateSensor(_memoryTotal);
        }
    }

    /// <summary>
    /// Update load sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateLoadSensors()
    {
        // Memory used
        if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Adapter_DedicatedVRAMUsage_Get)) &&
            AtiAdlxx.ADL2_Adapter_DedicatedVRAMUsage_Get(_context, _adapterInfo.AdapterIndex, out var vramUsed) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            _memoryUsed.Value = vramUsed;
            ActivateSensor(_memoryUsed);
        }

        // Memory total
        if (!(_memoryTotal.Value > 0)) return;
        _memoryFree.Value = _memoryTotal.Value - _memoryUsed.Value;
        ActivateSensor(_memoryFree);
    }

    /// <summary>
    /// Create control sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateControlSensors()
    {
        _controlSensor = new Sensor("GPU Fan", 0, SensorType.Control, this, Settings);

        AtiAdlxx.ADLFanSpeedInfo fanSpeedInfo = new();
        if (AtiAdlxx.ADL2_Overdrive5_FanSpeedInfo_Get(_context, _adapterInfo.AdapterIndex, 0, ref fanSpeedInfo) != 
            AtiAdlxx.ADLStatus.ADL_OK)
        {
            fanSpeedInfo.iMaxPercent = 100;
            fanSpeedInfo.iMinPercent = 0;
        }

        _fanControl = new Control(_controlSensor, Settings, fanSpeedInfo.iMinPercent, fanSpeedInfo.iMaxPercent);
        _fanControl.ControlModeChanged += ControlModeChanged;
        _fanControl.SoftwareControlValueChanged += SoftwareControlValueChanged;
        ControlModeChanged(_fanControl);
        _controlSensor.Control = _fanControl;
    }

    /// <summary>
    /// Create FPS sensors.
    /// </summary>
    /// <param name="frameMetricsSupported">The supported.</param>
    private void CreateFpsSensors(ref int frameMetricsSupported)
    {
        _fullscreenFps = new Sensor("Fullscreen FPS", 0, SensorType.Factor, this, Settings);

        if (!AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Adapter_FrameMetrics_Caps)) ||
            AtiAdlxx.ADL2_Adapter_FrameMetrics_Caps(_context, _adapterInfo.AdapterIndex, ref frameMetricsSupported) !=
                AtiAdlxx.ADLStatus.ADL_OK ||
                frameMetricsSupported != AtiAdlxx.ADL_TRUE ||
            AtiAdlxx.ADL2_Adapter_FrameMetrics_Start(_context, _adapterInfo.AdapterIndex, 0) !=
                AtiAdlxx.ADLStatus.ADL_OK) return;
        _frameMetricsStarted = true;
        _fullscreenFps.Value = -1;
        ActivateSensor(_fullscreenFps);
    }

    /// <summary>
    /// Update FPS sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateFpsSensors()
    {
        if (!_frameMetricsStarted) return;

        float framesPerSecond = 0;
        if (AtiAdlxx.ADL2_Adapter_FrameMetrics_Get(_context, _adapterInfo.AdapterIndex, 0, ref framesPerSecond) !=
            AtiAdlxx.ADLStatus.ADL_OK) return;
        _fullscreenFps.Value = framesPerSecond;
    }

    /// <summary>
    /// Create power sensors.
    /// </summary>
    /// <returns></returns>
    private void CreatePowerSensors(ref int frameMetricsSupported)
    {
        _powerCore = new Sensor("GPU Core", 0, SensorType.Power, this, Settings);
        _powerPpt = new Sensor("GPU PPT", 1, SensorType.Power, this, Settings);
        _powerSoC = new Sensor("GPU SoC", 2, SensorType.Power, this, Settings);
        _powerTotal = new Sensor("GPU Package", 3, SensorType.Power, this, Settings);

        // PM log start
        CreatePmLogStartSensors();
        CreatePowerManagementCapSensors(ref frameMetricsSupported);
    }

    /// <summary>
    /// Creates PM Log sensors.
    /// </summary>
    private void CreatePmLogStartSensors()
    {
        if (!AtiAdlxx.UsePmLogForFamily(_adlGcnInfo.ASICFamilyId) ||
            !AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Adapter_PMLog_Support_Get)) ||
            !AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Device_PMLog_Device_Create)) ||
            !AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Adapter_PMLog_Start))) return;

        AtiAdlxx.ADLPMLogStartInput adlPmLogStartInput = new()
        {
            usSensors = new ushort[AtiAdlxx.ADL_PMLOG_MAX_SENSORS]
        };

        _adlPmLogSupportInfo = new AtiAdlxx.ADLPMLogSupportInfo();
        if (_device != 0 ||
            AtiAdlxx.ADLStatus.ADL_OK !=
            AtiAdlxx.ADL2_Device_PMLog_Device_Create(_context, _adapterInfo.AdapterIndex, ref _device) ||
            AtiAdlxx.ADLStatus.ADL_OK !=
            AtiAdlxx.ADL2_Adapter_PMLog_Support_Get(_context, _adapterInfo.AdapterIndex, ref _adlPmLogSupportInfo))
            return;
        int i = 0;
        while (_adlPmLogSupportInfo.usSensors[i] != (ushort)AtiAdlxx.ADLPMLogSensors.ADL_SENSOR_MAXTYPES)
        {
            adlPmLogStartInput.usSensors[i] = _adlPmLogSupportInfo.usSensors[i];
            i++;
        }

        adlPmLogStartInput.usSensors[i] = (ushort)AtiAdlxx.ADLPMLogSensors.ADL_SENSOR_MAXTYPES;
        adlPmLogStartInput.ulSampleRate = _pmLogSampleRate;

        _adlPmLogStartOutput = new AtiAdlxx.ADLPMLogStartOutput();
        if (AtiAdlxx.ADL2_Adapter_PMLog_Start(_context,
                _adapterInfo.AdapterIndex,
                ref adlPmLogStartInput,
                ref _adlPmLogStartOutput,
                _device) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            _pmLogStarted = true;
        }
    }

    /// <summary>
    /// Creates power management capability sensors.
    /// </summary>
    /// <param name="frameMetricsSupported">The frame metrics supported.</param>
    private void CreatePowerManagementCapSensors(ref int frameMetricsSupported)
    {
        // Power management capabilities
        int enabled = 0;
        int version = 0;
        if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Overdrive_Caps)) &&
            AtiAdlxx.ADL2_Overdrive_Caps(_context, _adapterInfo.AdapterIndex, ref frameMetricsSupported, ref enabled, ref version) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            _overdriveApiSupported = frameMetricsSupported == AtiAdlxx.ADL_TRUE;
            _currentOverdriveApiLevel = version;
        }
        else
        {
            AtiAdlxx.ADLOD6Capabilities capabilities = new();
            if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Overdrive6_Capabilities_Get)) && 
                AtiAdlxx.ADL2_Overdrive6_Capabilities_Get(_context, _adapterInfo.AdapterIndex, ref capabilities) == AtiAdlxx.ADLStatus.ADL_OK && capabilities.iCapabilities > 0)
            {
                _overdriveApiSupported = true;
                _currentOverdriveApiLevel = 6;
            }

            if (_overdriveApiSupported) return;
            if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Overdrive5_ODParameters_Get)) &&
                AtiAdlxx.ADL2_Overdrive5_ODParameters_Get(_context, _adapterInfo.AdapterIndex, out AtiAdlxx.ADLODParameters p) == AtiAdlxx.ADLStatus.ADL_OK &&
                p.iActivityReportingSupported > 0)
            {
                _overdriveApiSupported = true;
                _currentOverdriveApiLevel = 5;
            }
            else
            {
                _currentOverdriveApiLevel = -1;
            }
        }
    }

    /// <summary>
    /// Update power sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdatePowerSensors()
    {
        if (_overdriveApiSupported && _currentOverdriveApiLevel >= 6)
        {
            GetOd6Power(AtiAdlxx.ADLODNCurrentPowerType.ODN_GPU_TOTAL_POWER, _powerTotal);
            GetOd6Power(AtiAdlxx.ADLODNCurrentPowerType.ODN_GPU_PPT_POWER, _powerPpt);
            GetOd6Power(AtiAdlxx.ADLODNCurrentPowerType.ODN_GPU_SOCKET_POWER, _powerSoC);
            GetOd6Power(AtiAdlxx.ADLODNCurrentPowerType.ODN_GPU_CHIP_POWER, _powerCore);
        }

        if (_overdriveApiSupported && _currentOverdriveApiLevel < 8) return;

        _overdrive8LogExists = false;
        AtiAdlxx.ADLPMLogDataOutput logDataOutput = new();
        if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_New_QueryPMLogData_Get)) &&
            AtiAdlxx.ADL2_New_QueryPMLogData_Get(_context, _adapterInfo.AdapterIndex, ref logDataOutput) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            _overdrive8LogExists = true;
        }

        AtiAdlxx.ADLPMLogData adlPmLogData = new();
        if (_pmLogStarted)
        {
            adlPmLogData = (AtiAdlxx.ADLPMLogData)Marshal.PtrToStructure(_adlPmLogStartOutput.pLoggingAddress, typeof(AtiAdlxx.ADLPMLogData))!;
        }

        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_CLK_GFXCLK, _coreClock, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_CLK_SOCCLK, _socClock);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_CLK_MEMCLK, _memoryClock, reset: false);

        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_EDGE, _temperatureCore, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_MEM, _temperatureMemory, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_VRVDDC, _temperatureVddc, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_VRMVDD, _temperatureMvdd, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_LIQUID, _temperatureLiquid, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_PLX, _temperaturePlx, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_HOTSPOT, _temperatureHotSpot, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_TEMPERATURE_SOC, _temperatureSoC);


        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_FAN_RPM, _fan, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_FAN_PERCENTAGE, _controlSensor, reset: false);

        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_GFX_VOLTAGE, _coreVoltage, 0.001f, false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_SOC_VOLTAGE, _socVoltage, 0.001f);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_MEM_VOLTAGE, _memoryVoltage, 0.001f);

        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_INFO_ACTIVITY_GFX, _coreLoad, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_INFO_ACTIVITY_MEM, _memoryLoad);

        if (_adlGcnInfo.ASICFamilyId >= (int)AtiAdlxx.GCNFamilies.FAMILY_NV3 || !GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_ASIC_POWER, _powerTotal, reset: false))
        {
            GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_BOARD_POWER, _powerTotal, reset: false);
        }

        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_GFX_POWER, _powerCore, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AtiAdlxx.ADLPMLogSensors.ADL_PMLOG_SOC_POWER, _powerSoC, reset: false);
    }

    /// <summary>
    /// Create D3D sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateD3dSensors()
    {
        // Non-Unix
        if (Software.OperatingSystem.IsUnix) return;

        // Get D3D devices
        string[] deviceIds = D3dDisplayDevice.GetDeviceIdentifiers();
        if (deviceIds == null) return;

        // Process devices
        foreach (string deviceId in deviceIds)
        {
            string actualDeviceId = D3dDisplayDevice.GetActualDeviceIdentifier(deviceId);

            if ((actualDeviceId.IndexOf(_adapterInfo.PNPString, StringComparison.OrdinalIgnoreCase) == -1 &&
                 _adapterInfo.PNPString.IndexOf(actualDeviceId, StringComparison.OrdinalIgnoreCase) == -1) ||
                !D3dDisplayDevice.GetDeviceInfoByIdentifier(deviceId,
                    out D3dDeviceInfo deviceInfo)) continue;
            _d3dDeviceId = deviceId;

            int nodeSensorIndex = 2;
            int memorySensorIndex = 3;

            _gpuDedicatedMemoryUsage = new Sensor("D3D Dedicated Memory Used", memorySensorIndex++, SensorType.SmallData, this, Settings);
            _gpuDedicatedMemoryFree = new Sensor("D3D Dedicated Memory Free", memorySensorIndex++, SensorType.SmallData, this, Settings);
            _gpuDedicatedMemoryTotal = new Sensor("D3D Dedicated Memory Total", memorySensorIndex++, SensorType.SmallData, this, Settings);
            _gpuSharedMemoryUsage = new Sensor("D3D Shared Memory Used", memorySensorIndex++, SensorType.SmallData, this, Settings);
            _gpuSharedMemoryFree = new Sensor("D3D Shared Memory Free", memorySensorIndex++, SensorType.SmallData, this, Settings);
            _gpuSharedMemoryTotal = new Sensor("D3D Shared Memory Total", memorySensorIndex, SensorType.SmallData, this, Settings);

            _gpuNodeUsage = new Sensor[deviceInfo.Nodes.Length];
            _gpuNodeUsagePrevValue = new long[deviceInfo.Nodes.Length];
            _gpuNodeUsagePrevTick = new DateTime[deviceInfo.Nodes.Length];

            foreach (D3dDeviceNodeInfo node in deviceInfo.Nodes.OrderBy(x => x.Name))
            {
                _gpuNodeUsage[node.Id] = new Sensor(node.Name, nodeSensorIndex++, SensorType.Load, this, Settings);
                _gpuNodeUsagePrevValue[node.Id] = node.RunningTime;
                _gpuNodeUsagePrevTick[node.Id] = node.QueryTime;
            }

            break;
        }
    }

    /// <summary>
    /// Update D3D sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateD3dSensors()
    {
        if (_d3dDeviceId == null ||
            !D3dDisplayDevice.GetDeviceInfoByIdentifier(_d3dDeviceId, out D3dDeviceInfo deviceInfo))
            return;

        _gpuDedicatedMemoryTotal.Value = 1f * deviceInfo.GpuVideoMemoryLimit / 1024 / 1024;
        ActivateSensor(_gpuDedicatedMemoryTotal);

        _gpuDedicatedMemoryUsage.Value = 1f * deviceInfo.GpuDedicatedUsed / 1024 / 1024;
        ActivateSensor(_gpuDedicatedMemoryUsage);

        _gpuDedicatedMemoryFree.Value = _gpuDedicatedMemoryTotal.Value - _gpuDedicatedMemoryUsage.Value;
        ActivateSensor(_gpuDedicatedMemoryFree);

        _gpuSharedMemoryUsage.Value = 1f * deviceInfo.GpuSharedUsed / 1024 / 1024;
        ActivateSensor(_gpuSharedMemoryUsage);

        _gpuSharedMemoryTotal.Value = 1f * deviceInfo.GpuSharedLimit / 1024 / 1024;
        ActivateSensor(_gpuSharedMemoryTotal);

        _gpuSharedMemoryFree.Value = _gpuSharedMemoryTotal.Value - _gpuSharedMemoryUsage.Value;
        ActivateSensor(_gpuSharedMemoryFree);

        foreach (D3dDeviceNodeInfo node in deviceInfo.Nodes)
        {
            long runningTimeDiff = node.RunningTime - _gpuNodeUsagePrevValue[node.Id];
            long timeDiff = node.QueryTime.Ticks - _gpuNodeUsagePrevTick[node.Id].Ticks;

            _gpuNodeUsage[node.Id].Value = 100f * runningTimeDiff / timeDiff;
            _gpuNodeUsagePrevValue[node.Id] = node.RunningTime;
            _gpuNodeUsagePrevTick[node.Id] = node.QueryTime;
            ActivateSensor(_gpuNodeUsage[node.Id]);
        }
    }

    /// <summary>
    /// Determines whether [is sensor supported by PM log] [the specified sensor type].
    /// </summary>
    /// <param name="sensorType">Type of the sensor.</param>
    /// <returns>
    ///   <c>true</c> if [is sensor supported by pm log] [the specified sensor type]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsSensorSupportedByPmLog(AtiAdlxx.ADLPMLogSensors sensorType)
    {
        if (!_pmLogStarted || sensorType == 0) return false;

        for (int i = 0; i < AtiAdlxx.ADL_PMLOG_MAX_SENSORS; i++)
        {
            if (_adlPmLogSupportInfo.usSensors[i] != (int)sensorType) continue;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets a sensor value.
    /// </summary>
    /// <param name="adlPmLogData">Current pmlog struct, used with pmlog-support/start.</param>
    /// <param name="od8Log">Legacy pmlogdataoutput struct, used with ADL2_New_QueryPMLogData_Get.</param>
    /// <param name="sensorType">Type of the sensor.</param>
    /// <param name="sensor">The sensor.</param>
    /// <param name="factor">The factor.</param>
    /// <param name="reset">If set to <c>true</c>, resets the sensor value to <c>null</c>.</param>
    /// <returns>
    /// true if sensor is supported, false otherwise
    /// </returns>
    private bool GetAdlSensor(AtiAdlxx.ADLPMLogData adlPmLogData, AtiAdlxx.ADLPMLogDataOutput od8Log,
                              AtiAdlxx.ADLPMLogSensors sensorType, Sensor sensor, float factor = 1.0f, bool reset = true)
    {
        int i = (int)sensorType;
        bool supportedByPmLog = IsSensorSupportedByPmLog(sensorType);
        bool supportedByOd8 = _overdrive8LogExists && i < od8Log.sensors.Length && od8Log.sensors[i].supported != 0;

        if (!supportedByPmLog && !supportedByOd8)
        {
            if (reset)
                sensor.Value = null;

            return false;
        }

        if (_pmLogStarted)
        {
            //check if ulLastUpdated is a valid number, avoid timezone issues with unspecified kind and 48h offset
            DateTime now = new(DateTime.Now.Ticks, DateTimeKind.Unspecified);
            if (adlPmLogData.ulLastUpdated == 0 || adlPmLogData.ulActiveSampleRate > 86400000 ||
                now.AddHours(48).ToFileTime() < (long)adlPmLogData.ulLastUpdated ||
                now.AddHours(-48).ToFileTime() > (long)adlPmLogData.ulLastUpdated)
            {
                supportedByPmLog = false;
            }
        }

        if (supportedByPmLog)
        {
            bool found = false;

            if (adlPmLogData.ulValues != null)
            {
                for (int k = 0; k < adlPmLogData.ulValues.Length - 1; k += 2)
                {
                    if (adlPmLogData.ulValues[k] == (ushort)AtiAdlxx.ADLPMLogSensors.ADL_SENSOR_MAXTYPES) break;
                    if (adlPmLogData.ulValues[k] != i) continue;
                    
                    sensor.Value = adlPmLogData.ulValues[k + 1] * factor;
                    ActivateSensor(sensor);
                    found = true;
                }
            }

            if (!found && reset)
            {
                sensor.Value = null;
            }
        }
        else if (_overdrive8LogExists)
        {
            if (supportedByOd8)
            {
                sensor.Value = od8Log.sensors[i].value * factor;
                ActivateSensor(sensor);
            }
            else if (reset)
            {
                sensor.Value = null;
            }
        }
        return true;
    }

    #region OD5

    /// <summary>
    /// Gets the OD5 current activity.
    /// </summary>
    private void GetOd5CurrentActivity()
    {
        AtiAdlxx.ADLPMActivity adlpmActivity = new();
        if (AtiAdlxx.ADL2_Overdrive5_CurrentActivity_Get(_context, _adapterInfo.AdapterIndex, ref adlpmActivity) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            if (adlpmActivity.iEngineClock > 0)
            {
                _coreClock.Value = 0.01f * adlpmActivity.iEngineClock;
                ActivateSensor(_coreClock);
            }
            else
            {
                _coreClock.Value = null;
            }

            if (adlpmActivity.iMemoryClock > 0)
            {
                _memoryClock.Value = 0.01f * adlpmActivity.iMemoryClock;
                ActivateSensor(_memoryClock);
            }
            else
            {
                _memoryClock.Value = null;
            }

            if (adlpmActivity.iVddc > 0)
            {
                _coreVoltage.Value = 0.001f * adlpmActivity.iVddc;
                ActivateSensor(_coreVoltage);
            }
            else
            {
                _coreVoltage.Value = null;
            }

            _coreLoad.Value = Math.Min(adlpmActivity.iActivityPercent, 100);
            ActivateSensor(_coreLoad);
        }
        else
        {
            _coreClock.Value = null;
            _memoryClock.Value = null;
            _coreVoltage.Value = null;
            _coreLoad.Value = null;
        }
    }

    /// <summary>
    /// Gets the OD5 fan speed.
    /// </summary>
    /// <param name="speedType">Type of the speed.</param>
    /// <param name="sensor">The sensor.</param>
    private void GetOd5FanSpeed(int speedType, Sensor sensor)
    {
        AtiAdlxx.ADLFanSpeedValue fanSpeedValue = new() { iSpeedType = speedType };
        if (AtiAdlxx.ADL2_Overdrive5_FanSpeed_Get(_context, _adapterInfo.AdapterIndex, 0, ref fanSpeedValue) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            sensor.Value = fanSpeedValue.iFanSpeed;
            ActivateSensor(sensor);
        }
        else
        {
            sensor.Value = null;
        }
    }

    /// <summary>
    /// Gets the OD5 temperature.
    /// </summary>
    /// <param name="temperatureCore">The temperature core.</param>
    private void GetOd5Temperature(Sensor temperatureCore)
    {
        AtiAdlxx.ADLTemperature temperature = new();
        if (AtiAdlxx.ADL2_Overdrive5_Temperature_Get(_context, _adapterInfo.AdapterIndex, 0, ref temperature) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            temperatureCore.Value = 0.001f * temperature.iTemperature;
            ActivateSensor(temperatureCore);
        }
        else
        {
            temperatureCore.Value = null;
        }
    }

    /// <summary>
    /// Gets the OverdriveN temperature.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="sensor">The sensor.</param>
    /// <param name="minTemperature">The minimum temperature.</param>
    /// <param name="scale">The scale.</param>
    /// <param name="reset">If set to <c>true</c>, resets the sensor value to <c>null</c>.</param>
    private void GetOdnTemperature(AtiAdlxx.ADLODNTemperatureType type, Sensor sensor, double minTemperature = -256, double scale = 1, bool reset = true)
    {
        // If a sensor isn't available, some cards report 54000 degrees C.
        // 110C is expected for Navi, so 256C should be enough to use as a maximum.

        int maxTemperature = (int)(256 / scale);
        minTemperature = (int)(minTemperature / scale);

        int temperature = 0;
        if (AtiAdlxx.ADL2_OverdriveN_Temperature_Get(_context, _adapterInfo.AdapterIndex, type, ref temperature) == AtiAdlxx.ADLStatus.ADL_OK &&
            temperature >= minTemperature &&
            temperature <= maxTemperature)
        {
            sensor.Value = (float)(scale * temperature);
            ActivateSensor(sensor);
        }
        else if (reset)
        {
            sensor.Value = null;
        }
    }

    #endregion

    #region OD6

    /// <summary>
    /// Gets the Overdrive6 power.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="sensor">The sensor.</param>
    private void GetOd6Power(AtiAdlxx.ADLODNCurrentPowerType type, Sensor sensor)
    {
        int powerOf8 = 0;
        if (AtiAdlxx.ADL2_Overdrive6_CurrentPower_Get(_context, _adapterInfo.AdapterIndex, type, ref powerOf8) == AtiAdlxx.ADLStatus.ADL_OK)
        {
            sensor.Value = powerOf8 >> 8;
            ActivateSensor(sensor);
        }
        else
        {
            sensor.Value = null;
        }
    }

    #endregion

    #endregion
}
