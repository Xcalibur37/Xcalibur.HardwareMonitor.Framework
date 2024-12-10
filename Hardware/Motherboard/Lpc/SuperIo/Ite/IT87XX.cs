using System;
using System.Linq;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.Gigabyte;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Ite;

/// <summary>
/// ITE 87XX
/// </summary>
/// <seealso cref="ISuperIo" />
internal class It87Xx : ISuperIo
{
    #region Fields

    private const byte AddressRegisterOffset = 0x05;

    private const byte ConfigurationRegister = 0x00;
    private const byte DataRegisterOffset = 0x06;
    private const byte BankRegister = 0x06; // bit 5-6 define selected bank
    private const byte FanTachometer16BitRegister = 0x0C;
    private const byte FanTachometerDivisorRegister = 0x0B;

    private readonly byte[] _iteVendorIds = [0x90, 0x7F];

    private const byte TemperatureBaseReg = 0x29;
    private const byte VendorIdRegister = 0x58;
    private const byte VoltageBaseReg = 0x20;

    private byte[] _fanPwmCtrlReg;
    private readonly byte[] _fanPwmCtrlExtReg = [0x63, 0x6b, 0x73, 0x7b, 0xa3, 0xab];
    private readonly byte[] _fanTachometerExtReg = [0x18, 0x19, 0x1a, 0x81, 0x83, 0x4d];
    private readonly byte[] _fanTachometerReg = [0x0d, 0x0e, 0x0f, 0x80, 0x82, 0x4c];

    // Address of the Fan Controller Main Control Register.
    // No need for the 2nd control register (bit 7 of 0x15 0x16 0x17),
    // as PWM value will set it to manual mode when new value is set.
    private const byte FanMainCtrlReg = 0x13;

