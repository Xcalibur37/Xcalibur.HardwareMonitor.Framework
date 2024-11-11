using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo
{
    /// <summary>
    /// Default Configurations.
    /// </summary>
    internal class DefaultConfigurations
    {
        /// <summary>
        /// Gets the default voltages.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="voltages">The voltages.</param>
        internal static void GetVoltages(ISuperIo superIo, IList<Voltage> voltages)
        {
            for (int i = 0; i < superIo.Voltages.Length; i++)
            {
                voltages.Add(new Voltage(string.Format(SuperIoConstants.VoltageVoltNumber,  i + 1), i, true));
            }
        }

        /// <summary>
        /// Gets the default temperatures.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="temps">The temps.</param>
        internal static void GetTemps(ISuperIo superIo, IList<Temperature> temps)
        {
            for (int i = 0; i < superIo.Temperatures.Length; i++)
            {
                temps.Add(new Temperature(string.Format(SuperIoConstants.TempNumber,  i + 1), i));
            }
        }

        /// <summary>
        /// Gets the default fans.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="fans">The fans.</param>
        internal static void GetFans(ISuperIo superIo, IList<Fan> fans)
        {
            for (int i = 0; i < superIo.Fans.Length; i++)
            {
                fans.Add(new Fan(string.Format(SuperIoConstants.FanNumber,  i + 1), i));
            }
        }

        /// <summary>
        /// Gets the default controls.
        /// </summary>
        /// <param name="superIo">The super io.</param>
        /// <param name="controls">The controls.</param>
        internal static void GetControls(ISuperIo superIo, ICollection<Control> controls)
        {
            for (int i = 0; i < superIo.Controls.Length; i++)
            {
                controls.Add(new Control(string.Format(SuperIoConstants.FanControlNumber,  i + 1), i));
            }
        }
    }
}
