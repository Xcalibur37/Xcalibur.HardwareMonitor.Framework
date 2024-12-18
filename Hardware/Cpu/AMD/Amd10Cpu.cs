﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD;

/// <summary>
/// AMD 10-series CPU
/// </summary>
/// <seealso cref="AmdCpuBase" />
internal sealed class Amd10Cpu : AmdCpuBase
{
    #region Fields

    private const uint ClockPowerTimingControl0Register = 0xD4;
    private const uint CofvidStatus = 0xC0010071;
    private const uint CstatesIoPort = 0xCD6;
    private const uint SmuReportedTempCtrlOffset = 0xD8200CA4;
    private const uint Hwcr = 0xC0010015;
    private const byte MiscellaneousControlFunction = 3;
    private const uint PState0 = 0xC0010064;
    private const uint PerfCtl0 = 0xC0010000;
    private const uint PerfCtr0 = 0xC0010004;
    private const uint ReportedTemperatureControlRegister = 0xA4;

    private const ushort Family10HMiscellaneousControlDeviceId = 0x1203;
    private const ushort Family11HMiscellaneousControlDeviceId = 0x1303;
    private const ushort Family12HMiscellaneousControlDeviceId = 0x1703;
    private const ushort Family14HMiscellaneousControlDeviceId = 0x1703;
    private const ushort Family15HModel00MiscControlDeviceId = 0x1603;
    private const ushort Family15HModel10MiscControlDeviceId = 0x1403;
    private const ushort Family15HModel30MiscControlDeviceId = 0x141D;
    private const ushort Family15HModel60MiscControlDeviceId = 0x1573;
    private const ushort Family15HModel70MiscControlDeviceId = 0x15B3;
    private const ushort Family16HModel00MiscControlDeviceId = 0x1533;
    private const ushort Family16HModel30MiscControlDeviceId = 0x1583;
    
