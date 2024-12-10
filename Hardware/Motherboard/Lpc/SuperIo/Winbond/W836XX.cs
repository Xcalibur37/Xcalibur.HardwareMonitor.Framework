using System;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Winbond;

/// <summary>
/// Winbond W836XX
/// </summary>
/// <seealso cref="ISuperIo" />
internal class W836XX : ISuperIo
{
    #region Fields

    private const byte AddressRegisterOffset = 0x05;
    private const byte BankSelectRegister = 0x4E;
    private const byte DataRegisterOffset = 0x06;
    private const byte HighByte = 0x80;
    private const byte TemperatureSourceSelectReg = 0x49;
    private const byte VendorIdRegister = 0x4F;
    private const byte VoltageVbatReg = 0x51;

    private const ushort WinbondVendorId = 0x5CA3;

    private readonly byte[] _fanBitReg = [0x47, 0x4B, 0x4C, 0x59, 0x5D];
    private readonly byte[] _fanDivBit0 = [36, 38, 30, 8, 10];
    private readonly byte[] _fanDivBit1 = [37, 39, 31, 9, 11];
    private readonly byte[] _fanDivBit2 = [5, 6, 7, 23, 15];
    private readonly byte[] _fanTachoBank = [0, 0, 0, 0, 5];
    private readonly byte[] _fanTachoReg = [0x28, 0x29, 0x2A, 0x3F, 0x53];
    private readonly byte[] _temperatureBank = { 1, 2, 0 };
    private readonly byte[] _temperatureReg = [0x50, 0x50, 0x27];

    private readonly ushort _address;
    private readonly bool[] _peciTemperature = [];
    private readonly bool[] _restoreDefaultFanPwmControlRequired = [];

    private byte[] _voltageBank = [];
    private float _voltageGain = 0.008f;
    private byte[] _voltageRegister = [];

    // Added to control fans. 
    private byte[] _fanPwmRegister = [];
    private byte[] _fanPrimaryControlModeRegister = [];
    private byte[] _fanPrimaryControlValue = [];
    private byte[] _fanSecondaryControlModeRegister = [];
    private byte[] _fanSecondaryControlValue = [];
    private byte[] _fanTertiaryControlModeRegister = [];
    private byte[] _fanTertiaryControlValue = [];

