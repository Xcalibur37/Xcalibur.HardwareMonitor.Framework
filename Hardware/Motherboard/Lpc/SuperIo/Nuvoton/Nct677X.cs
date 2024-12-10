using System;
using System.Collections.Generic;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Helpers;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton;

/// <summary>
/// Nuvoton 677X
/// </summary>
/// <seealso cref="ISuperIo" />
internal class Nct677X : ISuperIo
{
    #region Fields

    // ReSharper disable InconsistentNaming
    private const uint ADDRESS_REGISTER_OFFSET = 0x05;
    private const byte BANK_SELECT_REGISTER = 0x4E;
    private const uint DATA_REGISTER_OFFSET = 0x06;

    // NCT668X
    private const uint EC_SPACE_PAGE_REGISTER_OFFSET = 0x04;
    private const uint EC_SPACE_INDEX_REGISTER_OFFSET = 0x05;
    private const uint EC_SPACE_DATA_REGISTER_OFFSET = 0x06;
    private const byte EC_SPACE_PAGE_SELECT = 0xFF;

    private const ushort NUVOTON_VENDOR_ID = 0x5CA3;

    private ushort[] FAN_CONTROL_MODE_REG;
    private ushort[] FAN_PWM_COMMAND_REG;
    private ushort[] FAN_PWM_OUT_REG;
    private ushort[] FAN_PWM_REQUEST_REG;

    private ushort VENDOR_ID_HIGH_REGISTER;
    private ushort VENDOR_ID_LOW_REGISTER;
    // ReSharper restore InconsistentNaming

    private readonly LpcPort _lpcPort;
    private readonly ushort _port;
    private readonly Manufacturer _manufacturer;
    private readonly MotherboardModel _model;
    private ushort[] _fanCountRegister;
    private ushort[] _fanRpmRegister;
    private byte[] _initialFanControlMode = new byte[7];
    private byte[] _initialFanPwmCommand = new byte[7];
    private bool _isNuvotonVendor;
    private int _maxFanCount;
    private int _minFanCount;
    private int _minFanRpm;
    private bool[] _restoreDefaultFanControlRequired = new bool[7];
    private TemperatureSourceData[] _temperaturesSource;
    private ushort _vBatMonitorControlRegister;
    private ushort[] _voltageRegisters;
    private ushort _voltageVBatRegister;

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
    public float?[] Controls { get; private set; } = [];

    /// <summary>
    /// Gets the fans.
    /// </summary>
    /// <value>
    /// The fans.
    /// </value>
    public float?[] Fans { get; private set; } = [];

    /// <summary>
    /// Gets the temperatures.
    /// </summary>
    /// <value>
    /// The temperatures.
    /// </value>
    public float?[] Temperatures { get; private set; } = [];

