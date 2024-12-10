using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc;

/// <summary>
/// LPC Port
/// </summary>
internal class LpcPort
{
    #region Fields

    private const byte ConfigurationControlRegister = 0x02;
    private const byte DeviceSelectRegister = 0x07;
    private const byte NuvotonHardwareMonitorIoSpaceLock = 0x28;
    
    #endregion

    #region Properties

    /// <summary>
    /// Gets the register port.
    /// </summary>
    /// <value>
    /// The register port.
    /// </value>
    public ushort RegisterPort { get; }

    /// <summary>
    /// Gets the value port.
    /// </summary>
    /// <value>
    /// The value port.
    /// </value>
    public ushort ValuePort { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LpcPort"/> class.
    /// </summary>
    /// <param name="registerPort">The register port.</param>
    /// <param name="valuePort">The value port.</param>
    public LpcPort(ushort registerPort, ushort valuePort)
    {
        RegisterPort = registerPort;
        ValuePort = valuePort;
    }

    #endregion

    #region Methods

    /// <summary>
    /// IT87: Write to port
    /// </summary>
    public void It87Enter()
    {
        Ring0.WriteIoPort(RegisterPort, 0x87);
        Ring0.WriteIoPort(RegisterPort, 0x01);
        Ring0.WriteIoPort(RegisterPort, 0x55);
        Ring0.WriteIoPort(RegisterPort, RegisterPort == 0x4E ? (byte)0xAA : (byte)0x55);
    }

    /// <summary>
    /// is the T87 exit.
    /// </summary>
    public void It87Exit()
    {
        // Do not exit config mode for secondary super IO.
        if (RegisterPort != 0x4E)
        {
            Ring0.WriteIoPort(RegisterPort, ConfigurationControlRegister);
            Ring0.WriteIoPort(ValuePort, 0x02);
        }
    }

    /// <summary>
    /// Nuvoton: Disable IO space lock.
    /// </summary>
    public void NuvotonDisableIoSpaceLock()
    {
        byte options = ReadByte(NuvotonHardwareMonitorIoSpaceLock);
        // if the i/o space lock is enabled
        if ((options & 0x10) > 0)
        {
            // disable the i/o space lock
            WriteByte(NuvotonHardwareMonitorIoSpaceLock, (byte)(options & ~0x10));
        }
    }
    
    /// <summary>
    /// Reads the byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <returns></returns>
    public byte ReadByte(byte register)
    {
        Ring0.WriteIoPort(RegisterPort, register);
        return Ring0.ReadIoPort(ValuePort);
    }

    /// <summary>
    /// Reads the word.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <returns></returns>
    public ushort ReadWord(byte register) =>
        (ushort)((ReadByte(register) << 8) | ReadByte((byte)(register + 1)));

    /// <summary>
    /// Selects the specified logical device number.
    /// </summary>
    /// <param name="logicalDeviceNumber">The logical device number.</param>
    public void Select(byte logicalDeviceNumber)
    {
        Ring0.WriteIoPort(RegisterPort, DeviceSelectRegister);
        Ring0.WriteIoPort(ValuePort, logicalDeviceNumber);
    }

    /// <summary>
    /// SMSCs the enter.
    /// </summary>
    public void SmscEnter()
    {
        Ring0.WriteIoPort(RegisterPort, 0x55);
    }

    /// <summary>
    /// SMSCs the exit.
    /// </summary>
    public void SmscExit()
    {
        Ring0.WriteIoPort(RegisterPort, 0xAA);
    }

    /// <summary>
    /// Tries the read word.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool TryReadWord(byte register, out ushort value)
    {
        value = ReadWord(register);
        return value != 0xFFFF;
    }
    
    /// <summary>
    /// Winbond Nuvoton Fintek enter.
    /// </summary>
    public void WinbondNuvotonFintekEnter()
    {
        Ring0.WriteIoPort(RegisterPort, 0x87);
        Ring0.WriteIoPort(RegisterPort, 0x87);
    }

    /// <summary>
    /// Winbond Nuvoton Fintek exit.
    /// </summary>
    public void WinbondNuvotonFintekExit()
    {
        Ring0.WriteIoPort(RegisterPort, 0xAA);
    }

    /// <summary>
    /// Writes the byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    public void WriteByte(byte register, byte value)
    {
        Ring0.WriteIoPort(RegisterPort, register);
        Ring0.WriteIoPort(ValuePort, value);
    }

    #endregion
}
