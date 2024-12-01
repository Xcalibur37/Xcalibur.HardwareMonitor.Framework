




using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.IPMI;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo;

/// <summary>
/// Intelligent Platform Management Interface
/// </summary>
/// <seealso cref="ISuperIo" />
internal class Ipmi : ISuperIo
{
    #region Fields

    // ReSharper disable InconsistentNaming
    private const byte COMMAND_FAN_LEVEL = 0x70;
    private const byte COMMAND_FAN_MODE = 0x45;
    private const byte COMMAND_GET_SDR = 0x23;
    private const byte COMMAND_GET_SDR_REPOSITORY_INFO = 0x20;
    private const byte COMMAND_GET_SENSOR_READING = 0x2d;

    private const byte FAN_MODE_FULL = 0x01;
    private const byte FAN_MODE_OPTIMAL = 0x02;

    private const byte NETWORK_FUNCTION_SENSOR_EVENT = 0x04;
    private const byte NETWORK_FUNCTION_STORAGE = 0x0a;
    private const byte NETWORK_FUNCTION_SUPERMICRO = 0x30;
    // ReSharper restore InconsistentNaming

    private readonly List<string> _controlNames = new();
    private readonly List<float> _controls = new();
    private readonly List<string> _fanNames = new();
    private readonly List<float> _fans = new();

    private readonly ManagementObject _ipmi;
    private readonly Manufacturer _manufacturer;

    private readonly List<Sdr> _sdrs = new();
    private readonly List<string> _temperatureNames = new();
    private readonly List<float> _temperatures = new();
    private readonly List<string> _voltageNames = new();
    private readonly List<float> _voltages = new();

    private bool _touchedFans;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the chip.
    /// </summary>
    /// <value>
    /// The chip.
    /// </value>
    public Chip Chip { get; }

    /// <summary>
    /// Gets the controls.
    /// </summary>
    /// <value>
    /// The controls.
    /// </value>
    public float?[] Controls { get; }

    /// <summary>
    /// Gets the fans.
    /// </summary>
    /// <value>
    /// The fans.
    /// </value>
    public float?[] Fans { get; }

