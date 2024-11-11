using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC
{
    /// <summary>
    /// Motherboard Info
    /// </summary>
    internal struct BoardInfo
    {
        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>
        /// The models.
        /// </value>
        public Model[] Models { get; }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <value>
        /// The family.
        /// </value>
        public BoardFamily Family { get; }

        /// <summary>
        /// Gets the sensors.
        /// </summary>
        /// <value>
        /// The sensors.
        /// </value>
        public ECSensor[] Sensors { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardInfo"/> struct.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="family">The family.</param>
        /// <param name="sensors">The sensors.</param>
        public BoardInfo(Model[] models, BoardFamily family, params ECSensor[] sensors)
        {
            Models = models;
            Family = family;
            Sensors = sensors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardInfo"/> struct.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="family">The family.</param>
        /// <param name="sensors">The sensors.</param>
        public BoardInfo(Model model, BoardFamily family, params ECSensor[] sensors)
        {
            Models = new[] { model };
            Family = family;
            Sensors = sensors;
        }
    }
}
