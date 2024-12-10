using System.Collections.Generic;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Fintek;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Ite;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Winbond;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard;

/// <summary>
/// Super I/O Hardware
/// </summary>
/// <seealso cref="Hardware" />
internal sealed class SuperIoHardware : Hardware
{
    #region Fields

    private readonly ISuperIo _superIo;
    private readonly Motherboard _motherboard;

    private readonly List<Sensor> _voltages = [];
    private readonly List<Sensor> _temperatures = [];
    private readonly List<Sensor> _fans = [];
    private readonly List<Sensor> _controls = [];
    
    private readonly SuperIoDelegates.UpdateDelegate _postUpdate;
    private readonly SuperIoDelegates.ReadValueDelegate _readControl;
    private readonly SuperIoDelegates.ReadValueDelegate _readFan;
    private readonly SuperIoDelegates.ReadValueDelegate _readTemperature;
    private readonly SuperIoDelegates.ReadValueDelegate _readVoltage;

    #endregion

    #region Properties

    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.SuperIO;

    /// <inheritdoc />
    public override IHardware Parent => _motherboard;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SuperIoHardware"/> class.
    /// </summary>
    /// <param name="motherboard">The motherboard.</param>
    /// <param name="superIo">The super io.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="model">The model.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="index">The index.</param>
    public SuperIoHardware(Motherboard motherboard, ISuperIo superIo, Manufacturer manufacturer, MotherboardModel model, ISettings settings, int index)
        : base(ChipHelper.GetName(superIo.Chip), new Identifier(HardwareConstants.LpcIdentifier, superIo.Chip.ToString().ToLowerInvariant(), index.ToString()), settings)
    {
        _motherboard = motherboard;
        _superIo = superIo;

        // Get board configuration
        GetBoardSpecificConfiguration(
            superIo,
            manufacturer,
            model,
            out var voltages,
            out var temps,
            out var fans,
            out var controls,
            out _readVoltage,
            out _readTemperature,
            out _readFan,
            out _readControl,
            out _postUpdate);

        // Create sensors
        CreateVoltageSensors(superIo, settings, voltages);
        CreateTemperatureSensors(superIo, settings, temps);
        CreateFanSensors(superIo, settings, fans);
        CreateControlSensors(superIo, settings, controls);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public override void Close()
    {
        foreach (Sensor sensor in _controls)
        {
            // restore all controls back to default
            _superIo.SetControl(sensor.Index, null);
        }

        base.Close();
    }

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override void Update()
    {
        _superIo.Update();

        // Update sensors
        UpdateVoltageSensors();
        UpdateTemperatureSensors();
        UpdateFanSensors();
        UpdateControlSensors();

        _postUpdate();
    }

    /// <summary>
    /// Gets the software value as byte.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    private static byte GetSoftwareValueAsByte(ControlSensor control)
    {
        const float percentToByteRatio = 2.55f;
        float value = control.SoftwareValue * percentToByteRatio;
        return (byte)value;
    }

    /// <summary>
    /// Gets the board specific configuration.
    /// </summary>
    /// <param name="superIo">The super io.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="model">The model.</param>
    /// <param name="voltages">The v.</param>
    /// <param name="temps">The t.</param>
    /// <param name="fans">The f.</param>
    /// <param name="controls">The c.</param>
    /// <param name="readVoltage">The read voltage.</param>
    /// <param name="readTemperature">The read temperature.</param>
    /// <param name="readFan">The read fan.</param>
    /// <param name="readControl">The read control.</param>
    /// <param name="postUpdate">The post update.</param>
    private static void GetBoardSpecificConfiguration
    (
        ISuperIo superIo,
        Manufacturer manufacturer,
        MotherboardModel model,
        out IList<Voltage> voltages,
        out IList<Temperature> temps,
        out IList<Fan> fans,
        out IList<Control> controls,
        out SuperIoDelegates.ReadValueDelegate readVoltage,
        out SuperIoDelegates.ReadValueDelegate readTemperature,
        out SuperIoDelegates.ReadValueDelegate readFan,
        out SuperIoDelegates.ReadValueDelegate readControl,
        out SuperIoDelegates.UpdateDelegate postUpdate)
    {
        readVoltage = index => superIo.Voltages[index];
        readTemperature = index => superIo.Temperatures[index];
        readFan = index => superIo.Fans[index];
        readControl = index => superIo.Controls[index];
        postUpdate = () => { };

        // Lists
        voltages = new List<Voltage>();
        temps = new List<Temperature>();
        fans = new List<Fan>();
        controls = new List<Control>();

        // Get board configuration by chip
        GetBoardConfigurationByChip(
            superIo,
            manufacturer,
            model,
            ref voltages,
            ref temps,
            ref fans,
            ref controls,
            ref readFan,
            ref postUpdate);
    }

    /// <summary>
    /// Gets the board specific configuration: NCT6687D.
    /// </summary>
    /// <param name="superIo">The super io.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="model">The model.</param>
    /// <param name="voltages">The voltages.</param>
    /// <param name="temps">The temps.</param>
    /// <param name="fans">The fans.</param>
    /// <param name="controls">The controls.</param>
    /// <param name="readFan">The read fan.</param>
    /// <param name="postUpdate">The post update.</param>
    private static void GetBoardConfigurationByChip
    (
        ISuperIo superIo,
        Manufacturer manufacturer,
        MotherboardModel model,
        ref IList<Voltage> voltages,
        ref IList<Temperature> temps,
        ref IList<Fan> fans,
        ref IList<Control> controls,
        ref SuperIoDelegates.ReadValueDelegate readFan,
        ref SuperIoDelegates.UpdateDelegate postUpdate)
    {
        Mutex mutex = null;
        switch (superIo.Chip)
        {
            case Chip.IT8705F:
            case Chip.IT8712F:
            case Chip.IT8716F:
            case Chip.IT8718F:
            case Chip.IT8720F:
            case Chip.IT8726F:
                IteConfigurations.GetConfigurationsA(superIo, manufacturer, model, voltages, temps, fans, controls, ref readFan, ref postUpdate, ref mutex);
                break;

            case Chip.IT8613E:
            case Chip.IT8620E:
            case Chip.IT8625E:
            case Chip.IT8628E:
            case Chip.IT8631E:
            case Chip.IT8655E:
            case Chip.IT8665E:
            case Chip.IT8686E:
            case Chip.IT8688E:
            case Chip.IT8689E:
            case Chip.IT8721F:
            case Chip.IT8728F:
            case Chip.IT8771E:
            case Chip.IT8772E:
                IteConfigurations.GetConfigurationsB(superIo, manufacturer, model, voltages, temps, fans, controls);
                break;

            case Chip.IT87952E:
            case Chip.IT8792E:
            case Chip.IT8790E:
                IteConfigurations.GetConfigurationsC(superIo, manufacturer, model, voltages, temps, fans, controls);
                break;

            case Chip.F71858:
                // Voltages
                voltages.Add(new Voltage(SuperIoConstants.Vcc3vVolts, 0, 150, 150));
                voltages.Add(new Voltage(SuperIoConstants.Vsb3vVolts, 1, 150, 150));
                voltages.Add(new Voltage(SuperIoConstants.BatteryVolts, 2, 150, 150));

                // Temps
                DefaultConfigurations.GetTemps(superIo, temps);
                
                // Fans
                DefaultConfigurations.GetFans(superIo, fans);
                break;

            case Chip.F71808E:
            case Chip.F71862:
            case Chip.F71869:
            case Chip.F71869A:
            case Chip.F71882:
            case Chip.F71889AD:
            case Chip.F71889ED:
            case Chip.F71889F:
                FintekConfigurations.GetConfiguration(superIo, manufacturer, model, voltages, temps, fans, controls);
                break;

            case Chip.W83627EHF:
                WinbondConfigurations.GetConfigurationEhf(manufacturer, model, voltages, temps, fans, controls);
                break;

            case Chip.W83627DHG:
            case Chip.W83627DHGP:
            case Chip.W83667HG:
            case Chip.W83667HGB:
                WinbondConfigurations.GetConfigurationHg(manufacturer, model, voltages, temps, fans, controls);
                break;

            case Chip.W83627HF:
                // Voltages, Temps, Fans
                WinbondConfigurations.GetW83627ChipConfiguration(voltages, temps, fans);
                
                // Controls
                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, "1"), 0));
                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, "2"), 1));
                break;

            case Chip.W83627THF:
            case Chip.W83687THF:
                // Voltages, Temps, Fans
                WinbondConfigurations.GetW83627ChipConfiguration(voltages, temps, fans);
                
                // Controls
                controls.Add(new Control(SuperIoConstants.SystemFan, 0));
                controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                controls.Add(new Control(SuperIoConstants.AuxiliaryFan, 2));
                break;

            case Chip.NCT6771F:
            case Chip.NCT6776F:
                NuvotonConfigurations.GetConfigurationF(superIo, manufacturer, model, voltages, temps, fans, controls);
                break;

            case Chip.NCT610XD:
                // Voltages
                voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "0"), 1, true));
                voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 4, true));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 5, true));
                voltages.Add(new Voltage(SuperIoConstants.ReservedVolts, 6, true));
                voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "10"), 9, true));
                
                // Temps
                temps.Add(new Temperature(SuperIoConstants.SystemTemp, 1));
                temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 2));
                temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 3));

                // Fans
                DefaultConfigurations.GetFans(superIo, fans);
                
                // Controls
                DefaultConfigurations.GetControls(superIo, controls);
                break;

            case Chip.NCT6683D:
            case Chip.NCT6779D:
            case Chip.NCT6791D:
            case Chip.NCT6792D:
            case Chip.NCT6792DA:
            case Chip.NCT6793D:
            case Chip.NCT6795D:
                NuvotonConfigurations.GetConfigurationD(superIo, manufacturer, model, voltages, temps, fans, controls);
                break;
            case Chip.NCT6796D:
            case Chip.NCT6796DR:
            case Chip.NCT6797D:
            case Chip.NCT6798D:
            case Chip.NCT6799D:
                NuvotonConfigurations.GetConfiguration9Xd(superIo, manufacturer, model, voltages, temps, fans, controls);
                break;

            case Chip.NCT6686D:
            case Chip.NCT6687D:
                GetBoardSpecificConfigurationNct6687D(manufacturer, model, ref voltages, ref temps, ref fans, ref controls);
                break;

            case Chip.Ipmi:
                Ipmi ipmi = (Ipmi)superIo;

                // Voltages
                foreach (Voltage voltage in ipmi.GetVoltages())
                {
                    voltages.Add(voltage);
                }

                // Temps
                foreach (Temperature temperature in ipmi.GetTemperatures())
                {
                    temps.Add(temperature);
                }

                // Fans
                foreach (Fan fan in ipmi.GetFans())
                {
                    fans.Add(fan);
                }

                // Controls
                foreach (Control control in ipmi.GetControls())
                {
                    controls.Add(control);
                }
                break;

            default:
                GetDefaultConfiguration(superIo, voltages, temps, fans, controls);
                break;
        }
    }

    /// <summary>
    /// Gets the board specific configuration: NCT6687D.
    /// </summary>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="model">The model.</param>
    /// <param name="voltages">The voltages.</param>
    /// <param name="temps">The temps.</param>
    /// <param name="fans">The fans.</param>
    /// <param name="controls">The controls.</param>
    private static void GetBoardSpecificConfigurationNct6687D
    (
        Manufacturer manufacturer,
        MotherboardModel model,
        ref IList<Voltage> voltages,
        ref IList<Temperature> temps,
        ref IList<Fan> fans,
        ref IList<Control> controls)
    {
        switch (manufacturer)
        {
            case Manufacturer.ASRock when model == MotherboardModel.Z790Taichi:
                // Temps
                temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));
                temps.Add(new Temperature(SuperIoConstants.MosTemp, 2));

                // Fans
                fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "1"), 0));
                fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 1));
                fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2));
                fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 3));
                fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 4));
                fans.Add(new Fan(string.Format(SuperIoConstants.MosFanNumber, "4"), 5));

                // Controls
                controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "1"), 0));
                controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 1));
                controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2));
                controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 3));
                controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 4));
                controls.Add(new Control(string.Format(SuperIoConstants.MosFanNumber, "4"), 5));
                break;

            case Manufacturer.MSI when model == MotherboardModel.B550APro:
                // Voltages
                voltages.Add(new Voltage(SuperIoConstants.V120Volts, 0));
                voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                voltages.Add(new Voltage(SuperIoConstants.CpuNorthbridgeSocVolts, 2));
                voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 3, 1, 1));
                voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 4, -1, 2));
                voltages.Add(new Voltage(SuperIoConstants.ChipsetVolts, 5));
                voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 6));
                voltages.Add(new Voltage(SuperIoConstants.V33Volts, 8));
                voltages.Add(new Voltage(SuperIoConstants.V18Volts, 9));
                voltages.Add(new Voltage(SuperIoConstants.CpuVddpVolts, 10));
                voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 11));
                voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 12));

                // Temps
                temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                temps.Add(new Temperature(SuperIoConstants.SystemTemp, 1));
                temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 2));
                temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 3));
                temps.Add(new Temperature(SuperIoConstants.CpuSocketTemp, 4));
                temps.Add(new Temperature(SuperIoConstants.PciEx1Temp, 5));

                // Fans
                fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                fans.Add(new Fan(SuperIoConstants.PumpFan, 1));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "1"), 2));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "2"), 3));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "3"), 4));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "4"), 5));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "5"), 6));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "6"), 7));

                // Controls
                controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                controls.Add(new Control(SuperIoConstants.PumpFan, 1));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "1"), 2));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "2"), 3));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "3"), 4));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "4"), 5));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "5"), 6));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "6"), 7));
                break;

            default:
                // Voltages
                voltages.Add(new Voltage(SuperIoConstants.V120Volts, 0));
                voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 2));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 3));
                voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 4));
                voltages.Add(new Voltage(SuperIoConstants.CpuIoVolts, 5));
                voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 6));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 7));
                voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 8));
                voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                voltages.Add(new Voltage(SuperIoConstants.VrefVolts, 10));
                voltages.Add(new Voltage(SuperIoConstants.VsbVolts, 11));
                voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 12));
                voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 13));

                // Temps
                temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                temps.Add(new Temperature(SuperIoConstants.SystemTemp, 1));
                temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 2));
                temps.Add(new Temperature(SuperIoConstants.PchTemp, 3));
                temps.Add(new Temperature(SuperIoConstants.CpuSocketTemp, 4));
                temps.Add(new Temperature(SuperIoConstants.PciEx1Temp, 5));
                temps.Add(new Temperature(SuperIoConstants.M21Temp, 6));

                // Fans
                fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                fans.Add(new Fan(SuperIoConstants.PumpFan, 1));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "1"), 2));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "2"), 3));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "3"), 4));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "4"), 5));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "5"), 6));
                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "6"), 7));

                // Controls
                controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                controls.Add(new Control(SuperIoConstants.PumpFan, 1));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "1"), 2));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "2"), 3));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "3"), 4));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "4"), 5));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "5"), 6));
                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "6"), 7));
                break;
        }
    }

    /// <summary>
    /// Gets the default configuration.
    /// </summary>
    /// <param name="superIo">The super io.</param>
    /// <param name="voltages">The voltages.</param>
    /// <param name="temps">The temps.</param>
    /// <param name="fans">The fans.</param>
    /// <param name="controls">The controls.</param>
    private static void GetDefaultConfiguration(
        ISuperIo superIo,
        IList<Voltage> voltages,
        IList<Temperature> temps,
        IList<Fan> fans,
        ICollection<Control> controls)
    {
        DefaultConfigurations.GetVoltages(superIo, voltages);
        DefaultConfigurations.GetTemps(superIo, temps);
        DefaultConfigurations.GetFans(superIo, fans);
        DefaultConfigurations.GetControls(superIo, controls);
    }

    #region Sensors

    /// <summary>
    /// Creates the control sensors.
    /// </summary>
    /// <param name="superIo">The super io.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="controls">The controls.</param>
    private void CreateControlSensors(ISuperIo superIo, ISettings settings, IList<Control> controls)
    {
        foreach (var ctrl in controls)
        {
            int index = ctrl.Index;
            if (index >= superIo.Controls.Length) continue;
            var sensor = new Sensor(ctrl.Name, index, SensorType.Control, this, settings);
            var control = new ControlSensor(sensor, settings, 0, 100);
            control.ControlModeChanged += cc =>
            {
                switch (cc.ControlMode)
                {
                    case ControlSensorMode.Default:
                        superIo.SetControl(index, null);
                        break;
                    case ControlSensorMode.Software:
                        superIo.SetControl(index, GetSoftwareValueAsByte(cc));
                        break;
                    case ControlSensorMode.Undefined:
                    default:
                        return;
                }
            };

            control.SoftwareControlValueChanged += cc =>
            {
                if (cc.ControlMode != ControlSensorMode.Software) return;
                superIo.SetControl(index, GetSoftwareValueAsByte(cc));
            };

            switch (control.ControlMode)
            {
                case ControlSensorMode.Default:
                    superIo.SetControl(index, null);
                    break;
                case ControlSensorMode.Software:
                    superIo.SetControl(index, GetSoftwareValueAsByte(control));
                    break;
                case ControlSensorMode.Undefined:
                default:
                    break;
            }

            sensor.Control = control;
            _controls.Add(sensor);
            ActivateSensor(sensor);
        }
    }

    /// <summary>
    /// Updates the control sensors.
    /// </summary>
    private void UpdateControlSensors()
    {
        foreach (var sensor in _controls)
        {
            sensor.Value = _readControl(sensor.Index);
        }
    }

    /// <summary>
    /// Creates the fan sensors.
    /// </summary>
    /// <param name="superIo">The super io.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="fans">The fans.</param>
    private void CreateFanSensors(ISuperIo superIo, ISettings settings, IList<Fan> fans)
    {
        foreach (var fan in fans)
        {
            if (fan.Index >= superIo.Fans.Length) continue;
            Sensor sensor = new(fan.Name, fan.Index, SensorType.Fan, this, settings);
            _fans.Add(sensor);
        }
    }

    /// <summary>
    /// Updates the fan sensors.
    /// </summary>
    private void UpdateFanSensors()
    {
        foreach (var sensor in _fans)
        {
            float? value = _readFan(sensor.Index);
            if (!value.HasValue) continue;
            sensor.Value = value;
            ActivateSensor(sensor);
        }
    }

    /// <summary>
    /// Creates the temperature sensors.
    /// </summary>
    /// <param name="superIo">The super io.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="temperatures">The temperatures.</param>
    private void CreateTemperatureSensors(ISuperIo superIo, ISettings settings, IList<Temperature> temperatures)
    {
        foreach (Temperature temperature in temperatures)
        {
            if (temperature.Index >= superIo.Temperatures.Length) continue;
            Sensor sensor = new(temperature.Name,
                temperature.Index,
                SensorType.Temperature,
                this,
                [new ParameterDescription("Offset [°C]", "Temperature offset.", 0)],
                settings);
            _temperatures.Add(sensor);
        }
    }

    /// <summary>
    /// Updates the temperature sensors.
    /// </summary>
    private void UpdateTemperatureSensors()
    {
        foreach (var sensor in _temperatures)
        {
            float? value = _readTemperature(sensor.Index);
            if (!value.HasValue) continue;
            sensor.Value = value + sensor.Parameters[0].Value;
            ActivateSensor(sensor);
        }
    }

    /// <summary>
    /// Creates the voltage sensors.
    /// </summary>
    /// <param name="superIo">The super io.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="v">The v.</param>
    private void CreateVoltageSensors(ISuperIo superIo, ISettings settings, IList<Voltage> v)
    {
        const string formula = "Voltage = value + (value - Vf) * Ri / Rf.";
        foreach (Voltage voltage in v)
        {
            if (voltage.Index >= superIo.Voltages.Length) continue;
            Sensor sensor = new(voltage.Name,
                voltage.Index,
                voltage.IsHidden,
                SensorType.Voltage,
                this,
                [
                    new ParameterDescription("Ri [kΩ]", "Input resistance.\n" + formula, voltage.Ri),
                    new ParameterDescription("Rf [kΩ]", "Reference resistance.\n" + formula, voltage.Rf),
                    new ParameterDescription("Vf [V]", "Reference voltage.\n" + formula, voltage.Vf)
                ],
                settings);

            _voltages.Add(sensor);
        }
    }

    /// <summary>
    /// Updates the voltage sensors.
    /// </summary>
    private void UpdateVoltageSensors()
    {
        foreach (var sensor in _voltages)
        {
            float? value = _readVoltage(sensor.Index);
            if (!value.HasValue) continue;
            sensor.Value = value + ((value - sensor.Parameters[2].Value) * sensor.Parameters[0].Value / sensor.Parameters[1].Value);
            ActivateSensor(sensor);
        }
    }

    #endregion

    #endregion
}
