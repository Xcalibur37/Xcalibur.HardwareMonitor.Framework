





namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo;

/// <summary>
/// Super I/O: Interface
/// </summary>
internal interface ISuperIo
{
    #region Properties

    /// <summary>
    /// Gets the chip.
    /// </summary>
    /// <value>
    /// The chip.
    /// </value>
    Chip Chip { get; }

    /// <summary>
    /// Gets the controls.
    /// </summary>
    /// <value>
    /// The controls.
    /// </value>
    float?[] Controls { get; }

    /// <summary>
    /// Gets the fans.
    /// </summary>
    /// <value>
    /// The fans.
    /// </value>
    float?[] Fans { get; }

    /// <summary>
    /// Gets the temperatures.
    /// </summary>
    /// <value>
    /// The temperatures.
    /// </value>
    float?[] Temperatures { get; }

    // get voltage, temperature, fan and control channel values
    /// <summary>
    /// Gets the voltages.
    /// </summary>
    /// <value>
    /// The voltages.
    /// </value>
    float?[] Voltages { get; }

    #endregion

    #region Methods

    // set control value, null = auto
    /// <summary>
    /// Sets the control.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    void SetControl(int index, byte? value);

    // read and write GPIO
    /// <summary>
    /// Reads the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    byte? ReadGpio(int index);

    /// <summary>
    /// Writes the gpio.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="value">The value.</param>
    void WriteGpio(int index, byte value);

    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update();

    #endregion
}