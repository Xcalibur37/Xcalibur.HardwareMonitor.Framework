using System;
using System.Collections.Generic;
using System.Threading;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo
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
            Model model,
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
                        case Model.CROSSHAIR_III_FORMULA: // IT8720F
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("CPU", 0));

                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan("Fan #" + (i + 1), i));
                            }

                            break;

                        case Model.M2N_SLI_Deluxe:
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("+3.3V", 1));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 4, 30, 10));
                            voltages.Add(new Voltage("+5VSB", 7, 6.8f, 10));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("CPU", 0));
                            temps.Add(new Temperature("Motherboard", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("Chassis Fan #1", 1));
                            fans.Add(new Fan("Power Fan", 2));

                            break;

                        case Model.M4A79XTD_EVO: // IT8720F
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("CPU", 0));
                            temps.Add(new Temperature("Motherboard", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("Chassis Fan #1", 1));
                            fans.Add(new Fan("Chassis Fan #2", 2));

                            break;

                        default:
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("Voltage #2", 1, true));
                            voltages.Add(new Voltage("Voltage #3", 2, true));
                            voltages.Add(new Voltage("Voltage #4", 3, true));
                            voltages.Add(new Voltage("Voltage #5", 4, true));
                            voltages.Add(new Voltage("Voltage #6", 5, true));
                            voltages.Add(new Voltage("Voltage #7", 6, true));
                            voltages.Add(new Voltage("Voltage #8", 7, true));
                            voltages.Add(new Voltage("CMOS Battery", 8));

                            for (int i = 0; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature("Temperature #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan("Fan #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control("Fan #" + (i + 1), i));
                            }

                            break;
                    }

                    break;
                case Manufacturer.ASRock:
                    switch (model)
                    {
                        case Model.P55_Deluxe: // IT8720F
                            GetAsRockConfiguration(superIo,
                                                   voltages,
                                                   temps,
                                                   fans,
                                                   ref readFan,
                                                   ref postUpdate,
                                                   out mutex);

                            break;

                        default:
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("Voltage #2", 1, true));
                            voltages.Add(new Voltage("Voltage #3", 2, true));
                            voltages.Add(new Voltage("Voltage #4", 3, true));
                            voltages.Add(new Voltage("Voltage #5", 4, true));
                            voltages.Add(new Voltage("Voltage #6", 5, true));
                            voltages.Add(new Voltage("Voltage #7", 6, true));
                            voltages.Add(new Voltage("Voltage #8", 7, true));
                            voltages.Add(new Voltage("CMOS Battery", 8));

                            for (int i = 0; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature("Temperature #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan("Fan #" + (i + 1), i));
                            }

                            break;
                    }

                    break;
                case Manufacturer.DFI:
                    switch (model)
                    {
                        case Model.LP_BI_P45_T2RS_Elite: // IT8718F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("CPU Termination", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 4, 30, 10));
                            voltages.Add(new Voltage("Northbridge Core", 5));
                            voltages.Add(new Voltage("DIMM", 6));
                            voltages.Add(new Voltage("+5VSB", 7, 6.8f, 10));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("CPU", 0));
                            temps.Add(new Temperature("System", 1));
                            temps.Add(new Temperature("Chipset", 2));
                            fans.Add(new Fan("Fan #1", 0));
                            fans.Add(new Fan("Fan #2", 1));
                            fans.Add(new Fan("Fan #3", 2));

                            break;

                        case Model.LP_DK_P55_T3EH9: // IT8720F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("CPU Termination", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 4, 30, 10));
                            voltages.Add(new Voltage("Phase Locked Loop", 5));
                            voltages.Add(new Voltage("DIMM", 6));
                            voltages.Add(new Voltage("+5VSB", 7, 6.8f, 10));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("Chipset", 0));
                            temps.Add(new Temperature("CPU PWM", 1));
                            temps.Add(new Temperature("CPU", 2));
                            fans.Add(new Fan("Fan #1", 0));
                            fans.Add(new Fan("Fan #2", 1));
                            fans.Add(new Fan("Fan #3", 2));

                            break;

                        default:
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("CPU Termination", 1, true));
                            voltages.Add(new Voltage("+3.3V", 2, true));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10, 0, true));
                            voltages.Add(new Voltage("+12V", 4, 30, 10, 0, true));
                            voltages.Add(new Voltage("Voltage #6", 5, true));
                            voltages.Add(new Voltage("DIMM", 6, true));
                            voltages.Add(new Voltage("+5VSB", 7, 6.8f, 10, 0, true));
                            voltages.Add(new Voltage("CMOS Battery", 8));

                            for (int i = 0; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature("Temperature #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan("Fan #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control("Fan #" + (i + 1), i));
                            }

                            break;
                    }

                    break;
                case Manufacturer.Gigabyte:
                    switch (model)
                    {
                        case Model._965P_S3: // IT8718F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan", 1));

                            break;

                        case Model.EP45_DS3R: // IT8718F
                        case Model.EP45_UD3R:
                        case Model.X38_DS5:
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #2", 1));
                            fans.Add(new Fan("Power Fan", 2));
                            fans.Add(new Fan("System Fan #1", 3));

                            break;

                        case Model.EX58_EXTREME: // IT8720F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            temps.Add(new Temperature("Northbridge", 2));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #2", 1));
                            fans.Add(new Fan("Power Fan", 2));
                            fans.Add(new Fan("System Fan #1", 3));

                            break;

                        case Model.P35_DS3: // IT8718F
                        case Model.P35_DS3L: // IT8718F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #1", 1));
                            fans.Add(new Fan("System Fan #2", 2));
                            fans.Add(new Fan("Power Fan", 3));

                            break;

                        case Model.P55_UD4: // IT8720F
                        case Model.P55A_UD3: // IT8720F
                        case Model.P55M_UD4: // IT8720F
                        case Model.H55_USB3: // IT8720F
                        case Model.EX58_UD3R: // IT8720F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 5, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 2));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #2", 1));
                            fans.Add(new Fan("Power Fan", 2));
                            fans.Add(new Fan("System Fan #1", 3));
                            controls.Add(new Control("CPU Fan", 0));
                            controls.Add(new Control("System Fan #2", 1));

                            break;

                        case Model.H55N_USB3: // IT8720F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 5, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 2));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan", 1));

                            break;

                        case Model.G41M_COMBO: // IT8718F
                        case Model.G41MT_S2: // IT8718F
                        case Model.G41MT_S2P: // IT8718F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("CPU", 2));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan", 1));

                            break;

                        case Model._970A_UD3: // IT8720F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #1", 1));
                            fans.Add(new Fan("System Fan #2", 2));
                            fans.Add(new Fan("Power Fan", 4));
                            controls.Add(new Control("PWM #1", 0));
                            controls.Add(new Control("PWM #2", 1));
                            controls.Add(new Control("PWM #3", 2));

                            break;

                        case Model.MA770T_UD3: // IT8720F
                        case Model.MA770T_UD3P: // IT8720F
                        case Model.MA790X_UD3P: // IT8720F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #1", 1));
                            fans.Add(new Fan("System Fan #2", 2));
                            fans.Add(new Fan("Power Fan", 3));

                            break;

                        case Model.MA78LM_S2H: // IT8718F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            temps.Add(new Temperature("VRM", 2));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #1", 1));
                            fans.Add(new Fan("System Fan #2", 2));
                            fans.Add(new Fan("Power Fan", 3));

                            break;

                        case Model.MA785GM_US2H: // IT8718F
                        case Model.MA785GMT_UD2H: // IT8718F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan", 1));
                            fans.Add(new Fan("Northbridge Fan", 2));

                            break;

                        case Model.X58A_UD3R: // IT8720F
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1));
                            voltages.Add(new Voltage("+3.3V", 2));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10));
                            voltages.Add(new Voltage("+12V", 5, 24.3f, 8.2f));
                            voltages.Add(new Voltage("CMOS Battery", 8));
                            temps.Add(new Temperature("System", 0));
                            temps.Add(new Temperature("CPU", 1));
                            temps.Add(new Temperature("Northbridge", 2));
                            fans.Add(new Fan("CPU Fan", 0));
                            fans.Add(new Fan("System Fan #2", 1));
                            fans.Add(new Fan("Power Fan", 2));
                            fans.Add(new Fan("System Fan #1", 3));

                            break;

                        default:
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM", 1, true));
                            voltages.Add(new Voltage("+3.3V", 2, true));
                            voltages.Add(new Voltage("+5V", 3, 6.8f, 10, 0, true));
                            voltages.Add(new Voltage("Voltage #5", 4, true));
                            voltages.Add(new Voltage("Voltage #6", 5, true));
                            voltages.Add(new Voltage("Voltage #7", 6, true));
                            voltages.Add(new Voltage("Voltage #8", 7, true));
                            voltages.Add(new Voltage("CMOS Battery", 8));

                            for (int i = 0; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature("Temperature #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan("Fan #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control("Fan #" + (i + 1), i));
                            }

                            break;
                    }

                    break;
                default:
                    voltages.Add(new Voltage("Vcore", 0));
                    voltages.Add(new Voltage("Voltage #2", 1, true));
                    voltages.Add(new Voltage("Voltage #3", 2, true));
                    voltages.Add(new Voltage("Voltage #4", 3, true));
                    voltages.Add(new Voltage("Voltage #5", 4, true));
                    voltages.Add(new Voltage("Voltage #6", 5, true));
                    voltages.Add(new Voltage("Voltage #7", 6, true));
                    voltages.Add(new Voltage("Voltage #8", 7, true));
                    voltages.Add(new Voltage("CMOS Battery", 8));

                    for (int i = 0; i < superIo.Temperatures.Length; i++)
                    {
                        temps.Add(new Temperature("Temperature #" + (i + 1), i));
                    }

                    for (int i = 0; i < superIo.Fans.Length; i++)
                    {
                        fans.Add(new Fan("Fan #" + (i + 1), i));
                    }

                    for (int i = 0; i < superIo.Controls.Length; i++)
                    {
                        controls.Add(new Control("Fan #" + (i + 1), i));
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
            Model model, 
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
                            case Model.PRIME_X370_PRO: // IT8665E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("Southbridge 2.5V", 1));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("Voltage #4", 4, true));
                                voltages.Add(new Voltage("Voltage #6", 5, true));
                                voltages.Add(new Voltage("Voltage #7", 6, true));
                                voltages.Add(new Voltage("+3.3V", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                voltages.Add(new Voltage("Voltage #10", 9, true));
                                temps.Add(new Temperature("CPU", 0));
                                temps.Add(new Temperature("Motherboard", 1));
                                temps.Add(new Temperature("VRM", 2));

                                for (int i = 3; i < superIo.Temperatures.Length; i++)
                                {
                                    temps.Add(new Temperature("Temperature #" + (i + 1), i));
                                }

                                // Don't know how to get the Pump Fans readings (bios? DC controller? driver?)
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("Chassis Fan #1", 1));
                                fans.Add(new Fan("Chassis Fan #2", 2));
                                fans.Add(new Fan("AIO Pump", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                fans.Add(new Fan("Water Pump", 5));

                                for (int i = 6; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;

                            case Model.TUF_X470_PLUS_GAMING: // IT8665E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("Southbridge 2.5V", 1));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("Voltage #4", 4, true));
                                voltages.Add(new Voltage("Voltage #6", 5, true));
                                voltages.Add(new Voltage("Voltage #7", 6, true));
                                voltages.Add(new Voltage("+3.3V", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                voltages.Add(new Voltage("Voltage #10", 9, true));
                                temps.Add(new Temperature("CPU", 0));
                                temps.Add(new Temperature("Motherboard", 1));
                                temps.Add(new Temperature("PCH", 2));

                                for (int i = 3; i < superIo.Temperatures.Length; i++)
                                {
                                    temps.Add(new Temperature("Temperature #" + (i + 1), i));
                                }

                                fans.Add(new Fan("CPU Fan", 0));

                                for (int i = 1; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;

                            case Model.ROG_ZENITH_EXTREME: // IT8665E
                                voltages.Add(new Voltage("Vcore", 0, 10, 10));
                                voltages.Add(new Voltage("DIMM A/B", 1, 10, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("Southbridge 1.05V", 4, 10, 10));
                                voltages.Add(new Voltage("DIMM C/D", 5, 10, 10));
                                voltages.Add(new Voltage("Phase Locked Loop", 6, 10, 10));
                                voltages.Add(new Voltage("+3.3V", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("CPU", 0));
                                temps.Add(new Temperature("Motherboard", 1));
                                temps.Add(new Temperature("CPU Socket", 2));
                                temps.Add(new Temperature("Temperature #4", 3));
                                temps.Add(new Temperature("Temperature #5", 4));
                                temps.Add(new Temperature("VRM", 5));

                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("Chassis Fan #1", 1));
                                fans.Add(new Fan("Chassis Fan #2", 2));
                                fans.Add(new Fan("High Amp Fan", 3));
                                fans.Add(new Fan("Fan 5", 4));
                                fans.Add(new Fan("Fan 6", 5));

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;

                            case Model.ROG_STRIX_X470_I: // IT8665E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("Southbridge 2.5V", 1));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("+3.3V", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("CPU", 0));
                                temps.Add(new Temperature("Motherboard", 1));
                                temps.Add(new Temperature("T_Sensor", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM", 4));
                                temps.Add(new Temperature("Temperature #6", 5));

                                fans.Add(new Fan("CPU Fan", 0));

                                //Does not work when in AIO pump mode (shows 0). I don't know how to fix it.
                                fans.Add(new Fan("Chassis Fan #1", 1));
                                fans.Add(new Fan("Chassis Fan #2", 2));

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + i, i));
                                }

                                break;

                            default:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("Voltage #2", 1, true));
                                voltages.Add(new Voltage("Voltage #3", 2, true));
                                voltages.Add(new Voltage("Voltage #4", 3, true));
                                voltages.Add(new Voltage("Voltage #5", 4, true));
                                voltages.Add(new Voltage("Voltage #6", 5, true));
                                voltages.Add(new Voltage("Voltage #7", 6, true));
                                voltages.Add(new Voltage("Voltage #8", 7, true));
                                voltages.Add(new Voltage("CMOS Battery", 8));

                                for (int i = 0; i < superIo.Temperatures.Length; i++)
                                {
                                    temps.Add(new Temperature("Temperature #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;
                        }

                        break;
                    case Manufacturer.ECS:
                        switch (model)
                        {
                            case Model.A890GXM_A: // IT8721F
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("DIMM", 1));
                                voltages.Add(new Voltage("Northbridge", 2));
                                voltages.Add(new Voltage("AVCC", 3, 10, 10));
                                // voltages.Add(new Voltage("DIMM", 6, true));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("CPU", 0));
                                temps.Add(new Temperature("System", 1));
                                temps.Add(new Temperature("Northbridge", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan", 1));
                                fans.Add(new Fan("Power Fan", 2));

                                break;

                            default:
                                voltages.Add(new Voltage("Voltage #1", 0, true));
                                voltages.Add(new Voltage("Voltage #2", 1, true));
                                voltages.Add(new Voltage("Voltage #3", 2, true));
                                voltages.Add(new Voltage("AVCC", 3, 10, 10, 0, true));
                                voltages.Add(new Voltage("Voltage #5", 4, true));
                                voltages.Add(new Voltage("Voltage #6", 5, true));
                                voltages.Add(new Voltage("Voltage #7", 6, true));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10, 0, true));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                                for (int i = 0; i < superIo.Temperatures.Length; i++)
                                {
                                    temps.Add(new Temperature("Temperature #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;
                        }

                        break;
                    case Manufacturer.Gigabyte:
                        switch (model)
                        {
                            case Model.H61M_DS2_REV_1_2: // IT8728F
                            case Model.H61M_USB3_B3_REV_2_0: // IT8728F
                                voltages.Add(new Voltage("CPU Termination", 0));
                                voltages.Add(new Voltage("+12V", 2, 30.9f, 10));
                                voltages.Add(new Voltage("Vcore", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan", 1));

                                break;

                            case Model.H67A_UD3H_B3: // IT8728F
                            case Model.H67A_USB3_B3: // IT8728F
                                voltages.Add(new Voltage("CPU Termination", 0));
                                voltages.Add(new Voltage("+5V", 1, 15, 10));
                                voltages.Add(new Voltage("+12V", 2, 30.9f, 10));
                                voltages.Add(new Voltage("Vcore", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("Power Fan", 2));
                                fans.Add(new Fan("System Fan #2", 3));

                                break;

                            case Model.B75M_D3H: // IT8728F
                                voltages.Add(new Voltage("CPU Termination", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+5V", 3, 15, 10));
                                voltages.Add(new Voltage("+12V", 2, 10, 2));
                                voltages.Add(new Voltage("iGPU VAXG", 4));
                                voltages.Add(new Voltage("Vcore", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan", 1));
                                controls.Add(new Control("CPU Fan", 2));
                                controls.Add(new Control("System Fan", 1));

                                break;

                            case Model._970A_DS3P: // IT8620E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("DIMM", 1));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("+3.3V", 4, 6.5f, 10));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU Package", 1));
                                temps.Add(new Temperature("CPU Cores", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("Power Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));

                                break;

                            case Model.H81M_HD3: //IT8620E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5f, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("iGPU", 4));
                                voltages.Add(new Voltage("CPU Input Auxiliary", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("System", 0));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan", 1));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan", 1));

                                break;

                            case Model.H97_D3H: //IT8620E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5f, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("iGPU", 4));
                                voltages.Add(new Voltage("CPU Input Auxiliary", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("System", 0));

                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("CPU Optional Fan", 1));
                                fans.Add(new Fan("System Fan #1", 4));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));

                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("CPU Optional Fan", 1));
                                controls.Add(new Control("System Fan #1", 4));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));

                                break;

                            case Model.Z170N_WIFI: // ITE IT8628E
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                // NO DIMM C/D channels on this motherboard; gives a very tiny voltage reading
                                // voltages.Add(new Voltage("DIMM C/D", 4, 0, 1));
                                voltages.Add(new Voltage("iGPU VAXG", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM A/B", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                voltages.Add(new Voltage("AVCC3", 9, 54, 10));

                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM", 4));
                                temps.Add(new Temperature("System #2", 5));

                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan", 1));

                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan", 1));

                                break;

                            case Model.AX370_Gaming_K7: // IT8686E
                            case Model.AX370_Gaming_5:
                            case Model.AB350_Gaming_3: // IT8686E
                                                       // Note: v3.3, v12, v5, and AVCC3 might be slightly off.
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 0.65f, 1));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("VSOC", 4));
                                voltages.Add(new Voltage("VDDP", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                voltages.Add(new Voltage("AVCC3", 9, 7.53f, 1));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;

                            case Model.X399_AORUS_Gaming_7: // ITE IT8686E
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("DIMM C/D", 4, 0, 1));
                                voltages.Add(new Voltage("Vcore SoC", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM A/B", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                voltages.Add(new Voltage("AVCC3", 9, 54, 10));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM", 4));

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;

                            case Model.B450_AORUS_PRO:
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("VSoC MOS", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));

                                break;

                            case Model.B450_GAMING_X:
                            case Model.B450_AORUS_ELITE:
                            case Model.B450M_AORUS_ELITE:
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("VSoC MOS", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));

                                break;

                            case Model.B450M_GAMING: // ITE IT8686E
                            case Model.B450_AORUS_M:
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("VSoC MOS", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                break;

                            case Model.B450_I_AORUS_PRO_WIFI:
                            case Model.B450M_DS3H: // ITE IT8686E
                            case Model.B450M_S2H:
                            case Model.B450M_H:
                            case Model.B450M_K:
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("VSoC MOS", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan", 1));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan", 1));
                                break;

                            case Model.X470_AORUS_GAMING_7_WIFI: // ITE IT8686E
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM A/B", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                voltages.Add(new Voltage("AVCC3", 9, 54, 10));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM", 4));

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }
                                break;

                            case Model.B560M_AORUS_ELITE: // IT8689E
                            case Model.B560M_AORUS_PRO:
                            case Model.B560M_AORUS_PRO_AX:
                            case Model.B560I_AORUS_PRO_AX:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 29.4f, 45.3f));
                                voltages.Add(new Voltage("+12V", 2, 10f, 2f));
                                voltages.Add(new Voltage("+5V", 3, 15f, 10f));
                                voltages.Add(new Voltage("iGPU VAGX", 4));
                                voltages.Add(new Voltage("CPU System Agent", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10f, 10f));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10f, 10f));
                                voltages.Add(new Voltage("AVCC3", 9, 59.9f, 9.8f));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("System #2", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.B650_AORUS_ELITE: // IT8689E
                            case Model.B650_AORUS_ELITE_AX: // IT8689E
                            case Model.B650_AORUS_ELITE_V2: // IT8689E
                            case Model.B650_AORUS_ELITE_AX_V2: // IT8689E
                            case Model.B650_AORUS_ELITE_AX_ICE: // IT8689E
                            case Model.B650E_AORUS_ELITE_AX_ICE: // IT8689E
                            case Model.B650M_AORUS_PRO: // IT8689E
                            case Model.B650M_AORUS_PRO_AX:
                            case Model.B650M_AORUS_ELITE:
                            case Model.B650M_AORUS_ELITE_AX:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 29.4f, 45.3f));
                                voltages.Add(new Voltage("+12V", 2, 10f, 2f));
                                voltages.Add(new Voltage("+5V", 3, 15f, 10f));
                                voltages.Add(new Voltage("Vcore SoC", 4));
                                voltages.Add(new Voltage("Vcore Misc", 5));
                                voltages.Add(new Voltage("Dual DDR5 5V", 6, 1.5f, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10f, 10f));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10f, 10f));
                                voltages.Add(new Voltage("AVCC3", 9, 59.9f, 9.8f));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("VSoC MOS", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                fans.Add(new Fan("System Fan #4 / Pump", 4));
                                fans.Add(new Fan("CPU Optional Fan", 5));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                controls.Add(new Control("System Fan #4 / Pump", 4));
                                controls.Add(new Control("CPU Optional Fan", 5));
                                break;

                            case Model.B360_AORUS_GAMING_3_WIFI_CF: // IT8688E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 29.4f, 45.3f));
                                voltages.Add(new Voltage("+12V", 2, 10f, 2f));
                                voltages.Add(new Voltage("+5V", 3, 15f, 10f));
                                voltages.Add(new Voltage("CPU Vcore", 4, 0, 1));
                                voltages.Add(new Voltage("CPU System Agent", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM A/B", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 1, 1));
                                voltages.Add(new Voltage("CMOS Battery", 8, 1, 1));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("EC_TEMP1", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("PCH", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("PCH Fan", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("PCH Fan", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.X570_AORUS_MASTER: // IT8688E
                            case Model.X570_AORUS_ULTRA:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 29.4f, 45.3f));
                                voltages.Add(new Voltage("+12V", 2, 10f, 2f));
                                voltages.Add(new Voltage("+5V", 3, 15f, 10f));
                                voltages.Add(new Voltage("Vcore SoC", 4));
                                voltages.Add(new Voltage("VDDP", 5));
                                voltages.Add(new Voltage("DIMM A/B", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 1f, 10f));
                                voltages.Add(new Voltage("CMOS Battery", 8, 1f, 10f));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("EC_TEMP1", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("PCH", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("PCH Fan", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("PCH Fan", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.X570_AORUS_PRO: // IT8688E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 29.4f, 45.3f));
                                voltages.Add(new Voltage("+12V", 2, 10f, 2f));
                                voltages.Add(new Voltage("+5V", 3, 15f, 10f));
                                voltages.Add(new Voltage("Vcore SoC", 4));
                                voltages.Add(new Voltage("VDDP", 5));
                                voltages.Add(new Voltage("DIMM A/B", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10f, 10f));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10f, 10f));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("External #1", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("PCH", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("PCH Fan", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("PCH Fan", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.X570_GAMING_X: // IT8688E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 29.4f, 45.3f));
                                voltages.Add(new Voltage("+12V", 2, 10f, 2f));
                                voltages.Add(new Voltage("+5V", 3, 15f, 10f));
                                voltages.Add(new Voltage("Vcore SoC", 4));
                                voltages.Add(new Voltage("VDDP", 5));
                                voltages.Add(new Voltage("DIMM A/B", 6));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("System #2", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("PCH", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("PCH Fan", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("PCH Fan", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.Z390_M_GAMING: // IT8688E
                            case Model.Z390_AORUS_ULTRA:
                            case Model.Z390_UD:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+12V", 2, 5f, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("CPU VCCGT", 4));
                                voltages.Add(new Voltage("CPU System Agent", 5));
                                voltages.Add(new Voltage("VDDQ", 6));
                                voltages.Add(new Voltage("DDRVTT", 7));
                                voltages.Add(new Voltage("PCHCore", 8));
                                voltages.Add(new Voltage("CPU VCCIO", 9));
                                voltages.Add(new Voltage("DDRVPP", 10));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("System #2", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                break;

                            case Model.Z390_AORUS_PRO:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+12V", 2, 5f, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("CPU VCCGT", 4));
                                voltages.Add(new Voltage("CPU System Agent", 5));
                                voltages.Add(new Voltage("DDR", 6));
                                voltages.Add(new Voltage("Voltage #7", 7, true));
                                voltages.Add(new Voltage("+3V Standby", 8, 1f, 1f, -0.312f));
                                voltages.Add(new Voltage("CMOS Battery", 9, 6f, 1f, 0.01f));
                                voltages.Add(new Voltage("AVCC3", 10, 6f, 1f, 0.048f));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("EC_TEMP1/System #2", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.Z790_UD: // ITE IT8689E
                            case Model.Z790_UD_AC: // ITE IT8689E
                            case Model.Z790_GAMING_X: // ITE IT8689E
                            case Model.Z790_GAMING_X_AX: // ITE IT8689E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+12V", 2, 5f, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("iGPU", 4));
                                voltages.Add(new Voltage("CPU Input Auxiliary", 5));
                                voltages.Add(new Voltage("Dual DDR5 5V", 6, 1.5f, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 1, 1));
                                voltages.Add(new Voltage("CMOS Battery", 8, 1, 1));
                                voltages.Add(new Voltage("AVCC3", 9, true));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("System #2", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3 / Pump", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3 / Pump", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.Z790_AORUS_PRO_X: // ITE IT8689E
                            case Model.Z690_AORUS_PRO:
                            case Model.Z690_AORUS_ULTRA: // ITE IT8689E
                            case Model.Z690_AORUS_MASTER: // ITE IT8689E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+12V", 2, 5f, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5f, 1));
                                voltages.Add(new Voltage("iGPU VAXG", 4));
                                voltages.Add(new Voltage("CPU Input Auxiliary", 5));
                                voltages.Add(new Voltage("Dual DDR5 5V", 6, 1.5f, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 1f, 1f));
                                voltages.Add(new Voltage("CMOS Battery", 8, 1f, 1f));
                                voltages.Add(new Voltage("AVCC3", 9, true));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("External #1", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3 / Pump", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3 / Pump", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.Z690_GAMING_X_DDR4:
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("System #2", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                fans.Add(new Fan("System Fan #4 / Pump", 5));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                controls.Add(new Control("System Fan #4 / Pump", 5));
                                break;

                            case Model.Z68A_D3H_B3: // IT8728F
                                voltages.Add(new Voltage("CPU Termination", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+12V", 2, 30.9f, 10));
                                voltages.Add(new Voltage("+5V", 3, 7.15f, 10));
                                voltages.Add(new Voltage("Vcore", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("Power Fan", 2));
                                fans.Add(new Fan("System Fan #2", 3));

                                break;

                            case Model.P67A_UD3_B3: // IT8728F
                            case Model.P67A_UD3R_B3: // IT8728F
                            case Model.P67A_UD4_B3: // IT8728F
                            case Model.Z68AP_D3: // IT8728F
                            case Model.Z68X_UD3H_B3: // IT8728F
                            case Model.Z68XP_UD3R: // IT8728F
                                voltages.Add(new Voltage("CPU Termination", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+12V", 2, 30.9f, 10));
                                voltages.Add(new Voltage("+5V", 3, 7.15f, 10));
                                voltages.Add(new Voltage("Vcore", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #2", 1));
                                fans.Add(new Fan("Power Fan", 2));
                                fans.Add(new Fan("System Fan #1", 3));
                                break;

                            case Model.Z68X_UD7_B3: // IT8728F
                                voltages.Add(new Voltage("CPU Termination", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.49f, 10));
                                voltages.Add(new Voltage("+12V", 2, 30.9f, 10));
                                voltages.Add(new Voltage("+5V", 3, 7.15f, 10));
                                voltages.Add(new Voltage("Vcore", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 1));
                                temps.Add(new Temperature("System #3", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("Power Fan", 1));
                                fans.Add(new Fan("System Fan #1", 2));
                                fans.Add(new Fan("System Fan #2", 3));
                                fans.Add(new Fan("System Fan #3", 4));
                                break;

                            case Model.X79_UD3: // IT8728F
                                voltages.Add(new Voltage("CPU Termination", 0));
                                voltages.Add(new Voltage("DIMM A/B", 1));
                                voltages.Add(new Voltage("+12V", 2, 10, 2));
                                voltages.Add(new Voltage("+5V", 3, 15, 10));
                                voltages.Add(new Voltage("VIN4", 4));
                                voltages.Add(new Voltage("VCore", 5));
                                voltages.Add(new Voltage("DIMM C/D", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 1, 1));
                                voltages.Add(new Voltage("CMOS Battery", 8, 1, 1));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 1));
                                temps.Add(new Temperature("Northbridge", 2));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                break;

                            case Model.B550_AORUS_MASTER:
                            case Model.B550_AORUS_PRO:
                            case Model.B550_AORUS_PRO_AC:
                            case Model.B550_AORUS_PRO_AX:
                            case Model.B550_VISION_D:
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("External #1", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("Chipset", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.B550_AORUS_ELITE:
                            case Model.B550_AORUS_ELITE_AX:
                            case Model.B550_GAMING_X:
                            case Model.B550_UD_AC:
                            case Model.B550M_AORUS_PRO:
                            case Model.B550M_AORUS_PRO_AX:
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("System #2", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("Chipset", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.B550I_AORUS_PRO_AX:
                            case Model.B550M_AORUS_ELITE:
                            case Model.B550M_GAMING:
                            case Model.B550M_DS3H:
                            case Model.B550M_DS3H_AC:
                            case Model.B550M_S2H:
                            case Model.B550M_H:
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("Vcore SoC", 4, 0, 1));
                                voltages.Add(new Voltage("VDDP", 5, 0, 1));
                                voltages.Add(new Voltage("DIMM", 6, 0, 1));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("VSoC MOS", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("Chipset", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                break;

                            case Model.B660_DS3H_DDR4:
                            case Model.B660_DS3H_AC_DDR4:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("+3.3V", 1, 6.5F, 10));
                                voltages.Add(new Voltage("+12V", 2, 5, 1));
                                voltages.Add(new Voltage("+5V", 3, 1.5F, 1));
                                voltages.Add(new Voltage("iGPU", 4));
                                voltages.Add(new Voltage("CPU Input Auxiliary", 5));
                                voltages.Add(new Voltage("DIMM", 6));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System #1", 0));
                                temps.Add(new Temperature("Chipset", 1));
                                temps.Add(new Temperature("CPU", 2));
                                temps.Add(new Temperature("PCIe x16", 3));
                                temps.Add(new Temperature("VRM MOS", 4));
                                temps.Add(new Temperature("System #2", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 1));
                                fans.Add(new Fan("System Fan #2", 2));
                                fans.Add(new Fan("System Fan #3 / Pump", 3));
                                fans.Add(new Fan("CPU Optional Fan", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 1));
                                controls.Add(new Control("System Fan #2", 2));
                                controls.Add(new Control("System Fan #3 / Pump", 3));
                                controls.Add(new Control("CPU Optional Fan", 4));
                                break;

                            case Model.B660M_DS3H_AX_DDR4:
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("VAXG", 1));
                                voltages.Add(new Voltage("CPU Input Auxiliary", 2));
                                voltages.Add(new Voltage("DIMM A/B", 3));
                                voltages.Add(new Voltage("+12V", 4));
                                voltages.Add(new Voltage("+3.3V", 5));
                                voltages.Add(new Voltage("+5V", 6));
                                temps.Add(new Temperature("CPU", 0));
                                temps.Add(new Temperature("PCH", 1));
                                temps.Add(new Temperature("PCIEX16", 2));
                                temps.Add(new Temperature("System #1", 3));
                                temps.Add(new Temperature("System #2", 4));
                                temps.Add(new Temperature("VRAM MOS", 5));
                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("System Fan #1", 2));
                                fans.Add(new Fan("System Fan #2", 3));
                                fans.Add(new Fan("System Fan #3", 4));
                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("System Fan #1", 2));
                                controls.Add(new Control("System Fan #2", 3));
                                controls.Add(new Control("System Fan #3", 4));
                                break;

                            default:
                                voltages.Add(new Voltage("Voltage #1", 0, true));
                                voltages.Add(new Voltage("Voltage #2", 1, true));
                                voltages.Add(new Voltage("Voltage #3", 2, true));
                                voltages.Add(new Voltage("Voltage #4", 3, true));
                                voltages.Add(new Voltage("Voltage #5", 4, true));
                                voltages.Add(new Voltage("Voltage #6", 5, true));
                                voltages.Add(new Voltage("Voltage #7", 6, true));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10, 0, true));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                                for (int i = 0; i < superIo.Temperatures.Length; i++)
                                {
                                    temps.Add(new Temperature("Temperature #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;
                        }

                        break;
                    case Manufacturer.Biostar:
                        switch (model)
                        {
                            case Model.B660GTN: //IT8613E
                                                // This board has some problems with their app controlling fans that I was able to replicate here so I guess is a BIOS problem with the pins.
                                                // Biostar is aware so expect changes in the control pins with new bios.
                                                // In the meantime, it's possible to control CPUFAN and CPUOPT1m but not SYSFAN1.
                                                // The parameters are extracted from the Biostar app config file.
                                voltages.Add(new Voltage("Vcore", 0, 0, 1));
                                voltages.Add(new Voltage("DIMM", 1, 0, 1));
                                voltages.Add(new Voltage("+12V", 2, 5, 1)); // Reads higher than it should.
                                voltages.Add(new Voltage("+5V", 3, 147, 100)); // Reads higher than it should.
                                                                        // Commented because I don't know if it makes sense.
                                                                        //voltages.Add(new Voltage("VCC ST", 4)); // Reads 4.2V.
                                                                        //voltages.Add(new Voltage("CPU Input Auxiliary", 5)); // Reads 2.2V.
                                                                        //voltages.Add(new Voltage("CPU GT", 6)); // Reads 2.6V.
                                                                        //voltages.Add(new Voltage("+3V Standby", 7, 10, 10)); // Reads 5.8V ?
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10)); // Reads higher than it should at 3.4V.
                                temps.Add(new Temperature("System 1", 0));
                                temps.Add(new Temperature("System 2", 1)); // Not sure what sensor is this.
                                temps.Add(new Temperature("CPU", 2));
                                fans.Add(new Fan("CPU Fan", 1));
                                fans.Add(new Fan("CPU Optional fan", 2));
                                fans.Add(new Fan("System Fan", 4));
                                controls.Add(new Control("CPU Fan", 1));
                                controls.Add(new Control("CPU Optional Fan", 2));
                                controls.Add(new Control("System Fan", 4));
                                break;

                            case Model.X670E_Valkyrie: //IT8625E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("DIMM", 1));
                                voltages.Add(new Voltage("+12V", 2, 10, 2));
                                // Voltage of unknown use
                                voltages.Add(new Voltage("Voltage #4", 3, true));
                                // The biostar utility shows CPU MISC Voltage.
                                voltages.Add(new Voltage("Voltage #5", 4));
                                voltages.Add(new Voltage("VDDP", 5));
                                voltages.Add(new Voltage("VSOC", 6));

                                temps.Add(new Temperature("CPU", 0));
                                temps.Add(new Temperature("VRM", 1));
                                temps.Add(new Temperature("System", 2));

                                fans.Add(new Fan("CPU Fan", 0));
                                fans.Add(new Fan("CPU Optional Fan", 1));
                                for (int i = 2; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan($"System Fan #{i - 1}", i));
                                }

                                controls.Add(new Control("CPU Fan", 0));
                                controls.Add(new Control("CPU Optional Fan", 1));
                                for (int i = 2; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control($"System Fan #{i - 1}", i));
                                }

                                break;

                            default:
                                voltages.Add(new Voltage("Voltage #1", 0, true));
                                voltages.Add(new Voltage("Voltage #2", 1, true));
                                voltages.Add(new Voltage("Voltage #3", 2, true));
                                voltages.Add(new Voltage("Voltage #4", 3, true));
                                voltages.Add(new Voltage("Voltage #5", 4, true));
                                voltages.Add(new Voltage("Voltage #6", 5, true));
                                voltages.Add(new Voltage("Voltage #7", 6, true));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10, 0, true));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                                for (int i = 0; i < superIo.Temperatures.Length; i++)
                                {
                                    temps.Add(new Temperature("Temperature #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }
                                break;
                        }

                        break;
                    case Manufacturer.Shuttle:
                        switch (model)
                        {
                            case Model.FH67: // IT8772E
                                voltages.Add(new Voltage("Vcore", 0));
                                voltages.Add(new Voltage("DIMM", 1));
                                voltages.Add(new Voltage("PCH VCCIO", 2));
                                voltages.Add(new Voltage("CPU VCCIO", 3));
                                voltages.Add(new Voltage("Graphics", 4));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                                temps.Add(new Temperature("System", 0));
                                temps.Add(new Temperature("CPU", 1));
                                fans.Add(new Fan("Fan #1", 0));
                                fans.Add(new Fan("CPU Fan", 1));
                                break;

                            default:
                                voltages.Add(new Voltage("Voltage #1", 0, true));
                                voltages.Add(new Voltage("Voltage #2", 1, true));
                                voltages.Add(new Voltage("Voltage #3", 2, true));
                                voltages.Add(new Voltage("Voltage #4", 3, true));
                                voltages.Add(new Voltage("Voltage #5", 4, true));
                                voltages.Add(new Voltage("Voltage #6", 5, true));
                                voltages.Add(new Voltage("Voltage #7", 6, true));
                                voltages.Add(new Voltage("+3V Standby", 7, 10, 10, 0, true));
                                voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                                for (int i = 0; i < superIo.Temperatures.Length; i++)
                                {
                                    temps.Add(new Temperature("Temperature #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Fans.Length; i++)
                                {
                                    fans.Add(new Fan("Fan #" + (i + 1), i));
                                }

                                for (int i = 0; i < superIo.Controls.Length; i++)
                                {
                                    controls.Add(new Control("Fan #" + (i + 1), i));
                                }

                                break;
                        }

                        break;
                    default:
                        voltages.Add(new Voltage("Voltage #1", 0, true));
                        voltages.Add(new Voltage("Voltage #2", 1, true));
                        voltages.Add(new Voltage("Voltage #3", 2, true));
                        voltages.Add(new Voltage("Voltage #4", 3, true));
                        voltages.Add(new Voltage("Voltage #5", 4, true));
                        voltages.Add(new Voltage("Voltage #6", 5, true));
                        voltages.Add(new Voltage("Voltage #7", 6, true));
                        voltages.Add(new Voltage("+3V Standby", 7, 10, 10, 0, true));
                        voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                        for (int i = 0; i < superIo.Temperatures.Length; i++)
                        {
                            temps.Add(new Temperature("Temperature #" + (i + 1), i));
                        }

                        for (int i = 0; i < superIo.Fans.Length; i++)
                        {
                            fans.Add(new Fan("Fan #" + (i + 1), i));
                        }

                        for (int i = 0; i < superIo.Controls.Length; i++)
                        {
                            controls.Add(new Control("Fan #" + (i + 1), i));
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
            Model model,
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
                        case Model.X570_AORUS_MASTER: // IT879XE
                        case Model.X570_AORUS_PRO:
                        case Model.X570_AORUS_ULTRA:
                        case Model.B550_AORUS_MASTER:
                        case Model.B550_AORUS_PRO:
                        case Model.B550_AORUS_PRO_AC:
                        case Model.B550_AORUS_PRO_AX:
                        case Model.B550_VISION_D:
                            voltages.Add(new Voltage("VIN0", 0));
                            voltages.Add(new Voltage("DDRVTT AB", 1));
                            voltages.Add(new Voltage("Chipset Core", 2));
                            voltages.Add(new Voltage("Voltage #4", 3, true));
                            voltages.Add(new Voltage("CPU VDD18", 4));
                            voltages.Add(new Voltage("PM_CLDO12", 5));
                            voltages.Add(new Voltage("Voltage #7", 6, true));
                            voltages.Add(new Voltage("+3V Standby", 7, 1f, 1f));
                            voltages.Add(new Voltage("CMOS Battery", 8, 1f, 1f));
                            temps.Add(new Temperature("PCIe x8", 0));
                            temps.Add(new Temperature("External #2", 1));
                            temps.Add(new Temperature("System #2", 2));
                            fans.Add(new Fan("System Fan #5 / Pump", 0));
                            fans.Add(new Fan("System Fan #6 / Pump", 1));
                            fans.Add(new Fan("System Fan #4", 2));
                            controls.Add(new Control("System Fan #5 / Pump", 0));
                            controls.Add(new Control("System Fan #6 / Pump", 1));
                            controls.Add(new Control("System Fan #4", 2));
                            break;

                        case Model.X470_AORUS_GAMING_7_WIFI: // ITE IT8792
                            voltages.Add(new Voltage("VIN0", 0, 0, 1));
                            voltages.Add(new Voltage("DDR VTT", 1, 0, 1));
                            voltages.Add(new Voltage("Chipset Core", 2, 0, 1));
                            voltages.Add(new Voltage("VIN3", 3, 0, 1));
                            voltages.Add(new Voltage("CPU VDD18", 4, 0, 1));
                            voltages.Add(new Voltage("Chipset Core +2.5V", 5, 0.5F, 1));
                            voltages.Add(new Voltage("+3V Standby", 6, 1, 10));
                            voltages.Add(new Voltage("CMOS Battery", 7, 0.7F, 1));
                            temps.Add(new Temperature("PCIe x8", 0));
                            temps.Add(new Temperature("System #2", 2));

                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan("Fan #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control("Fan #" + (i + 1), i));
                            }
                            break;

                        case Model.Z390_AORUS_PRO: // IT879XE
                            voltages.Add(new Voltage("VCore", 0));
                            voltages.Add(new Voltage("DDRVTT AB", 1));
                            voltages.Add(new Voltage("Chipset Core", 2));
                            voltages.Add(new Voltage("VIN3", 3, true));
                            voltages.Add(new Voltage("VCCIO", 4));
                            voltages.Add(new Voltage("Voltage #7", 5, true));
                            voltages.Add(new Voltage("DDR VPP", 6));
                            voltages.Add(new Voltage("+3V Standby", 7, 1f, 1f));
                            voltages.Add(new Voltage("CMOS Battery", 8, 1f, 1f));
                            temps.Add(new Temperature("PCIe x8", 0));
                            temps.Add(new Temperature("External #2", 1));
                            temps.Add(new Temperature("System #2", 2));
                            fans.Add(new Fan("System Fan #5 / Pump", 0));
                            fans.Add(new Fan("System Fan #6 / Pump", 1));
                            fans.Add(new Fan("System Fan #4", 2));
                            controls.Add(new Control("System Fan #5 / Pump", 0));
                            controls.Add(new Control("System Fan #6 / Pump", 1));
                            controls.Add(new Control("System Fan #4", 2));
                            break;

                        case Model.Z790_AORUS_PRO_X: // ITE IT87952E
                        case Model.Z690_AORUS_PRO:
                        case Model.Z690_AORUS_MASTER: // ITE IT87952E
                            voltages.Add(new Voltage("Vcore", 0));
                            voltages.Add(new Voltage("DIMM I/O", 1));
                            voltages.Add(new Voltage("Chipset +0.82V", 2));
                            voltages.Add(new Voltage("Voltage #4", 3, true));
                            voltages.Add(new Voltage("CPU System Agent", 4));
                            voltages.Add(new Voltage("Chipset +1.8V", 5));
                            voltages.Add(new Voltage("Voltage #7", 6, true));
                            voltages.Add(new Voltage("+3V Standby", 7, 10, 10));
                            voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));
                            temps.Add(new Temperature("PCIe x4", 0));
                            temps.Add(new Temperature("External #2", 1));
                            temps.Add(new Temperature("System #2", 2));
                            fans.Add(new Fan("System Fan #5 / Pump", 0));
                            fans.Add(new Fan("System Fan #6 / Pump", 1));
                            fans.Add(new Fan("System Fan #4", 2));
                            controls.Add(new Control("System Fan #5 / Pump", 0));
                            controls.Add(new Control("System Fan #6 / Pump", 1));
                            controls.Add(new Control("System Fan #4", 2));
                            break;

                        default:
                            voltages.Add(new Voltage("Voltage #1", 0, true));
                            voltages.Add(new Voltage("Voltage #2", 1, true));
                            voltages.Add(new Voltage("Voltage #3", 2, true));
                            voltages.Add(new Voltage("Voltage #4", 3, true));
                            voltages.Add(new Voltage("Voltage #5", 4, true));
                            voltages.Add(new Voltage("Voltage #6", 5, true));
                            voltages.Add(new Voltage("Voltage #7", 6, true));
                            voltages.Add(new Voltage("+3V Standby", 7, 10, 10, 0, true));
                            voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                            for (int i = 0; i < superIo.Temperatures.Length; i++)
                            {
                                temps.Add(new Temperature("Temperature #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Fans.Length; i++)
                            {
                                fans.Add(new Fan("Fan #" + (i + 1), i));
                            }

                            for (int i = 0; i < superIo.Controls.Length; i++)
                            {
                                controls.Add(new Control("Fan #" + (i + 1), i));
                            }
                            break;
                    }

                    break;
                default:
                    voltages.Add(new Voltage("Voltage #1", 0, true));
                    voltages.Add(new Voltage("Voltage #2", 1, true));
                    voltages.Add(new Voltage("Voltage #3", 2, true));
                    voltages.Add(new Voltage("Voltage #4", 3, true));
                    voltages.Add(new Voltage("Voltage #5", 4, true));
                    voltages.Add(new Voltage("Voltage #6", 5, true));
                    voltages.Add(new Voltage("Voltage #7", 6, true));
                    voltages.Add(new Voltage("+3V Standby", 7, 10, 10, 0, true));
                    voltages.Add(new Voltage("CMOS Battery", 8, 10, 10));

                    for (int i = 0; i < superIo.Temperatures.Length; i++)
                    {
                        temps.Add(new Temperature("Temperature #" + (i + 1), i));
                    }

                    for (int i = 0; i < superIo.Fans.Length; i++)
                    {
                        fans.Add(new Fan("Fan #" + (i + 1), i));
                    }

                    for (int i = 0; i < superIo.Controls.Length; i++)
                    {
                        controls.Add(new Control("Fan #" + (i + 1), i));
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
            voltages.Add(new Voltage("Vcore", 0));
            voltages.Add(new Voltage("+3.3V", 2));
            voltages.Add(new Voltage("+12V", 4, 30, 10));
            voltages.Add(new Voltage("+5V", 5, 6.8f, 10));
            voltages.Add(new Voltage("CMOS Battery", 8));
            temps.Add(new Temperature("CPU", 0));
            temps.Add(new Temperature("Motherboard", 1));
            fans.Add(new Fan("CPU Fan", 0));
            fans.Add(new Fan("Chassis Fan #1", 1));

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
            fans.Add(new Fan("Chassis Fan #2", 2));
            fans.Add(new Fan("Chassis Fan #3", 3));
            fans.Add(new Fan("Power Fan", 4));

            readFan = index =>
            {
                if (index < 2) return superIo.Fans[index];

                // get GPIO 80-87
                byte? gpio = superIo.ReadGpio(7);
                if (!gpio.HasValue) return null;

                // read the last 3 fans based on GPIO 83-85
                int[] masks = { 0x05, 0x03, 0x06 };
                return ((gpio.Value >> 3) & 0x07) == masks[index - 2] ? superIo.Fans[2] : null;
            };

            int fanIndex = 0;

            postUpdate = () =>
            {
                // get GPIO 80-87
                byte? gpio = superIo.ReadGpio(7);
                if (!gpio.HasValue) return;

                // prepare the GPIO 83-85 for the next update
                int[] masks = { 0x05, 0x03, 0x06 };
                superIo.WriteGpio(7, (byte)((gpio.Value & 0xC7) | (masks[fanIndex] << 3)));
                fanIndex = (fanIndex + 1) % 3;
            };
        }
    }
}
