using System;
using System.Globalization;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

internal class Parameter : IParameter
{
    #region Fields

    private readonly ISettings _settings;
    private readonly ParameterDescription _description;
    private bool _isDefault;
    private float _value;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a parameter default value defined by library.
    /// </summary>
    public float DefaultValue => _description.DefaultValue;

    /// <summary>
    /// Gets a parameter description defined by library.
    /// </summary>
    public string Description => _description.Description;

    /// <summary>
    /// Gets a unique parameter ID that represents its location.
    /// </summary>
    public Identifier Identifier => new(Sensor.Identifier, "parameter", Name.Replace(" ", string.Empty).ToLowerInvariant());

    /// <summary>
    /// Gets or sets information whether the given <see cref="IParameter" /> is the default for <see cref="ISensor" />.
    /// </summary>
    public bool IsDefault
    {
        get => _isDefault;
        set
        {
            _isDefault = value;
            if (!value) return;
            _value = _description.DefaultValue;
            _settings.Remove(Identifier.ToString());
        }
    }

    /// <summary>
    /// Gets a parameter name defined by library.
    /// </summary>
    public string Name => _description.Name;

    /// <summary>
    /// Gets the sensor that is the data container for the given parameter.
    /// </summary>
    public ISensor Sensor { get; }

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public float Value
    {
        get => _value;
        set
        {
            _isDefault = false;
            _value = value;
            _settings.SetValue(Identifier.ToString(), value.ToString(CultureInfo.InvariantCulture));
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Parameter"/> class.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <param name="sensor">The sensor.</param>
    /// <param name="settings">The settings.</param>
    public Parameter(ParameterDescription description, ISensor sensor, ISettings settings)
    {
        Sensor = sensor;
        _description = description;
        _settings = settings;
        _isDefault = !settings.Contains(Identifier.ToString());
        _value = description.DefaultValue;
        if (!_isDefault && !float.TryParse(settings.GetValue(Identifier.ToString(), "0"), NumberStyles.Float, CultureInfo.InvariantCulture, out _value))
        {
            _value = description.DefaultValue;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Accepts the observer for this instance.
    /// </summary>
    /// <param name="visitor">Computer observer making the calls.</param>
    /// <exception cref="System.ArgumentNullException">visitor</exception>
    public void Accept(IVisitor visitor)
    {
        if (visitor != null)
        {
            visitor.VisitParameter(this);
        }
        else
        {
            throw new ArgumentNullException(nameof(visitor));
        }
    }

    /// <summary>
    /// Call the <see cref="Accept" /> method for all child instances <c>(called only from visitors).</c>
    /// </summary>
    /// <param name="visitor">Computer observer making the calls.</param>
    public void Traverse(IVisitor visitor) { }

    #endregion
}
