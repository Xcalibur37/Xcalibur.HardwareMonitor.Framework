﻿using System.Collections.Generic;
using System.Linq;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware;

/// <summary>
/// Object representing a component of the computer.
/// <para>
/// Individual information can be read from the <see cref="Sensors"/>.
/// </para>
/// </summary>
public abstract class Hardware : IHardware
{
    #region Fields

    private string _name;

    #endregion

    #region Properties

    /// <inheritdoc />
    public abstract HardwareType HardwareType { get; }

    /// <inheritdoc />
    public Identifier Identifier { get; }

    /// <inheritdoc />
    public string Name
    {
        get => _name;
        set
        {
            _name = !string.IsNullOrEmpty(value) ? value : OriginalName;
            Settings.SetValue(new Identifier(Identifier, "name").ToString(), _name);
        }
    }

    /// <inheritdoc />
    public IHardware[] SubHardware => [];

    /// <inheritdoc />
    public virtual IHardware Parent => null;

    /// <inheritdoc />
    public virtual IDictionary<string, string> Properties => new SortedDictionary<string, string>();

    /// <inheritdoc />
    public virtual ISensor[] Sensors => Active.ToArray();

    /// <summary>
    /// Gets the active.
    /// </summary>
    /// <value>
    /// The active.
    /// </value>
    protected HashSet<ISensor> Active { get; } = [];

    /// <summary>
    /// Gets the name of the original.
    /// </summary>
    /// <value>
    /// The name of the original.
    /// </value>
    protected string OriginalName { get; }

    /// <summary>
    /// Gets the settings.
    /// </summary>
    /// <value>
    /// The settings.
    /// </value>
    internal ISettings Settings { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new <see cref="Hardware"/> instance based on the data provided.
    /// </summary>
    /// <param name="name">Component name.</param>
    /// <param name="identifier">Identifier that will be assigned to the device. Based on <see cref="Identifier"/></param>
    /// <param name="settings">Additional settings passed by the <see cref="IComputer"/>.</param>
    protected Hardware(string name, Identifier identifier, ISettings settings)
    {
        Settings = settings;
        OriginalName = name;
        Identifier = identifier;
        _name = settings.GetValue(new Identifier(Identifier, "name").ToString(), name);
    }

    #endregion

    #region Methods
    
    /// <inheritdoc />
    public abstract void Update();

    /// <inheritdoc />
    public void Accept(IVisitor visitor)
    {
        visitor?.VisitHardware(this);
    }

    /// <inheritdoc />
    public virtual void Traverse(IVisitor visitor)
    {
        Active.Apply(x => x.Accept(visitor));
    }

    /// <summary>
    /// Activates the sensor.
    /// </summary>
    /// <param name="sensor">The sensor.</param>
    protected internal virtual void ActivateSensor(ISensor sensor)
    {
        if (!Active.Add(sensor)) return;
        SensorAdded?.Invoke(sensor);
    }

    /// <summary>
    /// Deactivates the sensor.
    /// </summary>
    /// <param name="sensor">The sensor.</param>
    protected virtual void DeactivateSensor(ISensor sensor)
    {
        if (!Active.Remove(sensor)) return;
        SensorRemoved?.Invoke(sensor);
    }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public virtual void Close()
    {
        Closing?.Invoke(this);
    }

    #endregion

    #region Events

    /// <summary>
    /// Event triggered when <see cref="Hardware"/> is closing.
    /// </summary>
    public event HardwareEventHandler Closing;

    /// <summary>
    /// An <see langword="event" /> that will be triggered when a new sensor appears.
    /// </summary>
    public event SensorEventHandler SensorAdded;

    /// <summary>
    /// An <see langword="event" /> that will be triggered when one of the sensors is removed.
    /// </summary>
    public event SensorEventHandler SensorRemoved;

    #endregion
}
