// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Win32;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.Nvidia;

/// <summary>
/// Nvidia GPU
/// </summary>
/// <seealso cref="GpuBase" />
internal sealed class NvidiaGpu : GpuBase
{
    #region Fields

    private readonly int _adapterIndex;
    private readonly NvApi.NvDisplayHandle? _displayHandle;
    private readonly NvApi.NvPhysicalGpuHandle _handle;

    private Sensor[] _clocks;
    private int _clockVersion;
    private Sensor[] _controls;
    private string _d3dDeviceId;
    private Control[] _fanControls;
    private Sensor[] _fans;
    private Sensor _gpuDedicatedMemoryUsage;
    private Sensor[] _gpuNodeUsage;
    private DateTime[] _gpuNodeUsagePrevTick;
    private long[] _gpuNodeUsagePrevValue;
    private Sensor _gpuSharedMemoryUsage;

    private Sensor _hotSpotTemperature;
    private Sensor[] _loads;
    private Sensor _memoryFree;
    private Sensor _memoryJunctionTemperature;
    private Sensor _memoryTotal;
    private Sensor _memoryUsed;
    private Sensor _memoryLoad;
    private NvidiaML.NvmlDevice? _nvmlDevice;
    private Sensor _pcieThroughputRx;
    private Sensor _pcieThroughputTx;
    private Sensor[] _powers;
    private Sensor _powerUsage;
    private Sensor[] _temperatures;
    private uint _thermalSensorsMask;

    #endregion

    #region Properties

    /// <inheritdoc />
    public override string DeviceId => _d3dDeviceId != null ? D3DDisplayDevice.GetActualDeviceIdentifier(_d3dDeviceId) : null;

    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.GpuNvidia;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NvidiaGpu"/> class.
    /// </summary>
    /// <param name="adapterIndex">Index of the adapter.</param>
    /// <param name="handle">The handle.</param>
    /// <param name="displayHandle">The display handle.</param>
    /// <param name="settings">The settings.</param>
    public NvidiaGpu(int adapterIndex, NvApi.NvPhysicalGpuHandle handle, NvApi.NvDisplayHandle? displayHandle, ISettings settings)
        : base(GetName(handle), new Identifier("gpu-nvidia", adapterIndex.ToString(CultureInfo.InvariantCulture)), settings)
    {
        _adapterIndex = adapterIndex;
        _handle = handle;
        _displayHandle = displayHandle;

        // Sensors
        CreateThermalSensors();
        CreateClockSensors();
        CreateFanSensors();
        CreateLoadSensors();
        CreatePowerSensors();
        CreateNvmlSensors();
        CreateD3dSenors();

        // Update
        Update();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Close()
    {
        if (_fanControls != null)
        {
            for (int i = 0; i < _fanControls.Length; i++)
            {
                _fanControls[i].ControlModeChanged -= ControlModeChanged;
                _fanControls[i].SoftwareControlValueChanged -= SoftwareControlValueChanged;
                if (_fanControls[i].ControlMode == ControlMode.Undefined) continue;
                RestoreDefaultFanBehavior(i);
            }
        }

        base.Close();
    }

    /// <inheritdoc />
    public override void Update()
    {
        UpdateD3dSensors();
        UpdateThermalSensors();
        UpdateClockSensors();
        UpdateFanSensors();
        UpdateLoadSensors();
        UpdatePowerSensors();
        UpdateNvmlSensors();
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    private static string GetName(NvApi.NvPhysicalGpuHandle handle)
    {
        if (NvApi.NvAPI_GPU_GetFullName(handle, out string gpuName) == NvApi.NvStatus.OK)
        {
            string name = gpuName.Trim();
            return name.StartsWith("NVIDIA", StringComparison.OrdinalIgnoreCase) ? name : "NVIDIA " + name;
        }

        return "NVIDIA";
    }

    /// <summary>
    /// Gets the memory information.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvMemoryInfo GetMemoryInfo(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_GetMemoryInfo == null || _displayHandle == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        NvApi.NvMemoryInfo memoryInfo = new()
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvMemoryInfo>(2)
        };

        status = NvApi.NvAPI_GPU_GetMemoryInfo(_displayHandle.Value, ref memoryInfo);
        return status == NvApi.NvStatus.OK ? memoryInfo : default;
    }

