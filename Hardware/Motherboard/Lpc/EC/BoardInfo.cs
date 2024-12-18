﻿using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

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
        public MotherboardModel[] Models { get; }

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
        public EcSensor[] Sensors { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardInfo"/> struct.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="family">The family.</param>
        /// <param name="sensors">The sensors.</param>
        public BoardInfo(MotherboardModel[] models, BoardFamily family, params EcSensor[] sensors)
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
        public BoardInfo(MotherboardModel model, BoardFamily family, params EcSensor[] sensors)
        {
            Models = [model];
            Family = family;
            Sensors = sensors;
        }
    }
}
