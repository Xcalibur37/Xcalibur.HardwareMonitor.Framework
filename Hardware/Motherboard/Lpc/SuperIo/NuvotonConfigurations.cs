using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo
{
    /// <summary>
    /// Nuvoton Configurations
    /// </summary>
    internal static class NuvotonConfigurations
    {
        /// <summary>
        /// Gets Nuvoton configuration: F
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfigurationF(
            ISuperIo superIo,
            Manufacturer manufacturer,
            Model model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls)
        {
            switch (manufacturer)
            {
                case Manufacturer.ASUS:
                    voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                    switch (model)
                    {
                        case Model.P8P67: // NCT6776F
                        case Model.P8P67_EVO: // NCT6776F
                        case Model.P8P67_PRO: // NCT6776F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 11, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 4, 12, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 3));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 3));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 0));
                            controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 2));
                            break;

                        case Model.P8P67_M_PRO: // NCT6776F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 11, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 4, 12, 3));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 3));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 3));
                            fans.Add(new Fan(SuperIoConstants.AuxiliaryFan, 4));
                            break;

                        case Model.P8Z68_V_PRO: // NCT6776F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 11, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 4, 12, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 3));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        case Model.P9X79: // NCT6776F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 11, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 4, 12, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 3));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
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
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;
                    }
                    break;

                case Manufacturer.ASRock:
                    switch (model)
                    {
                        case Model.H61M_DGS: // NCT6776F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 28, 5));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, 0, 1, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 2, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, 0, 1, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 3));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.ChassisFan, 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.ChassisFan, 0));
                            controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                            break;

                        case Model.B85M_DGS:
                            {
                                // Voltages
                                voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 1, 1));
                                voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 56, 10));
                                voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                                voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                                voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, "1"), 4, true));
                                voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 12, 3));
                                voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, "3"), 6, true));
                                voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));

                                // Temps
                                temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                                temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 2));
                                temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 3));

                                // Fans
                                fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                                fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                                fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                                fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 3));

                                // Controls
                                controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 0));
                                controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                                controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 2));
                            }
                            break;

                        case Model.Z77Pro4M: // NCT6776F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 56, 10));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 20, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 3));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        default:
                            GetConfigurationFDefaults(superIo, voltages, temps, fans, controls);
                            break;
                    }
                    break;

                default:
                    GetConfigurationFDefaults(superIo, voltages, temps, fans, controls);
                    break;
            }
        }

        /// <summary>
        /// Gets the Nuvoton configuration: D.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfigurationD(
            ISuperIo superIo,
            Manufacturer manufacturer,
            Model model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls)
        {
            switch (manufacturer)
            {
                case Manufacturer.ASRock:
                    voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 10, 10));
                    switch (model)
                    {
                        case Model.A320M_HDV: // NCT6779D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.Chipset105Volts, 1, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 56, 10));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreRefVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 12, 20, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 5));
                            break;

                        case Model.AB350_Pro4: // NCT6779D
                        case Model.AB350M_Pro4:
                        case Model.AB350M:
                        case Model.Fatal1ty_AB350_Gaming_K4:
                        case Model.AB350M_HDV:
                        case Model.B450_Steel_Legend:
                        case Model.B450M_Steel_Legend:
                        case Model.B450_Pro4:
                        case Model.B450M_Pro4:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 28, 5));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreRefinVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset105Volts, 11, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 12, 20, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V18Volts, 14, 0, 1));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTinTempNumber, "2"), 5));
                            break;

                        case Model.X399_Phantom_Gaming_6: // NCT6779D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.Chipset105Volts, 1, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 56, 10));
                            voltages.Add(new Voltage(SuperIoConstants.VddcrSocVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 12, 20, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V18Volts, 13, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 3));
                            temps.Add(new Temperature(SuperIoConstants.CoreVrmTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.CoreSocTemp, 5));
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
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            break;
                    }

                    // Fans
                    DefaultConfigurations.GetFans(superIo, fans);

                    // Controls
                    DefaultConfigurations.GetControls(superIo, controls);
                    break;

                case Manufacturer.ASUS:
                    string[] fanControlNames;
                    switch (model)
                    {
                        case Model.P8Z77_V: // NCT6779D
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
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"), 
                                SuperIoConstants.CpuFan, 
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"), 
                                string.Format(SuperIoConstants.ChassisFanNumber, "3")
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_MAXIMUS_X_APEX: // NCT6793D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 2, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(SuperIoConstants.CpuGraphicsVolts, 6, 2, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 10, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 11));
                            voltages.Add(new Voltage(SuperIoConstants.PchCoreVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.PhaseLockedLoopVolts, 13));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccioImcVolts, 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPeciTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.T2Temp, 1));
                            temps.Add(new Temperature(SuperIoConstants.T1Temp, 2));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 3));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 6));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.Z170_A: // NCT6793D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 2, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, 0, 1, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.CpuGraphicsVolts, 6, 2, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 10, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 11));
                            voltages.Add(new Voltage(SuperIoConstants.PchCoreVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.PhaseLockedLoopVolts, 13));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccioImcVolts, 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPeciTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 3));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 6));

                            // CPU Fan Optional uses the same fan control as CPU Fan.
                            // Water Pump speed can only be read from the EC.
                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "4"),
                                SuperIoConstants.WaterPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.B150M_C: // NCT6791D
                        case Model.B150M_C_D3: // NCT6791D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 2, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.PchVolts, 9));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPeciTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "1"), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "2"), 2));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "1"), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "2"), 2));
                            break;

                        case Model.ROG_MAXIMUS_X_HERO_WIFI_AC: // NCT6793D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 2, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(SuperIoConstants.CpuGraphicsVolts, 6, 2, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 10, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 11));
                            voltages.Add(new Voltage(SuperIoConstants.PchCoreVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.PhaseLockedLoopVolts, 13));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccioImcVolts, 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPeciTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.T2Temp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 6));

                            // note: CPU_Opt, W_Pump+, EXT_FAN 1 & 2 are on the ASUS EC controller.
                            // Together with VRM og PCH temperatures. And additional voltages and power
                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                SuperIoConstants.AioPumpFan,
                                SuperIoConstants.HighAmpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        default:
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
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;
                    }
                    break;

                case Manufacturer.MSI:
                    voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                    switch (model)
                    {
                        case Model.Z270_PC_MATE: // NCT6795D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(SuperIoConstants.CpuIoVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(SuperIoConstants.PchVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 13, 1, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));
                            
                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            
                            fanControlNames = [
                                SuperIoConstants.PumpFan,
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.SystemFanNumber, "1"),
                                string.Format(SuperIoConstants.SystemFanNumber, "2"),
                                string.Format(SuperIoConstants.SystemFanNumber, "3"),
                                string.Format(SuperIoConstants.SystemFanNumber, "4")
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
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
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));
                            
                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;
                    }
                    break;

                default:
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
                    voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));
                    
                    // Temps
                    temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                    temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
                    temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
                    temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                    temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                    temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                    temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));

                    // Fans
                    DefaultConfigurations.GetFans(superIo, fans);

                    // Controls
                    DefaultConfigurations.GetControls(superIo, controls);
                    break;
            }
        }

        /// <summary>
        /// Gets the Nuvoton configuration: 9XD.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfiguration9Xd(
            ISuperIo superIo,
            Manufacturer manufacturer,
            Model model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls)
        {
            switch (manufacturer)
            {
                case Manufacturer.ASRock:
                    string[] fanControlNames;
                    switch (model)
                    {
                        case Model.X570_Taichi:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 10, 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 8));
                            temps.Add(new Temperature(SuperIoConstants.SouthbridgeTemp, 9));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.CpuFanNumber, "1"),
                                string.Format(SuperIoConstants.CpuFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                SuperIoConstants.SouthbridgeFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "4")
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.X570_Phantom_Gaming_ITX:
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
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.SouthbridgeTemp, 3));

                            fanControlNames = [
                                string.Format(SuperIoConstants.CpuFanNumber, "1"),
                                SuperIoConstants.ChassisFan,
                                string.Format(SuperIoConstants.CpuFanNumber, "2"),
                                SuperIoConstants.ChipsetFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.Z690_Extreme:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 20, 10));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 110, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V33StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Cpu105Volts, 10, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset082Volts, 11, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset100Volts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 13, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 14, 2.35f, 1));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 5));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 6));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1)); // CPU_FAN1
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2)); // CPU_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3)); // CHA_FAN1/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4)); // CHA_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 0)); // CHA_FAN3/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 5)); // CHA_FAN4/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 6)); // CHA_FAN5/WP

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            break;

                        case Model.Z690_Steel_Legend:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 20, 10));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 110, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DramVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V33StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Cpu105Volts, 10, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset082Volts, 11, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset100Volts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 13, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 14, 2.35f, 1));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 5));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 6));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1)); // CPU_FAN1
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2)); // CPU_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3)); // CHA_FAN1/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4)); // CHA_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 0)); // CHA_FAN3/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 5)); // CHA_FAN4/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 6)); // CHA_FAN5/WP

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            break;

                        case Model.Z790_Pro_RS:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 20, 10));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 110, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuImcVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V33StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Cpu105Volts, 10, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset082Volts, 11, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset100Volts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 13, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 14, 2.35f, 1));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 5));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 6));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1)); // CPU_FAN1
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "2"), 2)); // CPU_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3)); // CHA_FAN1/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4)); // CHA_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 0)); // CHA_FAN3/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 5)); // CHA_FAN4/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 6)); // CHA_FAN5/WP

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            break;

                        case Model.X570_Phantom_Gaming_4: // NCT6796D (-R?)
                            // Voltages                              
                            // internal on NCT6796D have a 1/2 voltage divider (by way of two 34kOhm resistors)
                            // "Six internal signals connected to the power supplies (CPUVCORE, AVSB, VBAT, VTT, 3VSB, 3VCC)"
                            // "All the internal inputs of the ADC, AVSB, VBAT, 3VSB, 3VCC utilize an integrated voltage divider
                            // with both resistors equal to 34kOhm"
                            // it seems that VTT doesn't actually have the 1/2 divider
                            // external sources can have whatever divider that gets them in the 0V to 2.048V range
                            // assuming Vf = 0, then Ri = R1 and Rf = R2 (from voltage divider equation)
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 2, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 56, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSocVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.VppmVolts, 11, 3, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSocPremVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.DimmWriteVolts, 13));
                            voltages.Add(new Voltage(SuperIoConstants.V18Volts, 14, 1, 1));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 9));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 10));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            
                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.CpuFanNumber, "1"),
                                string.Format(SuperIoConstants.CpuFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                SuperIoConstants.ChipsetFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.Z790_Taichi:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V18Volts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset082Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.Cpu105Volts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset105Volts, 12, 5, 100));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 6));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 6));
                            break;

                        case Model.B650M_C: // NCT6799D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 1, 56, 10));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 4, 20, 10));
                            voltages.Add(new Voltage(SuperIoConstants.Alw105Volts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.V33StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 10, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreMiscVolts, 11, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V18Volts, 13, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 9));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1)); // CPU_FAN1
                            fans.Add(new Fan(string.Format(SuperIoConstants.CpuFanNumber, "2"), 0)); // CPU_FAN2/WP
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3)); // CHA_FAN1/WP
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4)); // CHA_FAN2/WP
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 6)); // CHA_FAN3/WP

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "1"), 1)); // CPU_FAN1
                            controls.Add(new Control(string.Format(SuperIoConstants.CpuFanNumber, "2"), 0)); // CPU_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 3)); // CHA_FAN1/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 4)); // CHA_FAN2/WP
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 6)); // CHA_FAN3/WP
                            break;

                        default:
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
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));
                            
                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;
                    }
                    break;

                case Manufacturer.ASUS:
                    switch (model)
                    {
                        case Model.TUF_GAMING_X570_PLUS_WIFI: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 22));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 10));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 6));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 3));
                            fans.Add(new Fan(SuperIoConstants.ChipsetFan, 4));
                            fans.Add(new Fan(SuperIoConstants.AioPumpFan, 5));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 6));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 3));
                            controls.Add(new Control(SuperIoConstants.ChipsetFan, 4));
                            controls.Add(new Control(SuperIoConstants.AioPumpFan, 5));
                            break;

                        case Model.TUF_GAMING_B550M_PLUS_WIFI: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));
                            
                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciTempNumber, "0"), 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "0"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "1"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "2"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "3"), 6));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "4"), 7));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SmbusTempNumber, "0"), 8));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SmbusTempNumber, "1"), 9));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciTempNumber, "1"), 10));
                            temps.Add(new Temperature(SuperIoConstants.PchChipCpuMaxTemp, 11));
                            temps.Add(new Temperature(SuperIoConstants.PchChipTemp, 12));
                            temps.Add(new Temperature(SuperIoConstants.PchCpuTemp, 13));
                            temps.Add(new Temperature(SuperIoConstants.PchMchTemp, 14));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "0", "0"), 15));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "0", "1"), 16));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "1", "0"), 17));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "1", "1"), 18));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DeviceTempNumber, "0"), 19));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DeviceTempNumber, "1"), 20));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciCalibratedTempNumber, "0"), 21));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciCalibratedTempNumber, "1"), 22));
                            temps.Add(new Temperature(SuperIoConstants.VirtualTemp, 23));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        case Model.ROG_CROSSHAIR_VIII_HERO: // NCT6798D
                        case Model.ROG_CROSSHAIR_VIII_HERO_WIFI: // NCT6798D
                        case Model.ROG_CROSSHAIR_VIII_DARK_HERO: // NCT6798D
                        case Model.ROG_CROSSHAIR_VIII_FORMULA: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSocVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 13));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));
                            
                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciTempNumber, "0"), 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "0"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "1"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "2"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "3"), 6));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.AuxTempNumber, "4"), 7));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SmbusTempNumber, "0"), 8));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SmbusTempNumber, "1"), 9));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciTempNumber, "1"), 10));
                            temps.Add(new Temperature(SuperIoConstants.PchChipCpuMaxTemp, 11));
                            temps.Add(new Temperature(SuperIoConstants.PchChipTemp, 12));
                            temps.Add(new Temperature(SuperIoConstants.PchCpuTemp, 13));
                            temps.Add(new Temperature(SuperIoConstants.PchMchTemp, 14));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "0", "0"), 15));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "0", "1"), 16));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "1", "0"), 17));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "1", "1"), 18));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DeviceTempNumber, "0"), 19));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DeviceTempNumber, "1"), 20));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciCalibratedTempNumber, "0"), 21));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciCalibratedTempNumber, "1"), 22));
                            temps.Add(new Temperature(SuperIoConstants.VirtualTemp, 23));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                SuperIoConstants.HighAmpFan,
                                SuperIoConstants.WaterPumpFan,
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_MAXIMUS_XI_FORMULA: // NC6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, "8"), 5));
                            voltages.Add(new Voltage(SuperIoConstants.CpuGraphicsVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccioVolts, 11, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.PchCoreVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.PhaseLockedLoopVolts, 13));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.CpuWeightedVolts, 6));
                            temps.Add(new Temperature(SuperIoConstants.CpuPeciTemp, 7));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 8));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                SuperIoConstants.HighAmpFan,
                                SuperIoConstants.WaterPumpFan,
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_MAXIMUS_XII_Z490_FORMULA: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3vccVolts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "1"), 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.VBatVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.VttVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "0"), 11, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.PchVolts, 12));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "7"), 7));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 12));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "9"), 21));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "4"),
                                SuperIoConstants.WaterPumpFan,
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_MAXIMUS_Z690_FORMULA: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3vccVolts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "1"), 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "10"), 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "0"), 11));
                            voltages.Add(new Voltage(SuperIoConstants.PchVolts, 12));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "7"), 7));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 12));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "9"), 21));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "4"),
                                SuperIoConstants.WaterPumpFan,
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_MAXIMUS_Z690_HERO: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3vccVolts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "1"), 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "0"), 11));
                            voltages.Add(new Voltage(SuperIoConstants.PchVolts, 12));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPackageTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuWeightedVolts, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 12));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 21));

                            // Note that CPU Opt Fan is on the ASUS EC controller. Together with VRM, T_Sensor, WaterIn, WaterOut and WaterFlow + additional sensors.
                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "4"),
                                SuperIoConstants.WaterPumpFan,
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_MAXIMUS_Z690_EXTREME_GLACIAL: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3vccVolts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "1"), 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "0"), 11, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.PchVolts, 12));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "7"), 7));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 12));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "9"), 21));
                            
                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.RadiatorFanNumber, "1"),
                                string.Format(SuperIoConstants.RadiatorFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.WaterPumpFanNumber, "1"),
                                string.Format(SuperIoConstants.WaterPumpFanNumber, "2")
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_MAXIMUS_Z790_HERO: // NCT6798D
                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPackageTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "8"), 8));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "9"), 9));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "10"), 10));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "11"), 11));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 12));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "13"), 13));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "14"), 14));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "15"), 15));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "16"), 16));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "17"), 17));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "18"), 18));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "19"), 19));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "20"), 20));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 21));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        case Model.ROG_MAXIMUS_Z790_DARK_HERO: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V3vccVolts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "1"), 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, 1, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "0"), 11, 1, 1));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 12));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 13, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 14, 1, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 15));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPackageTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 12));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 13));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 22));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "4"),
                                SuperIoConstants.WaterPumpFan,
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_STRIX_Z790_E_GAMING_WIFI: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.IvrAtomL2ClusterVoltNumber, "1"), 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.AuxTinVoltNumber, "0"), 6));
                            voltages.Add(new Voltage(SuperIoConstants.V33StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuImcVolts, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuL2CacheVolts, 11));
                            voltages.Add(new Voltage(SuperIoConstants.Pch105Volts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 13));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 14));
                            voltages.Add(new Voltage(SuperIoConstants.SysTinVolts, 15));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuPackageTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuWeightedVolts, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 13));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 22));

                            fanControlNames = [
                                string.Format(SuperIoConstants.ChassisFanNumber, "1"),
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.ChassisFanNumber, "2"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "3"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "4"),
                                string.Format(SuperIoConstants.ChassisFanNumber, "5"),
                                SuperIoConstants.AioPumpFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.ROG_STRIX_B550_I_GAMING: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1)); //Probably not updating properly
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1)); //Probably not updating properly
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PchChipCpuMaxTemp, 11));
                            temps.Add(new Temperature(SuperIoConstants.PchChipTemp, 12));
                            temps.Add(new Temperature(SuperIoConstants.PchCpuTemp, 13));
                            temps.Add(new Temperature(SuperIoConstants.PchMchTemp, 14));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "0", "0"), 15));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DimmAgentNumbersTemp, "1", "0"), 17));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DeviceTempNumber, "0"), 19));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.DeviceTempNumber, "1"), 20));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciCalibratedTempNumber, "0"), 21));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.PeciCalibratedTempNumber, "1"), 22));
                            temps.Add(new Temperature(SuperIoConstants.VirtualTemp, 23));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        fans.Add(new Fan(SuperIoConstants.ChassisFan, 0));
                                        break;
                                    case 1:
                                        fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                                        break;
                                    case 4:
                                        fans.Add(new Fan(SuperIoConstants.AioPumpFan, 4));
                                        break;
                                }
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        controls.Add(new Control(SuperIoConstants.ChassisFan, 0));
                                        break;
                                    case 1:
                                        controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                                        break;
                                    case 4:
                                        controls.Add(new Control(SuperIoConstants.AioPumpFan, 4));
                                        break;
                                }
                            }
                            break;

                        case Model.ROG_ZENITH_II_EXTREME: // NCT6798D
                            // Voltages
                            // Voltage = value + (value - Vf) * Ri / Rf.
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 6, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmCDVolts, 11, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.DimmABVolts, 13));
                            voltages.Add(new Voltage(SuperIoConstants.PhaseLockedLoopVolts, 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "7"), 7));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "21"), 21));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        fans.Add(new Fan(SuperIoConstants.ChassisFan, 0));
                                        break;
                                    case 1:
                                        fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                                        break;
                                    case 2:
                                        fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 2));
                                        break;
                                    case 4:
                                        fans.Add(new Fan(SuperIoConstants.AioPumpFan, 4));
                                        break;
                                }
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        controls.Add(new Control(SuperIoConstants.ChassisFan, 0));
                                        break;
                                    case 1:
                                        controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                                        break;
                                    case 2:
                                        controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 2));
                                        break;
                                    case 4:
                                        controls.Add(new Control(SuperIoConstants.AioPumpFan, 4));
                                        break;
                                }
                            }
                            break;

                        case Model.ROG_STRIX_X570_I_GAMING: // NCT6798D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1)); //Probably not updating properly
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1)); //Probably not updating properly
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "7"), 7));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "21"), 21));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        fans.Add(new Fan(SuperIoConstants.ChassisFan, 0));
                                        break;
                                    case 1:
                                        fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                                        break;
                                    case 4:
                                        fans.Add(new Fan(SuperIoConstants.AioPumpFan, 4));
                                        break;
                                }
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        controls.Add(new Control(SuperIoConstants.ChassisFan, 0));
                                        break;
                                    case 1:
                                        controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                                        break;
                                    case 4:
                                        controls.Add(new Control(SuperIoConstants.AioPumpFan, 4));
                                        break;
                                }
                            }
                            break;

                        case Model.ROG_STRIX_B550_F_GAMING_WIFI: // NCT6798D-R
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));
                            
                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        case Model.PRIME_B650_PLUS: // NCT6799D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvStandbyVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVddioImcVolts, 10, 1, 1));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 22));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.CpuPackageTemp, 3));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 3));
                            fans.Add(new Fan(SuperIoConstants.AioPumpFan, 5));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 3));
                            controls.Add(new Control(SuperIoConstants.AioPumpFan, 5));

                            break;

                        case Model.ROG_CROSSHAIR_X670E_GENE: // NCT6799D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVddioImcVolts, 10, 1, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            temps.Add(new Temperature(SuperIoConstants.TSensorTemp, 24));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        case Model.ROG_STRIX_X670E_E_GAMING_WIFI: // NCT6799D
                        case Model.ROG_STRIX_X670E_F_GAMING_WIFI: // NCT6799D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVddioImcVolts, 10, 1, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));
                            temps.Add(new Temperature(SuperIoConstants.TSensorTemp, 24));

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        case Model.PROART_X670E_CREATOR_WIFI: // NCT6799D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVddioImcVolts, 10, 1, 1));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 22));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.TSensorTemp, 24)); // Aligned with Armoury Crate
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1)); // Unknown, Possibly VRM with 23 offset

                            // Fans
                            DefaultConfigurations.GetFans(superIo, fans);

                            // Controls
                            DefaultConfigurations.GetControls(superIo, controls);
                            break;

                        default:
                            GetConfiguration9XdDefaults(superIo, voltages, temps, fans, controls);
                            break;
                    }
                    break;

                case Manufacturer.MSI:
                    voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                    switch (model)
                    {
                        case Model.B360M_PRO_VDH: // NCT6797D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuIoVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuNorthbridgeSocVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 13, 1, 1));
                            
                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.AuxiliaryTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "1"), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, "2"), 3));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "1"), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, "2"), 3));
                            break;

                        case Model.B450A_PRO: // NCT6797D
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CpuNorthbridgeSocVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 13, 1, 1));
                            
                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 3));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 5));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SmbusTempNumber, "0"), 8));

                            fanControlNames = [
                                SuperIoConstants.PumpFan,
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.SystemFanNumber, "1"),
                                string.Format(SuperIoConstants.SystemFanNumber, "2"),
                                string.Format(SuperIoConstants.SystemFanNumber, "3"),
                                string.Format(SuperIoConstants.SystemFanNumber, "4")
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        case Model.X570_Gaming_Plus:
                            // NCT6797D
                            // NCT771x : PCIE 1, M.2 1, not supported
                            // RF35204 : VRM not supported
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 4, 1));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 2, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 3, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 11, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, "4"), 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 34, 34));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 11));
                            voltages.Add(new Voltage(SuperIoConstants.CpuNorthbridgeSocVolts, 12));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 13, 1, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 14));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.MosTemp, 3));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 5));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 9));

                            fanControlNames = [
                                SuperIoConstants.PumpFan,
                                SuperIoConstants.CpuFan,
                                string.Format(SuperIoConstants.SystemFanNumber, "1"),
                                string.Format(SuperIoConstants.SystemFanNumber, "2"),
                                string.Format(SuperIoConstants.SystemFanNumber, "3"),
                                string.Format(SuperIoConstants.SystemFanNumber, "4"),
                                SuperIoConstants.ChipsetFan
                            ];

                            // Fans
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                fans.Add(new Fan(fanControlNames[i], i));
                            }

                            // Controls
                            for (int i = 0; i < fanControlNames.Length; i++)
                            {
                                controls.Add(new Control(fanControlNames[i], i));
                            }
                            break;

                        default:
                            GetConfiguration9XdDefaults(superIo, voltages, temps, fans, controls);
                            break;
                    }
                    break;

                default:
                    GetConfiguration9XdDefaults(superIo, voltages, temps, fans, controls);
                    break;
            }
        }

        /// <summary>
        /// Gets Nuvoton configuration: F - Defaults.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        private static void GetConfigurationFDefaults(
            ISuperIo superIo,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls)
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
            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));

            // Fans
            DefaultConfigurations.GetFans(superIo, fans);

            // Controls
            DefaultConfigurations.GetControls(superIo, controls);
        }

        /// <summary>
        /// Gets the Nuvoton configuration: 9XD - Defaults.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        private static void GetConfiguration9XdDefaults(
            ISuperIo superIo,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls)
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
            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 9));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "11"), 10, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "12"), 11, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "13"), 12, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "14"), 13, true));
            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "15"), 14, true));

            // Temps
            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 0));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "1"), 1));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "2"), 2));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "3"), 3));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "4"), 4));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "5"), 5));
            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, "6"), 6));

            // Fans
            DefaultConfigurations.GetFans(superIo, fans);

            // Controls
            DefaultConfigurations.GetControls(superIo, controls);
        }
    }
}
