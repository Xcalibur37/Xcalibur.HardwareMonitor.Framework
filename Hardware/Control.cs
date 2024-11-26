using System.Globalization;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware;

internal delegate void ControlEventHandler(Control control);

/// <summary>
/// Control
/// </summary>
/// <seealso cref="IControl" />
internal class Control : IControl
{
    #region Fields
    
    private readonly ISettings _settings;
    private ControlMode _mode;
    private float _softwareValue;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the control mode.
    /// </summary>
    /// <value>
    /// The control mode.
    /// </value>
    public ControlMode ControlMode
    {
        get => _mode;
        private set
        {
            if (_mode == value) return;
            _mode = value;
            ControlModeChanged?.Invoke(this);
            _settings.SetValue(new Identifier(Identifier, "mode").ToString(), ((int)_mode).ToString(CultureInfo.InvariantCulture));
        }
    }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public Identifier Identifier { get; }

    /// <summary>
    /// Gets the maximum software value.
    /// </summary>
    /// <value>
    /// The maximum software value.
    /// </value>
    public float MaxSoftwareValue { get; }

    /// <summary>
    /// Gets the minimum software value.
    /// </summary>
    /// <value>
    /// The minimum software value.
    /// </value>
    public float MinSoftwareValue { get; }

    /// <summary>
    /// Gets the sensor.
    /// </summary>
    /// <value>
    /// The sensor.
    /// </value>
    public ISensor Sensor { get; }

    /// <summary>
    /// Gets the software value.
    /// </summary>
    /// <value>
    /// The software value.
    /// </value>
    public float SoftwareValue
    {
        get => _softwareValue;
        private set
        {
            if (_softwareValue.Equals(value)) return;
            _softwareValue = value;
            SoftwareControlValueChanged?.Invoke(this);
            _settings.SetValue(new Identifier(Identifier, "value").ToString(), value.ToString(CultureInfo.InvariantCulture));
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Control" /> class.
    /// </summary>
    /// <param name="sensor">The sensor.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="minSoftwareValue">The minimum software value.</param>
    /// <param name="maxSoftwareValue">The maximum software value.</param>
    public Control(ISensor sensor, ISettings settings, float minSoftwareValue, float maxSoftwareValue)
    {
        _settings = settings;
        Identifier = new Identifier(sensor.Identifier, "control");
        Sensor = sensor;
        MinSoftwareValue = minSoftwareValue;
        MaxSoftwareValue = maxSoftwareValue;


        if (!float.TryParse(
                settings.GetValue(new Identifier(Identifier, "value").ToString(), "0"),
                NumberStyles.Float, CultureInfo.InvariantCulture, out _softwareValue))
        {
            _softwareValue = 0;
        }

        _mode = !int.TryParse(
            settings.GetValue(new Identifier(Identifier, "mode").ToString(),
                ((int)ControlMode.Undefined).ToString(CultureInfo.InvariantCulture)),
            NumberStyles.Integer, CultureInfo.InvariantCulture, out int mode)
            ? ControlMode.Undefined : (ControlMode)mode;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets the default.
    /// </summary>
    public void SetDefault()
    {
        ControlMode = ControlMode.Default;
    }

    /// <summary>
    /// Sets the software.
    /// </summary>
    /// <param name="value">The value.</param>
    public void SetSoftware(float value)
    {
        ControlMode = ControlMode.Software;
        SoftwareValue = value;
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when [control mode changed].
    /// </summary>
    internal event ControlEventHandler ControlModeChanged;

    /// <summary>
    /// Occurs when [software control value changed].
    /// </summary>
    internal event ControlEventHandler SoftwareControlValueChanged;

    #endregion
}
