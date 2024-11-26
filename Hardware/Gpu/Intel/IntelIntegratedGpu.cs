using System;
using System.Globalization;
using System.Linq;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.Intel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.D3d;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.Intel;

/// <summary>
/// Intel Integrated GPU
/// </summary>
/// <seealso cref="Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.GpuBase" />
internal sealed class IntelIntegratedGpu : GpuBase
{
    #region Fields

    // ReSharper disable once InconsistentNaming
    private const uint MSR_PP1_ENERGY_STATUS = 0x641;

    private readonly string _deviceId;
    private Sensor _dedicatedMemoryUsage;
    private Sensor _sharedMemoryLimit;
    private Sensor _sharedMemoryFree;
    private float _energyUnitMultiplier;
    private Sensor[] _nodeUsage;
    private DateTime[] _nodeUsagePrevTick;
    private long[] _nodeUsagePrevValue;
    private Sensor _powerSensor;
    private Sensor _sharedMemoryUsage;

    private uint _lastEnergyConsumed;
    private DateTime _lastEnergyTime;

    #endregion

    #region Properties

    /// <inheritdoc />
    public override string DeviceId => D3dDisplayDevice.GetActualDeviceIdentifier(_deviceId);

    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.GpuIntel;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IntelIntegratedGpu"/> class.
    /// </summary>
    /// <param name="intelCpu">The intel cpu.</param>
    /// <param name="deviceId">The device identifier.</param>
    /// <param name="deviceInfo">The device information.</param>
    /// <param name="settings">The settings.</param>
    public IntelIntegratedGpu(IntelCpu intelCpu, string deviceId, D3dDeviceInfo deviceInfo, ISettings settings)
        : base(GetName(deviceId),
               new Identifier("gpu-intel-integrated", deviceId.ToString(CultureInfo.InvariantCulture)), settings)
    {
        _deviceId = deviceId;

        // Create sensors
        CreateSensors(intelCpu, deviceInfo);
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Update()
    {
        if (!D3dDisplayDevice.GetDeviceInfoByIdentifier(_deviceId, out D3dDeviceInfo deviceInfo)) return;

        // Dedicated memory usage
        if (_dedicatedMemoryUsage != null)
        {
            _dedicatedMemoryUsage.Value = 1f * deviceInfo.GpuDedicatedUsed / 1024 / 1024;
            ActivateSensor(_dedicatedMemoryUsage);
        }

        // Shared Memory limit
        if (_sharedMemoryLimit != null)
        {
            _sharedMemoryLimit.Value = 1f * deviceInfo.GpuSharedLimit / 1024 / 1024;
            ActivateSensor(_sharedMemoryLimit);
            if (_sharedMemoryUsage != null)
            {
                _sharedMemoryFree.Value = _sharedMemoryLimit.Value - _sharedMemoryUsage.Value;
                ActivateSensor(_sharedMemoryFree);
            }
        }

        // Shared memory usage
        if (_sharedMemoryUsage != null)
        {
            _sharedMemoryUsage.Value = 1f * deviceInfo.GpuSharedUsed / 1024 / 1024;
            ActivateSensor(_sharedMemoryUsage);
        }

        // Power sensor
        if (_powerSensor != null && Ring0.ReadMsr(MSR_PP1_ENERGY_STATUS, out uint eax, out uint _))
        {
            DateTime time = DateTime.UtcNow;
            float deltaTime = (float)(time - _lastEnergyTime).TotalSeconds;
            if (deltaTime >= 0.01)
            {
                _powerSensor.Value = _energyUnitMultiplier * unchecked(eax - _lastEnergyConsumed) / deltaTime;
                _lastEnergyTime = time;
                _lastEnergyConsumed = eax;
            }
        }

        // D3D device info
        if (_nodeUsage.Length != deviceInfo.Nodes.Length) return;
        foreach (D3dDeviceNodeInfo node in deviceInfo.Nodes)
        {
            long runningTimeDiff = node.RunningTime - _nodeUsagePrevValue[node.Id];
            long timeDiff = node.QueryTime.Ticks - _nodeUsagePrevTick[node.Id].Ticks;

            _nodeUsage[node.Id].Value = 100f * runningTimeDiff / timeDiff;
            _nodeUsagePrevValue[node.Id] = node.RunningTime;
            _nodeUsagePrevTick[node.Id] = node.QueryTime;
            ActivateSensor(_nodeUsage[node.Id]);
        }
    }

    /// <summary>
    /// Creates the sensors.
    /// </summary>
    /// <param name="intelCpu">The intel cpu.</param>
    /// <param name="deviceInfo">The device information.</param>
    private void CreateSensors(IntelCpu intelCpu, D3dDeviceInfo deviceInfo)
    {
        int memorySensorIndex = 0;

        // Dedicated memory usage
        if (deviceInfo.GpuDedicatedLimit > 0)
        {
            _dedicatedMemoryUsage = new Sensor("D3D Dedicated Memory Used", memorySensorIndex++, SensorType.SmallData, this, Settings);
        }

        // Shared memory usage
        _sharedMemoryUsage = new Sensor("D3D Shared Memory Used", memorySensorIndex++, SensorType.SmallData, this, Settings);
        if (deviceInfo.GpuSharedLimit > 0)
        {
            _sharedMemoryFree = new Sensor("D3D Shared Memory Free", memorySensorIndex++, SensorType.SmallData, this, Settings);
            _sharedMemoryLimit = new Sensor("D3D Shared Memory Total", memorySensorIndex++, SensorType.SmallData, this, Settings);
        }

        // Power sensor
        if (Ring0.ReadMsr(MSR_PP1_ENERGY_STATUS, out uint eax, out uint _))
        {
            _energyUnitMultiplier = intelCpu.EnergyUnitsMultiplier;
            if (_energyUnitMultiplier != 0)
            {
                _lastEnergyTime = DateTime.UtcNow;
                _lastEnergyConsumed = eax;
                _powerSensor = new Sensor("GPU Power", 0, SensorType.Power, this, Settings);
                ActivateSensor(_powerSensor);
            }
        }

        // D3D device info
        _nodeUsage = new Sensor[deviceInfo.Nodes.Length];
        _nodeUsagePrevValue = new long[deviceInfo.Nodes.Length];
        _nodeUsagePrevTick = new DateTime[deviceInfo.Nodes.Length];
        int nodeSensorIndex = 0;
        foreach (D3dDeviceNodeInfo node in deviceInfo.Nodes.OrderBy(x => x.Name))
        {
            _nodeUsage[node.Id] = new Sensor(node.Name, nodeSensorIndex++, SensorType.Load, this, Settings);
            _nodeUsagePrevValue[node.Id] = node.RunningTime;
            _nodeUsagePrevTick[node.Id] = node.QueryTime;
        }
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <param name="deviceId">The device identifier.</param>
    /// <returns></returns>
    private static string GetName(string deviceId)
    {
        var result = "Intel Integrated Graphics";
        
        #if WINDOWS
        string path = @$"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\{D3DDisplayDevice.GetActualDeviceIdentifier(deviceId)}";
        if (Registry.GetValue(path, "DeviceDesc", null) is string deviceDesc)
        {
            return deviceDesc.Split(';').Last();
        }
#endif

        return result;
    }

    #endregion
}
