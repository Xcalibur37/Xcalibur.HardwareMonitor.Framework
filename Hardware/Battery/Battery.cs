using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Battery;

/// <summary>
/// Battery class
/// </summary>
/// <seealso cref="Hardware" />
internal sealed class Battery : Hardware
{
    #region Fields

    private readonly SafeFileHandle _batteryHandle;
    private readonly Kernel32.BATTERY_INFORMATION _batteryInformation;
    private readonly uint _batteryTag;
    private Sensor _chargeDischargeCurrentSensor;
    private Sensor _chargeDischargeRateSensor;
    private Sensor _chargeLevelSensor;
    private Sensor _degradationPercentageSensor;
    private Sensor _designedCapacitySensor;
    private Sensor _fullChargedCapacitySensor;
    private Sensor _remainingCapacitySensor;
    private Sensor _remainingTimeSensor;
    private Sensor _voltageSensor;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the charge discharge current.
    /// </summary>
    /// <value>
    /// The charge discharge current.
    /// </value>
    public float ChargeDischargeCurrent { get; private set; }

    /// <summary>
    /// Gets the charge discharge rate.
    /// </summary>
    /// <value>
    /// The charge discharge rate.
    /// </value>
    public float ChargeDischargeRate { get; private set; }

    /// <summary>
    /// Gets the charge level.
    /// </summary>
    /// <value>
    /// The charge level.
    /// </value>
    public float ChargeLevel { get; private set; }

    /// <summary>
    /// Gets the chemistry.
    /// </summary>
    /// <value>
    /// The chemistry.
    /// </value>
    public BatteryChemistry Chemistry { get; }

    /// <summary>
    /// Gets the degradation level.
    /// </summary>
    /// <value>
    /// The degradation level.
    /// </value>
    public float DegradationLevel { get; }

    /// <summary>
    /// Gets the designed capacity.
    /// </summary>
    /// <value>
    /// The designed capacity.
    /// </value>
    public float DesignedCapacity { get; }

    /// <summary>
    /// Gets the full charged capacity.
    /// </summary>
    /// <value>
    /// The full charged capacity.
    /// </value>
    public float FullChargedCapacity { get; }

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.Battery;

    /// <summary>
    /// Gets the manufacturer.
    /// </summary>
    /// <value>
    /// The manufacturer.
    /// </value>
    public string Manufacturer { get; }

    /// <summary>
    /// Gets the remaining capacity.
    /// </summary>
    /// <value>
    /// The remaining capacity.
    /// </value>
    public float RemainingCapacity { get; private set; }

    /// <summary>
    /// Gets the remaining time.
    /// </summary>
    /// <value>
    /// The remaining time.
    /// </value>
    public uint RemainingTime { get; private set; }