    /// <summary>
    /// Determines whether [is BMC present].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is BMC present]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBmcPresent()
    {
        try
        {
#pragma warning disable CA1416
            using ManagementObjectSearcher searcher = new("root\\WMI", "SELECT * FROM Microsoft_IPMI WHERE Active='True'");
            return searcher.Get().Count > 0;
#pragma warning restore CA1416
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the temperatures.
    /// </summary>
    /// <value>
    /// The temperatures.
    /// </value>
    public float?[] Temperatures { get; }

    /// <summary>
    /// Gets the voltages.
    /// </summary>
    /// <value>
    /// The voltages.
    /// </value>
    public float?[] Voltages { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Ipmi"/> class.
    /// </summary>
    /// <param name="manufacturer">The manufacturer.</param>
    public Ipmi(Manufacturer manufacturer)
    {
        Chip = Chip.IPMI;
        _manufacturer = manufacturer;

#pragma warning disable CA1416
        using ManagementClass ipmiClass = new("root\\WMI", "Microsoft_IPMI", null);
        foreach (var ipmi in ipmiClass.GetInstances())
        {
            if (ipmi is ManagementObject managementObject)
            {
                _ipmi = managementObject;
            }
        }
#pragma warning restore CA1416

        // Fan control is exposed for Supermicro only as it differs between IPMI implementations
        if (_manufacturer == Manufacturer.Supermicro)
        {
            _controlNames.Add("CPU Fan");
            _controlNames.Add("System Fan");
        }

        // Perform an early update to count the number of sensors and get their names
        Update();

        Controls = new float?[_controls.Count];
        Fans = new float?[_fans.Count];
        Temperatures = new float?[_temperatures.Count];
        Voltages = new float?[_voltages.Count];
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets the control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetControl(int index, byte? value)
    {
        if (_manufacturer == Manufacturer.Supermicro)
        {
            if (value == null && !_touchedFans) return;
            _touchedFans = true;

            if (value == null)
            {
                RunIpmiCommand(COMMAND_FAN_MODE, NETWORK_FUNCTION_SUPERMICRO, new byte[] { 0x01 /* Set */, FAN_MODE_OPTIMAL });
            }
            else
            {
                byte[] fanMode = RunIpmiCommand(COMMAND_FAN_MODE, NETWORK_FUNCTION_SUPERMICRO, new byte[] { 0x00 });
                if (fanMode == null || fanMode.Length < 2 || fanMode[0] != 0 || fanMode[1] != FAN_MODE_FULL)
                    RunIpmiCommand(COMMAND_FAN_MODE, NETWORK_FUNCTION_SUPERMICRO, new byte[] { 0x01 /* Set */, FAN_MODE_FULL });

                float speed = (float)value / 255.0f * 100.0f;
                RunIpmiCommand(COMMAND_FAN_LEVEL, NETWORK_FUNCTION_SUPERMICRO, new byte[] { 0x66, 0x01 /* Set */, (byte)index, (byte)speed });
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    public void Update()
    {
        Update(null);
    }

    /// <summary>
    /// Updates the specified string builder.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    private unsafe void Update(StringBuilder stringBuilder)
    {
        _fans.Clear();
        _temperatures.Clear();
        _voltages.Clear();
        _controls.Clear();

        if (_sdrs.Count == 0 || stringBuilder != null)
        {
            byte[] sdrInfo = RunIpmiCommand(COMMAND_GET_SDR_REPOSITORY_INFO, NETWORK_FUNCTION_STORAGE, new byte[] { });
            if (sdrInfo?[0] == 0)
            {
                int recordCount = sdrInfo[3] * 256 + sdrInfo[2];

                byte recordLower = 0;
                byte recordUpper = 0;
                for (int i = 0; i < recordCount; ++i)
                {
                    byte[] sdrRaw = RunIpmiCommand(COMMAND_GET_SDR, NETWORK_FUNCTION_STORAGE, new byte[] { 0, 0, recordLower, recordUpper, 0, 0xff });
                    if (!(sdrRaw?.Length >= 3) || sdrRaw[0] != 0) break;
                    recordLower = sdrRaw[1];
                    recordUpper = sdrRaw[2];

                    fixed (byte* pSdr = sdrRaw)
                    {
                        Sdr sdr =
                            (Sdr)Marshal.PtrToStructure((nint)pSdr + 3, typeof(Sdr));
                        _sdrs.Add(sdr);
                        stringBuilder?.AppendLine("IPMI sensor " + i + " num: " + sdr.sens_num + " info: " +
                                                  BitConverter.ToString(sdrRaw).Replace("-", ""));
                    }
                }
            }
        }

        foreach (Sdr sdr in _sdrs)
        {
            if (sdr.rectype != 1) continue;

            byte[] reading = RunIpmiCommand(COMMAND_GET_SENSOR_READING, NETWORK_FUNCTION_SENSOR_EVENT, [sdr.sens_num]);
            if (!(reading?.Length > 1) || reading[0] != 0) continue;

            switch (sdr.sens_type)
            {
                case 1:
                    _temperatures.Add(RawToFloat(reading[1], sdr));
                    if (Temperatures == null || Temperatures.Length == 0)
                    {
                        _temperatureNames.Add(sdr.id_string.Replace(" Temp", ""));
                    }
                    break;

                case 2:
                    _voltages.Add(RawToFloat(reading[1], sdr));
                    if (Voltages == null || Voltages.Length == 0)
                    {
                        _voltageNames.Add(sdr.id_string);
                    }
                    break;

                case 4:
                    _fans.Add(RawToFloat(reading[1], sdr));
                    if (Fans == null || Fans.Length == 0)
                    {
                        _fanNames.Add(sdr.id_string);
                    }
                    break;
            }

            stringBuilder?.AppendLine("IPMI sensor num: " + sdr.sens_num + " reading: " + BitConverter.ToString(reading).Replace("-", ""));
        }

        if (_manufacturer == Manufacturer.Supermicro)
        {
            for (int i = 0; i < _controlNames.Count; ++i)
            {
                byte[] fanLevel = RunIpmiCommand(COMMAND_FAN_LEVEL, NETWORK_FUNCTION_SUPERMICRO, new byte[] { 0x66, 0x00 /* Get */, (byte)i });
                if (!(fanLevel?.Length >= 2) || fanLevel[0] != 0) continue;
                _controls.Add(fanLevel[1]);

                stringBuilder?.AppendLine("IPMI fan " + i + ": " + BitConverter.ToString(fanLevel).Replace("-", ""));
            }
        }

        SetTemperatures();
        SetVoltages();
        SetFans();
        SetControls();
    }

    /// <summary>
    /// Gets the temperatures.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Temperature> GetTemperatures() => _temperatureNames.Select((t, i) => new Temperature(t, i));

    /// <summary>
    /// Sets the temperatures.
    /// </summary>
    private void SetTemperatures()
    {
        if (Temperatures == null) return;
        for (int i = 0; i < Math.Min(_temperatures.Count, Temperatures.Length); ++i)
        {
            Temperatures[i] = _temperatures[i];
        }
    }

    /// <summary>
    /// Gets the fans.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Fan> GetFans() => _fanNames.Select((t, i) => new Fan(t, i));

    /// <summary>
    /// Sets the fans.
    /// </summary>
    private void SetFans()
    {
        if (Fans == null) return;
        for (int i = 0; i < Math.Min(_fans.Count, Fans.Length); ++i)
        {
            Fans[i] = _fans[i];
        }
    }

    /// <summary>
    /// Gets the voltages.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Voltage> GetVoltages() => _voltageNames.Select((t, i) => new Voltage(t, i));

    /// <summary>
    /// Sets the voltages.
    /// </summary>
    private void SetVoltages()
    {
        if (Voltages == null) return;
        for (int i = 0; i < Math.Min(_voltages.Count, Voltages.Length); ++i)
        {
            Voltages[i] = _voltages[i];
        }
    }

    /// <summary>
    /// Gets the controls.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Models.Control> GetControls() => _controlNames.Select((t, i) => new Models.Control(t, i));

    /// <summary>
    /// Sets the controls.
    /// </summary>
    private void SetControls()
    {
        if (Controls == null) return;
        for (int i = 0; i < Math.Min(_controls.Count, Controls.Length); ++i)
        {
            Controls[i] = _controls[i];
        }
    }

    /// <summary>
    /// Reads the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public byte? ReadGpio(int index) => null;

    /// <summary>
    /// Writes the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    public void WriteGpio(int index, byte value) { }

    /// <summary>
    /// Runs the IPMI command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="networkFunction">The network function.</param>
    /// <param name="requestData">The request data.</param>
    /// <returns></returns>
    private byte[] RunIpmiCommand(byte command, byte networkFunction, byte[] requestData)
    {
#pragma warning disable CA1416
        using ManagementBaseObject inParams = _ipmi.GetMethodParameters("RequestResponse");
        inParams["NetworkFunction"] = networkFunction;
        inParams["Lun"] = 0;
        inParams["ResponderAddress"] = 0x20;
        inParams["Command"] = command;
        inParams["RequestDataSize"] = requestData.Length;
        inParams["RequestData"] = requestData;

        using ManagementBaseObject outParams = _ipmi.InvokeMethod("RequestResponse", inParams, null);
        var result = (byte[])outParams["ResponseData"];
#pragma warning restore CA1416

        return result;
    }

    /// <summary>
    /// Ported from ipmiutil
    /// Bare minimum to read Supermicro X13 IPMI sensors, may need expanding for other boards
    /// </summary>
    /// <param name="sensorReading">The sensor reading.</param>
    /// <param name="sdr">The SDR.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static float RawToFloat(byte sensorReading, Sdr sdr)
    {
        double reading = sensorReading;

        int m = sdr.m + ((sdr.m_t & 0xc0) << 2);
        if (Convert.ToBoolean(m & 0x0200))
        {
            m -= 0x0400;
        }

        int b = sdr.b + ((sdr.b_a & 0xc0) << 2);
        if (Convert.ToBoolean(b & 0x0200))
        {
            b -= 0x0400;
        }

        int rx = (sdr.rx_bx & 0xf0) >> 4;
        if (Convert.ToBoolean(rx & 0x08))
        {
            rx -= 0x10;
        }

        int bExp = sdr.rx_bx & 0x0f;
        if (Convert.ToBoolean(bExp & 0x08))
        {
            bExp -= 0x10;
        }

        if ((sdr.sens_units & 0xc0) != 0)
        {
            reading = Convert.ToBoolean(sensorReading & 0x80) ? sensorReading - 0x100 : sensorReading;
        }

        reading *= m;
        reading += b * Math.Pow(10, bExp);
        reading *= Math.Pow(10, rx);

        return sdr.linear != 0 ? throw new NotImplementedException() : (float)reading;
    }

    #endregion
}