    private byte[] _initialFanControlValue = [];
    private byte[] _initialFanSecondaryControlValue = [];
    private byte[] _initialFanTertiaryControlValue = [];

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
    /// Initializes a new instance of the <see cref="W836XX"/> class.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <param name="address">The address.</param>
    public W836XX(Chip chip, ushort address)
    {
        _address = address;
        Chip = chip;

        if (!IsWinbondVendor()) return;

        Temperatures = new float?[3];
        _peciTemperature = new bool[3];

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
    /// <exception cref="System.ArgumentOutOfRangeException">index</exception>
    public void SetControl(int index, byte? value)
    {
        if (index < 0 || index >= Controls.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (!Mutexes.WaitIsaBus(10)) return;

        if (value.HasValue)
        {
            SaveDefaultFanPwmControl(index);
            if (_fanPrimaryControlModeRegister.Length > 0)
            {
                WriteByte(0, _fanPrimaryControlModeRegister[index], (byte)(_fanPrimaryControlValue[index] & ReadByte(0, _fanPrimaryControlModeRegister[index])));
                if (_fanSecondaryControlModeRegister.Length > 0)
                {
                    if (_fanSecondaryControlModeRegister[index] != _fanPrimaryControlModeRegister[index])
                    {
                        WriteByte(0, _fanSecondaryControlModeRegister[index], (byte)(_fanSecondaryControlValue[index] & ReadByte(0, _fanSecondaryControlModeRegister[index])));
                    }

                    if (_fanTertiaryControlModeRegister.Length > 0 && _fanTertiaryControlModeRegister[index] != _fanSecondaryControlModeRegister[index])
                    {
                        WriteByte(0, _fanTertiaryControlModeRegister[index], (byte)(_fanTertiaryControlValue[index] & ReadByte(0, _fanTertiaryControlModeRegister[index])));
                    }
                }
            }

            // set output value
            WriteByte(0, _fanPwmRegister[index], value.Value);
        }
        else
        {
            RestoreDefaultFanPwmControl(index);
        }

        Mutexes.ReleaseIsaBus();
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    public void Update()
    {
        if (!Mutexes.WaitIsaBus(10)) return;

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
    public void WriteGpio(int index, byte value) { }

    /// <summary>
    /// Determines whether [is winbond vendor].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is winbond vendor]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsWinbondVendor()
    {
        var vendorId = (ushort)(ReadByte(HighByte, VendorIdRegister) << 8 | ReadByte(0, VendorIdRegister));
        return vendorId == WinbondVendorId;
    }

    /// <summary>
    /// Reads the byte.
    /// </summary>
    /// <param name="bank">The bank.</param>
    /// <param name="register">The register.</param>
    /// <returns></returns>
    private byte ReadByte(byte bank, byte register)
    {
        Ring0.WriteIoPort((ushort)(_address + AddressRegisterOffset), BankSelectRegister);
        Ring0.WriteIoPort((ushort)(_address + DataRegisterOffset), bank);
        Ring0.WriteIoPort((ushort)(_address + AddressRegisterOffset), register);
        return Ring0.ReadIoPort((ushort)(_address + DataRegisterOffset));
    }

    /// <summary>
    /// Restores the default fan PWM control.
    /// </summary>
    /// <param name="index">The index.</param>
    private void RestoreDefaultFanPwmControl(int index) //added to restore initial control values
    {
        if (_fanPrimaryControlModeRegister.Length <= 0 ||
            _initialFanControlValue.Length <= 0 ||
            _fanPrimaryControlValue.Length <= 0 ||
            _restoreDefaultFanPwmControlRequired.Length <= 0 ||
            !_restoreDefaultFanPwmControlRequired[index]) return;

        WriteByte(0,
            _fanPrimaryControlModeRegister[index],
            (byte)(_initialFanControlValue[index] & ~_fanPrimaryControlValue[index] |
                   ReadByte(0, _fanPrimaryControlModeRegister[index]))); //bitwise operands to change only desired bits

        if (_fanSecondaryControlModeRegister.Length > 0 && _initialFanSecondaryControlValue.Length > 0 && _fanSecondaryControlValue.Length > 0)
        {
            if (_fanSecondaryControlModeRegister[index] != _fanPrimaryControlModeRegister[index])
            {
                WriteByte(0,
                    _fanSecondaryControlModeRegister[index],
                    (byte)(_initialFanSecondaryControlValue[index] & ~_fanSecondaryControlValue[index] |
                           ReadByte(0, _fanSecondaryControlModeRegister[index]))); //bitwise operands to change only desired bits
            }

            if (_fanTertiaryControlModeRegister.Length > 0 &&
                _initialFanTertiaryControlValue.Length > 0 &&
                _fanTertiaryControlValue.Length > 0 &&
                _fanTertiaryControlModeRegister[index] != _fanSecondaryControlModeRegister[index])
            {
                WriteByte(0,
                    _fanTertiaryControlModeRegister[index],
                    (byte)(_initialFanTertiaryControlValue[index] & ~_fanTertiaryControlValue[index] | ReadByte(0, _fanTertiaryControlModeRegister[index]))); //bitwise operands to change only desired bits
            }
        }

        _restoreDefaultFanPwmControlRequired[index] = false;
    }

    /// <summary>
    /// Saves the default fan PWM control.
    /// </summary>
    /// <param name="index">The index.</param>
    private void SaveDefaultFanPwmControl(int index) //added to save initial control values
    {
        if (_fanPrimaryControlModeRegister.Length <= 0 ||
            _initialFanControlValue.Length <= 0 ||
            _fanPrimaryControlValue.Length <= 0 ||
            _restoreDefaultFanPwmControlRequired.Length <= 0 ||
            _restoreDefaultFanPwmControlRequired[index]) return;

        _initialFanControlValue[index] = ReadByte(0, _fanPrimaryControlModeRegister[index]);
        if (_fanSecondaryControlModeRegister.Length > 0 && _initialFanSecondaryControlValue.Length > 0 && _fanSecondaryControlValue.Length > 0)
        {
            if (_fanSecondaryControlModeRegister[index] != _fanPrimaryControlModeRegister[index])
            {
                _initialFanSecondaryControlValue[index] = ReadByte(0, _fanSecondaryControlModeRegister[index]);
            }

            if (_fanTertiaryControlModeRegister.Length > 0 &&
                _initialFanTertiaryControlValue.Length > 0 &&
                _fanTertiaryControlValue.Length > 0 &&
                _fanTertiaryControlModeRegister[index] != _fanSecondaryControlModeRegister[index])
            {
                _initialFanTertiaryControlValue[index] = ReadByte(0, _fanTertiaryControlModeRegister[index]);
            }
        }

        _restoreDefaultFanPwmControlRequired[index] = true;
    }

    /// <summary>
    /// Sets the bit.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="bit">The bit.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">
    /// Value must be one "bit only" or "bit out of range."
    /// </exception>
    private static ulong SetBit(ulong target, int bit, int value)
    {
        if ((value & 1) != value)
        {
            throw new ArgumentException("Value must be one bit only.");
        }

        if (bit is < 0 or > 63)
        {
            throw new ArgumentException("Bit out of range.");
        }

        ulong mask = (ulong)1 << bit;
        return value > 0 ? target | mask : target & ~mask;
    }

    /// <summary>
    /// Sets the registers.
    /// </summary>
    /// <param name="chip">The chip.</param>
    private void SetRegisters(Chip chip)
    {
        switch (chip)
        {
            case Chip.W83627EHF:
                SetRegistersW83627Ehf();
                break;

            case Chip.W83627DHG:
            case Chip.W83627DHGP:
                SetRegistersW83627Dhgp();
                break;

            case Chip.W83667HG:
            case Chip.W83667HGB:
                SetRegistersW83667Hgb();
                break;

            case Chip.W83627HF:
                SetRegistersW83627Hf();
                break;

            case Chip.W83627THF:
                SetRegistersW83627Thf();
                break;

            case Chip.W83687THF:
                SetRegistersW83687Thf();
                break;

            default:
                // no PECI support
                _peciTemperature[0] = false;
                _peciTemperature[1] = false;
                _peciTemperature[2] = false;
                break;
        }
    }

    /// <summary>
    /// Sets the registers for W83627EHF.
    /// </summary>
    private void SetRegistersW83627Ehf()
    {
        Voltages = new float?[10];
        _voltageRegister = [0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x50, 0x51, 0x52];
        _voltageBank = [0, 0, 0, 0, 0, 0, 0, 5, 5, 5];
        _voltageGain = 0.008f;

        Fans = new float?[5];
        _fanPwmRegister = [0x01, 0x03, 0x11]; // Fan PWM values.
        _fanPrimaryControlModeRegister = [0x04, 0x04, 0x12]; // Primary control register.
        _fanPrimaryControlValue = [0b11110011, 0b11001111, 0b11111001]; // Values to gain control of fans.
        _initialFanControlValue = new byte[3]; // To store primary default value.
        _initialFanSecondaryControlValue = new byte[3]; // To store secondary default value.

        Controls = new float?[3];
    }

    /// <summary>
    /// Sets the registers for W83627DHGP.
    /// </summary>
    private void SetRegistersW83627Dhgp()
    {
        // note temperature sensor registers that read PECI
        byte sel = ReadByte(0, TemperatureSourceSelectReg);
        _peciTemperature[0] = (sel & 0x07) != 0;
        _peciTemperature[1] = (sel & 0x70) != 0;
        _peciTemperature[2] = false;

        Voltages = new float?[9];
        _voltageRegister = [0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x50, 0x51];
        _voltageBank = [0, 0, 0, 0, 0, 0, 0, 5, 5];

        _voltageGain = 0.008f;
        Fans = new float?[5];
        _fanPwmRegister = [0x01, 0x03, 0x11]; // Fan PWM values
        _fanPrimaryControlModeRegister = [0x04, 0x04, 0x12]; // added. Primary control register
        _fanPrimaryControlValue = [0b11110011, 0b11001111, 0b11111001]; // Values to gain control of fans
        _initialFanControlValue = new byte[3]; // To store primary default value
        _initialFanSecondaryControlValue = new byte[3]; // To store secondary default value.

        Controls = new float?[3];
    }

    /// <summary>
    /// Sets the registers for W83667HGB.
    /// </summary>
    private void SetRegistersW83667Hgb()
    {
        // note temperature sensor registers that read PECI
        byte flag = ReadByte(0, TemperatureSourceSelectReg);
        _peciTemperature[0] = (flag & 0x04) != 0;
        _peciTemperature[1] = (flag & 0x40) != 0;
        _peciTemperature[2] = false;

        Voltages = new float?[9];
        _voltageRegister = [0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x50, 0x51];
        _voltageBank = [0, 0, 0, 0, 0, 0, 0, 5, 5];
        _voltageGain = 0.008f;

        Fans = new float?[5];
        _fanPwmRegister = [0x01, 0x03, 0x11]; // Fan PWM values.
        _fanPrimaryControlModeRegister = [0x04, 0x04, 0x12]; // Primary control register.
        _fanPrimaryControlValue = [0b11110011, 0b11001111, 0b11111001]; // Values to gain control of fans.
        _fanSecondaryControlModeRegister = [0x7c, 0x7c, 0x7c]; // Secondary control register for SmartFan4.
        _fanSecondaryControlValue = [0b11101111, 0b11011111, 0b10111111]; // Values for secondary register to gain control of fans.
        _fanTertiaryControlModeRegister = [0x62, 0x7c, 0x62]; // Tertiary control register. 2nd fan doesn't have Tertiary control, same as secondary to avoid change.
        _fanTertiaryControlValue = [0b11101111, 0b11011111, 0b11011111]; // Values for tertiary register to gain control of fans. 2nd fan doesn't have Tertiary control, same as secondary to avoid change.

        _initialFanControlValue = new byte[3]; // To store primary default value.
        _initialFanSecondaryControlValue = new byte[3]; // To store secondary default value.
        _initialFanTertiaryControlValue = new byte[3]; // To store tertiary default value.
        Controls = new float?[3];
    }

    /// <summary>
    /// Sets the registers W83627HF.
    /// </summary>
    private void SetRegistersW83627Hf()
    {
        Voltages = new float?[7];
        _voltageRegister = [0x20, 0x21, 0x22, 0x23, 0x24, 0x50, 0x51];
        _voltageBank = [0, 0, 0, 0, 0, 5, 5];
        _voltageGain = 0.016f;

        Fans = new float?[3];
        _fanPwmRegister = [0x5A, 0x5B]; // Fan PWM values.

        Controls = new float?[2];
    }

    /// <summary>
    /// Sets the registers W83627THF.
    /// </summary>
    private void SetRegistersW83627Thf()
    {
        Voltages = new float?[7];
        _voltageRegister = [0x20, 0x21, 0x22, 0x23, 0x24, 0x50, 0x51];
        _voltageBank = [0, 0, 0, 0, 0, 5, 5];
        _voltageGain = 0.016f;

        Fans = new float?[3];
        _fanPwmRegister = [0x01, 0x03, 0x11]; // Fan PWM values.
        _fanPrimaryControlModeRegister = [0x04, 0x04, 0x12]; // Primary control register.
        _fanPrimaryControlValue = [0b11110011, 0b11001111, 0b11111001]; // Values to gain control of fans.
        _initialFanControlValue = new byte[3]; // To store primary default value.

        Controls = new float?[3];
    }

    /// <summary>
    /// Sets the registers W83687THF.
    /// </summary>
    private void SetRegistersW83687Thf()
    {
        Voltages = new float?[7];
        _voltageRegister = [0x20, 0x21, 0x22, 0x23, 0x24, 0x50, 0x51];
        _voltageBank = [0, 0, 0, 0, 0, 5, 5];
        _voltageGain = 0.016f;

        Fans = new float?[3];
    }

    /// <summary>
    /// Updates the voltages.
    /// </summary>
    /// <returns></returns>
    private void UpdateVoltages()
    {
        for (int i = 0; i < Voltages.Length; i++)
        {
            if (_voltageRegister[i] != VoltageVbatReg)
            {
                // two special VCore measurement modes for W83627THF
                float fValue;
                if ((Chip == Chip.W83627HF || Chip == Chip.W83627THF || Chip == Chip.W83687THF) && i == 0)
                {
                    byte vrmConfiguration = ReadByte(0, 0x18);
                    int value = ReadByte(_voltageBank[i], _voltageRegister[i]);
                    fValue = (vrmConfiguration & 0x01) == 0
                        // VRM8 formula
                        ? 0.016f * value
                        // VRM9 formula
                        : 0.00488f * value + 0.69f;
                }
                else
                {
                    int value = ReadByte(_voltageBank[i], _voltageRegister[i]);
                    fValue = _voltageGain * value;
                }

                Voltages[i] = fValue > 0 ? fValue : null;
            }
            else
            {
                // Battery voltage
                bool valid = (ReadByte(0, 0x5D) & 0x01) > 0;
                Voltages[i] = valid ? _voltageGain * ReadByte(5, VoltageVbatReg) : null;
            }
        }
    }

    /// <summary>
    /// Updates the temperatures.
    /// </summary>
    private void UpdateTemperatures()
    {
        for (int i = 0; i < Temperatures.Length; i++)
        {
            int value = (sbyte)ReadByte(_temperatureBank[i], _temperatureReg[i]) << 1;
            if (_temperatureBank[i] > 0)
            {
                value |= ReadByte(_temperatureBank[i], (byte)(_temperatureReg[i] + 1)) >> 7;
            }
            float temperature = value / 2.0f;
            Temperatures[i] = temperature is <= 125 and >= -55 && !_peciTemperature[i] ? temperature : null;
        }
    }

    /// <summary>
    /// Updates the fans.
    /// </summary>
    /// <returns></returns>
    private void UpdateFans()
    {
        ulong bits = 0;
        foreach (byte t in _fanBitReg)
        {
            bits = bits << 8 | ReadByte(0, t);
        }

        ulong newBits = bits;
        for (int i = 0; i < Fans.Length; i++)
        {
            int count = ReadByte(_fanTachoBank[i], _fanTachoReg[i]);

            // assemble fan divisor
            int divisorBits = (int)(
                (bits >> _fanDivBit2[i] & 1) << 2 |
                (bits >> _fanDivBit1[i] & 1) << 1 |
                bits >> _fanDivBit0[i] & 1);

            int divisor = 1 << divisorBits;

            Fans[i] = count < 0xff ? 1.35e6f / (count * divisor) : 0;

            switch (count)
            {
                // update fan divisor
                case > 192 when divisorBits < 7:
                    divisorBits++;
                    break;
                case < 96 when divisorBits > 0:
                    divisorBits--;
                    break;
            }

            newBits = SetBit(newBits, _fanDivBit2[i], divisorBits >> 2 & 1);
            newBits = SetBit(newBits, _fanDivBit1[i], divisorBits >> 1 & 1);
            newBits = SetBit(newBits, _fanDivBit0[i], divisorBits & 1);
        }
    }

    /// <summary>
    /// Updates the controls.
    /// </summary>
    private void UpdateControls()
    {
        for (int i = 0; i < Controls.Length; i++)
        {
            byte value = ReadByte(0, _fanPwmRegister[i]);
            Controls[i] = (float)Math.Round(value * 100.0f / 0xFF);
        }
    }

    /// <summary>
    /// Writes the byte.
    /// </summary>
    /// <param name="bank">The bank.</param>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    private void WriteByte(byte bank, byte register, byte value)
    {
        Ring0.WriteIoPort((ushort)(_address + AddressRegisterOffset), BankSelectRegister);
        Ring0.WriteIoPort((ushort)(_address + DataRegisterOffset), bank);
        Ring0.WriteIoPort((ushort)(_address + AddressRegisterOffset), register);
        Ring0.WriteIoPort((ushort)(_address + DataRegisterOffset), value);
    }

    #endregion
}
