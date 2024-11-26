using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Xcalibur.Extensions.V2;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

/// <summary>
/// Sensor
/// </summary>
/// <seealso cref="ISensor" />
internal class Sensor : ISensor
{
    #region Fields

    private readonly string _defaultName;
    private readonly Hardware _hardware;
    private readonly ISettings _settings;
    private readonly bool _trackMinMax;
    private readonly List<SensorValue> _values = new();
    private int _count;
    private float? _currentValue;
    private string _name;
    private float _sum;
    private TimeSpan _valuesTimeWindow = TimeSpan.FromDays(1.0);

    #endregion

    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Sensor"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="index">The index.</param>
    /// <param name="sensorType">Type of the sensor.</param>
    /// <param name="hardware">The hardware.</param>
    /// <param name="settings">The settings.</param>
    public Sensor(string name, int index, SensorType sensorType, Hardware hardware, ISettings settings) :
        this(name, index, sensorType, hardware, null, settings) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sensor"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="index">The index.</param>
    /// <param name="sensorType">Type of the sensor.</param>
    /// <param name="hardware">The hardware.</param>
    /// <param name="parameterDescriptions">The parameter descriptions.</param>
    /// <param name="settings">The settings.</param>
    public Sensor(string name, int index, SensorType sensorType, Hardware hardware, ParameterDescription[] parameterDescriptions, ISettings settings) :
        this(name, index, false, sensorType, hardware, parameterDescriptions, settings) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sensor"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="index">The index.</param>
    /// <param name="defaultHidden">if set to <c>true</c> [default hidden].</param>
    /// <param name="sensorType">Type of the sensor.</param>
    /// <param name="hardware">The hardware.</param>
    /// <param name="parameterDescriptions">The parameter descriptions.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="disableHistory">if set to <c>true</c> [disable history].</param>
    public Sensor
    (
        string name,
        int index,
        bool defaultHidden,
        SensorType sensorType,
        Hardware hardware,
        ParameterDescription[] parameterDescriptions,
        ISettings settings,
        bool disableHistory = false)
    {
        Index = index;
        IsDefaultHidden = defaultHidden;
        SensorType = sensorType;
        _hardware = hardware;

        var parameters = new Parameter[parameterDescriptions?.Length ?? 0];
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameterDescriptions == null) continue;
            parameters[i] = new Parameter(parameterDescriptions[i], this, settings);
        }

        Parameters = parameters;

        _settings = settings;
        _defaultName = name;
        _name = settings.GetValue(new Identifier(Identifier, "name").ToString(), name);
        _trackMinMax = !disableHistory;
        if (disableHistory)
        {
            _valuesTimeWindow = TimeSpan.Zero;
        }

        GetSensorValuesFromSettings();

        hardware.Closing += delegate { SetSensorValuesToSettings(); };
    }

    #endregion

    #region Properties
    
    /// <summary>
    /// Gets the control.
    /// </summary>
    /// <value>
    /// The control.
    /// </value>
    public IControl Control { get; internal set; }

    /// <summary>
    ///   <inheritdoc cref="IHardware" />
    /// </summary>
    public IHardware Hardware => _hardware;

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public Identifier Identifier => 
        new(_hardware.Identifier, SensorType.ToString().ToLowerInvariant(), Index.ToString(CultureInfo.InvariantCulture));

    /// <summary>
    /// Gets the unique identifier of this sensor for a given <see cref="IHardware" />.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is default hidden.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is default hidden; otherwise, <c>false</c>.
    /// </value>
    public bool IsDefaultHidden { get; }

    /// <summary>
    /// Determines the maximum of the parameters.
    /// </summary>
    public float? Max { get; private set; }

    /// <summary>
    /// Determines the minimum of the parameters.
    /// </summary>
    public float? Min { get; private set; }

    /// <summary>
    /// Gets or sets a sensor name.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            _name = !string.IsNullOrEmpty(value) ? value : _defaultName;
            _settings.SetValue(new Identifier(Identifier, "name").ToString(), _name);
        }
    }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <value>
    /// The parameters.
    /// </value>
    public IReadOnlyList<IParameter> Parameters { get; }

    /// <summary>
    ///   <inheritdoc cref="Sensors.SensorType" />
    /// </summary>
    public SensorType SensorType { get; }

    /// <summary>
    /// Gets the last recorded value for the given sensor.
    /// </summary>
    public virtual float? Value
    {
        get => _currentValue;
        set
        {
            if (_valuesTimeWindow != TimeSpan.Zero)
            {
                DateTime now = DateTime.UtcNow;
                while (_values.Count > 0 && now - _values[0].Time > _valuesTimeWindow)
                    _values.RemoveAt(0);

                if (value.HasValue)
                {
                    _sum += value.Value;
                    _count++;
                    if (_count == 4)
                    {
                        AppendValue(_sum / _count, now);
                        _sum = 0;
                        _count = 0;
                    }
                }
            }

            _currentValue = value;
            if (!_trackMinMax) return;
            if (!Min.HasValue || Min > value)
            {
                Min = value;
            }

            if (!Max.HasValue || Max < value)
            {
                Max = value;
            }
        }
    }

    /// <summary>
    /// Gets a list of recorded values for the given sensor.
    /// </summary>
    public IEnumerable<SensorValue> Values => _values;

    /// <summary>
    /// Gets or sets the values time window.
    /// </summary>
    /// <value>
    /// The values time window.
    /// </value>
    public TimeSpan ValuesTimeWindow
    {
        get => _valuesTimeWindow;
        set
        {
            _valuesTimeWindow = value;
            if (value != TimeSpan.Zero) return;
            _values.Clear();
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
            visitor.VisitSensor(this);
        }
        else
        {
            throw new ArgumentNullException(nameof(visitor));
        }
    }

    /// <summary>
    /// Clears the values stored in <see cref="Values" />.
    /// </summary>
    public void ClearValues()
    {
        _values.Clear();
    }
    /// <summary>
    /// Resets a value stored in <see cref="Max" />.
    /// </summary>
    public void ResetMax()
    {
        Max = null;
    }

    /// <summary>
    /// Resets a value stored in <see cref="Min" />.
    /// </summary>
    public void ResetMin()
    {
        Min = null;
    }

    /// <summary>
    /// Call the <see cref="Accept" /> method for all child instances <c>(called only from visitors).</c>
    /// </summary>
    /// <param name="visitor">Computer observer making the calls.</param>
    public void Traverse(IVisitor visitor)
    {
        Parameters.Apply(x => x.Accept(visitor));
    }
    /// <summary>
    /// Appends the value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="time">The time.</param>
    private void AppendValue(float value, DateTime time)
    {
        if (_values.Count >= 2 && _values[_values.Count - 1].Value == value && _values[_values.Count - 2].Value == value)
        {
            _values[^1] = new SensorValue(value, time);
            return;
        }

        _values.Add(new SensorValue(value, time));
    }

    /// <summary>
    /// Gets the sensor values from settings.
    /// </summary>
    private void GetSensorValuesFromSettings()
    {
        string name = new Identifier(Identifier, "values").ToString();
        string s = _settings.GetValue(name, null);

        if (!string.IsNullOrEmpty(s))
        {
            try
            {
                byte[] array = Convert.FromBase64String(s);
                DateTime now = DateTime.UtcNow;

                using MemoryStream memoryStream = new(array);
                using GZipStream gZipStream = new(memoryStream, CompressionMode.Decompress);
                using MemoryStream destination = new();

                gZipStream.CopyTo(destination);
                destination.Seek(0, SeekOrigin.Begin);

                using BinaryReader reader = new(destination);
                try
                {
                    long t = 0;
                    long readLen = reader.BaseStream.Length - reader.BaseStream.Position;
                    while (readLen > 0)
                    {
                        t += reader.ReadInt64();
                        DateTime time = DateTime.FromBinary(t);
                        if (time > now)
                            break;

                        float value = reader.ReadSingle();
                        AppendValue(value, time);
                        readLen = reader.BaseStream.Length - reader.BaseStream.Position;
                    }
                }
                catch (EndOfStreamException)
                { }
            }
            catch
            {
                // Ignored.
            }
        }

        if (_values.Count > 0)
        {
            AppendValue(float.NaN, DateTime.UtcNow);
        }

        //remove the value string from the settings to reduce memory usage
        _settings.Remove(name);
    }

    /// <summary>
    /// Sets the sensor values to settings.
    /// </summary>
    private void SetSensorValuesToSettings()
    {
        using MemoryStream memoryStream = new();
        using GZipStream gZipStream = new(memoryStream, CompressionMode.Compress);
        using BufferedStream outputStream = new(gZipStream, 65536);
        using (BinaryWriter binaryWriter = new(outputStream))
        {
            long t = 0;

            foreach (SensorValue sensorValue in _values)
            {
                long v = sensorValue.Time.ToBinary();
                binaryWriter.Write(v - t);
                t = v;
                binaryWriter.Write(sensorValue.Value);
            }

            binaryWriter.Flush();
        }

        _settings.SetValue(new Identifier(Identifier, "values").ToString(), Convert.ToBase64String(memoryStream.ToArray()));
    }

    #endregion
}