    private Sensor _busClock;
    private Sensor[] _coreClocks;
    private Sensor _coreTemperature;
    private Sensor _coreVoltage;
    private byte _cStatesIoOffset;
    private Sensor[] _cStatesResidency;
    private bool _hasSmuTemperatureRegister;
    // Does this processor support the Serial VID (Voltage Identification) Interface 2.0 
    private bool _isSvi2;
    private uint _miscellaneousControlAddress;
    private Sensor _northBridgeVoltage;
    private FileStream _temperatureStream;
    private double _timeStampCounterMultiplier;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Amd10Cpu"/> class.
    /// </summary>
    /// <param name="processorIndex">Index of the processor.</param>
    /// <param name="cpuId">The cpu identifier.</param>
    /// <param name="settings">The settings.</param>
    public Amd10Cpu(int processorIndex, CpuId[][] cpuId, ISettings settings) : base(processorIndex, cpuId, settings)
    {
        // Is AMD SVI2
        _isSvi2 = (Family == 0x15 && Model >= 0x10) || Family == 0x16;

        // Set Misc. Control Address
        SetMiscellaneousControlAddress();

        // Sensors
        CreateTemperatureSensors();
        CreateClockSensors();
        CreateVoltageSensors();
        CreatePowerSavingModeSensors();

        // Performance counters
        SetPerformanceCounters();
        
        // Initialize
        Initialize();

        // Update
        Update();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Closes Hardware
    /// </summary>
    /// <inheritdoc />
    public override void Close()
    {
        _temperatureStream?.Close();
        base.Close();
    }

    /// <summary>
    /// Gets the MSRS.
    /// </summary>
    /// <returns></returns>
    protected override uint[] GetMsrs() => [PerfCtl0, PerfCtr0, Hwcr, PState0, CofvidStatus];

    /// <summary>
    /// Updates all sensors.
    /// </summary>
    /// <inheritdoc />
    public override void Update()
    {
        base.Update();

        // Sensors
        UpdateTemperatureSensors();
        UpdateClockSensors();
        UpdateVoltageSensors();
        UpdatePowerSavingModeSensors();
    }

    /// <summary>
    /// Reads the first line.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns></returns>
    private static string ReadFirstLine(Stream stream)
    {
        StringBuilder stringBuilder = new();

        try
        {
            stream.Seek(0, SeekOrigin.Begin);
            int b = stream.ReadByte();
            while (b is not -1 and not 10)
            {
                stringBuilder.Append((char)b);
                b = stream.ReadByte();
            }
        }
        catch
        {
            // Do nothing
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Reads the smu register.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private static bool ReadSmuRegister(uint address, out uint value)
    {
        // Block
        if (Mutexes.WaitPciBus(10))
        {
            if (!Ring0.WritePciConfig(0, 0xB8, address))
            {
                value = 0;

                Mutexes.ReleasePciBus();
                return false;
            }

            var result = Ring0.ReadPciConfig(0, 0xBC, out value);

            // Release
            Mutexes.ReleasePciBus();
            return result;
        }

        value = 0;
        return false;
    }

    /// <summary>
    /// Estimates the time stamp counter multiplier.
    /// </summary>
    /// <returns></returns>
    private double EstimateTimeStampCounterMultiplier()
    {
        // preload the function
        EstimateTimeStampCounterMultiplier(0);
        EstimateTimeStampCounterMultiplier(0);

        // estimate the multiplier
        List<double> estimate = new(3);
        for (int i = 0; i < 3; i++)
        {
            estimate.Add(EstimateTimeStampCounterMultiplier(0.025));
        }

        estimate.Sort();
        return estimate[1];
    }

    /// <summary>
    /// Estimates the time stamp counter multiplier.
    /// </summary>
    /// <param name="timeWindow">The time window.</param>
    /// <returns></returns>
    private double EstimateTimeStampCounterMultiplier(double timeWindow)
    {
        // select event "076h CPU Clocks not Halted" and enable the counter
        Ring0.WriteMsr(PerfCtl0,
                       (1 << 22) | // enable performance counter
                       (1 << 17) | // count events in user mode
                       (1 << 16) | // count events in operating-system mode
                       0x76,
                       0x00000000);

        // set the counter to 0
        Ring0.WriteMsr(PerfCtr0, 0, 0);

        long ticks = (long)(timeWindow * Stopwatch.Frequency);
        
        long timeBegin = Stopwatch.GetTimestamp() + (long)Math.Ceiling(0.001 * ticks);
        while (Stopwatch.GetTimestamp() < timeBegin) { }
        Ring0.ReadMsr(PerfCtr0, out uint lsbBegin, out uint msbBegin);

        long timeEnd = timeBegin + ticks;
        while (Stopwatch.GetTimestamp() < timeEnd) { }
        Ring0.ReadMsr(PerfCtr0, out uint lsbEnd, out uint msbEnd);
        
        Ring0.ReadMsr(CofvidStatus, out uint eax, out uint _);
        double coreMultiplier = GetCoreMultiplier(eax);
        ulong countBegin = ((ulong)msbBegin << 32) | lsbBegin;
        ulong countEnd = ((ulong)msbEnd << 32) | lsbEnd;

        double coreFrequency = 1e-6 * ((double)(countEnd - countBegin) * Stopwatch.Frequency) / (timeEnd - timeBegin);
        double busFrequency = coreFrequency / coreMultiplier;
        return 0.25 * Math.Round(4 * TimeStampCounterFrequency / busFrequency);
    }

    /// <summary>
    /// Gets the core multiplier.
    /// </summary>
    /// <param name="cofVidEax">The cof vid eax.</param>
    /// <returns></returns>
    private double GetCoreMultiplier(uint cofVidEax)
    {
        uint cpuDid;
        uint cpuFid;

        switch (Family)
        {
            case 0x10:
            case 0x11:
            case 0x15:
            case 0x16:
                // 8:6 CpuDid: current core divisor ID
                // 5:0 CpuFid: current core frequency ID
                cpuDid = (cofVidEax >> 6) & 7;
                cpuFid = cofVidEax & 0x1F;
                return 0.5 * (cpuFid + 0x10) / (1 << (int)cpuDid);

            case 0x12:
                // 8:4 CpuFid: current CPU core frequency ID
                // 3:0 CpuDid: current CPU core divisor ID
                cpuFid = (cofVidEax >> 4) & 0x1F;
                cpuDid = cofVidEax & 0xF;
                double divisor = cpuDid switch
                {
                    0 => 1,
                    1 => 1.5,
                    2 => 2,
                    3 => 3,
                    4 => 4,
                    5 => 6,
                    6 => 8,
                    7 => 12,
                    8 => 16,
                    _ => 1
                };
                return (cpuFid + 0x10) / divisor;

            case 0x14:
                // 8:4: current CPU core divisor ID most significant digit
                // 3:0: current CPU core divisor ID least significant digit
                uint divisorIdMsd = (cofVidEax >> 4) & 0x1F;
                uint divisorIdLsd = cofVidEax & 0xF;
                Ring0.ReadPciConfig(_miscellaneousControlAddress, ClockPowerTimingControl0Register, out uint value);
                uint frequencyId = value & 0x1F;
                return (frequencyId + 0x10) / (divisorIdMsd + (divisorIdLsd * 0.25) + 1);

            default:
                return 1;
        }
    }

    /// <summary>
    /// Sets the miscellaneous control address.
    /// </summary>
    private void SetMiscellaneousControlAddress()
    {
        // AMD family 1Xh processors support only one temperature sensor
        ushort miscellaneousControlDeviceId;
        switch (Family)
        {
            case 0x10:
                miscellaneousControlDeviceId = Family10HMiscellaneousControlDeviceId;
                break;
            case 0x11:
                miscellaneousControlDeviceId = Family11HMiscellaneousControlDeviceId;
                break;
            case 0x12:
                miscellaneousControlDeviceId = Family12HMiscellaneousControlDeviceId;
                break;
            case 0x14:
                miscellaneousControlDeviceId = Family14HMiscellaneousControlDeviceId;
                break;
            case 0x15:
                switch (Model & 0xF0)
                {
                    case 0x00:
                        miscellaneousControlDeviceId = Family15HModel00MiscControlDeviceId;
                        break;
                    case 0x10:
                        miscellaneousControlDeviceId = Family15HModel10MiscControlDeviceId;
                        break;
                    case 0x30:
                        miscellaneousControlDeviceId = Family15HModel30MiscControlDeviceId;
                        break;
                    case 0x70:
                        miscellaneousControlDeviceId = Family15HModel70MiscControlDeviceId;
                        _hasSmuTemperatureRegister = true;
                        break;
                    case 0x60:
                        miscellaneousControlDeviceId = Family15HModel60MiscControlDeviceId;
                        _hasSmuTemperatureRegister = true;
                        break;
                    default:
                        miscellaneousControlDeviceId = 0;
                        break;
                }

                break;
            case 0x16:
                miscellaneousControlDeviceId = (Model & 0xF0) switch
                {
                    0x00 => Family16HModel00MiscControlDeviceId,
                    0x30 => Family16HModel30MiscControlDeviceId,
                    _ => 0
                };
                break;
            default:
                miscellaneousControlDeviceId = 0;
                break;
        }

        // get the pci address for the Miscellaneous Control registers
        _miscellaneousControlAddress = GetPciAddress(MiscellaneousControlFunction, miscellaneousControlDeviceId);
    }

    /// <summary>
    /// Sets the performance counters.
    /// </summary>
    private void SetPerformanceCounters()
    {
        bool corePerformanceBoostSupport = (Cpu0.ExtData[7, 3] & (1 << 9)) > 0;

        // Set affinity to the first thread for all frequency estimations
        GroupAffinity previousAffinity = ThreadAffinity.Set(Cpu0.Affinity);

        // Disable core performance boost
        Ring0.ReadMsr(Hwcr, out uint hwcrEax, out uint hwcrEdx);
        if (corePerformanceBoostSupport)
        {
            Ring0.WriteMsr(Hwcr, hwcrEax | (1 << 25), hwcrEdx);
        }
        Ring0.ReadMsr(PerfCtl0, out uint ctlEax, out uint ctlEdx);
        Ring0.ReadMsr(PerfCtr0, out uint ctrEax, out uint ctrEdx);

        _timeStampCounterMultiplier = EstimateTimeStampCounterMultiplier();

        // Restore the performance counter registers
        Ring0.WriteMsr(PerfCtl0, ctlEax, ctlEdx);
        Ring0.WriteMsr(PerfCtr0, ctrEax, ctrEdx);

        // Restore core performance boost
        if (corePerformanceBoostSupport)
        {
            Ring0.WriteMsr(Hwcr, hwcrEax, hwcrEdx);
        }

        // Restore the thread affinity.
        ThreadAffinity.Set(previousAffinity);
    }

    /// <summary>
    /// Create CPU temperature sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateTemperatureSensors()
    {
        _coreTemperature = new Sensor(CpuConstants.CpuCores,
        0,
            SensorType.Temperature,
            this,
            [],
            Settings);

        // the file reader for lm-sensors support on Linux
        _temperatureStream = null;

        // Linux-only
        if (!Software.OperatingSystem.IsUnix) return;
        foreach (string path in Directory.GetDirectories("/sys/class/hwmon/"))
        {
            string name = null;
            try
            {
                using StreamReader reader = new(path + "/device/name");
                name = reader.ReadLine();
            }
            catch (IOException)
            {
                // Do nothing
            }

            // Set stream
            _temperatureStream = name switch
            {
                "k10temp" => new FileStream(path + "/device/temp1_input", FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                _ => _temperatureStream
            };
        }
    }

    /// <summary>
    /// Updates the temperature sensors.
    /// </summary>
    private void UpdateTemperatureSensors()
    {
        if (_temperatureStream == null)
        {
            if (_miscellaneousControlAddress != Interop.Ring0.InvalidPciAddress)
            {
                var isValueValid = _hasSmuTemperatureRegister
                    ? ReadSmuRegister(SmuReportedTempCtrlOffset, out uint value)
                    : Ring0.ReadPciConfig(_miscellaneousControlAddress, ReportedTemperatureControlRegister, out value);

                if (isValueValid)
                {
                    _coreTemperature.Value = (Family == 0x15 || Family == 0x16) && (value & 0x30000) == 0x3000
                        ? Family == 0x15 && (Model & 0xF0) == 0x00
                            ? (((value >> 21) & 0x7FC) / 8.0f) + _coreTemperature.Parameters[0].Value - 49
                            : (((value >> 21) & 0x7FF) / 8.0f) + _coreTemperature.Parameters[0].Value - 49
                        : ((value >> 21) & 0x7FF) / 8.0f + _coreTemperature.Parameters[0].Value;

                    ActivateSensor(_coreTemperature);
                }
                else
                {
                    DeactivateSensor(_coreTemperature);
                }
            }
            else
            {
                DeactivateSensor(_coreTemperature);
            }
        }
        else
        {
            string s = ReadFirstLine(_temperatureStream);
            try
            {
                _coreTemperature.Value = 0.001f * long.Parse(s, CultureInfo.InvariantCulture);
                ActivateSensor(_coreTemperature);
            }
            catch
            {
                DeactivateSensor(_coreTemperature);
            }
        }
    }

    /// <summary>
    /// Create CPU clock sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateClockSensors()
    {
        _busClock = new Sensor(CpuConstants.BusSpeed, 0, SensorType.Clock, this, Settings);
        _coreClocks = new Sensor[CoreCount];
        for (int i = 0; i < _coreClocks.Length; i++)
        {
            _coreClocks[i] = new Sensor(SetCoreName(i), i + 1, SensorType.Clock, this, Settings);
            if (!HasTimeStampCounter) continue;
            ActivateSensor(_coreClocks[i]);
        }
    }

    /// <summary>
    /// Update CPU clock sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateClockSensors()
    {
        if (!HasTimeStampCounter) return;

        // Evaluate cores
        double newBusClock = 0;
        for (int i = 0; i < _coreClocks.Length; i++)
        {
            Thread.Sleep(1);

            if (Ring0.ReadMsr(CofvidStatus, out uint curEax, out uint _, CpuId[i][0].Affinity))
            {
                double multiplier = GetCoreMultiplier(curEax);
                _coreClocks[i].Value = (float)(multiplier * TimeStampCounterFrequency / _timeStampCounterMultiplier);
                newBusClock = (float)(TimeStampCounterFrequency / _timeStampCounterMultiplier);
            }
            else
            {
                _coreClocks[i].Value = (float)TimeStampCounterFrequency;
            }
        }

        // Set Bus Clock
        if (!(newBusClock > 0)) return;
        _busClock.Value = (float)newBusClock;
        ActivateSensor(_busClock);
    }

    /// <summary>
    /// Create CPU voltage sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateVoltageSensors()
    {
        // Core
        _coreVoltage = new Sensor(CpuConstants.CpuCores, 0, SensorType.Voltage, this, Settings);
        ActivateSensor(_coreVoltage);

        // Northbridge
        _northBridgeVoltage = new Sensor(CpuConstants.Northbridge, 0, SensorType.Voltage, this, Settings);
        ActivateSensor(_northBridgeVoltage);
    }

    /// <summary>
    /// Update CPU voltage sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateVoltageSensors()
    {
        if (!HasTimeStampCounter) return;

        float maxCoreVoltage = 0;
        float maxNbVoltage = 0;

        // Evaluate cores
        for (int i = 0; i < _coreClocks.Length; i++)
        {
            Thread.Sleep(1);
            if (!Ring0.ReadMsr(CofvidStatus, out uint curEax, out uint _, CpuId[i][0].Affinity)) continue;

            float newCoreVoltage = 0;
            float newNbVoltage = 0;
            uint coreVid60 = (curEax >> 9) & 0x7F;

            // Delegates
            float Svi2Volt(uint vid) => vid < 0b1111_1000 ? 1.5500f - (0.00625f * vid) : 0;
            float Svi1Volt(uint vid) => vid < 0x7C ? 1.550f - (0.0125f * vid) : 0;

            // AMD SVI2
            if (_isSvi2)
            {
                newCoreVoltage = Svi2Volt((curEax >> 13 & 0x80) | coreVid60);
                newNbVoltage = Svi2Volt(curEax >> 24);
            }
            else
            {
                newCoreVoltage = Svi1Volt(coreVid60);
                newNbVoltage = Svi1Volt(curEax >> 25);
            }

            // Max core
            if (newCoreVoltage > maxCoreVoltage)
            {
                maxCoreVoltage = newCoreVoltage;
            }

            // Max Northbridge
            if (newNbVoltage > maxNbVoltage)
            {
                maxNbVoltage = newNbVoltage;
            }
        }

        // Set values
        _coreVoltage.Value = maxCoreVoltage;
        _northBridgeVoltage.Value = maxNbVoltage;
    }

    /// <summary>
    /// Creates the power saving mode sensors.
    /// </summary>
    private void CreatePowerSavingModeSensors()
    {
        uint addr = Ring0.GetPciAddress(0, 20, 0);
        if (Ring0.ReadPciConfig(addr, 0, out uint dev))
        {
            Ring0.ReadPciConfig(addr, 8, out uint rev);
            _cStatesIoOffset = dev switch
            {
                0x43851002 => (byte)((rev & 0xFF) < 0x40 ? 0xB3 : 0x9C),
                0x780B1022 or 0x790B1022 => 0x9C,
                _ => _cStatesIoOffset
            };
        }

        // Set C-State Residency sensors
        if (_cStatesIoOffset == 0) return;
        _cStatesResidency = [
            new Sensor(CpuConstants.CpuPackageC2, 0, SensorType.Level, this, Settings),
            new Sensor(CpuConstants.CpuPackageC3, 1, SensorType.Level, this, Settings)
        ];
        ActivateSensor(_cStatesResidency[0]);
        ActivateSensor(_cStatesResidency[1]);
    }

    /// <summary>
    /// Updates CPU power saving mode sensors.
    /// </summary>
    private void UpdatePowerSavingModeSensors()
    {
        if (_cStatesResidency == null) return;
        for (int i = 0; i < _cStatesResidency.Length; i++)
        {
            Ring0.WriteIoPort(CstatesIoPort, (byte)(_cStatesIoOffset + i));
            _cStatesResidency[i].Value = Ring0.ReadIoPort(CstatesIoPort + 1) / 256f * 100;
        }
    }

    #endregion
}
