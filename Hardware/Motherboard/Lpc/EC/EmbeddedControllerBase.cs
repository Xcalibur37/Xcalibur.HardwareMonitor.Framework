using System;
using System.Collections.Generic;
using System.Linq;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

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

    private static readonly Dictionary<BoardFamily, Dictionary<ECSensor, EmbeddedControllerSource>> _knownSensors =
        new()
        {
            {
                BoardFamily.Amd400,
                new Dictionary<ECSensor, EmbeddedControllerSource>() // no chipset fans in this generation
                {
                    { ECSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    { ECSensor.TempCPU, new EmbeddedControllerSource("CPU", SensorType.Temperature, 0x003b) },
                    { ECSensor.TempMB, new EmbeddedControllerSource("Motherboard", SensorType.Temperature, 0x003c) },
                    {
                        ECSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { ECSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    {
                        ECSensor.VoltageCPU,
                        new EmbeddedControllerSource("CPU Core", SensorType.Voltage, 0x00a2, 2, factor: 1e-3f)
                    },
                    { ECSensor.FanCPUOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00bc, 2) },
                    { ECSensor.FanVrmHS, new EmbeddedControllerSource("VRM Heat Sink Fan", SensorType.Fan, 0x00b2, 2) },
                    {
                        ECSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water flow", SensorType.Flow, 0x00b4, 2, factor: 1.0f / 42f * 60f)
                    },
                    { ECSensor.CurrCPU, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        ECSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x010d, blank: -40)
                    },
                    {
                        ECSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x010b, blank: -40)
                    }
                }
            },
            {
                BoardFamily.Amd500, new Dictionary<ECSensor, EmbeddedControllerSource>
                {
                    { ECSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    { ECSensor.TempCPU, new EmbeddedControllerSource("CPU", SensorType.Temperature, 0x003b) },
                    { ECSensor.TempMB, new EmbeddedControllerSource("Motherboard", SensorType.Temperature, 0x003c) },
                    {
                        ECSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { ECSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    {
                        ECSensor.VoltageCPU,
                        new EmbeddedControllerSource("CPU Core", SensorType.Voltage, 0x00a2, 2, factor: 1e-3f)
                    },
                    { ECSensor.FanCPUOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    { ECSensor.FanVrmHS, new EmbeddedControllerSource("VRM Heat Sink Fan", SensorType.Fan, 0x00b2, 2) },
                    { ECSensor.FanChipset, new EmbeddedControllerSource("Chipset Fan", SensorType.Fan, 0x00b4, 2) },
                    // TODO: "why 42?" is a silly question, I know, but still, why? On the serious side, it might be 41.6(6)
                    {
                        ECSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water flow", SensorType.Flow, 0x00bc, 2, factor: 1.0f / 42f * 60f)
                    },
                    { ECSensor.CurrCPU, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        ECSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        ECSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    }
                }
            },
            {
                BoardFamily.Amd600, new Dictionary<ECSensor, EmbeddedControllerSource>
                {
                    { ECSensor.FanCPUOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    {
                        ECSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        ECSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    }
                }
            },
            {
                BoardFamily.Intel100, new Dictionary<ECSensor, EmbeddedControllerSource>
                {
                    { ECSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    {
                        ECSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { ECSensor.FanWaterPump, new EmbeddedControllerSource("Water Pump", SensorType.Fan, 0x00bc, 2) },
                    { ECSensor.CurrCPU, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        ECSensor.VoltageCPU,
                        new EmbeddedControllerSource("CPU Core", SensorType.Voltage, 0x00a2, 2, factor: 1e-3f)
                    }
                }
            },
            {
                BoardFamily.Intel300, new Dictionary<ECSensor, EmbeddedControllerSource>
                {
                    { ECSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    { ECSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    {
                        ECSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    {
                        ECSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        ECSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                    { ECSensor.FanCPUOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) }
                }
            },
            {
                BoardFamily.Intel400, new Dictionary<ECSensor, EmbeddedControllerSource>
                {
                    { ECSensor.TempChipset, new EmbeddedControllerSource("Chipset", SensorType.Temperature, 0x003a) },
                    {
                        ECSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { ECSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    { ECSensor.FanCPUOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    {
                        ECSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water Flow", SensorType.Flow, 0x00be, 2, factor: 1.0f / 42f * 60f)
                    }, // todo: need validation for this calculation
                    { ECSensor.CurrCPU, new EmbeddedControllerSource("CPU", SensorType.Current, 0x00f4) },
                    {
                        ECSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        ECSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                }
            },
            {
                BoardFamily.Intel600, new Dictionary<ECSensor, EmbeddedControllerSource>
                {
                    {
                        ECSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x003d, blank: -40)
                    },
                    { ECSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x003e) },
                    {
                        ECSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        ECSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                    {
                        ECSensor.TempWaterBlockIn,
                        new EmbeddedControllerSource("Water Block In", SensorType.Temperature, 0x0102, blank: -40)
                    },
                    {
                        ECSensor.FanWaterFlow,
                        new EmbeddedControllerSource("Water Flow", SensorType.Flow, 0x00be, 2, factor: 1.0f / 42f * 60f)
                    } // todo: need validation for this calculation
                }
            },
            {
                BoardFamily.Intel700, new Dictionary<ECSensor, EmbeddedControllerSource>
                {
                    { ECSensor.TempVrm, new EmbeddedControllerSource("VRM", SensorType.Temperature, 0x0033) },
                    { ECSensor.FanCPUOpt, new EmbeddedControllerSource("CPU Optional Fan", SensorType.Fan, 0x00b0, 2) },
                    {
                        ECSensor.TempTSensor,
                        new EmbeddedControllerSource("T Sensor", SensorType.Temperature, 0x0109, blank: -40)
                    },
                    {
                        ECSensor.TempTSensor2,
                        new EmbeddedControllerSource("T Sensor 2", SensorType.Temperature, 0x105, blank: -40)
                    },
                    {
                        ECSensor.TempWaterIn,
                        new EmbeddedControllerSource("Water In", SensorType.Temperature, 0x0100, blank: -40)
                    },
                    {
                        ECSensor.TempWaterOut,
                        new EmbeddedControllerSource("Water Out", SensorType.Temperature, 0x0101, blank: -40)
                    },
                    {
                        ECSensor.FanWaterFlow,
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
        new(MotherboardModel.PRIME_X470_PRO,
            BoardFamily.Amd400,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempVrm,
            ECSensor.TempVrm,
            ECSensor.FanCPUOpt,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.PRIME_X570_PRO,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempVrm,
            ECSensor.TempTSensor,
            ECSensor.FanChipset),
        new(MotherboardModel.PROART_X570_CREATOR_WIFI,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempVrm,
            ECSensor.TempTSensor,
            ECSensor.FanCPUOpt,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.PRO_WS_X570_ACE,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempVrm,
            ECSensor.FanChipset,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(new[] { MotherboardModel.ROG_CROSSHAIR_VIII_HERO, MotherboardModel.ROG_CROSSHAIR_VIII_HERO_WIFI, MotherboardModel.ROG_CROSSHAIR_VIII_FORMULA },
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanCPUOpt,
            ECSensor.FanChipset,
            ECSensor.FanWaterFlow,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.ROG_CROSSHAIR_X670E_EXTREME,
            BoardFamily.Amd600,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_CROSSHAIR_X670E_HERO,
            BoardFamily.Amd600,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_CROSSHAIR_X670E_GENE,
            BoardFamily.Amd600,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_STRIX_X670E_E_GAMING_WIFI,
            BoardFamily.Amd600,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_STRIX_X670E_F_GAMING_WIFI,
            BoardFamily.Amd600,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_CROSSHAIR_VIII_DARK_HERO,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanCPUOpt,
            ECSensor.FanWaterFlow,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.ROG_CROSSHAIR_VIII_IMPACT,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.FanChipset,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.ROG_STRIX_B550_E_GAMING,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_STRIX_B550_I_GAMING,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.FanVrmHS,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.ROG_STRIX_X570_E_GAMING,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.FanChipset,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.ROG_STRIX_X570_E_GAMING_WIFI_II,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.TempVrm),
        new(MotherboardModel.ROG_STRIX_X570_F_GAMING,
            BoardFamily.Amd500,
            ECSensor.TempChipset,
            ECSensor.TempCPU,
            ECSensor.TempMB,
            ECSensor.TempTSensor,
            ECSensor.FanChipset),
        new(MotherboardModel.ROG_STRIX_X570_I_GAMING,
            BoardFamily.Amd500,
            ECSensor.TempTSensor,
            ECSensor.FanVrmHS,
            ECSensor.FanChipset,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU,
            ECSensor.TempChipset,
            ECSensor.TempVrm),
        new(MotherboardModel.ROG_STRIX_Z390_E_GAMING,
            BoardFamily.Intel300,
            ECSensor.TempVrm,
            ECSensor.TempChipset,
            ECSensor.TempTSensor,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_STRIX_Z390_F_GAMING,
            BoardFamily.Intel300,
            ECSensor.TempVrm,
            ECSensor.TempChipset,
            ECSensor.TempTSensor,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_STRIX_Z390_I_GAMING,
            BoardFamily.Intel300,
            ECSensor.TempVrm,
            ECSensor.TempChipset,
            ECSensor.TempTSensor),
        new(MotherboardModel.ROG_MAXIMUS_XI_FORMULA,
            BoardFamily.Intel300,
            ECSensor.TempVrm,
            ECSensor.TempChipset,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.TempTSensor,
            ECSensor.FanCPUOpt),
        new(MotherboardModel.ROG_MAXIMUS_XII_Z490_FORMULA,
            BoardFamily.Intel400,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanWaterFlow),
        new(MotherboardModel.ROG_STRIX_Z690_A_GAMING_WIFI_D4,
            BoardFamily.Intel600,
            ECSensor.TempTSensor,
            ECSensor.TempVrm),
        new(MotherboardModel.ROG_MAXIMUS_Z690_HERO,
            BoardFamily.Intel600,
            ECSensor.TempTSensor,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanWaterFlow),
        new(MotherboardModel.ROG_MAXIMUS_Z690_FORMULA,
            BoardFamily.Intel600,
            ECSensor.TempTSensor,
            ECSensor.TempVrm,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.TempWaterBlockIn,
            ECSensor.FanWaterFlow),
        new(MotherboardModel.ROG_MAXIMUS_Z690_EXTREME_GLACIAL,
            BoardFamily.Intel600,
            ECSensor.TempVrm,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.TempWaterBlockIn,
            ECSensor.FanWaterFlow),
        new(MotherboardModel.ROG_MAXIMUS_Z790_HERO,
            BoardFamily.Intel700,
            ECSensor.TempTSensor,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanWaterFlow),
        new(MotherboardModel.ROG_MAXIMUS_Z790_DARK_HERO,
            BoardFamily.Intel700,
            ECSensor.TempVrm,
            ECSensor.FanCPUOpt,
            ECSensor.TempTSensor,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.FanWaterFlow),
        new(MotherboardModel.Z170_A,
            BoardFamily.Intel100,
            ECSensor.TempTSensor,
            ECSensor.TempChipset,
            ECSensor.FanWaterPump,
            ECSensor.CurrCPU,
            ECSensor.VoltageCPU),
        new(MotherboardModel.PRIME_Z690_A,
            BoardFamily.Intel600,
            ECSensor.TempTSensor,
            ECSensor.TempVrm),
        new(MotherboardModel.ROG_STRIX_Z790_I_GAMING_WIFI,
            BoardFamily.Intel700,
            ECSensor.TempTSensor,
            ECSensor.TempTSensor2),
        new(MotherboardModel.ROG_STRIX_Z790_E_GAMING_WIFI,
             BoardFamily.Intel700,
             ECSensor.TempVrm,
             ECSensor.FanCPUOpt,
             ECSensor.TempTSensor,
             ECSensor.TempWaterIn,
             ECSensor.TempWaterOut,
             ECSensor.FanWaterFlow),
        new(MotherboardModel.ROG_MAXIMUS_Z790_FORMULA,
            BoardFamily.Intel700,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut),
        new(MotherboardModel.ROG_MAXIMUS_XII_HERO_WIFI,
            BoardFamily.Intel400,
            ECSensor.TempTSensor,
            ECSensor.TempChipset,
            ECSensor.TempVrm,
            ECSensor.TempWaterIn,
            ECSensor.TempWaterOut,
            ECSensor.CurrCPU,
            ECSensor.FanCPUOpt,
            ECSensor.FanWaterFlow)
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
    protected EmbeddedControllerBase(IEnumerable<EmbeddedControllerSource> sources, ISettings settings) : base("Embedded Controller", new Identifier("lpc", "ec"), settings)
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

        _sensors = new List<Sensor>();
        List<ushort> registers = new();
        foreach (EmbeddedControllerSource s in _sources)
        {
            int index = indices[s.Type];
            indices[s.Type] = index + 1;
            _sensors.Add(new Sensor(s.Name, index, s.Type, this, settings));
            for (int i = 0; i < s.Size; ++i)
            {
                registers.Add((ushort)(s.Register + i));
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
    /// <exception cref="Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions.MultipleBoardRecordsFoundException"></exception>
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
        if (!TryUpdateData())
        {
            // just skip this update cycle?
            return;
        }

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