    /// <summary>
    /// Gets the voltage.
    /// </summary>
    /// <value>
    /// The voltage.
    /// </value>
    public float Voltage { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Battery"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="batteryHandle">The battery handle.</param>
    /// <param name="batteryInfo">The battery information.</param>
    /// <param name="batteryTag">The battery tag.</param>
    /// <param name="settings">The settings.</param>
    public Battery (
        string name,
        string manufacturer,
        SafeFileHandle batteryHandle,
        Kernel32.BATTERY_INFORMATION batteryInfo,
        uint batteryTag,
        ISettings settings) :
        base(name, new Identifier("battery"), settings)
    {
        Name = name;
        Manufacturer = manufacturer;

        _batteryTag = batteryTag;
        _batteryHandle = batteryHandle;
        _batteryInformation = batteryInfo;

        Chemistry = GetBatteryChemistry(batteryInfo.Chemistry);
        DegradationLevel = 100f - (batteryInfo.FullChargedCapacity * 100f / batteryInfo.DesignedCapacity);
        DesignedCapacity = batteryInfo.DesignedCapacity;
        FullChargedCapacity = batteryInfo.FullChargedCapacity;

        // Create sensors
        CreateSensors();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Update()
    {
        Kernel32.BATTERY_WAIT_STATUS bws = new()
        {
            BatteryTag = _batteryTag
        };

        Kernel32.BATTERY_STATUS batteryStatus = new();
        if (Kernel32.DeviceIoControl(_batteryHandle,
                                     Kernel32.IOCTL.IOCTL_BATTERY_QUERY_STATUS,
                                     ref bws,
                                     Marshal.SizeOf(bws),
                                     ref batteryStatus,
                                     Marshal.SizeOf(batteryStatus),
                                     out _,
                                     IntPtr.Zero))
        {
            // Designed Capacity
            _designedCapacitySensor.Value = Convert.ToSingle(_batteryInformation.DesignedCapacity);

            // Full Charged Capacity
            _fullChargedCapacitySensor.Value = Convert.ToSingle(_batteryInformation.FullChargedCapacity);
            
            // Remaining Capacity
            _remainingCapacitySensor.Value = Convert.ToSingle(batteryStatus.Capacity);
            RemainingCapacity = Convert.ToSingle(batteryStatus.Capacity);
            
            // Voltage
            _voltageSensor.Value = Convert.ToSingle(batteryStatus.Voltage) / 1000f;
            Voltage = Convert.ToSingle(batteryStatus.Voltage) / 1000f;

            // Charge Level
            _chargeLevelSensor.Value = _remainingCapacitySensor.Value * 100f / _fullChargedCapacitySensor.Value;
            ChargeLevel = (_remainingCapacitySensor.Value * 100f / _fullChargedCapacitySensor.Value).GetValueOrDefault();

            // Charge/Discharge Rate
            ChargeDischargeRate = batteryStatus.Rate / 1000f;
            
            // Update the charge rates
            UpdateChargeRates(batteryStatus.Rate);
            
            // Degradation Level
            _degradationPercentageSensor.Value = 100f - (_fullChargedCapacitySensor.Value * 100f / _designedCapacitySensor.Value);
        }

        // Remaining Time
        UpdateRemainingTime();
    }

    /// <inheritdoc />
    public override void Close()
    {
        base.Close();
        _batteryHandle.Close();
    }

    /// <summary>
    /// Creates the sensors.
    /// </summary>
    private void CreateSensors()
    {
        // Designed Capacity
        _designedCapacitySensor = new Sensor("Designed Capacity", 3, SensorType.Energy, this, Settings);
        ActivateSensor(_designedCapacitySensor);

        // Full Charged Capacity
        _fullChargedCapacitySensor = new Sensor("Full Charged Capacity", 4, SensorType.Energy, this, Settings);
        ActivateSensor(_fullChargedCapacitySensor);

        // Remaining Capacity
        _remainingCapacitySensor = new Sensor("Remaining Capacity", 5, SensorType.Energy, this, Settings);
        ActivateSensor(_remainingCapacitySensor);

        // Voltage
        _voltageSensor = new Sensor("Voltage", 1, SensorType.Voltage, this, Settings);
        ActivateSensor(_voltageSensor);

        // Charge Level
        _chargeLevelSensor = new Sensor("Charge Level", 0, SensorType.Level, this, Settings);
        ActivateSensor(_chargeLevelSensor);

        // Charge/Discharge Rate
        _chargeDischargeRateSensor = new Sensor("Charge/Discharge Rate", 0, SensorType.Power, this, Settings);
        ActivateSensor(_chargeDischargeRateSensor);

        // Discharge Current
        _chargeDischargeCurrentSensor = new Sensor("Current", 2, SensorType.Current, this, Settings);
        ActivateSensor(_chargeDischargeCurrentSensor);
        
        // Degradation Level
        _degradationPercentageSensor = new Sensor("Degradation Level", 0, SensorType.Level, this, Settings);
        ActivateSensor(_degradationPercentageSensor);

        // Remaining Time
        _remainingTimeSensor = new Sensor("Remaining Time (Estimated)", 0, SensorType.TimeSpan, this, Settings);
        ActivateSensor(_remainingTimeSensor);
    }

    /// <summary>
    /// Gets the battery chemistry.
    /// </summary>
    /// <param name="chemistry">The chemistry.</param>
    /// <returns></returns>
    private static BatteryChemistry GetBatteryChemistry(char[] chemistry)
    {
        BatteryChemistry batteryChemistry;
        if (chemistry.SequenceEqual(['P', 'b', 'A', 'c']))
        {
            batteryChemistry = BatteryChemistry.LeadAcid;
        }
        else if (chemistry.SequenceEqual(['L', 'I', 'O', 'N']) || chemistry.SequenceEqual(['L', 'i', '-', 'I']))
        {
            batteryChemistry = BatteryChemistry.LithiumIon;
        }
        else if (chemistry.SequenceEqual(['N', 'i', 'C', 'd']))
        {
            batteryChemistry = BatteryChemistry.NickelCadmium;
        }
        else if (chemistry.SequenceEqual(['N', 'i', 'M', 'H']))
        {
            batteryChemistry = BatteryChemistry.NickelMetalHydride;
        }
        else if (chemistry.SequenceEqual(['N', 'i', 'Z', 'n']))
        {
            batteryChemistry = BatteryChemistry.NickelZinc;
        }
        else if (chemistry.SequenceEqual(['R', 'A', 'M', '\x00']))
        {
            batteryChemistry = BatteryChemistry.AlkalineManganese;
        }
        else
        {
            batteryChemistry = BatteryChemistry.Unknown;
        }
        return batteryChemistry;
    }

    /// <summary>
    /// Updates the charge rates.
    /// </summary>
    /// <param name="rate">The rate.</param>
    private void UpdateChargeRates(int rate)
    {
        switch (rate)
        {
            case > 0:
                // Charge/Discharge Rate
                _chargeDischargeRateSensor.Name = "Charge Rate";
                _chargeDischargeRateSensor.Value = rate / 1000f;

                // Discharge Current
                _chargeDischargeCurrentSensor.Name = "Charge Current";
                _chargeDischargeCurrentSensor.Value = _chargeDischargeRateSensor.Value / _voltageSensor.Value;
                ChargeDischargeCurrent = (_chargeDischargeRateSensor.Value / _voltageSensor.Value).GetValueOrDefault();
                break;
            case < 0:
                // Charge/Discharge Rate
                _chargeDischargeRateSensor.Name = "Discharge Rate";
                _chargeDischargeRateSensor.Value = Math.Abs(rate / 1000f);

                // Discharge Current
                _chargeDischargeCurrentSensor.Name = "Discharge Current";
                _chargeDischargeCurrentSensor.Value = _chargeDischargeRateSensor.Value / _voltageSensor.Value;
                ChargeDischargeCurrent = (_chargeDischargeRateSensor.Value / _voltageSensor.Value).GetValueOrDefault();
                break;
            default:
                // Charge/Discharge Rate
                _chargeDischargeRateSensor.Name = "Charge/Discharge Rate";
                _chargeDischargeRateSensor.Value = 0f;
                ChargeDischargeRate = 0f;

                // Discharge Current
                _chargeDischargeCurrentSensor.Name = "Charge/Discharge Current";
                _chargeDischargeCurrentSensor.Value = 0f;
                ChargeDischargeCurrent = 0f;
                break;
        }
    }

    /// <summary>
    /// Updates the remaining time.
    /// </summary>
    private void UpdateRemainingTime()
    {
        uint estimatedRunTime = 0;
        Kernel32.BATTERY_QUERY_INFORMATION bqi = new()
        {
            BatteryTag = _batteryTag,
            InformationLevel = Kernel32.BATTERY_QUERY_INFORMATION_LEVEL.BatteryEstimatedTime
        };

        // Remaining Time
        if (Kernel32.DeviceIoControl(_batteryHandle,
                Kernel32.IOCTL.IOCTL_BATTERY_QUERY_INFORMATION,
                ref bqi,
                Marshal.SizeOf(bqi),
                ref estimatedRunTime,
                Marshal.SizeOf<uint>(),
                out _,
                IntPtr.Zero))
        {
            RemainingTime = estimatedRunTime;
            if (estimatedRunTime != Kernel32.BATTERY_UNKNOWN_TIME)
            {
                ActivateSensor(_remainingTimeSensor);
                _remainingTimeSensor.Value = estimatedRunTime;
            }
            else
            {
                DeactivateSensor(_remainingTimeSensor);
            }
        }
    }

    #endregion
}
