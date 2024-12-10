using System;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

// ReSharper disable once InconsistentNaming

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Fintek;

/// <summary>
/// Fintek Integrated Circuits: F718xx
/// </summary>
/// <seealso cref="ISuperIo" />
internal class F718XX : ISuperIo
{
    #region Fields

    private const byte AddressRegisterOffset = 0x05;
    private const byte DataRegisterOffset = 0x06;
    private const byte PwmValuesOffset = 0x2D;
    private const byte TemperatureBaseReg = 0x70;
    private const byte TemperatureConfigReg = 0x69;

    private const byte VoltageBaseReg = 0x20;
    private readonly byte[] _fanPwmReg = { 0xA3, 0xB3, 0xC3, 0xD3 };
    private readonly byte[] _fanTachometerReg = { 0xA0, 0xB0, 0xC0, 0xD0 };

    private readonly ushort _address;
    private readonly byte[] _initialFanPwmControl = new byte[4];
    private readonly bool[] _restoreDefaultFanPwmControlRequired = new bool[4];

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
    /// Initializes a new instance of the <see cref="F718XX"/> class.
    /// </summary>
    /// <param name="chip">The chip.</param>
    /// <param name="address">The address.</param>
    public F718XX(Chip chip, ushort address)
    {
        _address = address;
        Chip = chip;

        Voltages = new float?[chip == Chip.F71858 ? 3 : 9];
        Temperatures = new float?[chip == Chip.F71808E ? 2 : 3];
        Fans = new float?[chip is Chip.F71882 or Chip.F71858 ? 4 : 3];
        Controls = new float?[chip switch
        {
            Chip.F71878AD or Chip.F71889AD => 3,
            Chip.F71882 => 4,
            _ => 0
        }];
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
    /// Writes the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    public void WriteGpio(int index, byte value) { }

    /// <summary>
    /// Sets the control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="ArgumentOutOfRangeException">index</exception>
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
            WriteByte(_fanPwmReg[index], value.Value);
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

        for (int i = 0; i < Voltages.Length; i++)
        {
            // 0x26 is reserved on F71808E
            Voltages[i] = Chip == Chip.F71808E && i == 6 
                ? 0 
                : 0.008f * ReadByte((byte)(VoltageBaseReg + i));
        }

        for (int i = 0; i < Temperatures.Length; i++)
        {
            switch (Chip)
            {
                case Chip.F71858:
                    {
                        int tableMode = 0x3 & ReadByte(TemperatureConfigReg);
                        int high = ReadByte((byte)(TemperatureBaseReg + 2 * i));
                        int low = ReadByte((byte)(TemperatureBaseReg + 2 * i + 1));
                        if (high is not 0xbb and not 0xcc)
                        {
                            int bits = 0;
                            switch (tableMode)
                            {
                                case 0:
                                    break;
                                case 1:
                                    bits = 0;
                                    break;
                                case 2:
                                    bits = (high & 0x80) << 8;
                                    break;
                                case 3:
                                    bits = (low & 0x01) << 15;
                                    break;
                            }

                            bits |= high << 7;
                            bits |= (low & 0xe0) >> 1;
                            short value = (short)(bits & 0xfff0);
                            Temperatures[i] = value / 128.0f;
                        }
                        else
                        {
                            Temperatures[i] = null;
                        }
                    }

                    break;
                default:
                    {
                        sbyte value = (sbyte)ReadByte((byte)(TemperatureBaseReg + 2 * (i + 1)));
                        Temperatures[i] = value is < sbyte.MaxValue and > 0 ? value : null;
                    }
                    break;
            }
        }

        for (int i = 0; i < Fans.Length; i++)
        {
            int value = ReadByte(_fanTachometerReg[i]) << 8;
            value |= ReadByte((byte)(_fanTachometerReg[i] + 1));
            Fans[i] = value > 0 ? value < 0x0fff ? 1.5e6f / value : 0 : null;
        }

        for (int i = 0; i < Controls.Length; i++)
        {
            Controls[i] = Chip == Chip.F71882 || Chip == Chip.F71889AD
                ? ReadByte(_fanPwmReg[i]) * 100.0f / 0xFF
                : ReadByte((byte)(PwmValuesOffset + i)) * 100.0f / 0xFF;
        }

        Mutexes.ReleaseIsaBus();
    }

    /// <summary>
    /// Saves the default fan PWM control.
    /// </summary>
    /// <param name="index">The index.</param>
    private void SaveDefaultFanPwmControl(int index)
    {
        if (_restoreDefaultFanPwmControlRequired[index]) return;
        _initialFanPwmControl[index] = ReadByte(_fanPwmReg[index]);
        _restoreDefaultFanPwmControlRequired[index] = true;
    }

    /// <summary>
    /// Restores the default fan PWM control.
    /// </summary>
    /// <param name="index">The index.</param>
    private void RestoreDefaultFanPwmControl(int index)
    {
        if (!_restoreDefaultFanPwmControlRequired[index]) return;
        WriteByte(_fanPwmReg[index], _initialFanPwmControl[index]);
        _restoreDefaultFanPwmControlRequired[index] = false;
    }

    /// <summary>
    /// Reads the byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <returns></returns>
    private byte ReadByte(byte register)
    {
        Ring0.WriteIoPort((ushort)(_address + AddressRegisterOffset), register);
        return Ring0.ReadIoPort((ushort)(_address + DataRegisterOffset));
    }

    /// <summary>
    /// Writes the byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    private void WriteByte(byte register, byte value)
    {
        Ring0.WriteIoPort((ushort)(_address + AddressRegisterOffset), register);
        Ring0.WriteIoPort((ushort)(_address + DataRegisterOffset), value);
    }

    #endregion
}
