using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Winbond
{
    internal class WinbondConfigurations
    {
        /// <summary>
        /// Gets the Winbond configuration: EHF.
        /// </summary>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfigurationEhf(
            Manufacturer manufacturer,
            MotherboardModel model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Models.Control> controls)
        {
            // Vcore
            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));

            // ASRock and W83627EHF
            if (manufacturer == Manufacturer.ASRock && model == MotherboardModel.AOD790GX_128M)
            {
                // Voltages
                voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                voltages.Add(new Voltage(SuperIoConstants.V33Volts, 4, 10, 10));
                voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 20, 10));
                voltages.Add(new Voltage(SuperIoConstants.V120Volts, 6, 28, 5));
                voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                // Temps
                temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));

                // Fans
                fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                fans.Add(new Fan(SuperIoConstants.ChassisFan, 1));
            }
            else
            {
                // Voltages
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
                voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "10"), 9, true));

                // Temps
                temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 1));
                temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

                // Fans
                fans.Add(new Fan(SuperIoConstants.SystemFan, 0));
                fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                fans.Add(new Fan(SuperIoConstants.AuxiliaryFan, 2));
                fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 3));
                fans.Add(new Fan(string.Format(SuperIoConstants.AuxiliaryFanNumber, "2"), 4));
            }

            // Controls
            controls.Add(new Models.Control(SuperIoConstants.SystemFan, 0));
            controls.Add(new Models.Control(SuperIoConstants.CpuFan, 1));
            controls.Add(new Models.Control(SuperIoConstants.AuxiliaryFan, 2));
        }

        /// <summary>
        /// Gets the W83627 chip configuration.
        /// </summary>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        internal static void GetW83627ChipConfiguration(
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans)
        {
            // Voltages
            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "3"), 2, true));
            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 3, 34, 51));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 5, 34, 51));
            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 6));

            // Temps
            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 1));
            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

            // Fans
            fans.Add(new Fan(SuperIoConstants.SystemFan, 0));
            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
            fans.Add(new Fan(SuperIoConstants.AuxiliaryFan, 2));
        }

        /// <summary>
        /// Gets the Winbond configuration: HG.
        /// </summary>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfigurationHg(
            Manufacturer manufacturer,
            MotherboardModel model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Models.Control> controls)
        {
            switch (manufacturer)
            {
                case Manufacturer.ASRock:
                    voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                    switch (model)
                    {
                        case MotherboardModel._880GMH_USB3: // W83627DHG-P
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 15, 7.5f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 6, 56, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.ChassisFan, 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.AuxiliaryFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.AuxiliaryFanNumber, "2"), 4));
                            break;
                    }

                    // Controls
                    controls.Add(new Models.Control(SuperIoConstants.SystemFan, 0));
                    controls.Add(new Models.Control(SuperIoConstants.CpuFan, 1));
                    controls.Add(new Models.Control(SuperIoConstants.AuxiliaryFan, 2));
                    break;

                case Manufacturer.ASUS:
                    switch (model)
                    {
                        case MotherboardModel.P6T: // W83667HG
                        case MotherboardModel.P6X58D_E: // W83667HG
                        case MotherboardModel.RAMPAGE_II_GENE: // W83667HG
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 11.5f, 1.91f));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 4, 15, 7.5f));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 4));

                            // Controls
                            controls.Add(new Models.Control(SuperIoConstants.SystemFan, 0));
                            controls.Add(new Models.Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Models.Control(SuperIoConstants.AuxiliaryFan, 2));
                            break;

                        case MotherboardModel.RAMPAGE_EXTREME: // W83667HG
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 12, 2));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 4, 15, 7.5f));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 4));

                            // Controls
                            controls.Add(new Models.Control(SuperIoConstants.SystemFan, 0));
                            controls.Add(new Models.Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Models.Control(SuperIoConstants.AuxiliaryFan, 2));
                            break;

                        default:
                            GetConfigurationHgDefaults(voltages, temps, fans, controls);
                            break;
                    }
                    break;

                default:
                    GetConfigurationHgDefaults(voltages, temps, fans, controls);
                    break;
            }
        }

        /// <summary>
        /// Gets the Winbond configuration: HG - Defaults.
        /// </summary>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        private static void GetConfigurationHgDefaults(
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Models.Control> controls)
        {
            // Voltages
            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

            // Temps
            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 1));
            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

            // Fans
            fans.Add(new Fan(SuperIoConstants.SystemFan, 0));
            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
            fans.Add(new Fan(SuperIoConstants.AuxiliaryFan, 2));
            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 3));
            fans.Add(new Fan(string.Format(SuperIoConstants.AuxiliaryFanNumber, "2"), 4));

            // Controls
            controls.Add(new Models.Control(SuperIoConstants.SystemFan, 0));
            controls.Add(new Models.Control(SuperIoConstants.CpuFan, 1));
            controls.Add(new Models.Control(SuperIoConstants.AuxiliaryFan, 2));
        }
    }
}