    /// <summary>
    /// Gets the clock frequencies.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvGpuClockFrequencies GetClockFrequencies(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_GetAllClockFrequencies == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        NvApi.NvGpuClockFrequencies clockFrequencies = new()
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvGpuClockFrequencies>(_clockVersion)
        };

        status = NvApi.NvAPI_GPU_GetAllClockFrequencies(_handle, ref clockFrequencies);
        return status == NvApi.NvStatus.OK ? clockFrequencies : default;
    }

    /// <summary>
    /// Gets the thermal settings.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvThermalSettings GetThermalSettings(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_GetThermalSettings == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        NvApi.NvThermalSettings settings = new()
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvThermalSettings>(1),
            Count = NvApi.MAX_THERMAL_SENSORS_PER_GPU
        };

        status = NvApi.NvAPI_GPU_GetThermalSettings(_handle, (int)NvApi.NvThermalTarget.All, ref settings);
        return status == NvApi.NvStatus.OK ? settings : default;
    }

    /// <summary>
    /// Gets the thermal sensors.
    /// </summary>
    /// <param name="mask">The mask.</param>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvThermalSensors GetThermalSensors(uint mask, out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_ThermalGetSensors == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        var thermalSensors = new NvApi.NvThermalSensors
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvThermalSensors>(2),
            Mask = mask
        };

        status = NvApi.NvAPI_GPU_ThermalGetSensors(_handle, ref thermalSensors);
        return status == NvApi.NvStatus.OK ? thermalSensors : default;
    }

    /// <summary>
    /// Gets the fan coolers status.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvFanCoolersStatus GetFanCoolersStatus(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_ClientFanCoolersGetStatus == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        var coolers = new NvApi.NvFanCoolersStatus
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvFanCoolersStatus>(1),
            Items = new NvApi.NvFanCoolersStatusItem[NvApi.MAX_FAN_COOLERS_STATUS_ITEMS]
        };

        status = NvApi.NvAPI_GPU_ClientFanCoolersGetStatus(_handle, ref coolers);
        return status == NvApi.NvStatus.OK ? coolers : default;
    }

    /// <summary>
    /// Gets the fan coolers controllers.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvFanCoolerControl GetFanCoolersControllers(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_ClientFanCoolersGetControl == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        var controllers = new NvApi.NvFanCoolerControl
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvFanCoolerControl>(1)
        };

        status = NvApi.NvAPI_GPU_ClientFanCoolersGetControl(_handle, ref controllers);
        return status == NvApi.NvStatus.OK ? controllers : default;
    }

    /// <summary>
    /// Gets the cooler settings.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvCoolerSettings GetCoolerSettings(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_GetCoolerSettings == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        NvApi.NvCoolerSettings settings = new()
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvCoolerSettings>(2),
            Cooler = new NvApi.NvCooler[NvApi.MAX_COOLERS_PER_GPU]
        };

        status = NvApi.NvAPI_GPU_GetCoolerSettings(_handle, NvApi.NvCoolerTarget.All, ref settings);
        return status == NvApi.NvStatus.OK ? settings : default;
    }

    /// <summary>
    /// Gets the dynamic p-states information ex.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvDynamicPStatesInfo GetDynamicPStatesInfoEx(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_GetDynamicPstatesInfoEx == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        NvApi.NvDynamicPStatesInfo pStatesInfo = new()
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvDynamicPStatesInfo>(1),
            Utilizations = new NvApi.NvDynamicPState[NvApi.MAX_GPU_UTILIZATIONS]
        };

        status = NvApi.NvAPI_GPU_GetDynamicPstatesInfoEx(_handle, ref pStatesInfo);
        return status == NvApi.NvStatus.OK ? pStatesInfo : default;
    }

    /// <summary>
    /// Gets the usages.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvUsages GetUsages(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_GetUsages == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        NvApi.NvUsages usages = new()
        {
            Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvUsages>(1)
        };

        status = NvApi.NvAPI_GPU_GetUsages(_handle, ref usages);
        return status == NvApi.NvStatus.OK ? usages : default;
    }

    /// <summary>
    /// Gets the power topology.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private NvApi.NvPowerTopology GetPowerTopology(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_ClientPowerTopologyGetStatus == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        NvApi.NvPowerTopology powerTopology = new()
        {
            Version = NvApi.MAKE_NVAPI_VERSION<NvApi.NvPowerTopology>(1)
        };

        status = NvApi.NvAPI_GPU_ClientPowerTopologyGetStatus(_handle, ref powerTopology);
        return status == NvApi.NvStatus.OK ? powerTopology : default;
    }

    /// <summary>
    /// Gets the tach reading.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <returns></returns>
    private int GetTachReading(out NvApi.NvStatus status)
    {
        if (NvApi.NvAPI_GPU_GetTachReading == null)
        {
            status = NvApi.NvStatus.Error;
            return default;
        }

        status = NvApi.NvAPI_GPU_GetTachReading(_handle, out int value);
        return value;
    }

    /// <summary>
    /// Gets the name of the utilization domain.
    /// </summary>
    /// <param name="utilizationDomain">The utilization domain.</param>
    /// <returns></returns>
    private static string GetUtilizationDomainName(NvApi.NvUtilizationDomain utilizationDomain) => utilizationDomain switch
    {
        NvApi.NvUtilizationDomain.Gpu => "GPU Core",
        NvApi.NvUtilizationDomain.FrameBuffer => "GPU Memory Controller",
        NvApi.NvUtilizationDomain.VideoEngine => "GPU Video Engine",
        NvApi.NvUtilizationDomain.BusInterface => "GPU Bus",
        _ => null
    };

    /// <summary>
    /// Controls the mode changed.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    private void ControlModeChanged(IControl control)
    {
        switch (control.ControlMode)
        {
            case ControlMode.Default:
                RestoreDefaultFanBehavior(control.Sensor.Index);
                break;
            case ControlMode.Software:
                SoftwareControlValueChanged(control);
                break;
        }
    }

    /// <summary>
    /// Software control value changed.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    private void SoftwareControlValueChanged(IControl control)
    {
        int index = control.Sensor?.Index ?? 0;

        NvApi.NvCoolerLevels coolerLevels = new() { Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvCoolerLevels>(1), Levels = new NvApi.NvLevel[NvApi.MAX_COOLERS_PER_GPU] };
        coolerLevels.Levels[0].Level = (int)control.SoftwareValue;
        coolerLevels.Levels[0].Policy = NvApi.NvLevelPolicy.Manual;
        if (NvApi.NvAPI_GPU_SetCoolerLevels(_handle, index, ref coolerLevels) == NvApi.NvStatus.OK)
            return;

        NvApi.NvFanCoolerControl fanCoolersControllers = GetFanCoolersControllers(out _);

        for (int i = 0; i < fanCoolersControllers.Count; i++)
        {
            NvApi.NvFanCoolerControlItem nvFanCoolerControlItem = fanCoolersControllers.Items[i];
            if (nvFanCoolerControlItem.CoolerId == index)
            {
                nvFanCoolerControlItem.ControlMode = NvApi.NvFanControlMode.Manual;
                nvFanCoolerControlItem.Level = (uint)control.SoftwareValue;

                fanCoolersControllers.Items[i] = nvFanCoolerControlItem;
            }
        }

        NvApi.NvAPI_GPU_ClientFanCoolersSetControl(_handle, ref fanCoolersControllers);
    }

    /// <summary>
    /// Restores the default fan behavior.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    private void RestoreDefaultFanBehavior(int index)
    {
        NvApi.NvCoolerLevels coolerLevels = new() { Version = (uint)NvApi.MAKE_NVAPI_VERSION<NvApi.NvCoolerLevels>(1), Levels = new NvApi.NvLevel[NvApi.MAX_COOLERS_PER_GPU] };
        coolerLevels.Levels[0].Policy = NvApi.NvLevelPolicy.Auto;
        if (NvApi.NvAPI_GPU_SetCoolerLevels(_handle, index, ref coolerLevels) == NvApi.NvStatus.OK)
            return;

        NvApi.NvFanCoolerControl fanCoolersControllers = GetFanCoolersControllers(out _);

        for (int i = 0; i < fanCoolersControllers.Count; i++)
        {
            NvApi.NvFanCoolerControlItem nvFanCoolerControlItem = fanCoolersControllers.Items[i];
            if (nvFanCoolerControlItem.CoolerId == index)
            {
                nvFanCoolerControlItem.ControlMode = NvApi.NvFanControlMode.Auto;
                nvFanCoolerControlItem.Level = 0;

                fanCoolersControllers.Items[i] = nvFanCoolerControlItem;
            }
        }

        NvApi.NvAPI_GPU_ClientFanCoolersSetControl(_handle, ref fanCoolersControllers);
    }

    #region Sensors

    /// <summary>
    /// Creates thermal sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateThermalSensors()
    {
        NvApi.NvThermalSettings thermalSettings = GetThermalSettings(out NvApi.NvStatus status);
        if (status != NvApi.NvStatus.OK || thermalSettings.Count <= 0) return;

        // Thermal sensors
        _temperatures = new Sensor[thermalSettings.Count];
        for (int i = 0; i < thermalSettings.Count; i++)
        {
            NvApi.NvSensor sensor = thermalSettings.Sensor[i];
            string name = sensor.Target switch
            {
                NvApi.NvThermalTarget.Gpu => "GPU Core",
                NvApi.NvThermalTarget.Memory => "GPU Memory",
                NvApi.NvThermalTarget.PowerSupply => "GPU Power Supply",
                NvApi.NvThermalTarget.Board => "GPU Board",
                NvApi.NvThermalTarget.VisualComputingBoard => "GPU Visual Computing Board",
                NvApi.NvThermalTarget.VisualComputingInlet => "GPU Visual Computing Inlet",
                NvApi.NvThermalTarget.VisualComputingOutlet => "GPU Visual Computing Outlet",
                _ => "GPU"
            };

            _temperatures[i] = new Sensor(name, i, SensorType.Temperature, this, [], Settings);
            ActivateSensor(_temperatures[i]);
        }

        // Additional thermal sensors
        _hotSpotTemperature = new Sensor("GPU Hot Spot", (int)thermalSettings.Count + 1, SensorType.Temperature, this, Settings);
        _memoryJunctionTemperature = new Sensor("GPU Memory Junction", (int)thermalSettings.Count + 2, SensorType.Temperature, this, Settings);
        bool hasAnyThermalSensor = false;

        for (int thermalSensorsMaxBit = 0; thermalSensorsMaxBit < 32; thermalSensorsMaxBit++)
        {
            // Find the maximum thermal sensor mask value.
            _thermalSensorsMask = 1u << thermalSensorsMaxBit;
            GetThermalSensors(_thermalSensorsMask, out NvApi.NvStatus thermalSensorsStatus);

            if (thermalSensorsStatus == NvApi.NvStatus.OK)
            {
                hasAnyThermalSensor = true;
                continue;
            }

            _thermalSensorsMask--;
            break;
        }

        if (!hasAnyThermalSensor)
        {
            _thermalSensorsMask = 0;
        }
    }

    /// <summary>
    /// Updates thermal sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateThermalSensors()
    {
        NvApi.NvStatus status;

        if (_temperatures is { Length: > 0 })
        {
            NvApi.NvThermalSettings settings = GetThermalSettings(out status);
            // settings.Count is 0 when no valid data available, this happens when you try to read out this
            // value with a high polling interval.
            if (status == NvApi.NvStatus.OK && settings.Count > 0)
            {
                _temperatures.Apply(x => x.Value = settings.Sensor[x.Index].CurrentTemp);
            }
        }

        if (_thermalSensorsMask > 0)
        {
            NvApi.NvThermalSensors thermalSensors = GetThermalSensors(_thermalSensorsMask, out status);
            if (status == NvApi.NvStatus.OK)
            {
                _hotSpotTemperature.Value = thermalSensors.Temperatures[1] / 256.0f;
                _memoryJunctionTemperature.Value = thermalSensors.Temperatures[9] / 256.0f;
            }

            // Hot spot
            if (_hotSpotTemperature.Value != 0)
            {
                ActivateSensor(_hotSpotTemperature);
            }

            // Memory junction
            if (_memoryJunctionTemperature.Value != 0)
            {
                ActivateSensor(_memoryJunctionTemperature);
            }
        }
        else
        {
            _hotSpotTemperature.Value = null;
            _memoryJunctionTemperature.Value = null;
        }
    }

    /// <summary>
    /// Creates clock sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateClockSensors()
    {
        for (int clockVersion = 1; clockVersion <= 3; clockVersion++)
        {
            _clockVersion = clockVersion;

            NvApi.NvGpuClockFrequencies clockFrequencies = GetClockFrequencies(out var status);
            if (status != NvApi.NvStatus.OK) continue;
            var clocks = new List<Sensor>();
            for (int i = 0; i < clockFrequencies.Clocks.Length; i++)
            {
                NvApi.NvGpuClockFrequenciesDomain clock = clockFrequencies.Clocks[i];
                if (!clock.IsPresent || !Enum.IsDefined(typeof(NvApi.NvGpuPublicClockId), i)) continue;
                var clockId = (NvApi.NvGpuPublicClockId)i;
                string name = clockId switch
                {
                    NvApi.NvGpuPublicClockId.Graphics => "GPU Core",
                    NvApi.NvGpuPublicClockId.Memory => "GPU Memory",
                    NvApi.NvGpuPublicClockId.Processor => "GPU Shader",
                    NvApi.NvGpuPublicClockId.Video => "GPU Video",
                    _ => null
                };

                if (name != null)
                {
                    clocks.Add(new Sensor(name, i, SensorType.Clock, this, Settings));
                }
            }

            if (clocks.Count <= 0) continue;
            _clocks = clocks.ToArray();
            clocks.Apply(ActivateSensor);

            break;
        }
    }

    /// <summary>
    /// Updates clock sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateClockSensors()
    {
        if (_clocks is not { Length: > 0 }) return;

        NvApi.NvGpuClockFrequencies clockFrequencies = GetClockFrequencies(out var status);
        if (status != NvApi.NvStatus.OK) return;

        int current = 0;
        for (int i = 0; i < clockFrequencies.Clocks.Length; i++)
        {
            NvApi.NvGpuClockFrequenciesDomain clock = clockFrequencies.Clocks[i];
            if (clock.IsPresent && Enum.IsDefined(typeof(NvApi.NvGpuPublicClockId), i))
            {
                _clocks[current++].Value = clock.Frequency / 1000f;
            }
        }
    }

    /// <summary>
    /// Creates fan sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateFanSensors()
    {
        // Fan coolers
        NvApi.NvFanCoolersStatus fanCoolers = GetFanCoolersStatus(out var status);
        if (status == NvApi.NvStatus.OK && fanCoolers.Count > 0)
        {
            _fans = new Sensor[fanCoolers.Count];

            for (int i = 0; i < fanCoolers.Count; i++)
            {
                NvApi.NvFanCoolersStatusItem item = fanCoolers.Items[i];

                string name = "GPU Fan" + (fanCoolers.Count > 1 ? " " + (i + 1) : string.Empty);

                _fans[i] = new Sensor(name, (int)item.CoolerId, SensorType.Fan, this, Settings);
                ActivateSensor(_fans[i]);
            }
        }
        else
        {
            GetTachReading(out status);
            if (status == NvApi.NvStatus.OK)
            {
                _fans = [new Sensor("GPU", 1, SensorType.Fan, this, Settings)];
                ActivateSensor(_fans[0]);
            }
        }

        // Fan controllers
        NvApi.NvFanCoolerControl fanControllers = GetFanCoolersControllers(out status);
        if (status == NvApi.NvStatus.OK && fanControllers.Count > 0 && fanCoolers.Count > 0)
        {
            _controls = new Sensor[fanControllers.Count];
            _fanControls = new Control[fanControllers.Count];

            for (int i = 0; i < fanControllers.Count; i++)
            {
                NvApi.NvFanCoolerControlItem item = fanControllers.Items[i];

                string name = "GPU Fan" + (fanControllers.Count > 1 ? " " + (i + 1) : string.Empty);

                NvApi.NvFanCoolersStatusItem fanItem = Array.Find(fanCoolers.Items, x => x.CoolerId == item.CoolerId);
                if (fanItem.Equals(default(NvApi.NvFanCoolersStatusItem))) continue;

                _controls[i] = new Sensor(name, (int)item.CoolerId, SensorType.Control, this, Settings);
                ActivateSensor(_controls[i]);

                _fanControls[i] = new Control(_controls[i], Settings, fanItem.CurrentMinLevel, fanItem.CurrentMaxLevel);
                _fanControls[i].ControlModeChanged += ControlModeChanged;
                _fanControls[i].SoftwareControlValueChanged += SoftwareControlValueChanged;
                _controls[i].Control = _fanControls[i];

                ControlModeChanged(_fanControls[i]);
            }
        }
        else
        {
            NvApi.NvCoolerSettings coolerSettings = GetCoolerSettings(out status);
            if (status != NvApi.NvStatus.OK || coolerSettings.Count <= 0) return;

            _controls = new Sensor[coolerSettings.Count];
            _fanControls = new Control[coolerSettings.Count];

            for (int i = 0; i < coolerSettings.Count; i++)
            {
                NvApi.NvCooler cooler = coolerSettings.Cooler[i];
                string name = "GPU Fan" + (coolerSettings.Count > 1 ? " " + cooler.Controller : string.Empty);

                _controls[i] = new Sensor(name, i, SensorType.Control, this, Settings);
                ActivateSensor(_controls[i]);

                _fanControls[i] = new Control(_controls[i], Settings, cooler.DefaultMin, cooler.DefaultMax);
                _fanControls[i].ControlModeChanged += ControlModeChanged;
                _fanControls[i].SoftwareControlValueChanged += SoftwareControlValueChanged;
                _controls[i].Control = _fanControls[i];

                ControlModeChanged(_fanControls[i]);
            }
        }
    }

    /// <summary>
    /// Updates fan sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateFanSensors()
    {
        NvApi.NvFanCoolersStatus fanCoolers = GetFanCoolersStatus(out var status);
        if (_fans is { Length: > 0 })
        {
            if (status == NvApi.NvStatus.OK && fanCoolers.Count > 0)
            {
                for (int i = 0; i < fanCoolers.Count; i++)
                {
                    NvApi.NvFanCoolersStatusItem item = fanCoolers.Items[i];
                    _fans[i].Value = item.CurrentRpm;
                }
            }
            else
            {
                int tachReading = GetTachReading(out status);
                if (status == NvApi.NvStatus.OK)
                {
                    _fans[0].Value = tachReading;
                }
            }
        }

        if (_controls is not { Length: > 0 }) return;


        if (status == NvApi.NvStatus.OK && fanCoolers.Count > 0 && fanCoolers.Count == _controls.Length)
        {
            for (int i = 0; i < fanCoolers.Count; i++)
            {
                NvApi.NvFanCoolersStatusItem item = fanCoolers.Items[i];

                if (Array.Find(_controls, c => c.Index == item.CoolerId) is { } control)
                    control.Value = item.CurrentLevel;
            }
        }
        else
        {
            NvApi.NvCoolerSettings coolerSettings = GetCoolerSettings(out status);
            if (status != NvApi.NvStatus.OK || coolerSettings.Count <= 0) return;
            for (int i = 0; i < coolerSettings.Count; i++)
            {
                NvApi.NvCooler cooler = coolerSettings.Cooler[i];
                _controls[i].Value = cooler.CurrentLevel;
            }
        }

    }

    /// <summary>
    /// Creates load sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateLoadSensors()
    {
        NvApi.NvDynamicPStatesInfo pStatesInfo = GetDynamicPStatesInfoEx(out var pStateStatus);
        if (pStateStatus == NvApi.NvStatus.OK)
        {
            var loads = new List<Sensor>();
            for (int index = 0; index < pStatesInfo.Utilizations.Length; index++)
            {
                NvApi.NvDynamicPState load = pStatesInfo.Utilizations[index];
                if (!load.IsPresent || !Enum.IsDefined(typeof(NvApi.NvUtilizationDomain), index)) continue;
                var utilizationDomain = (NvApi.NvUtilizationDomain)index;
                string name = GetUtilizationDomainName(utilizationDomain);

                if (name != null)
                {
                    loads.Add(new Sensor(name, index, SensorType.Load, this, Settings));
                }
            }

            if (loads.Count > 0)
            {
                _loads = loads.ToArray();
                loads.Apply(ActivateSensor);
            }
        }
        else
        {
            var usages = GetUsages(out var usageStatus);
            if (usageStatus == NvApi.NvStatus.OK)
            {
                var loads = new List<Sensor>();
                for (int index = 0; index < usages.Entries.Length; index++)
                {
                    NvApi.NvUsagesEntry load = usages.Entries[index];
                    if (load.IsPresent <= 0 || !Enum.IsDefined(typeof(NvApi.NvUtilizationDomain), index)) continue;
                    var utilizationDomain = (NvApi.NvUtilizationDomain)index;
                    string name = GetUtilizationDomainName(utilizationDomain);

                    if (name != null)
                    {
                        loads.Add(new Sensor(name, index, SensorType.Load, this, Settings));
                    }
                }

                if (loads.Count > 0)
                {
                    _loads = loads.ToArray();
                    loads.Apply(ActivateSensor);
                }
            }
        }

        // Memory load sensors
        _memoryFree = new Sensor("GPU Memory Free", 0, SensorType.SmallData, this, Settings);
        _memoryUsed = new Sensor("GPU Memory Used", 1, SensorType.SmallData, this, Settings);
        _memoryTotal = new Sensor("GPU Memory Total", 2, SensorType.SmallData, this, Settings);
        _memoryLoad = new Sensor("GPU Memory", 3, SensorType.Load, this, Settings);
    }

    /// <summary>
    /// Updates load sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateLoadSensors()
    {
        if (_loads is not { Length: > 0 }) return;
        NvApi.NvDynamicPStatesInfo pStatesInfo = GetDynamicPStatesInfoEx(out var status);
        if (status == NvApi.NvStatus.OK)
        {
            for (int index = 0; index < pStatesInfo.Utilizations.Length; index++)
            {
                NvApi.NvDynamicPState load = pStatesInfo.Utilizations[index];
                if (load.IsPresent && Enum.IsDefined(typeof(NvApi.NvUtilizationDomain), index))
                {
                    _loads[index].Value = load.Percentage;
                }
            }
        }
        else
        {
            NvApi.NvUsages usages = GetUsages(out status);
            if (status != NvApi.NvStatus.OK) return;
            for (int index = 0; index < usages.Entries.Length; index++)
            {
                NvApi.NvUsagesEntry load = usages.Entries[index];
                if (load.IsPresent > 0 && Enum.IsDefined(typeof(NvApi.NvUtilizationDomain), index))
                {
                    _loads[index].Value = load.Percentage;
                }
            }
        }

        if (_displayHandle == null) return;

        NvApi.NvMemoryInfo memoryInfo = GetMemoryInfo(out status);
        if (status != NvApi.NvStatus.OK) return;

        uint free = memoryInfo.CurrentAvailableDedicatedVideoMemory;
        uint total = memoryInfo.DedicatedVideoMemory;

        _memoryTotal.Value = total / 1024;
        ActivateSensor(_memoryTotal);

        _memoryFree.Value = free / 1024;
        ActivateSensor(_memoryFree);

        _memoryUsed.Value = (total - free) / 1024;
        ActivateSensor(_memoryUsed);

        _memoryLoad.Value = ((float)(total - free) / total) * 100;
        ActivateSensor(_memoryLoad);
    }

    /// <summary>
    /// Creates power sensors.
    /// </summary>
    /// <returns></returns>
    private void CreatePowerSensors()
    {
        NvApi.NvPowerTopology powerTopology = GetPowerTopology(out NvApi.NvStatus powerStatus);
        if (powerStatus != NvApi.NvStatus.OK || powerTopology.Count <= 0) return;
        _powers = new Sensor[powerTopology.Count];
        for (int i = 0; i < powerTopology.Count; i++)
        {
            NvApi.NvPowerTopologyEntry entry = powerTopology.Entries[i];
            string name = entry.Domain switch
            {
                NvApi.NvPowerTopologyDomain.Gpu => "GPU Power",
                NvApi.NvPowerTopologyDomain.Board => "GPU Board Power",
                _ => null
            };

            if (name == null) continue;
            _powers[i] = new Sensor(name, i + (_loads?.Length ?? 0), SensorType.Load, this, Settings);
            ActivateSensor(_powers[i]);
        }
    }

    /// <summary>
    /// Updates power sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdatePowerSensors()
    {
        if (_powers is not { Length: > 0 }) return;

        NvApi.NvPowerTopology powerTopology = GetPowerTopology(out var status);
        if (status != NvApi.NvStatus.OK || powerTopology.Count <= 0) return;

        for (int i = 0; i < powerTopology.Count; i++)
        {
            NvApi.NvPowerTopologyEntry entry = powerTopology.Entries[i];
            _powers[i].Value = entry.PowerUsage / 1000f;
        }
    }

    /// <summary>
    /// Creates NVML sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateNvmlSensors()
    {
        bool hasBusId = NvApi.NvAPI_GPU_GetBusId(_handle, out uint busId) == NvApi.NvStatus.OK;
        if (NvidiaML.IsAvailable || NvidiaML.Initialize())
        {
            _nvmlDevice = hasBusId
                ? NvidiaML.NvmlDeviceGetHandleByPciBusId($" 0000:{busId:X2}:00.0") ??
                  NvidiaML.NvmlDeviceGetHandleByIndex(_adapterIndex)
                : NvidiaML.NvmlDeviceGetHandleByIndex(_adapterIndex);
            if (!_nvmlDevice.HasValue) return;

            _powerUsage = new Sensor("GPU Package", 0, SensorType.Power, this, Settings);
            _pcieThroughputRx = new Sensor("GPU PCIe Rx", 0, SensorType.Throughput, this, Settings);
            _pcieThroughputTx = new Sensor("GPU PCIe Tx", 1, SensorType.Throughput, this, Settings);
        }
    }

    /// <summary>
    /// Updates NVML sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateNvmlSensors()
    {
        if (!NvidiaML.IsAvailable || !_nvmlDevice.HasValue) return;

        int? result = NvidiaML.NvmlDeviceGetPowerUsage(_nvmlDevice.Value);
        if (result.HasValue)
        {
            _powerUsage.Value = result.Value / 1000f;
            ActivateSensor(_powerUsage);
        }

        // In MB/s, throughput sensors are passed as in KB/s.
        uint? rx = NvidiaML.NvmlDeviceGetPcieThroughput(_nvmlDevice.Value, NvidiaML.NvmlPcieUtilCounter.RxBytes);
        if (rx.HasValue)
        {
            _pcieThroughputRx.Value = rx * 1024;
            ActivateSensor(_pcieThroughputRx);
        }

        uint? tx = NvidiaML.NvmlDeviceGetPcieThroughput(_nvmlDevice.Value, NvidiaML.NvmlPcieUtilCounter.TxBytes);
        if (tx.HasValue)
        {
            _pcieThroughputTx.Value = tx * 1024;
            ActivateSensor(_pcieThroughputTx);
        }
    }

    /// <summary>
    /// Creates D3D senors.
    /// </summary>
    /// <returns></returns>
    private void CreateD3dSenors()
    {
        if (Software.OperatingSystem.IsUnix || _nvmlDevice is null) return;
        var pciInfo = NvidiaML.NvmlDeviceGetPciInfo(_nvmlDevice.Value);
        if (pciInfo is not { } pci) return;

        // Get devices
        string[] deviceIds = D3DDisplayDevice.GetDeviceIdentifiers();
        if (deviceIds == null) return;

        // Process devices
        foreach (string deviceId in deviceIds)
        {
            if (deviceId.IndexOf($"VEN_{pci.pciVendorId:X}",
                    StringComparison.OrdinalIgnoreCase) == -1 ||
                deviceId.IndexOf($"DEV_{pci.pciDeviceId:X}",
                    StringComparison.OrdinalIgnoreCase) == -1 ||
                deviceId.IndexOf($"SUBSYS_{pci.pciSubSystemId:X}",
                    StringComparison.OrdinalIgnoreCase) == -1) continue;
            bool isMatch = false;

            string actualDeviceId = D3DDisplayDevice.GetActualDeviceIdentifier(deviceId);
            try
            {
#pragma warning disable CA1416
                if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\nvlddmkm\Enum",
                        _adapterIndex.ToString(), null) is string adapterPnpId)
#pragma warning restore CA1416
                {
                    if (actualDeviceId.IndexOf(adapterPnpId, StringComparison.OrdinalIgnoreCase) != -1 ||
                        adapterPnpId.IndexOf(actualDeviceId, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        isMatch = true;
                    }
                }
            }
            catch
            {
                // Do Nothing
            }

            if (!isMatch)
            {
                try
                {
                    string path = actualDeviceId;
                    path = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\" + path;

#pragma warning disable CA1416
                    if (Registry.GetValue(path, "LocationInformation", null) is string locationInformation)
#pragma warning restore CA1416
                    {
                        // For example:
                        // @System32\drivers\pci.sys,#65536;PCI bus %1, device %2, function %3;(38,0,0)

                        int index = locationInformation.IndexOf('(');
                        if (index != -1)
                        {
                            index++;
                            int secondIndex = locationInformation.IndexOf(',', index);
                            if (secondIndex != -1)
                            {
                                string bus = locationInformation.Substring(index, secondIndex - index);

                                if (pci.bus.ToString() == bus)
                                {
                                    isMatch = true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Ignored.
                }
            }

            if (!isMatch || !D3DDisplayDevice.GetDeviceInfoByIdentifier(deviceId, out var deviceInfo)) continue;
            int nodeSensorIndex = (_loads?.Length ?? 0) + (_powers?.Length ?? 0);
            int memorySensorIndex = 4; // There are 4 normal GPU memory sensors.

            _d3dDeviceId = deviceId;

            // Dedicated memory usage
            _gpuDedicatedMemoryUsage = new Sensor("D3D Dedicated Memory Used", memorySensorIndex++, SensorType.SmallData, this, Settings);

            // Shared memory usage
            _gpuSharedMemoryUsage = new Sensor("D3D Shared Memory Used", memorySensorIndex, SensorType.SmallData, this, Settings);

            // Node usage
            _gpuNodeUsage = new Sensor[deviceInfo.Nodes.Length];
            _gpuNodeUsagePrevValue = new long[deviceInfo.Nodes.Length];
            _gpuNodeUsagePrevTick = new DateTime[deviceInfo.Nodes.Length];

            foreach (var node in deviceInfo.Nodes.OrderBy(x => x.Name))
            {
                _gpuNodeUsage[node.Id] = new Sensor(node.Name, nodeSensorIndex++, SensorType.Load, this, Settings);
                _gpuNodeUsagePrevValue[node.Id] = node.RunningTime;
                _gpuNodeUsagePrevTick[node.Id] = node.QueryTime;
            }
        }
    }

    /// <summary>
    /// Updates D3D sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateD3dSensors()
    {
        if (_d3dDeviceId == null ||
            !D3DDisplayDevice.GetDeviceInfoByIdentifier(_d3dDeviceId, out D3DDisplayDevice.D3DDeviceInfo deviceInfo))
            return;

        _gpuDedicatedMemoryUsage.Value = 1f * deviceInfo.GpuDedicatedUsed / 1024 / 1024;
        _gpuSharedMemoryUsage.Value = 1f * deviceInfo.GpuSharedUsed / 1024 / 1024;
        ActivateSensor(_gpuDedicatedMemoryUsage);
        ActivateSensor(_gpuSharedMemoryUsage);

        foreach (D3DDisplayDevice.D3DDeviceNodeInfo node in deviceInfo.Nodes)
        {
            long runningTimeDiff = node.RunningTime - _gpuNodeUsagePrevValue[node.Id];
            long timeDiff = node.QueryTime.Ticks - _gpuNodeUsagePrevTick[node.Id].Ticks;

            _gpuNodeUsage[node.Id].Value = 100f * runningTimeDiff / timeDiff;
            _gpuNodeUsagePrevValue[node.Id] = node.RunningTime;
            _gpuNodeUsagePrevTick[node.Id] = node.QueryTime;
            ActivateSensor(_gpuNodeUsage[node.Id]);
        }
    }

    #endregion

    #endregion
}
