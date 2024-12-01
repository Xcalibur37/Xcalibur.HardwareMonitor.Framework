using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.D3d;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.AMD;

/// <summary>
/// AMD GPU
/// </summary>
/// <seealso cref="GpuBase" />
internal sealed class AmdGpu : GpuBase
{
    #region Fields

    private AdlAdapterInfo _adapterInfo;
    private AdlpmLogStartOutput _adlPmLogStartOutput;
    private AdlpmLogSupportInfo _adlPmLogSupportInfo;
    private AdlGcnInfo _adlGcnInfo;
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
    public AmdGpu(IntPtr amdContext, AdlAdapterInfo adapterInfo, AdlGcnInfo gcnInfo, ISettings settings)
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
        AdlFanSpeedValue fanSpeedValue = new()
        {
            iSpeedType = AtiAdlxx.AdlDlFanctrlSpeedTypePercent,
            iFlags = AtiAdlxx.AdlDlFanctrlFlagUserDefinedSpeed,
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
        GetOdnTemperature(AdlodnTemperatureType.Edge, _temperatureCore, -256, 0.001, false);
        GetOdnTemperature(AdlodnTemperatureType.Mem, _temperatureMemory);
        GetOdnTemperature(AdlodnTemperatureType.Vrvddc, _temperatureVddc);
        GetOdnTemperature(AdlodnTemperatureType.Vrmvdd, _temperatureMvdd);
        GetOdnTemperature(AdlodnTemperatureType.Liquid, _temperatureLiquid);
        GetOdnTemperature(AdlodnTemperatureType.Plx, _temperaturePlx);
        GetOdnTemperature(AdlodnTemperatureType.Hotspot, _temperatureHotSpot);
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
        GetOd5FanSpeed(AtiAdlxx.AdlDlFanctrlSpeedTypeRpm, _fan);
        GetOd5FanSpeed(AtiAdlxx.AdlDlFanctrlSpeedTypePercent, _controlSensor);
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
            AtiAdlxx.ADL2_Adapter_MemoryInfoX4_Get(_context, _adapterInfo.AdapterIndex, out var memoryInfo) == AdlStatus.AdlOk)
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
            AtiAdlxx.ADL2_Adapter_DedicatedVRAMUsage_Get(_context, _adapterInfo.AdapterIndex, out var vramUsed) == AdlStatus.AdlOk)
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

        AdlFanSpeedInfo fanSpeedInfo = new();
        if (AtiAdlxx.ADL2_Overdrive5_FanSpeedInfo_Get(_context, _adapterInfo.AdapterIndex, 0, ref fanSpeedInfo) != 
            AdlStatus.AdlOk)
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
                AdlStatus.AdlOk ||
                frameMetricsSupported != AtiAdlxx.AdlTrue ||
            AtiAdlxx.ADL2_Adapter_FrameMetrics_Start(_context, _adapterInfo.AdapterIndex, 0) !=
                AdlStatus.AdlOk) return;
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
            AdlStatus.AdlOk) return;
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

        AdlpmLogStartInput adlPmLogStartInput = new()
        {
            usSensors = new ushort[AtiAdlxx.AdlPmlogMaxSensors]
        };

        _adlPmLogSupportInfo = new AdlpmLogSupportInfo();
        if (_device != 0 ||
            AdlStatus.AdlOk !=
            AtiAdlxx.ADL2_Device_PMLog_Device_Create(_context, _adapterInfo.AdapterIndex, ref _device) ||
            AdlStatus.AdlOk !=
            AtiAdlxx.ADL2_Adapter_PMLog_Support_Get(_context, _adapterInfo.AdapterIndex, ref _adlPmLogSupportInfo))
            return;
        int i = 0;
        while (_adlPmLogSupportInfo.usSensors[i] != (ushort)AdlpmLogSensors.AdlSensorMaxtypes)
        {
            adlPmLogStartInput.usSensors[i] = _adlPmLogSupportInfo.usSensors[i];
            i++;
        }

        adlPmLogStartInput.usSensors[i] = (ushort)AdlpmLogSensors.AdlSensorMaxtypes;
        adlPmLogStartInput.ulSampleRate = _pmLogSampleRate;

        _adlPmLogStartOutput = new AdlpmLogStartOutput();
        if (AtiAdlxx.ADL2_Adapter_PMLog_Start(_context,
                _adapterInfo.AdapterIndex,
                ref adlPmLogStartInput,
                ref _adlPmLogStartOutput,
                _device) == AdlStatus.AdlOk)
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
            AtiAdlxx.ADL2_Overdrive_Caps(_context, _adapterInfo.AdapterIndex, ref frameMetricsSupported, ref enabled, ref version) == AdlStatus.AdlOk)
        {
            _overdriveApiSupported = frameMetricsSupported == AtiAdlxx.AdlTrue;
            _currentOverdriveApiLevel = version;
        }
        else
        {
            Adlod6Capabilities capabilities = new();
            if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Overdrive6_Capabilities_Get)) && 
                AtiAdlxx.ADL2_Overdrive6_Capabilities_Get(_context, _adapterInfo.AdapterIndex, ref capabilities) == AdlStatus.AdlOk && capabilities.iCapabilities > 0)
            {
                _overdriveApiSupported = true;
                _currentOverdriveApiLevel = 6;
            }

            if (_overdriveApiSupported) return;
            if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Overdrive5_ODParameters_Get)) &&
                AtiAdlxx.ADL2_Overdrive5_ODParameters_Get(_context, _adapterInfo.AdapterIndex, out AdlodParameters p) == AdlStatus.AdlOk &&
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
    private void UpdatePowerSensors()
    {
        if (_overdriveApiSupported && _currentOverdriveApiLevel >= 6)
        {
            GetOd6Power(AdlodnCurrentPowerType.OdnGpuTotalPower, _powerTotal);
            GetOd6Power(AdlodnCurrentPowerType.OdnGpuPptPower, _powerPpt);
            GetOd6Power(AdlodnCurrentPowerType.OdnGpuSocketPower, _powerSoC);
            GetOd6Power(AdlodnCurrentPowerType.OdnGpuChipPower, _powerCore);
        }

        if (_overdriveApiSupported && _currentOverdriveApiLevel < 8) return;

        _overdrive8LogExists = false;
        AdlpmLogDataOutput logDataOutput = new();
        if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_New_QueryPMLogData_Get)) &&
            AtiAdlxx.ADL2_New_QueryPMLogData_Get(_context, _adapterInfo.AdapterIndex, ref logDataOutput) == AdlStatus.AdlOk)
        {
            _overdrive8LogExists = true;
        }

        AdlpmLogData adlPmLogData = new();
        if (_pmLogStarted)
        {
            adlPmLogData = (AdlpmLogData)Marshal.PtrToStructure(_adlPmLogStartOutput.pLoggingAddress, typeof(AdlpmLogData))!;
        }

        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogClkGfxclk, _coreClock, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogClkSocclk, _socClock);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogClkMemclk, _memoryClock, reset: false);

        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperatureEdge, _temperatureCore, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperatureMem, _temperatureMemory, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperatureVrvddc, _temperatureVddc, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperatureVrmvdd, _temperatureMvdd, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperatureLiquid, _temperatureLiquid, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperaturePlx, _temperaturePlx, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperatureHotspot, _temperatureHotSpot, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogTemperatureSoc, _temperatureSoC);


        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogFanRpm, _fan, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogFanPercentage, _controlSensor, reset: false);

        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogGfxVoltage, _coreVoltage, 0.001f, false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogSocVoltage, _socVoltage, 0.001f);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogMemVoltage, _memoryVoltage, 0.001f);

        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogInfoActivityGfx, _coreLoad, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogInfoActivityMem, _memoryLoad);

        if (_adlGcnInfo.ASICFamilyId >= (int)GcnFamilies.FamilyNv3 || !GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogAsicPower, _powerTotal, reset: false))
        {
            GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogBoardPower, _powerTotal, reset: false);
        }

        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogGfxPower, _powerCore, reset: false);
        GetAdlSensor(adlPmLogData, logDataOutput, AdlpmLogSensors.AdlPmlogSocPower, _powerSoC, reset: false);
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
    private bool IsSensorSupportedByPmLog(AdlpmLogSensors sensorType)
    {
        if (!_pmLogStarted || sensorType == 0) return false;

        for (int i = 0; i < AtiAdlxx.AdlPmlogMaxSensors; i++)
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
    private bool GetAdlSensor(AdlpmLogData adlPmLogData, AdlpmLogDataOutput od8Log,
                              AdlpmLogSensors sensorType, Sensor sensor, float factor = 1.0f, bool reset = true)
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
                    if (adlPmLogData.ulValues[k] == (ushort)AdlpmLogSensors.AdlSensorMaxtypes) break;
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
        AdlpmActivity adlpmActivity = new();
        if (AtiAdlxx.ADL2_Overdrive5_CurrentActivity_Get(_context, _adapterInfo.AdapterIndex, ref adlpmActivity) == AdlStatus.AdlOk)
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
        AdlFanSpeedValue fanSpeedValue = new() { iSpeedType = speedType };
        if (AtiAdlxx.ADL2_Overdrive5_FanSpeed_Get(_context, _adapterInfo.AdapterIndex, 0, ref fanSpeedValue) == AdlStatus.AdlOk)
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
        AdlTemperature temperature = new();
        if (AtiAdlxx.ADL2_Overdrive5_Temperature_Get(_context, _adapterInfo.AdapterIndex, 0, ref temperature) == AdlStatus.AdlOk)
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
    private void GetOdnTemperature(AdlodnTemperatureType type, Sensor sensor, double minTemperature = -256, double scale = 1, bool reset = true)
    {
        // If a sensor isn't available, some cards report 54000 degrees C.
        // 110C is expected for Navi, so 256C should be enough to use as a maximum.

        int maxTemperature = (int)(256 / scale);
        minTemperature = (int)(minTemperature / scale);

        int temperature = 0;
        if (AtiAdlxx.ADL2_OverdriveN_Temperature_Get(_context, _adapterInfo.AdapterIndex, type, ref temperature) == AdlStatus.AdlOk &&
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
    private void GetOd6Power(AdlodnCurrentPowerType type, Sensor sensor)
    {
        int powerOf8 = 0;
        if (AtiAdlxx.ADL2_Overdrive6_CurrentPower_Get(_context, _adapterInfo.AdapterIndex, type, ref powerOf8) == AdlStatus.AdlOk)
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
