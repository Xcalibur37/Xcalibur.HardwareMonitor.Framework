using System;
using System.Collections.Generic;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Ite
{
    /// <summary>
    /// ITE Configurations
    /// </summary>
    internal static class IteConfigurations
    {
        /// <summary>
        /// Gets ITE Configurations: A.
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
        /// <param name="mutex">The mutex.</param>
        internal static void GetConfigurationsA
        (
            ISuperIo superIo,
            Manufacturer manufacturer,
            MotherboardModel model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls,
            ref SuperIoDelegates.ReadValueDelegate readFan,
            ref SuperIoDelegates.UpdateDelegate postUpdate,
            ref Mutex mutex)
        {
            switch (manufacturer)
            {
                case Manufacturer.ASUS:
                    switch (model)
                    {
                        case MotherboardModel.CrosshairIiiFormula: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.M2NSliDeluxe:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 30, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 7, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            break;

                        case MotherboardModel.M4A79XtdEvo: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "8"), 7, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                case Manufacturer.ASRock:
                    switch (model)
                    {
                        case MotherboardModel.P55Deluxe: // IT8720F
                            GetAsRockConfiguration(superIo, voltages, temps, fans,
                                                   ref readFan, ref postUpdate, out mutex);
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "8"), 7, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

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
                case Manufacturer.DFI:
                    switch (model)
                    {
                        case MotherboardModel.LpBiP45T2RsElite: // IT8718F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 30, 10));
                            voltages.Add(new Voltage(SuperIoConstants.NorthbridgeCore, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 7, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 2));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 1), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 2), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 3), 2));
                            break;

                        case MotherboardModel.LpDkP55T3Eh9: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 30, 10));
                            voltages.Add(new Voltage(SuperIoConstants.PhaseLockedLoopVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 7, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuPwmTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 1), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 2), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 3), 2));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 1, true));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2, true));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 30, 10, 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V50StandbyVolts, 7, 6.8f, 10, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                case Manufacturer.Gigabyte:
                    voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                    switch (model)
                    {
                        case MotherboardModel._965P_S3: // IT8718F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel.Ep45Ds3R: // IT8718F
                        case MotherboardModel.Ep45Ud3R:
                        case MotherboardModel.X38Ds5:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 3));
                            break;

                        case MotherboardModel.Ex58Extreme: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.NorthbridgeTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 3));
                            break;

                        case MotherboardModel.P35Ds3: // IT8718F
                        case MotherboardModel.P35Ds3L: // IT8718F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 3));
                            break;

                        case MotherboardModel.P55Ud4: // IT8720F
                        case MotherboardModel.P55AUd3: // IT8720F
                        case MotherboardModel.P55MUd4: // IT8720F
                        case MotherboardModel.H55Usb3: // IT8720F
                        case MotherboardModel.Ex58Ud3R: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 5, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 3));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 1));
                            break;

                        case MotherboardModel.H55NUsb3: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 5, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel.G41MCombo: // IT8718F
                        case MotherboardModel.G41MtS2: // IT8718F
                        case MotherboardModel.G41MtS2P: // IT8718F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel._970A_UD3: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 4));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.PwmControlNumber, 1), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.PwmControlNumber, 2), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.PwmControlNumber, 3), 2));
                            break;

                        case MotherboardModel.Ma770TUd3: // IT8720F
                        case MotherboardModel.Ma770TUd3P: // IT8720F
                        case MotherboardModel.Ma790XUd3P: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 3));
                            break;

                        case MotherboardModel.Ma78LmS2H: // IT8718F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 3));
                            break;

                        case MotherboardModel.Ma785GmUs2H: // IT8718F
                        case MotherboardModel.Ma785GmtUd2H: // IT8718F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));
                            fans.Add(new Fan(SuperIoConstants.NorthbridgeFan, 2));
                            break;

                        case MotherboardModel.X58AUd3R: // IT8720F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 5, 24.3f, 8.2f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.NorthbridgeTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 3));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1, true));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2, true));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 6.8f, 10, 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "8"), 7, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                default:
                    // Voltages
                    voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "8"), 7, true));
                    voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

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

                    // Controls
                    for (int i = 0; i < superIo.Controls.Length; i++)
                    {
                        controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                    }
                    break;
            }
        }
        /// <summary>
        /// Gets ITE Configurations: B.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfigurationsB(
            ISuperIo superIo,
            Manufacturer manufacturer,
            MotherboardModel model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls)
        {
            switch (manufacturer)
            {
                case Manufacturer.ASUS:
                    switch (model)
                    {
                        case MotherboardModel.PrimeX370Pro: // IT8665E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.Southbridge250Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "10"), 9, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 2));

                            for (int i = 3; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, i + 1), i));
                            }

                            // Fans
                            // Don't know how to get the Pump Fans readings (bios? DC controller? driver?)
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            fans.Add(new Fan(SuperIoConstants.AioPumpFan, 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));
                            fans.Add(new Fan(SuperIoConstants.WaterPumpFan, 5));

                            for (int i = 6; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.TufX470PlusGaming: // IT8665E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.Southbridge250Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "10"), 9, true));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 2));

                            for (int i = 3; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, i + 1), i));
                            }

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));

                            for (int i = 1; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.RogZenithExtreme: // IT8665E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 1, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Southbridge105Volts, 4, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.DimmCdVolts, 5, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.PhaseLockedLoopVolts, 6, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuSocketTemp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, 4), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, 5), 4));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
                            fans.Add(new Fan(SuperIoConstants.HighAmpFan, 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 5), 4));
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 6), 5));

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.RogStrixX470I: // IT8665E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.Southbridge250Volts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.TSensorTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber, 6), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));

                            //Does not work when in AIO pump mode (shows 0). I don't know how to fix it.
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control("Fan #" + i, i));
                            }
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "8"), 7, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }

                            break;
                    }

                    break;
                case Manufacturer.ECS:
                    switch (model)
                    {
                        case MotherboardModel.A890GxmA: // IT8721F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.NorthbridgeTemp, 2));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 3, 10, 10));
                            // voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.NorthbridgeTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(SuperIoConstants.AvccVolts, 3, 10, 10, 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                case Manufacturer.Gigabyte:
                    switch (model)
                    {
                        case MotherboardModel.H61MDs2Rev12: // IT8728F
                        case MotherboardModel.H61MUsb3B3Rev20: // IT8728F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 30.9f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel.H67AUd3HB3: // IT8728F
                        case MotherboardModel.H67AUsb3B3: // IT8728F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 1, 15, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 30.9f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 3));
                            break;

                        case MotherboardModel.B75MD3H: // IT8728F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10, 2));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVaxgVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 2));
                            controls.Add(new Control(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel._970A_DS3P: // IT8620E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 4, 6.5f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuPackageTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuCoreTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            break;

                        case MotherboardModel.H81MHd3: //IT8620E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel.H97D3H: //IT8620E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 4));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 4));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            break;

                        case MotherboardModel.Z170NWifi: // ITE IT8628E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            // NO DIMM C/D channels on this motherboard; gives a very tiny voltage reading
                            // voltages.Add(new Voltage(SuperIoConstants.DimmCdVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVaxgVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, 54, 10));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel.Ax370GamingK7: // IT8686E
                        case MotherboardModel.Ax370Gaming5:
                        case MotherboardModel.Ab350Gaming3: // IT8686E
                            // Voltages                                  // Note: v3.3, v12, v5, and AVCC3 might be slightly off.
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 0.65f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VsocVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, 7.53f, 1));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.X399AorusGaming7: // ITE IT8686E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmCdVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, 54, 10));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 4));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.B450AorusPro:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.VsocMosVolts, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.B450GamingX:
                        case MotherboardModel.B450AorusElite:
                        case MotherboardModel.B450MAorusElite:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.VsocMosVolts, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            break;

                        case MotherboardModel.B450MGaming: // ITE IT8686E
                        case MotherboardModel.B450AorusM:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.VsocMosVolts, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            break;

                        case MotherboardModel.B450IAorusProWifi:
                        case MotherboardModel.B450MDs3H: // ITE IT8686E
                        case MotherboardModel.B450MS2H:
                        case MotherboardModel.B450MH:
                        case MotherboardModel.B450MK:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.VsocMosVolts, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 1));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(SuperIoConstants.SystemFan, 1));
                            break;

                        case MotherboardModel.X470AorusGaming7Wifi: // ITE IT8686E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, 54, 10));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 4));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.B560MAorusElite: // IT8689E
                        case MotherboardModel.B560MAorusPro:
                        case MotherboardModel.B560MAorusProAx:
                        case MotherboardModel.B560IAorusProAx:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 29.4f, 45.3f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10f, 2f));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVaxgVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, 59.9f, 9.8f));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.B650AorusElite: // IT8689E
                        case MotherboardModel.B650AorusEliteAx: // IT8689E
                        case MotherboardModel.B650AorusEliteV2: // IT8689E
                        case MotherboardModel.B650AorusEliteAxV2: // IT8689E
                        case MotherboardModel.B650AorusEliteAxIce: // IT8689E
                        case MotherboardModel.B650EAorusEliteAxIce: // IT8689E
                        case MotherboardModel.B650MAorusPro: // IT8689E
                        case MotherboardModel.B650MAorusProAx:
                        case MotherboardModel.B650MAorusElite:
                        case MotherboardModel.B650MAorusEliteAx:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 29.4f, 45.3f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10f, 2f));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreMiscVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DualDDR55VVolts, 6, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, 59.9f, 9.8f));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.VsocMosVolts, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 4), 4));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 5));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 4), 4));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 5));
                            break;

                        case MotherboardModel.B360AorusGaming3WifiCf: // IT8688E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 29.4f, 45.3f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10f, 2f));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1, 1));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.EcTempTempNumber, 1), 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PchFan, 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(SuperIoConstants.PchFan, 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.X570AorusMaster: // IT8688E
                        case MotherboardModel.X570AorusUltra:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 29.4f, 45.3f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10f, 2f));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1f, 10f));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.EcTempTempNumber, 1), 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PchFan, 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(SuperIoConstants.PchFan, 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.X570AorusPro: // IT8688E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 29.4f, 45.3f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10f, 2f));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10f, 10f));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.ExternalTempNumber, 1), 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PchFan, 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(SuperIoConstants.PchFan, 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.X570GamingX: // IT8688E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 29.4f, 45.3f));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10f, 2f));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15f, 10f));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 6));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(SuperIoConstants.PchFan, 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(SuperIoConstants.PchFan, 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.Z390MGaming: // IT8688E
                        case MotherboardModel.Z390AorusUltra:
                        case MotherboardModel.Z390Ud:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccGTVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.VddqVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.DdrVttVolts, 7));
                            voltages.Add(new Voltage(SuperIoConstants.PchCoreVolts, 8));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccioVolts, 9));
                            voltages.Add(new Voltage(SuperIoConstants.DdrVppVolts, 10));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            break;

                        case MotherboardModel.Z390AorusPro:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccGTVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DdrVolts, 6));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 7, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 8, 1f, 1f, -0.312f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 9, 6f, 1f, 0.01f));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 10, 6f, 1f, 0.048f));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.EcTempTempNumber, 1), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.Z790Ud: // ITE IT8689E
                        case MotherboardModel.Z790UdAc: // ITE IT8689E
                        case MotherboardModel.Z790GamingX: // ITE IT8689E
                        case MotherboardModel.Z790GamingXAx: // ITE IT8689E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DualDDR55VVolts, 6, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, true));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.Z790AorusProX: // ITE IT8689E
                        case MotherboardModel.Z690AorusPro:
                        case MotherboardModel.Z690AorusUltra: // ITE IT8689E
                        case MotherboardModel.Z690AorusMaster: // ITE IT8689E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVaxgVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DualDDR55VVolts, 6, 1.5f, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1f, 1f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1f, 1f));
                            voltages.Add(new Voltage(SuperIoConstants.Avcc3Volts, 9, true));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.ExternalTempNumber, 1), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.Z690GamingXDdr4:
                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 4), 5));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 4), 5));
                            break;

                        case MotherboardModel.Z68AD3HB3: // IT8728F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 30.9f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 7.15f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 3));
                            break;

                        case MotherboardModel.P67AUd3B3: // IT8728F
                        case MotherboardModel.P67AUd3RB3: // IT8728F
                        case MotherboardModel.P67AUd4B3: // IT8728F
                        case MotherboardModel.Z68ApD3: // IT8728F
                        case MotherboardModel.Z68XUd3HB3: // IT8728F
                        case MotherboardModel.Z68XpUd3R: // IT8728F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 30.9f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 7.15f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 1));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 3));
                            break;

                        case MotherboardModel.Z68XUd7B3: // IT8728F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.49f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 30.9f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 30.9f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 7.15f, 10));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 3), 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.PowerFan, 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 4));
                            break;

                        case MotherboardModel.X79Ud3: // IT8728F
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.CpuTerminationVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10, 2));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 15, 10));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, 4), 4));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmCdVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1, 1));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.NorthbridgeTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            break;

                        case MotherboardModel.B550AorusMaster:
                        case MotherboardModel.B550AorusPro:
                        case MotherboardModel.B550AorusProAc:
                        case MotherboardModel.B550AorusProAx:
                        case MotherboardModel.B550VisionD:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.ExternalTempNumber, 1), 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.B550AorusElite:
                        case MotherboardModel.B550AorusEliteAx:
                        case MotherboardModel.B550GamingX:
                        case MotherboardModel.B550UdAc:
                        case MotherboardModel.B550MAorusPro:
                        case MotherboardModel.B550MAorusProAx:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.B550IAorusProAx:
                        case MotherboardModel.B550MAorusElite:
                        case MotherboardModel.B550MGaming:
                        case MotherboardModel.B550MDs3H:
                        case MotherboardModel.B550MDs3HAc:
                        case MotherboardModel.B550MS2H:
                        case MotherboardModel.B550MH:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VcoreSocVolts, 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.VsocMosVolts, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            break;

                        case MotherboardModel.B660Ds3HDdr4:
                        case MotherboardModel.B660Ds3HAcDdr4:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 1, 6.5F, 10));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 1.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(SuperIoConstants.ChipsetTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 3));
                            temps.Add(new Temperature(SuperIoConstants.VrmMosTemp, 4));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 3));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 4));
                            break;

                        case MotherboardModel.B660MDs3HAxDdr4:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.IGpuVaxgVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.DimmAbVolts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 6));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.PchTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.PciEx16Temp, 2));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 3));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 4));
                            temps.Add(new Temperature(SuperIoConstants.VramMosTemp, 5));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 1), 2));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 2), 3));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 3), 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 1), 2));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 2), 3));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 3), 4));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }
                    break;

                case Manufacturer.Biostar:
                    switch (model)
                    {
                        // IT8613E
                        // This board has some problems with their app controlling fans that I was able to replicate here so I guess is a BIOS problem with the pins.
                        // Biostar is aware so expect changes in the control pins with new bios.
                        // In the meantime, it's possible to control CPUFAN and CPUOPT1m but not SYSFAN1.
                        // The parameters are extracted from the Biostar app config file.
                        case MotherboardModel.B660Gtn:
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1, 0, 1));
                            // Reads higher than it should.
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 5, 1));
                            // Reads higher than it should.
                            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 3, 147, 100));
                            // Commented because I don't know if it makes sense.
                            //voltages.Add(new Voltage("VCC ST", 4)); // Reads 4.2V.
                            //voltages.Add(new Voltage(SuperIoConstants.CpuInputAuxVolts, 5)); // Reads 2.2V.
                            //voltages.Add(new Voltage("CPU GT", 6)); // Reads 2.6V.
                            //voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10)); // Reads 5.8V ?
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10)); // Reads higher than it should at 3.4V.

                            // Temps
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 1), 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 1)); // Not sure what sensor is this.
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 2));
                            fans.Add(new Fan(SuperIoConstants.SystemFan, 4));

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 1));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 2));
                            controls.Add(new Control(SuperIoConstants.SystemFan, 4));
                            break;

                        case MotherboardModel.X670EValkyrie: //IT8625E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 2, 10, 2));
                            // Voltage of unknown use
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            // The biostar utility shows CPU MISC Voltage.
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4));
                            voltages.Add(new Voltage(SuperIoConstants.VddpVolts, 5));
                            voltages.Add(new Voltage(SuperIoConstants.VsocVolts, 6));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.VrmTemp, 1));
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 2));

                            // Fans
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
                            fans.Add(new Fan(SuperIoConstants.CpuOptionalFan, 1));
                            for (int i = 2; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, i - 1), i));
                            }

                            // Controls
                            controls.Add(new Control(SuperIoConstants.CpuFan, 0));
                            controls.Add(new Control(SuperIoConstants.CpuOptionalFan, 1));
                            for (int i = 2; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, i - 1), i));
                            }
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                case Manufacturer.Shuttle:
                    switch (model)
                    {
                        case MotherboardModel.Fh67: // IT8772E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.DimmVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.PchVccioVolts, 2));
                            voltages.Add(new Voltage(SuperIoConstants.CpuVccioVolts, 3));
                            voltages.Add(new Voltage(SuperIoConstants.GraphicsVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.SystemTemp, 0));
                            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 1));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, 1), 0));
                            fans.Add(new Fan(SuperIoConstants.CpuFan, 1));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                default:
                    // Voltages
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 0, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                    voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10, 0, true));
                    voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

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

                    // Controls
                    for (int i = 0; i < superIo.Controls.Length; i++)
                    {
                        controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                    }
                    break;
            }
        }

        /// <summary>
        /// Get ITE Configurations: C.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="model">The model.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetConfigurationsC(
            ISuperIo superIo,
            Manufacturer manufacturer,
            MotherboardModel model,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ICollection<Control> controls)
        {
            switch (manufacturer)
            {
                case Manufacturer.Gigabyte:
                    switch (model)
                    {
                        case MotherboardModel.X570AorusMaster: // IT879XE
                        case MotherboardModel.X570AorusPro:
                        case MotherboardModel.X570AorusUltra:
                        case MotherboardModel.B550AorusMaster:
                        case MotherboardModel.B550AorusPro:
                        case MotherboardModel.B550AorusProAc:
                        case MotherboardModel.B550AorusProAx:
                        case MotherboardModel.B550VisionD:
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, 0), 0));
                            voltages.Add(new Voltage(SuperIoConstants.DdrVttAbVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.ChipsetCoreVolts, 2));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.CpuVddNumberVolts, 18), 4));
                            voltages.Add(new Voltage(SuperIoConstants.PmCldO12Volts, 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1f, 1f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1f, 1f));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.PciEx8Temp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.ExternalTempNumber, 2), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 2));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 5), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 6), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 4), 2));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 5), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 6), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 4), 2));
                            break;

                        case MotherboardModel.X470AorusGaming7Wifi: // ITE IT8792
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, 0), 0, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.DdrVttVolts, 1, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.ChipsetCoreVolts, 2, 0, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, 3), 3, 0, 1));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.CpuVddNumberVolts, 18), 4, 0, 1));
                            voltages.Add(new Voltage(SuperIoConstants.ChipsetCore250Volts, 5, 0.5F, 1));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 6, 1, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 7, 0.7F, 1));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.PciEx8Temp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 2));

                            // Fans
                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;

                        case MotherboardModel.Z390AorusPro: // IT879XE
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.DdrVttAbVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.ChipsetCoreVolts, 2));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VinVoltNumber, 3), 3, true));
                            voltages.Add(new Voltage(SuperIoConstants.VccIoVolts, 4));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 5, true));
                            voltages.Add(new Voltage(SuperIoConstants.DdrVppVolts, 6));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 1f, 1f));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 1f, 1f));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.PciEx8Temp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.ExternalTempNumber, 2), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 2));

                            // Fans 
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 5), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 6), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 4), 2));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 5), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 6), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 4), 2));
                            break;

                        case MotherboardModel.Z790AorusProX: // ITE IT87952E
                        case MotherboardModel.Z690AorusPro:
                        case MotherboardModel.Z690AorusMaster: // ITE IT87952E
                            // Voltages
                            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
                            voltages.Add(new Voltage(SuperIoConstants.DimmIoVolts, 1));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset082Volts, 2));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(SuperIoConstants.CpuSaVolts, 4));
                            voltages.Add(new Voltage(SuperIoConstants.Chipset180Volts, 5));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

                            // Temps
                            temps.Add(new Temperature(SuperIoConstants.PciEx4Temp, 0));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.ExternalTempNumber, 2), 1));
                            temps.Add(new Temperature(string.Format(SuperIoConstants.SystemNumberTemp, 2), 2));

                            // Fans
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 5), 0));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 6), 1));
                            fans.Add(new Fan(string.Format(SuperIoConstants.SystemFanNumber, 4), 2));

                            // Controls
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 5), 0));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 6), 1));
                            controls.Add(new Control(string.Format(SuperIoConstants.SystemFanNumber, 4), 2));
                            break;

                        default:
                            // Voltages
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 0, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                            voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                            voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10, 0, true));
                            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

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

                            // Controls
                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                            }
                            break;
                    }

                    break;
                default:
                    // Voltages
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber, "1"), 0, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 1, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 2, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "4"), 3, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "5"), 4, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "6"), 5, true));
                    voltages.Add(new Voltage(string.Format(SuperIoConstants.ChassisFanNumber, "7"), 6, true));
                    voltages.Add(new Voltage(SuperIoConstants.V3StandbyVolts, 7, 10, 10, 0, true));
                    voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8, 10, 10));

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

                    // Controls
                    for (int i = 0; i < superIo.Controls.Length; i++)
                    {
                        controls.Add(new Control(string.Format(SuperIoConstants.FanNumber, i + 1), i));
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets ASRock configuration.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="voltages">The voltages.</param>
        /// <param name="temps">The temps.</param>
        /// <param name="fans">The fans.</param>
        /// <param name="readFan">The read fan.</param>
        /// <param name="postUpdate">The post update.</param>
        /// <param name="mutex">The mutex.</param>
        private static void GetAsRockConfiguration
        (
            ISuperIo superIo,
            IList<Voltage> voltages,
            IList<Temperature> temps,
            IList<Fan> fans,
            ref SuperIoDelegates.ReadValueDelegate readFan,
            ref SuperIoDelegates.UpdateDelegate postUpdate,
            out Mutex mutex)
        {
            // Voltages
            voltages.Add(new Voltage(SuperIoConstants.VcoreVolts, 0));
            voltages.Add(new Voltage(SuperIoConstants.V33Volts, 2));
            voltages.Add(new Voltage(SuperIoConstants.V120Volts, 4, 30, 10));
            voltages.Add(new Voltage(SuperIoConstants.V50Volts, 5, 6.8f, 10));
            voltages.Add(new Voltage(SuperIoConstants.CmosBatteryVolts, 8));

            // Temps
            temps.Add(new Temperature(SuperIoConstants.CpuTemp, 0));
            temps.Add(new Temperature(SuperIoConstants.MotherboardTemp, 1));

            // Fans
            fans.Add(new Fan(SuperIoConstants.CpuFan, 0));
            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "1"), 1));

            // this mutex is also used by the official ASRock tool
            mutex = new Mutex(false, "ASRockOCMark");

            bool exclusiveAccess = false;
            try
            {
                exclusiveAccess = mutex.WaitOne(10, false);
            }
            catch (AbandonedMutexException)
            {
                // Do nothing
            }
            catch (InvalidOperationException)
            {
                // Do nothing
            }

            // only read additional fans if we get exclusive access
            if (!exclusiveAccess) return;
            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "2"), 2));
            fans.Add(new Fan(string.Format(SuperIoConstants.ChassisFanNumber, "3"), 3));
            fans.Add(new Fan(SuperIoConstants.PowerFan, 4));

            // Read fan delegate
            readFan = index =>
            {
                if (index < 2) return superIo.Fans[index];

                // get GPIO 80-87
                byte? gpio = superIo.ReadGpio(7);
                if (!gpio.HasValue) return null;

                // read the last 3 fans based on GPIO 83-85
                int[] masks = [0x05, 0x03, 0x06];
                return (gpio.Value >> 3 & 0x07) == masks[index - 2] ? superIo.Fans[2] : null;
            };

            // Post update delegate
            int fanIndex = 0;
            postUpdate = () =>
            {
                // get GPIO 80-87
                byte? gpio = superIo.ReadGpio(7);
                if (!gpio.HasValue) return;

                // prepare the GPIO 83-85 for the next update
                int[] masks = [0x05, 0x03, 0x06];
                superIo.WriteGpio(7, (byte)(gpio.Value & 0xC7 | masks[fanIndex] << 3));
                fanIndex = (fanIndex + 1) % 3;
            };
        }
    }
}