    /// <summary>
    /// Gets the voltages.
    /// </summary>
    /// <value>
    /// The voltages.
    /// </value>
    public float?[] Voltages { get; private set; } = [];

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Nct677X" /> class.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="model">The model.</param>
    /// <param name="port">The port.</param>
    /// <param name="lpcPort">The LPC port.</param>
    public Nct677X(Chip chip, Manufacturer manufacturer, MotherboardModel model, ushort port, LpcPort lpcPort)
    {
        Chip = chip;
        _manufacturer = manufacturer;
        _model = model;
        _port = port;
        _lpcPort = lpcPort;

        // Set registers
        SetRegisters(chip);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Reads the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public byte? ReadGpio(int index) => null;

    /// <summary>
    /// Sets the control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">nameof(index)</exception>
    public void SetControl(int index, byte? value)
    {
        if (!_isNuvotonVendor) return;

        if (index < 0 || index >= Controls.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (!Mutexes.WaitIsaBus(10)) return;

        if (value.HasValue)
        {
            SaveDefaultFanControl(index);

            if (Chip is not Chip.NCT6683D and not Chip.NCT6686D and not Chip.NCT6687D)
            {
                // set manual mode
                WriteByte(FAN_CONTROL_MODE_REG[index], 0);

                // set output value
                WriteByte(FAN_PWM_COMMAND_REG[index], value.Value);
            }
            else
            {
                // Manual mode, bit(1 : set, 0 : unset)
                // bit 0 : CPU Fan
                // bit 1 : PUMP Fan
                // bit 2 : SYS Fan 1
                // bit 3 : SYS Fan 2
                // bit 4 : SYS Fan 3
                // bit 5 : SYS Fan 4
                // bit 6 : SYS Fan 5
                // bit 7 : SYS Fan 6

                byte mode = ReadByte(FAN_CONTROL_MODE_REG[index]);
                byte bitMask = (byte)(0x01 << index);
                mode = (byte)(mode | bitMask);
                WriteByte(FAN_CONTROL_MODE_REG[index], mode);

                WriteByte(FAN_PWM_REQUEST_REG[index], 0x80);
                Thread.Sleep(50);

                WriteByte(FAN_PWM_COMMAND_REG[index], value.Value);
                WriteByte(FAN_PWM_REQUEST_REG[index], 0x40);
                Thread.Sleep(50);
            }
        }
        else
        {
            RestoreDefaultFanControl(index);
        }

        Mutexes.ReleaseIsaBus();
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        if (!_isNuvotonVendor) return;
        if (!Mutexes.WaitIsaBus(10)) return;

        DisableIoSpaceLock();

        // Updates
        UpdateVoltages();
        UpdateTemperatures();
        UpdateFans();
        UpdateControls();

        // Release
        Mutexes.ReleaseIsaBus();
    }

    /// <summary>
    /// Writes the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public void WriteGpio(int index, byte value) { }

    /// <summary>
    /// Disables the io space lock.
    /// </summary>
    /// <returns></returns>
    private void DisableIoSpaceLock()
    {
        if (Chip is not Chip.NCT6791D and
            not Chip.NCT6792D and
            not Chip.NCT6792DA and
            not Chip.NCT6793D and
            not Chip.NCT6795D and
            not Chip.NCT6796D and
            not Chip.NCT6796DR and
            not Chip.NCT6797D and
            not Chip.NCT6798D and
            not Chip.NCT6799D)
        {
            return;
        }

        // the lock is disabled already if the vendor ID can be read
        if (IsNuvotonVendor()) return;

        _lpcPort.WinbondNuvotonFintekEnter();
        _lpcPort.NuvotonDisableIoSpaceLock();
        _lpcPort.WinbondNuvotonFintekExit();
    }

    /// <summary>
    /// Determines whether [is nuvoton vendor].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is nuvoton vendor]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsNuvotonVendor() =>
        Chip is Chip.NCT6683D or Chip.NCT6686D or Chip.NCT6687D ||
        (ReadByte(VENDOR_ID_HIGH_REGISTER) << 8 | ReadByte(VENDOR_ID_LOW_REGISTER)) == NUVOTON_VENDOR_ID;

    /// <summary>
    /// Reads the byte.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <returns></returns>
    private byte ReadByte(ushort address)
    {
        if (Chip is not Chip.NCT6683D and not Chip.NCT6686D and not Chip.NCT6687D)
        {
            byte bank = (byte)(address >> 8);
            byte register = (byte)(address & 0xFF);
            Ring0.WriteIoPort(_port + ADDRESS_REGISTER_OFFSET, BANK_SELECT_REGISTER);
            Ring0.WriteIoPort(_port + DATA_REGISTER_OFFSET, bank);
            Ring0.WriteIoPort(_port + ADDRESS_REGISTER_OFFSET, register);
            return Ring0.ReadIoPort(_port + DATA_REGISTER_OFFSET);
        }

        byte page = (byte)(address >> 8);
        byte index = (byte)(address & 0xFF);

        //wait for access, access == EC_SPACE_PAGE_SELECT
        //timeout: after 500ms, abort and force access
        byte access;

        DateTime timeout = DateTime.UtcNow.AddMilliseconds(500);
        while (true)
        {
            access = Ring0.ReadIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET);
            if (access == EC_SPACE_PAGE_SELECT || DateTime.UtcNow > timeout) break;
            Thread.Sleep(1);
        }

        if (access != EC_SPACE_PAGE_SELECT)
        {
            // Failed to gain access: force register access
            Ring0.WriteIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET, EC_SPACE_PAGE_SELECT);
        }

        Ring0.WriteIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET, page);
        Ring0.WriteIoPort(_port + EC_SPACE_INDEX_REGISTER_OFFSET, index);
        byte result = Ring0.ReadIoPort(_port + EC_SPACE_DATA_REGISTER_OFFSET);