    private readonly ushort _addressReg;
    private readonly ushort _dataReg;
    private readonly ushort _gpioAddress;
    // Initial Fan Controller Main Control Register value. 
    private readonly bool[] _initialFanOutputModeEnabled = new bool[3]; 
    // This will also store the 2nd control register value.
    private readonly byte[] _initialFanPwmControl = new byte[MaxFanHeaders]; 
    private readonly byte[] _initialFanPwmControlExt = new byte[MaxFanHeaders];
    private readonly bool[] _restoreDefaultFanPwmControlRequired = new bool[MaxFanHeaders];
    private readonly byte _version;
    private int _gpioCount;
    private bool _has16BitFanCounter;
    private bool _hasExtReg;
    private float _voltageGain;
    private const int MaxFanHeaders = 6;
    private int _bankCount;
    private bool[] _fansDisabled = [];
    private readonly IGigabyteController _gigabyteController;
    // Fix #780 Set to true for those chips that need a SelectBank(0) to fix dodgy temps and fan speeds
    private bool _requiresBankSelect;  

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
    /// Initializes a new instance of the <see cref="It87Xx" /> class.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <param name="address">The address.</param>
    /// <param name="gpioAddress">The gpio address.</param>
    /// <param name="version">The version.</param>
    /// <param name="gigabyteController">The gigabyte controller.</param>
    public It87Xx(Chip chip, ushort address, ushort gpioAddress, byte version, IGigabyteController gigabyteController)
    {
        _version = version;
        _addressReg = (ushort)(address + AddressRegisterOffset);
        _dataReg = (ushort)(address + DataRegisterOffset);
        _gpioAddress = gpioAddress;
        _gigabyteController = gigabyteController;
        _requiresBankSelect = false;

        Chip = chip;

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
    public byte? ReadGpio(int index) => index >= _gpioCount ? null : Ring0.ReadIoPort((ushort)(_gpioAddress + index));

    /// <summary>
    /// Sets the control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">nameof(index)</exception>
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

            // Disable the controller when setting values to prevent it from overriding them
            _gigabyteController?.Enable(false);

            if (index < 3 && !_initialFanOutputModeEnabled[index])
            {
                WriteByte(FanMainCtrlReg, (byte)(ReadByte(FanMainCtrlReg, out _) | 1 << index));
            }

            if (_hasExtReg)
            {
                byte fanByte = (Chip == Chip.IT8689E) ? (byte)0x7F : (byte)(_initialFanPwmControl[index] & 0x7F);
                WriteByte(_fanPwmCtrlReg[index], fanByte);
                WriteByte(_fanPwmCtrlExtReg[index], value.Value);
            }
            else
            {
                WriteByte(_fanPwmCtrlReg[index], (byte)(value.Value >> 1));
            }
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
    /// <returns></returns>
    public void Update()
    {
        if (!Mutexes.WaitIsaBus(10)) return;

        // Is this needed on every update?  Yes, until a way to detect resume from sleep/hibernation
        // is added, as that invalidates the bank select.
        if (_requiresBankSelect) 
        {
            SelectBank(0);
        }

        for (int i = 0; i < Voltages.Length; i++)
        {
            float value = _voltageGain * ReadByte((byte)(VoltageBaseReg + i), out bool valid);
            if (!valid) continue;
            Voltages[i] = value > 0 ? value : null;
        }

        for (int i = 0; i < Temperatures.Length; i++)
        {
            sbyte value = (sbyte)ReadByte((byte)(TemperatureBaseReg + i), out bool valid);
            if (!valid) continue;
            Temperatures[i] = value is < sbyte.MaxValue and > 0 ? value : null;
        }

        if (_has16BitFanCounter)
        {
            for (int i = 0; i < Fans.Length; i++)
            {
                if (_fansDisabled[i]) continue;

                int value = ReadByte(_fanTachometerReg[i], out bool valid);
                if (!valid) continue;

                value |= ReadByte(_fanTachometerExtReg[i], out valid) << 8;
                if (!valid) continue;

                Fans[i] = value > 0x3f ? value < 0xffff ? 1.35e6f / (value * 2) : 0 : null;
            }
        }
        else
        {
            for (int i = 0; i < Fans.Length; i++)
            {
                int value = ReadByte(_fanTachometerReg[i], out bool valid);
                if (!valid) continue;
                int divisor = 2;
                if (i < 2)
                {
                    int divisors = ReadByte(FanTachometerDivisorRegister, out valid);
                    if (!valid) continue;
                    divisor = 1 << (divisors >> 3 * i & 0x7);
                }

                Fans[i] = value > 0 ? value < 0xff ? 1.35e6f / (value * divisor) : 0 : null;
            }
        }

        for (int i = 0; i < Controls.Length; i++)
        {
            byte value = ReadByte(_fanPwmCtrlReg[i], out bool valid);
            if (!valid) continue;

            if ((value & 0x80) > 0)
            {
                // Automatic operation (value can't be read).
                Controls[i] = null;
            }
            else
            {
                // Software operation.
                if (_hasExtReg)
                {
                    value = ReadByte(_fanPwmCtrlExtReg[i], out valid);
                    if (!valid) continue;
                    Controls[i] = (float)Math.Round(value * 100.0f / 0xFF);
                }
                else
                {
                    Controls[i] = (float)Math.Round((value & 0x7F) * 100.0f / 0x7F);
                }
            }
        }

        Mutexes.ReleaseIsaBus();
    }

    /// <summary>
    /// Writes the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public void WriteGpio(int index, byte value)
    {
        if (index >= _gpioCount) return;
        Ring0.WriteIoPort((ushort)(_gpioAddress + index), value);
    }

    /// <summary>
    /// Selects another bank. Memory from 0x10-0xAF swaps to data from new bank.
    /// Beware to select the default bank 0 after changing.
    /// Bank selection is reset after power cycle.
    /// </summary>
    /// <param name="bankIndex">New bank index. Can be a value of 0-3.</param>
    private void SelectBank(byte bankIndex)
    {
        // Current chip does not support that many banks
        if (bankIndex >= _bankCount) return; 

        // hard cap SelectBank to 2 bit values. If we ever have chips with more bank bits rewrite this method.
        bankIndex &= 0x3;
        byte value = ReadByte(BankRegister, out bool valid);
        if (!valid) return;
        value &= 0x9F;
        value |= (byte)(bankIndex << 5);
        WriteByte(BankRegister, value);
    }

    /// <summary>
    /// Sets the registers.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <returns></returns>
    private void SetRegisters(Chip chip)
    {
        // Check vendor id
        byte vendorId = ReadByte(VendorIdRegister, out bool valid);
        if (!valid) return;

        bool hasMatchingVendorId = false;
        foreach (byte iteVendorId in _iteVendorIds)
        {
            if (iteVendorId != vendorId) continue;
            hasMatchingVendorId = true;
            break;
        }

        if (!hasMatchingVendorId) return;

        // Bit 0x10 of the configuration register should always be 1
        byte configuration = ReadByte(ConfigurationRegister, out valid);
        if (!valid || (configuration & 0x10) == 0 && chip != Chip.IT8655E && chip != Chip.IT8665E) return;

        _fanPwmCtrlReg = chip switch
        {
            Chip.IT8665E or Chip.IT8625E => [0x15, 0x16, 0x17, 0x1e, 0x1f, 0x92],
            Chip.IT8792E => [0x15, 0x16, 0x17],
            _ => [0x15, 0x16, 0x17, 0x7f, 0xa7, 0xaf]
        };

        _bankCount = chip switch
        {
            Chip.IT8689E => 4,
            _ => 1
        };

        _hasExtReg = chip is Chip.IT8721F or
            Chip.IT8728F or
            Chip.IT8665E or
            Chip.IT8686E or
            Chip.IT8688E or
            Chip.IT8689E or
            Chip.IT87952E or
            Chip.IT8628E or
            Chip.IT8625E or
            Chip.IT8620E or
            Chip.IT8613E or
            Chip.IT8792E or
            Chip.IT8655E or
            Chip.IT8631E;

        switch (chip)
        {
            case Chip.IT8613E:
                Voltages = new float?[10];
                Temperatures = new float?[4];
                Fans = new float?[5];
                Controls = new float?[4];
                break;

            case Chip.IT8625E:
                Voltages = new float?[7];
                Temperatures = new float?[3];
                Fans = new float?[6];
                Controls = new float?[6];
                break;
            case Chip.IT8628E:
                Voltages = new float?[10];
                Temperatures = new float?[6];
                Fans = new float?[6];
                Controls = new float?[6];
                break;

            case Chip.IT8631E:
                Voltages = new float?[9];
                Temperatures = new float?[2];
                Fans = new float?[2];
                Controls = new float?[2];
                break;

            case Chip.IT8665E:
                Voltages = new float?[9];
                Temperatures = new float?[6];
                Fans = new float?[4];
                Controls = new float?[4];
                _requiresBankSelect = true;
                break;

            case Chip.IT8686E:
                Voltages = new float?[10];
                Temperatures = new float?[6];
                Fans = new float?[6];
                Controls = new float?[5];
                break;

            case Chip.IT8688E:
                Voltages = new float?[11];
                Temperatures = new float?[6];
                Fans = new float?[6];
                Controls = new float?[5];
                break;

            case Chip.IT8689E:
                Voltages = new float?[10];
                Temperatures = new float?[6];
                Fans = new float?[6];
                Controls = new float?[6];
                break;

            case Chip.IT87952E:
                Voltages = new float?[6];
                Temperatures = new float?[3];
                Fans = new float?[3];
                Controls = new float?[3];
                break;

            case Chip.IT8655E:
                Voltages = new float?[9];
                Temperatures = new float?[6];
                Fans = new float?[3];
                Controls = new float?[3];
                _requiresBankSelect = true;
                break;

            case Chip.IT8792E or Chip.IT8705F:
                Voltages = new float?[9];
                Temperatures = new float?[3];
                Fans = new float?[3];
                Controls = new float?[3];
                break;

            case Chip.IT8620E:
                Voltages = new float?[9];
                Temperatures = new float?[3];
                Fans = new float?[5];
                Controls = new float?[5];
                break;

            default:
                Voltages = new float?[9];
                Temperatures = new float?[3];
                Fans = new float?[5];
                Controls = new float?[3];
                break;
        }

        _fansDisabled = new bool[Fans.Length];

        // Voltage gain varies by model.
        // Conflicting reports on IT8792E: either 0.0109 in linux drivers or 0.011 comparing with Gigabyte board & SIV SW.
        _voltageGain = chip switch
        {
            Chip.IT8613E or Chip.IT8620E or Chip.IT8628E or Chip.IT8631E or Chip.IT8721F or Chip.IT8728F or Chip.IT8771E or Chip.IT8772E or Chip.IT8686E or Chip.IT8688E or Chip.IT8689E => 0.012f,
            Chip.IT8625E or Chip.IT8792E or Chip.IT87952E => 0.011f,
            Chip.IT8655E or Chip.IT8665E => 0.0109f,
            _ => 0.016f
        };

        // Older IT8705F and IT8721F revisions do not have 16-bit fan counters.
        _has16BitFanCounter = (chip != Chip.IT8705F || _version >= 3) && (chip != Chip.IT8712F || _version >= 8);

        // Disable any fans that aren't set with 16-bit fan counters
        if (_has16BitFanCounter)
        {
            int modes = ReadByte(FanTachometer16BitRegister, out valid);

            if (!valid) return;

            if (Fans.Length >= 5)
            {
                _fansDisabled[3] = (modes & 1 << 4) == 0;
                _fansDisabled[4] = (modes & 1 << 5) == 0;
            }

            if (Fans.Length >= 6)
            {
                _fansDisabled[5] = (modes & 1 << 2) == 0;
            }
        }

        // Set the number of GPIO sets
        _gpioCount = chip switch
        {
            Chip.IT8712F or Chip.IT8716F or Chip.IT8718F or Chip.IT8726F => 5,
            Chip.IT8720F or Chip.IT8721F => 8,
            _ => 0
        };
    }

    /// <summary>
    /// Reads the byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="valid">if set to <c>true</c> [valid].</param>
    /// <returns></returns>
    private byte ReadByte(byte register, out bool valid)
    {
        Ring0.WriteIoPort(_addressReg, register);
        byte value = Ring0.ReadIoPort(_dataReg);
        valid = register == Ring0.ReadIoPort(_addressReg) || Chip == Chip.IT8688E;
        // IT8688E doesn't return the value we wrote to
        // addressReg when we read it back.

        return value;
    }

    /// <summary>
    /// Writes the byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private void WriteByte(byte register, byte value)
    {
        Ring0.WriteIoPort(_addressReg, register);
        Ring0.WriteIoPort(_dataReg, value);
        Ring0.ReadIoPort(_addressReg);
    }

    /// <summary>
    /// Saves the default fan PWM control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    private void SaveDefaultFanPwmControl(int index)
    {
        if (!_restoreDefaultFanPwmControlRequired[index])
        {
            _initialFanPwmControl[index] = ReadByte(_fanPwmCtrlReg[index], out bool _);

            if (index < 3)
            {
                // Save default control reg value.
                _initialFanOutputModeEnabled[index] = ReadByte(FanMainCtrlReg, out bool _) != 0; 
            }

            if (_hasExtReg)
            {
                _initialFanPwmControlExt[index] = ReadByte(_fanPwmCtrlExtReg[index], out _);
            }
        }

        _restoreDefaultFanPwmControlRequired[index] = true;
    }

    /// <summary>
    /// Restores the default fan PWM control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    private void RestoreDefaultFanPwmControl(int index)
    {
        if (!_restoreDefaultFanPwmControlRequired[index]) return;
        WriteByte(_fanPwmCtrlReg[index], _initialFanPwmControl[index]);

        if (index < 3)
        {
            byte value = ReadByte(FanMainCtrlReg, out _);
            bool isEnabled = (value & 1 << index) != 0;
            if (isEnabled != _initialFanOutputModeEnabled[index])
            {
                WriteByte(FanMainCtrlReg, (byte)(value ^ 1 << index));
            }
        }

        if (_hasExtReg)
        {
            WriteByte(_fanPwmCtrlExtReg[index], _initialFanPwmControlExt[index]);
        }

        _restoreDefaultFanPwmControlRequired[index] = false;

        // restore the GB controller when all fans become restored
        if (_gigabyteController != null && _restoreDefaultFanPwmControlRequired.All(e => e == false))
        {
            _gigabyteController.Restore();
        }
    }

    #endregion
}
