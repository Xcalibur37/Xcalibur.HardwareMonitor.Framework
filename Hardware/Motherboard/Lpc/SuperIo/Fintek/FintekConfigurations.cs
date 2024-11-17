using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;
using Control = Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models.Control;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Fintek
{
    /// <summary>
    /// Fintek Configurations
    /// </summary>
    internal class FintekConfigurations
    {
        /// <summary>
        /// Gets the Fintek configuration.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfiguration(
            ISuperIo superIo,
            Manufacturer manufacturer,
            MotherboardModel model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Models.Control> controls)
        {
            switch (manufacturer)
            {
                case Manufacturer.EVGA:
                    voltages.Add(new Voltage(SuperIoConstants.Vcc3vVolts, 0, 150, 150));
                    switch (model)
                    {
                        case MotherboardModel.X58_SLI_Classified: // F71882
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 1, 47, 100));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 2, 47, 100));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 3, 24, 100));
                            voltages.Add(new Voltage(SuperIoConstants.IohVcoreVolts, 4, 24, 100));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 51, 12));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 6, 56, 6.8f));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 150, 150));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 150, 150));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.VregTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 1));
                            fans.Add(new Fan(SuperIoConstants.ChassisFan, 2));
                            break;

                        case MotherboardModel.X58_3X_SLI: // F71882
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 1, 47, 100));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 2, 47, 100));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 3, 24, 100));
                            voltages.Add(new Voltage(SuperIoConstants.IohVcoreVolts, 4, 24, 100));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 51, 12));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 6, 56, 6.8f));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 150, 150));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 150, 150));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.VregTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 1));
                            fans.Add(new Fan(SuperIoConstants.ChassisFan, 2));
                            fans.Add(new Fan(SuperIoConstants.ChipsetFan, 3));

                            // Controls
                            controls.Add(new Models.Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Models.Control(SuperIoConstants.PowerFan, 1));
                            controls.Add(new Models.Control(SuperIoConstants.ChassisFan, 2));
                            controls.Add(new Models.Control(SuperIoConstants.ChipsetFan, 3));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.Vsb3vVolts, 7, 150, 150));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 150, 150));

                            // Temps
                            for (int i = 0; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, i + 1), i));
                            }

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                case Manufacturer.MSI:
                    switch (model)
                    {
                        case MotherboardModel.Z77_MS7751: // F71889AD
                        case MotherboardModel.Z68_MS7672: // F71889AD
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.Vcc3vVolts, 0, 150, 150));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 20, 4.7f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 68, 6.8f));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 5, 150, 150));
                            voltages.Add(new Voltage(SuperIoConstants.CpuIoVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 7, 150, 150));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 150, 150));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.ProbeTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            for (int i = 1; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i), i));
                            }

                            // Controls
                            controls.Add(new Models.Control(SuperIoConstants.CpuFan, 0));
                            for (int i = 1; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Models.Control(string.Format(SuperIoConstants.FanControlNumber, i), i));
                            }
                            break;

                        default:
                            GetConfigurationDefaults(superIo, voltages, temps, fans, controls);
                            break;
                    }

                    break;
                default:
                    GetConfigurationDefaults(superIo, voltages, temps, fans, controls);
                    break;
            }
        }

        /// <summary>
        /// Gets the configuration defaults.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        private static void GetConfigurationDefaults(
            ISuperIo superIo,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Models.Control> controls)
        {
            // Voltages
            voltages.Add(new Voltage(SuperIoConstants.Vcc3vVolts, 0, 150, 150));
            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 1));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "3"), 2, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "4"), 3, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
            if (superIo.Chip != Chip.F71808E)
            {
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
            }
            voltages.Add(new Voltage(SuperIoConstants.Vsb3vVolts, 7, 150, 150));
            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 150, 150));

            // Temps
            DefaultConfigurations.GetTemps(superIo, temps);

            // Fans
            DefaultConfigurations.GetFans(superIo, fans);

            // Controls
            DefaultConfigurations.GetControls(superIo, controls);
        }
    }
}
