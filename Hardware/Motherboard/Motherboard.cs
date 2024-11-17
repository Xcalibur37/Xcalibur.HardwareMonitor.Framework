using System;
using System.Collections.Generic;
using System.Linq;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;
using OperatingSystem = Xcalibur.HardwareMonitor.Framework.Software.OperatingSystem;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard;

/// <summary>
/// Represents the motherboard of a computer with its <see cref="LpcIo" /> and
/// <see cref="EmbeddedControllerBase" /> as <see cref="SubHardware" />.
/// </summary>
public class Motherboard : IHardware
{
    #region Fields

    private readonly LMSensors _lmSensors;
    private readonly string _name;
    private readonly ISettings _settings;
    private string _customName;

    #endregion

    #region Properties

    /// <inheritdoc />
    public HardwareType HardwareType => HardwareType.Motherboard;

    /// <inheritdoc />
    public Identifier Identifier => new("motherboard");

    /// <summary>
    /// Gets the <see cref="Models.Manufacturer" />.
    /// </summary>
    public Manufacturer Manufacturer { get; }

    /// <summary>
    /// Gets the <see cref="MotherboardModel" />.
    /// </summary>
    public MotherboardModel Model { get; }

    /// <summary>
    /// Gets the name obtained from <see cref="SMBios" />.
    /// </summary>
    public string Name
    {
        get => _customName;
        set
        {
            _customName = !string.IsNullOrEmpty(value) ? value : _name;

            _settings.SetValue(new Identifier(Identifier, "name").ToString(), _customName);
        }
    }

    /// <inheritdoc />
    /// <returns>Always <see langword="null" /></returns>
    public virtual IHardware Parent => null;

    /// <inheritdoc />
    public virtual IDictionary<string, string> Properties => new SortedDictionary<string, string>();

    /// <inheritdoc />
    public ISensor[] Sensors => Array.Empty<ISensor>();

    /// <summary>
    /// Gets the <see cref="SMBios" /> information.
    /// </summary>
    public SMBios SmBios { get; }

    /// <inheritdoc />
    public IHardware[] SubHardware { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates motherboard instance by retrieving information from <see cref="SMBios" /> and creates a new <see cref="SubHardware" /> based on data from <see cref="LpcIo" />
    /// and <see cref="EmbeddedControllerBase" />.
    /// </summary>
    /// <param name="smBios"><see cref="SMBios" /> table containing motherboard data.</param>
    /// <param name="settings">Additional settings passed by <see cref="IComputer" />.</param>
    public Motherboard(SMBios smBios, ISettings settings)
    {
        _settings = settings;

        SmBios = smBios;
        var board = smBios.Board;
        Manufacturer = board == null ? Manufacturer.Unknown : IdentificationHelper.GetManufacturer(board.ManufacturerName);
        Model = board == null ? MotherboardModel.Unknown : IdentificationHelper.GetModel(board.ProductName);

        // Get motherboard name
        if (board != null)
        {
            _name = !string.IsNullOrEmpty(smBios.Board.ProductName)
                ? Manufacturer == Manufacturer.Unknown
                    ? board.ProductName
                    : $"{Manufacturer} {board.ProductName}"
                : Manufacturer.ToString();
        }
        else
        {
            _name = nameof(Manufacturer.Unknown);
        }

        _customName = settings.GetValue(new Identifier(Identifier, "name").ToString(), _name);

        // Get SuperIO list
        IReadOnlyList<ISuperIo> superIo;
        if (OperatingSystem.IsUnix)
        {
            _lmSensors = new LMSensors();
            superIo = _lmSensors.SuperIo;
        }
        else
        {
            var lpcIo = new LpcIo(this);
            superIo = lpcIo.SuperIo;
        }

        // Sub hardware
        List<IHardware> subHardwareList = [];

        // there may be more than 1 of the same SuperIO chip
        // group by chip
        foreach (var group in superIo.GroupBy(x => x.Chip))
        {
            // index by group
            foreach ((var io, int i) in group.Select((x, i) => (x, i)))
            {
                subHardwareList.Add(
                    new SuperIoHardware(this, io, Manufacturer, Model, settings, i));
            }
        }

        if (EmbeddedControllerBase.Create(Model, settings) is { } embeddedController)
        {
            subHardwareList.Add(embeddedController);
        }

        SubHardware = subHardwareList.ToArray();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public void Accept(IVisitor visitor)
    {
        if (visitor != null)
        {
            visitor.VisitHardware(this);
        }
        else
        {
            throw new ArgumentNullException(nameof(visitor));
        }
    }

    /// <summary>
    /// Closes <see cref="SubHardware" /> using <see cref="Hardware.Close" />.
    /// </summary>
    public void Close()
    {
        _lmSensors?.Close();
        foreach (IHardware iHardware in SubHardware)
        {
            if (iHardware is not Hardware hardware) continue;
            hardware.Close();
        }
    }

    /// <inheritdoc />
    public void Traverse(IVisitor visitor)
    {
        foreach (IHardware hardware in SubHardware)
        {
            hardware.Accept(visitor);
        }
    }

    /// <summary>
    /// Motherboard itself cannot be updated. Update <see cref="SubHardware" /> instead.
    /// </summary>
    public void Update() { }

    #endregion

    #region Events

    /// <inheritdoc />
    public event SensorEventHandler SensorAdded;

    /// <inheritdoc />
    public event SensorEventHandler SensorRemoved;

    #endregion
}