        // Free access for other instances
        Ring0.WriteIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET, EC_SPACE_PAGE_SELECT);

        return result;
    }

    /// <summary>
    /// Restores the default fan control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    private void RestoreDefaultFanControl(int index)
    {
        if (!_restoreDefaultFanControlRequired[index]) return;

        if (Chip is not Chip.NCT6683D and not Chip.NCT6686D and not Chip.NCT6687D)
        {
            WriteByte(FAN_CONTROL_MODE_REG[index], _initialFanControlMode[index]);
            WriteByte(FAN_PWM_COMMAND_REG[index], _initialFanPwmCommand[index]);
        }
        else
        {
            byte mode = ReadByte(FAN_CONTROL_MODE_REG[index]);
            mode = (byte)(mode & ~_initialFanControlMode[index]);
            WriteByte(FAN_CONTROL_MODE_REG[index], mode);

            WriteByte(FAN_PWM_REQUEST_REG[index], 0x80);
            Thread.Sleep(50);

            WriteByte(FAN_PWM_COMMAND_REG[index], _initialFanPwmCommand[index]);
            WriteByte(FAN_PWM_REQUEST_REG[index], 0x40);
            Thread.Sleep(50);
        }

        _restoreDefaultFanControlRequired[index] = false;
    }

    /// <summary>
    /// Sets the registers.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <returns></returns>
    private void SetRegisters(Chip chip)
    {
        // Standard registers
        switch (chip)
        {
            case Chip.NCT610XD:
                VENDOR_ID_HIGH_REGISTER = 0x80FE;
                VENDOR_ID_LOW_REGISTER = 0x00FE;

                FAN_PWM_OUT_REG = [0x04A, 0x04B, 0x04C];
                FAN_PWM_COMMAND_REG = [0x119, 0x129, 0x139];
                FAN_CONTROL_MODE_REG = [0x113, 0x123, 0x133];

                _vBatMonitorControlRegister = 0x0318;
                break;
            case Chip.NCT6683D or Chip.NCT6686D or Chip.NCT6687D:
                FAN_PWM_OUT_REG = [0x160, 0x161, 0x162, 0x163, 0x164, 0x165, 0x166, 0x167];
                FAN_PWM_COMMAND_REG = [0xA28, 0xA29, 0xA2A, 0xA2B, 0xA2C, 0xA2D, 0xA2E, 0xA2F];
                FAN_CONTROL_MODE_REG = [0xA00, 0xA00, 0xA00, 0xA00, 0xA00, 0xA00, 0xA00, 0xA00];
                FAN_PWM_REQUEST_REG = [0xA01, 0xA01, 0xA01, 0xA01, 0xA01, 0xA01, 0xA01, 0xA01];
                break;
            default:
                VENDOR_ID_HIGH_REGISTER = 0x804F;
                VENDOR_ID_LOW_REGISTER = 0x004F;

                FAN_PWM_OUT_REG = chip is Chip.NCT6797D or Chip.NCT6798D or Chip.NCT6799D
                    ? new ushort[] { 0x001, 0x003, 0x011, 0x013, 0x015, 0xA09, 0xB09 }
                    : new ushort[] { 0x001, 0x003, 0x011, 0x013, 0x015, 0x017, 0x029 };

                FAN_PWM_COMMAND_REG = [0x109, 0x209, 0x309, 0x809, 0x909, 0xA09, 0xB09];
                FAN_CONTROL_MODE_REG = [0x102, 0x202, 0x302, 0x802, 0x902, 0xA02, 0xB02];

                _vBatMonitorControlRegister = 0x005D;
                break;
        }

        // Is Nuvoton
        _isNuvotonVendor = IsNuvotonVendor();
        if (!_isNuvotonVendor) return;

        // Nuvoton registers
        switch (chip)
        {
            case Chip.NCT6771F:
            case Chip.NCT6776F:
                SetRegistersNct6776F(chip);
                break;

            case Chip.NCT6779D: // 15 voltages
            case Chip.NCT6791D: // 15 voltages
            case Chip.NCT6792D:
            case Chip.NCT6792DA:
            case Chip.NCT6793D: // 14 voltages
            case Chip.NCT6795D:
            case Chip.NCT6796D: // 16 voltages
            case Chip.NCT6796DR: // 16 voltages
            case Chip.NCT6797D:
            case Chip.NCT6798D:
            case Chip.NCT6799D:
                SetRegistersNct6779d(chip);
                break;

            case Chip.NCT610XD:
                SetRegistersNct610Xd();
                break;

            case Chip.NCT6683D:
            case Chip.NCT6686D:
            case Chip.NCT6687D:
                SetRegistersNct6687D();
                break;
        }
    }

    /// <summary>
    /// Sets the registers NCT610 xd.
    /// </summary>
    /// <returns></returns>
    private void SetRegistersNct610Xd()
    {
        Fans = new float?[3];
        Controls = new float?[3];

        _fanRpmRegister = new ushort[3];
        for (int i = 0; i < _fanRpmRegister.Length; i++)
        {
            _fanRpmRegister[i] = (ushort)(0x030 + (i << 1));
        }

        // min value RPM value with 13-bit fan counter
        _minFanRpm = (int)(1.35e6 / 0x1FFF);

        Voltages = new float?[9];
        _voltageRegisters = [0x300, 0x301, 0x302, 0x303, 0x304, 0x305, 0x307, 0x308, 0x309];
        _voltageVBatRegister = 0x308;

        Temperatures = new float?[4];
        _temperaturesSource =
        [
            new TemperatureSourceData(SourceNct610X.Peci0, 0x027, 0, -1, 0x621),
            new TemperatureSourceData(SourceNct610X.SysTin, 0x018, 0x01B, 7, 0x100, 0x018),
            new TemperatureSourceData(SourceNct610X.CpuTin, 0x019, 0x11B, 7, 0x200, 0x019),
            new TemperatureSourceData(SourceNct610X.AuxTin, 0x01A, 0x21B, 7, 0x300, 0x01A)
        ];
    }

    /// <summary>
    /// Sets the registers: NCT6776F.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <returns></returns>
    private void SetRegistersNct6776F(Chip chip)
    {
        if (chip == Chip.NCT6771F)
        {
            Fans = new float?[4];

            // min value RPM value with 16-bit fan counter
            _minFanRpm = (int)(1.35e6 / 0xFFFF);
        }
        else
        {
            Fans = new float?[5];

            // min value RPM value with 13-bit fan counter
            _minFanRpm = (int)(1.35e6 / 0x1FFF);
        }

        _fanRpmRegister = new ushort[5];
        for (int i = 0; i < _fanRpmRegister.Length; i++)
        {
            _fanRpmRegister[i] = (ushort)(0x656 + (i << 1));
        }

        Controls = new float?[3];
        Voltages = new float?[9];
        _voltageRegisters = [0x020, 0x021, 0x022, 0x023, 0x024, 0x025, 0x026, 0x550, 0x551];
        _voltageVBatRegister = 0x551;
        _temperaturesSource =
        [
            new TemperatureSourceData(chip == Chip.NCT6771F ? SourceNct6771F.Peci0 : SourceNct6776F.Peci0, 0x027, 0, -1, 0x621),
            new TemperatureSourceData(chip == Chip.NCT6771F ? SourceNct6771F.CpuTin : SourceNct6776F.CpuTin, 0x073, 0x074, 7, 0x100),
            new TemperatureSourceData(chip == Chip.NCT6771F ? SourceNct6771F.AuxTin : SourceNct6776F.AuxTin, 0x075, 0x076, 7, 0x200),
            new TemperatureSourceData(chip == Chip.NCT6771F ? SourceNct6771F.SysTin : SourceNct6776F.SysTin, 0x077, 0x078, 7, 0x300),
            new TemperatureSourceData(null, 0x150, 0x151, 7, 0x622),
            new TemperatureSourceData(null, 0x250, 0x251, 7, 0x623),
            new TemperatureSourceData(null, 0x62B, 0x62E, 0, 0x624),
            new TemperatureSourceData(null, 0x62C, 0x62E, 1, 0x625),
            new TemperatureSourceData(null, 0x62D, 0x62E, 2, 0x626)
        ];

        Temperatures = new float?[4];
    }

    /// <summary>
    /// Sets the registers NCT6687D.
    /// </summary>
    /// <returns></returns>
    private void SetRegistersNct6687D()
    {
        Fans = new float?[8];
        Controls = new float?[8];
        Voltages = new float?[14];
        Temperatures = new float?[7];

        // CPU
        // System
        // MOS
        // PCH
        // CPU Socket
        // PCIE_1
        // M2_1
        _temperaturesSource =
        [
            new TemperatureSourceData(null, 0x100),
            new TemperatureSourceData(null, 0x102),
            new TemperatureSourceData(null, 0x104),
            new TemperatureSourceData(null, 0x106),
            new TemperatureSourceData(null, 0x108),
            new TemperatureSourceData(null, 0x10A),
            new TemperatureSourceData(null, 0x10C)
        ];

        // VIN0 +12V
        // VIN1 +5V
        // VIN2 VCore
        // VIN3 SIO
        // VIN4 DRAM
        // VIN5 CPU IO
        // VIN6 CPU SA
        // VIN7 SIO
        // 3VCC I/O +3.3
        // SIO VTT
        // SIO VREF
        // SIO VSB
        // SIO AVSB
        // SIO VBAT
        _voltageRegisters = [0x120, 0x122, 0x124, 0x126, 0x128, 0x12A, 0x12C, 0x12E, 0x130, 0x13A, 0x13E, 0x136, 0x138, 0x13C];

        // CPU Fan
        // PUMP Fan
        // SYS Fan 1
        // SYS Fan 2
        // SYS Fan 3
        // SYS Fan 4
        // SYS Fan 5
        // SYS Fan 6
        _fanRpmRegister = new ushort[] { 0x140, 0x142, 0x144, 0x146, 0x148, 0x14A, 0x14C, 0x14E };

        _restoreDefaultFanControlRequired = new bool[_fanRpmRegister.Length];
        _initialFanControlMode = new byte[_fanRpmRegister.Length];
        _initialFanPwmCommand = new byte[_fanRpmRegister.Length];

        // initialize
        const ushort initRegister = 0x180;
        byte data = ReadByte(initRegister);
        if ((data & 0x80) == 0)
        {
            WriteByte(initRegister, (byte)(data | 0x80));
        }

        // enable SIO voltage
        WriteByte(0x1BB, 0x61);
        WriteByte(0x1BC, 0x62);
        WriteByte(0x1BD, 0x63);
        WriteByte(0x1BE, 0x64);
        WriteByte(0x1BF, 0x65);
    }

    /// <summary>
    /// Sets the registers NCT6779D.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <returns></returns>
    private void SetRegistersNct6779d(Chip chip)
    {
        switch (chip)
        {
            case Chip.NCT6779D:
                Fans = new float?[5];
                Controls = new float?[5];
                break;

            case Chip.NCT6796DR:
            case Chip.NCT6797D:
            case Chip.NCT6798D:
            case Chip.NCT6799D:
                Fans = new float?[7];
                Controls = new float?[7];
                break;

            default:
                Fans = new float?[6];
                Controls = new float?[6];
                break;
        }

        _fanCountRegister = new ushort[] { 0x4B0, 0x4B2, 0x4B4, 0x4B6, 0x4B8, 0x4BA, 0x4CC };

        // max value for 13-bit fan counter
        _maxFanCount = 0x1FFF;

        // min value that could be transferred to 16-bit RPM registers
        _minFanCount = 0x15;

        Voltages = new float?[16];
        _voltageRegisters = [0x480, 0x481, 0x482, 0x483, 0x484, 0x485, 0x486, 0x487, 0x488, 0x489, 0x48A, 0x48B, 0x48C, 0x48D, 0x48E, 0x48F];
        _voltageVBatRegister = 0x488;
        var temperaturesSources = new List<TemperatureSourceData>();
        SetRegistersNct6796d(chip, temperaturesSources);

        _temperaturesSource = temperaturesSources.ToArray();
        Temperatures = new float?[_temperaturesSource.Length];
    }

    /// <summary>
    /// Sets the registers NCT6796D.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <param name="temperaturesSources">The temperature sources.</param>
    /// <returns></returns>
    private void SetRegistersNct6796d(Chip chip, List<TemperatureSourceData> temperaturesSources)
    {
        switch (chip)
        {
            case Chip.NCT6796D:
            case Chip.NCT6796DR:
            case Chip.NCT6797D:
            case Chip.NCT6798D:
            case Chip.NCT6799D:
                temperaturesSources.AddRange([
                    // SYSFAN, MONITOR TEMPERATURE 1
                    new TemperatureSourceData(SourceNct67XXD.Peci0, 0x073, 0x074, 7, 0x100),
                    // CPUFAN, MONITOR TEMPERATURE 2, Value RAM (CPUTIN)
                    new TemperatureSourceData(SourceNct67XXD.CpuTin, 0x075, 0x076, 7, 0x200, 0x491),
                    // AUXFAN0, MONITOR TEMPERATURE 3, Value RAM (SYSTIN)
                    new TemperatureSourceData(SourceNct67XXD.SysTin, 0x077, 0x078, 7, 0x300, 0x490),
                    // AUXFAN1, MONITOR TEMPERATURE 4, Value RAM (AUXTIN0)
                    new TemperatureSourceData(SourceNct67XXD.AuxTin0, 0x079, 0x07A, 7, 0x800, 0x492),
                    // AUXFAN2, MONITOR TEMPERATURE 5, Value RAM (AUXTIN1)
                    new TemperatureSourceData(SourceNct67XXD.AuxTin1, 0x07B, 0x07C, 7, 0x900, 0x493),
                    // AUXFAN3, MONITOR TEMPERATURE 6, Value RAM (AUXTIN2)
                    new TemperatureSourceData(SourceNct67XXD.AuxTin2, 0x07D, 0x07E, 7, 0xA00, 0x494),
                    // AUXFAN4, AUXFANOUT4, Value RAM (AUXTIN3)
                    new TemperatureSourceData(SourceNct67XXD.AuxTin3, 0x4A0, 0x49E, 6, 0xB00, 0x495),
                    // SMIOVT1, SMIOVT1, Value RAM (SMIOVT1, SYSTIN)
                    new TemperatureSourceData(SourceNct67XXD.AuxTin4, 0x027, 0, -1, 0x621),
                    // ?
                    new TemperatureSourceData(SourceNct67XXD.Sensor, 0x4A2, 0x4A1, 7, 0xC00, 0x496),
                    // SMIOVT2, CPUTIN
                    new TemperatureSourceData(SourceNct67XXD.SmBusMaster0, 0x150, 0x151, 7, 0x622),
                    // SMIOVT3, AUXTIN0
                    new TemperatureSourceData(SourceNct67XXD.SmBusMaster1, 0x670, 0, -1, 0xC26),
                    // SMIOVT4, AUXTIN1
                    new TemperatureSourceData(SourceNct67XXD.Peci1, 0x672, 0, -1, 0xC27),
                    // SMIOVT5, AUXTIN2
                    new TemperatureSourceData(SourceNct67XXD.PchChipCpuMaxTemp, 0x674, 0, -1, 0xC28, 0x400),
                    // SMIOVT6, AUXTIN3
                    new TemperatureSourceData(SourceNct67XXD.PchChipTemp, 0x676, 0, -1, 0xC29, 0x401),
                    // SMIOVT7, AUXTIN4
                    new TemperatureSourceData(SourceNct67XXD.PchCpuTemp,  0x678, 0, -1, 0xC2A, 0x402),
                    // SMIOVT8, CPUTIN
                    new(SourceNct67XXD.PchMchTemp, 0x67A, 0, -1, 0xC2B, 0x404),
                    new(SourceNct67XXD.Agent0Dimm0, 0),
                    new(SourceNct67XXD.Agent0Dimm1, 0),
                    new(SourceNct67XXD.Agent1Dimm0,0),
                    new(SourceNct67XXD.Agent1Dimm1, 0),
                    new(SourceNct67XXD.ByteTemp0, 0),
                    new(SourceNct67XXD.ByteTemp1, 0),
                    new(SourceNct67XXD.Peci0Cal, 0),
                    new(SourceNct67XXD.Peci1Cal, 0),
                    new(SourceNct67XXD.VirtualTemp, 0)
                ]);
                break;

            default:
                temperaturesSources.AddRange(new TemperatureSourceData[]
                {
                    new(SourceNct67XXD.Peci0, 0x027, 0, -1, 0x621),
                    new(SourceNct67XXD.CpuTin, 0x073, 0x074, 7, 0x100, 0x491),
                    new(SourceNct67XXD.SysTin, 0x075, 0x076, 7, 0x200, 0x490),
                    new(SourceNct67XXD.AuxTin0, 0x077, 0x078, 7, 0x300, 0x492),
                    new(SourceNct67XXD.AuxTin1, 0x079, 0x07A, 7, 0x800, 0x493),
                    new(SourceNct67XXD.AuxTin2, 0x07B, 0x07C, 7, 0x900, 0x494),
                    new(SourceNct67XXD.AuxTin3, 0x150, 0x151, 7, 0x622, 0x495)
                });
                break;
        }
    }

    /// <summary>
    /// Saves the default fan control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    private void SaveDefaultFanControl(int index)
    {
        if (_restoreDefaultFanControlRequired[index]) return;
        if (Chip is not Chip.NCT6683D and not Chip.NCT6686D and not Chip.NCT6687D)
        {
            _initialFanControlMode[index] = ReadByte(FAN_CONTROL_MODE_REG[index]);
        }
        else
        {
            byte mode = ReadByte(FAN_CONTROL_MODE_REG[index]);
            byte bitMask = (byte)(0x01 << index);
            _initialFanControlMode[index] = (byte)(mode & bitMask);
        }

        _initialFanPwmCommand[index] = ReadByte(FAN_PWM_COMMAND_REG[index]);
        _restoreDefaultFanControlRequired[index] = true;
    }

    /// <summary>
    /// Updates the voltages.
    /// </summary>
    /// <returns></returns>
    private void UpdateVoltages()
    {
        for (var i = 0; i < Voltages.Length; i++)
        {
            switch (Chip)
            {
                case Chip.NCT6683D or Chip.NCT6686D or Chip.NCT6687D:
                    {
                        float value = 0.001f * (16 * ReadByte(_voltageRegisters[i]) + (ReadByte((ushort)(_voltageRegisters[i] + 1)) >> 4));
                        Voltages[i] = i switch
                        {
                            // 12V
                            0 => value * 12.0f,
                            // 5V
                            1 => value * 5.0f,
                            // DRAM
                            4 => value * 2.0f,
                            _ => value
                        };
                        break;
                    }
                case Chip.NCT6796D or Chip.NCT6796DR or Chip.NCT6797D or Chip.NCT6798D or Chip.NCT6799D:
                    {
                        // NCT 6796D - 8.6.2 Voltage Data Format
                        var value = 0.008f * ReadByte(_voltageRegisters[i]);

                        // Scale for Voltage Divider limitation of 2.048V (8mV LSB)
                        Voltages[i] = i switch
                        {
                            // VCore
                            0 => _manufacturer is Manufacturer.ASUS ? value * 1.11f : value,
                            // 5V
                            1 => _manufacturer is Manufacturer.ASUS or Manufacturer.MSI ? value * 5.0f : value,
                            // 3.3V
                            2 or 3 or 7 or 8 => value * 2.0f,
                            // 12V
                            4 => _manufacturer is Manufacturer.ASUS or Manufacturer.MSI ? value * 12.0f : value,
                            // Double value
                            9 or 13 => value * 2.0f,
                            // IMC VDD (VIN5)
                            10 => // DDR4
                                  _model.IsIntelSeries400() || _model.IsIntelSeries500() ||
                                  _model.IsIntelSeries600(true) || _model.IsIntelSeries700(true)
                                  ? value * 2.0f
                                  // DDR5
                                  : _model.IsIntelSeries600() || _model.IsIntelSeries700() ? value * 2.22f
                                  // Everything else
                                  : value,
                            // CPU L2 (VIN6)
                            11 => _model.IsIntelSeries600() || _model.IsIntelSeries700() ? value * 1.94f : value,
                            // Double value
                            14 => _manufacturer is Manufacturer.ASUS ? value * 2.0f : value,
                            // Use raw value - no scaling needed
                            _ => value
                        };
                        break;
                    }
                default:
                    {
                        var value = 0.008f * ReadByte(_voltageRegisters[i]);
                        var valid = value > 0;

                        // check if battery voltage monitor is enabled
                        if (valid && _voltageRegisters[i] == _voltageVBatRegister)
                        {
                            valid = (ReadByte(_vBatMonitorControlRegister) & 0x01) > 0;
                        }

                        Voltages[i] = valid ? value : null;
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Updates the temperatures.
    /// </summary>
    /// <returns></returns>
    private void UpdateTemperatures()
    {
        long temperatureSourceMask = 0;
        for (int i = 0; i < _temperaturesSource.Length; i++)
        {
            TemperatureSourceData ts = _temperaturesSource[i];
            int value;
            SourceNct67XXD source;
            float? temperature;

            switch (Chip)
            {
                case Chip.NCT6687D:
                case Chip.NCT6686D:
                case Chip.NCT6683D:
                    value = (sbyte)ReadByte(ts.Register);
                    int half = ReadByte((ushort)(ts.Register + 1)) >> 7 & 0x1;
                    Temperatures[i] = value + 0.5f * half;
                    break;

                case Chip.NCT6796D:
                case Chip.NCT6796DR:
                case Chip.NCT6797D:
                case Chip.NCT6798D:
                case Chip.NCT6799D:
                    if (_temperaturesSource[i].Register == 0) continue;

                    value = (sbyte)ReadByte(_temperaturesSource[i].Register) << 1;
                    if (_temperaturesSource[i].HalfBit > 0)
                    {
                        value |= ReadByte(_temperaturesSource[i].HalfRegister) >> ts.HalfBit & 0x1;
                    }

                    source = ts.SourceRegister > 0
                        ? (SourceNct67XXD)(ReadByte(ts.SourceRegister) & 0x1F)
                        : (SourceNct67XXD)ts.Source;

                    // Skip reading when already filled, because later values are without fractional
                    if ((temperatureSourceMask & 1L << (byte)source) > 0) continue;

                    temperature = 0.5f * value;

                    if (temperature is > 125 or < -55)
                    {
                        temperature = null;
                    }
                    else
                    {
                        temperatureSourceMask |= 1L << (byte)source;
                    }

                    for (int j = 0; j < Temperatures.Length; j++)
                    {
                        if ((SourceNct67XXD)_temperaturesSource[j].Source == source)
                        {
                            Temperatures[j] = temperature;
                        }
                    }
                    break;

                default:
                    value = (sbyte)ReadByte(ts.Register) << 1;
                    if (ts.HalfBit > 0)
                    {
                        value |= ReadByte(ts.HalfRegister) >> ts.HalfBit & 0x1;
                    }

                    source = (SourceNct67XXD)ReadByte(ts.SourceRegister);
                    temperatureSourceMask |= 1L << (byte)source;

                    temperature = 0.5f * value;
                    if (temperature is > 125 or < -55)
                    {
                        temperature = null;
                    }

                    for (int j = 0; j < Temperatures.Length; j++)
                    {
                        if ((SourceNct67XXD)_temperaturesSource[j].Source != source) continue;
                        Temperatures[j] = temperature;
                    }
                    break;
            }
        }

        for (int i = 0; i < _temperaturesSource.Length; i++)
        {
            TemperatureSourceData ts = _temperaturesSource[i];
            if (!ts.AlternateRegister.HasValue) continue;

            if ((temperatureSourceMask & 1L << (byte)(SourceNct67XXD)ts.Source) > 0) continue;

            float? temperature = (sbyte)ReadByte(ts.AlternateRegister.Value);

            if (temperature is > 125 or <= 0)
            {
                temperature = null;
            }

            Temperatures[i] = temperature;
        }
    }

    /// <summary>
    /// Updates the fans.
    /// </summary>
    /// <returns></returns>
    private void UpdateFans()
    {
        for (int i = 0; i < Fans.Length; i++)
        {
            if (Chip is not Chip.NCT6683D and not Chip.NCT6686D and not Chip.NCT6687D)
            {
                if (_fanCountRegister != null)
                {
                    byte high = ReadByte(_fanCountRegister[i]);
                    byte low = ReadByte((ushort)(_fanCountRegister[i] + 1));
                    int count = high << 5 | low & 0x1F;
                    Fans[i] = count < _maxFanCount ? count >= _minFanCount ? 1.35e6f / count : null : 0;
                }
                else
                {
                    byte high = ReadByte(_fanRpmRegister[i]);
                    byte low = ReadByte((ushort)(_fanRpmRegister[i] + 1));
                    int value = high << 8 | low;
                    Fans[i] = value > _minFanRpm ? value : 0;
                }
            }
            else
            {
                Fans[i] = ReadByte(_fanRpmRegister[i]) << 8 | ReadByte((ushort)(_fanRpmRegister[i] + 1));
            }
        }
    }

    /// <summary>
    /// Updates the controls.
    /// </summary>
    /// <returns></returns>
    private void UpdateControls()
    {
        for (int i = 0; i < Controls.Length; i++)
        {
            int value = ReadByte(FAN_PWM_OUT_REG[i]);
            Controls[i] = Chip is not Chip.NCT6683D and not Chip.NCT6686D and not Chip.NCT6687D
                ? value / 2.55f
                : (float)Math.Round(value / 2.55f);
        }
    }

    /// <summary>
    /// Writes the byte.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private void WriteByte(ushort address, byte value)
    {
        if (Chip is not Chip.NCT6683D and not Chip.NCT6686D and not Chip.NCT6687D)
        {
            byte bank = (byte)(address >> 8);
            byte register = (byte)(address & 0xFF);
            Ring0.WriteIoPort(_port + ADDRESS_REGISTER_OFFSET, BANK_SELECT_REGISTER);
            Ring0.WriteIoPort(_port + DATA_REGISTER_OFFSET, bank);
            Ring0.WriteIoPort(_port + ADDRESS_REGISTER_OFFSET, register);
            Ring0.WriteIoPort(_port + DATA_REGISTER_OFFSET, value);
        }
        else
        {
            byte page = (byte)(address >> 8);
            byte index = (byte)(address & 0xFF);

            //wait for access, access == EC_SPACE_PAGE_SELECT
            //timeout: after 500ms, abort and force access
            byte access;

            DateTime timeout = DateTime.UtcNow.AddMilliseconds(500);
            while (true)
            {
                access = Ring0.ReadIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET);
                if (access == EC_SPACE_PAGE_SELECT || DateTime.UtcNow > timeout) break;
                Thread.Sleep(1);
            }

            if (access != EC_SPACE_PAGE_SELECT)
            {
                // Failed to gain access: force register access
                Ring0.WriteIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET, EC_SPACE_PAGE_SELECT);
            }

            Ring0.WriteIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET, page);
            Ring0.WriteIoPort(_port + EC_SPACE_INDEX_REGISTER_OFFSET, index);
            Ring0.WriteIoPort(_port + EC_SPACE_DATA_REGISTER_OFFSET, value);

            //free access for other instances
            Ring0.WriteIoPort(_port + EC_SPACE_PAGE_REGISTER_OFFSET, EC_SPACE_PAGE_SELECT);
        }
    }

    #endregion
}
