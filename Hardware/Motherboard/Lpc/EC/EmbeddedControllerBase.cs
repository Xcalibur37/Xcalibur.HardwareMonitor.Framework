using System;
using System.Collections.Generic;
using System.Linq;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Embedded Controller Base.
/// </summary>
/// <seealso cref="Hardware" />
public abstract class EmbeddedControllerBase : Hardware
{
    #region Fields

    private readonly byte[] _data;
    private readonly ushort[] _registers;
    private readonly List<Sensor> _sensors;

    private readonly IReadOnlyList<EmbeddedControllerSource> _sources;

    private static readonly Dictionary<BoardFamily, Dictionary<EcSensor, EmbeddedControllerSource>> _knownSensors =
        new()
        {
            {
                BoardFamily.Amd400,
                new Dictionary<EcSensor, EmbeddedControllerSource>() // no chipset fans in this generation
                {
                    { EcSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    { EcSensor.TempCpu, new EmbeddedControllerSource("CPU", SensorType.Temperature, 0x003b) },
                    { EcSensor.TempMb, new EmbeddedControllerSource("Motherboard", SensorType.Temperature, 0x003c) },
                    {
                        EcSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { EcSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    {
                        EcSensor.VoltageCpu,
                        new EmbeddedControllerSource("CPU Core", SensorType.Voltage, 0x00a2, 2, factor: 1e-3f)
                    },
                    { EcSensor.FanCpuOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00bc, 2) },
                    { EcSensor.FanVrmHs, new EmbeddedControllerSource("VRM Heat Sink Fan", SensorType.Fan, 0x00b2, 2) },
                    {
                        EcSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water flow", SensorType.Flow, 0x00b4, 2, factor: 1.0f / 42f * 60f)
                    },
                    { EcSensor.CurrCpu, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        EcSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x010d, blank: -40)
                    },
                    {
                        EcSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x010b, blank: -40)
                    }
                }
            },
            {
                BoardFamily.Amd500, new Dictionary<EcSensor, EmbeddedControllerSource>
                {
                    { EcSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    { EcSensor.TempCpu, new EmbeddedControllerSource("CPU", SensorType.Temperature, 0x003b) },
                    { EcSensor.TempMb, new EmbeddedControllerSource("Motherboard", SensorType.Temperature, 0x003c) },
                    {
                        EcSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { EcSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    {
                        EcSensor.VoltageCpu,
                        new EmbeddedControllerSource("CPU Core", SensorType.Voltage, 0x00a2, 2, factor: 1e-3f)
                    },
                    { EcSensor.FanCpuOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    { EcSensor.FanVrmHs, new EmbeddedControllerSource("VRM Heat Sink Fan", SensorType.Fan, 0x00b2, 2) },
                    { EcSensor.FanChipset, new EmbeddedControllerSource("Chipset Fan", SensorType.Fan, 0x00b4, 2) },
                    // TODO: "why 42?" is a silly question, I know, but still, why? On the serious side, it might be 41.6(6)
                    {
                        EcSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water flow", SensorType.Flow, 0x00bc, 2, factor: 1.0f / 42f * 60f)
                    },
                    { EcSensor.CurrCpu, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        EcSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        EcSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    }
                }
            },
            {
                BoardFamily.Amd600, new Dictionary<EcSensor, EmbeddedControllerSource>
                {
                    { EcSensor.FanCpuOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    {
                        EcSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        EcSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    }
                }
            },
            {
                BoardFamily.Intel100, new Dictionary<EcSensor, EmbeddedControllerSource>
                {
                    { EcSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    {
                        EcSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { EcSensor.FanWaterPump, new EmbeddedControllerSource("Water Pump", SensorType.Fan, 0x00bc, 2) },
                    { EcSensor.CurrCpu, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        EcSensor.VoltageCpu,
                        new EmbeddedControllerSource("CPU Core", SensorType.Voltage, 0x00a2, 2, factor: 1e-3f)
                    }
                }
            },
            {
                BoardFamily.Intel300, new Dictionary<EcSensor, EmbeddedControllerSource>
                {
                    { EcSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    { EcSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    {
                        EcSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    {
                        EcSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        EcSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                    { EcSensor.FanCpuOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) }
                }
            },
            {
                BoardFamily.Intel400, new Dictionary<EcSensor, EmbeddedControllerSource>
                {
                    { EcSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    {
                        EcSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { EcSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    { EcSensor.FanCpuOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    {
                        EcSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water Flow", SensorType.Flow, 0x00be, 2, factor: 1.0f / 42f * 60f)
                    }, // todo: need validation for this calculation
                    { EcSensor.CurrCpu, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        EcSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        EcSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                }
            },
            {
                BoardFamily.Intel600, new Dictionary<EcSensor, EmbeddedControllerSource>
                {
                    {
                        EcSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { EcSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    {
                        EcSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        EcSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                    {
                        EcSensor.TempWaterBlockIn,
                        new EmbeddedControllerSource("Water Block In", SensorType.Temperature, 0x0102, blank: -40)
                    },
                    {
                        EcSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water Flow", SensorType.Flow, 0x00be, 2, factor: 1.0f / 42f * 60f)
                    } // todo: need validation for this calculation
                }
            },
            {
                BoardFamily.Intel700, new Dictionary<EcSensor, EmbeddedControllerSource>
                {
                    { EcSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x0033) },
                    { EcSensor.FanCpuOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    {
                        EcSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x0109, blank: -40)
                    },
                    {
                        EcSensor.TempTSensor2,
                        new EmbeddedControllerSource("T Sensor 2", SensorType.Temperature, 0x105, blank: -40)
                    },
                    {
                        EcSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        EcSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                    {
                        EcSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water Flow", SensorType.Flow, 0x00be, 2, factor: 1.0f / 42f * 60f)
                    } // todo: need validation for this calculation
                }
            }
        };

    // If you are updating board information, please consider sharing your changes with the corresponding Linux driver.
    // You can do that at https://github.com/zeule/asus-ec-sensors or contribute directly to Linux HWMON.
    // If you are adding a new board, please share DSDT table for the board at https://github.com/zeule/asus-ec-sensors.
    // https://dortania.github.io/Getting-Started-With-ACPI/Manual/dump.html
    private static readonly BoardInfo[] _boards =
    [
        new(MotherboardModel.PrimeX470Pro,
            BoardFamily.Amd400,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempVrm,
            EcSensor.TempVrm,
            EcSensor.FanCpuOpt,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.PrimeX570Pro,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempVrm,
            EcSensor.TempTSensor,
            EcSensor.FanChipset),
        new(MotherboardModel.ProartX570CreatorWifi,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempVrm,
            EcSensor.TempTSensor,
            EcSensor.FanCpuOpt,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.ProWsX570Ace,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempVrm,
            EcSensor.FanChipset,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(new[] { MotherboardModel.RogCrosshairViiiHero, MotherboardModel.RogCrosshairViiiHeroWifi, MotherboardModel.RogCrosshairViiiFormula },
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanCpuOpt,
            EcSensor.FanChipset,
            EcSensor.FanWaterFlow,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.RogCrosshairX670EExtreme,
            BoardFamily.Amd600,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogCrosshairX670EHero,
            BoardFamily.Amd600,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogCrosshairX670EGene,
            BoardFamily.Amd600,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogStrixX670EEGamingWifi,
            BoardFamily.Amd600,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogStrixX670EFGamingWifi,
            BoardFamily.Amd600,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogCrosshairViiiDarkHero,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanCpuOpt,
            EcSensor.FanWaterFlow,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.RogCrosshairViiiImpact,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.FanChipset,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.RogStrixB550EGaming,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogStrixB550IGaming,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.FanVrmHs,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.RogStrixX570EGaming,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.FanChipset,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.RogStrixX570EGamingWifiIi,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.TempVrm),
        new(MotherboardModel.RogStrixX570FGaming,
            BoardFamily.Amd500,
            EcSensor.TempChipset,
            EcSensor.TempCpu,
            EcSensor.TempMb,
            EcSensor.TempTSensor,
            EcSensor.FanChipset),
        new(MotherboardModel.RogStrixX570IGaming,
            BoardFamily.Amd500,
            EcSensor.TempTSensor,
            EcSensor.FanVrmHs,
            EcSensor.FanChipset,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu,
            EcSensor.TempChipset,
            EcSensor.TempVrm),
        new(MotherboardModel.RogStrixZ390EGaming,
            BoardFamily.Intel300,
            EcSensor.TempVrm,
            EcSensor.TempChipset,
            EcSensor.TempTSensor,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogStrixZ390FGaming,
            BoardFamily.Intel300,
            EcSensor.TempVrm,
            EcSensor.TempChipset,
            EcSensor.TempTSensor,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogStrixZ390IGaming,
            BoardFamily.Intel300,
            EcSensor.TempVrm,
            EcSensor.TempChipset,
            EcSensor.TempTSensor),
        new(MotherboardModel.RogMaximusXiFormula,
            BoardFamily.Intel300,
            EcSensor.TempVrm,
            EcSensor.TempChipset,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.TempTSensor,
            EcSensor.FanCpuOpt),
        new(MotherboardModel.RogMaximusXiiZ490Formula,
            BoardFamily.Intel400,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanWaterFlow),
        new(MotherboardModel.RogStrixZ690AGamingWifiD4,
            BoardFamily.Intel600,
            EcSensor.TempTSensor,
            EcSensor.TempVrm),
        new(MotherboardModel.RogMaximusZ690Hero,
            BoardFamily.Intel600,
            EcSensor.TempTSensor,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanWaterFlow),
        new(MotherboardModel.RogMaximusZ690Formula,
            BoardFamily.Intel600,
            EcSensor.TempTSensor,
            EcSensor.TempVrm,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.TempWaterBlockIn,
            EcSensor.FanWaterFlow),
        new(MotherboardModel.RogMaximusZ690ExtremeGlacial,
            BoardFamily.Intel600,
            EcSensor.TempVrm,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.TempWaterBlockIn,
            EcSensor.FanWaterFlow),
        new(MotherboardModel.RogMaximusZ790Hero,
            BoardFamily.Intel700,
            EcSensor.TempTSensor,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanWaterFlow),
        new(MotherboardModel.RogMaximusZ790DarkHero,
            BoardFamily.Intel700,
            EcSensor.TempVrm,
            EcSensor.FanCpuOpt,
            EcSensor.TempTSensor,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.FanWaterFlow),
        new(MotherboardModel.Z170A,
            BoardFamily.Intel100,
            EcSensor.TempTSensor,
            EcSensor.TempChipset,
            EcSensor.FanWaterPump,
            EcSensor.CurrCpu,
            EcSensor.VoltageCpu),
        new(MotherboardModel.PrimeZ690A,
            BoardFamily.Intel600,
            EcSensor.TempTSensor,
            EcSensor.TempVrm),
        new(MotherboardModel.RogStrixZ790IGamingWifi,
            BoardFamily.Intel700,
            EcSensor.TempTSensor,
            EcSensor.TempTSensor2),
        new(MotherboardModel.RogStrixZ790EGamingWifi,
             BoardFamily.Intel700,
             EcSensor.TempVrm,
             EcSensor.FanCpuOpt,
             EcSensor.TempTSensor,
             EcSensor.TempWaterIn,
             EcSensor.TempWaterOut,
             EcSensor.FanWaterFlow),
        new(MotherboardModel.RogMaximusZ790Formula,
            BoardFamily.Intel700,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut),
        new(MotherboardModel.RogMaximusXiiHeroWifi,
            BoardFamily.Intel400,
            EcSensor.TempTSensor,
            EcSensor.TempChipset,
            EcSensor.TempVrm,
            EcSensor.TempWaterIn,
            EcSensor.TempWaterOut,
            EcSensor.CurrCpu,
            EcSensor.FanCpuOpt,
            EcSensor.FanWaterFlow)
    ];

    #endregion

    #region Properties

    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.EmbeddedController;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddedControllerBase"/> class.
    /// </summary>
    /// <param name="sources">The sources.</param>
    /// <param name="settings">The settings.</param>
    protected EmbeddedControllerBase(IEnumerable<EmbeddedControllerSource> sources, ISettings settings) : 
        base(HardwareConstants.EmbeddedControllerName, 
            new Identifier(
                HardwareConstants.LpcIdentifier, 
                HardwareConstants.EmbeddedControllerIdentifier), 
            settings)
    {
        // sorting by address, which implies sorting by bank, for optimized EC access
        var sourcesList = sources.ToList();
        sourcesList.Sort((left, right) => left.Register.CompareTo(right.Register));
        _sources = sourcesList;
        var indices = new Dictionary<SensorType, int>();
        foreach (SensorType t in Enum.GetValues(typeof(SensorType)))
        {
            indices.Add(t, 0);
        }

        _sensors = [];
        List<ushort> registers = [];
        foreach (var source in _sources)
        {
            int index = indices[source.Type];
            indices[source.Type] = index + 1;
            _sensors.Add(new Sensor(source.Name, index, source.Type, this, settings));
            for (int i = 0; i < source.Size; ++i)
            {
                registers.Add((ushort)(source.Register + i));
            }

            // ReSharper disable once VirtualMemberCallInConstructor
            ActivateSensor(_sensors[^1]);
        }

        _registers = registers.ToArray();
        _data = new byte[_registers.Length];
    }

    #endregion

    #region Methods

    /// <summary>
    /// Creates the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="settings">The settings.</param>
    /// <returns></returns>
    /// <exception cref="MultipleBoardRecordsFoundException"></exception>
    internal static EmbeddedControllerBase Create(MotherboardModel model, ISettings settings)
    {
        var boards = _boards.Where(b => b.Models.Contains(model)).ToList();
        switch (boards.Count)
        {
            case 0:
                return null;
            case > 1:
                throw new MultipleBoardRecordsFoundException(model.ToString());
        }

        BoardInfo board = boards[0];
        IEnumerable<EmbeddedControllerSource> sources = board.Sensors.Select(ecs => _knownSensors[board.Family][ecs]);

        return Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => new WindowsEmbeddedController(sources, settings),
            _ => null
        };
    }

    /// <inheritdoc />
    public override void Update()
    {
        if (!TryUpdateData()) return;
        
        int readRegister = 0;
        for (int si = 0; si < _sensors.Count; ++si)
        {
            int val = _sources[si].Size switch
            {
                1 => _sources[si].Type switch { SensorType.Temperature => unchecked((sbyte)_data[readRegister]), _ => _data[readRegister] },
                2 => unchecked((short)((_data[readRegister] << 8) + _data[readRegister + 1])),
                _ => 0
            };

            readRegister += _sources[si].Size;

            _sensors[si].Value = val != _sources[si].Blank ? val * _sources[si].Factor : null;
        }
    }
    /// <summary>
    /// Acquires the io interface.
    /// </summary>
    /// <returns></returns>
    protected abstract IEmbeddedControllerIo AcquireIoInterface();

    /// <summary>
    /// Tries the update data.
    /// </summary>
    /// <returns></returns>
    private bool TryUpdateData()
    {
        try
        {
            using IEmbeddedControllerIo embeddedControllerIo = AcquireIoInterface();
            embeddedControllerIo.Read(_registers, _data);
            return true;
        }
        catch (IoException)
        {
            return false;
        }
    }

    #endregion
}
