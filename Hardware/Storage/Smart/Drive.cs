using System;
using System.Collections.Generic;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart
{
    /// <summary>
    /// Drive
    /// </summary>
    public class Drive
    {
        /// <summary>
        /// Gets the drive attribute values.
        /// </summary>
        /// <value>
        /// The drive attribute values.
        /// </value>
        public Interop.Models.Kernel32.SmartAttribute[] DriveAttributeValues { get; }

        /// <summary>
        /// Gets the drive threshold values.
        /// </summary>
        /// <value>
        /// The drive threshold values.
        /// </value>
        public SmartThreshold[] DriveThresholdValues { get; }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <value>
        /// The firmware version.
        /// </value>
        public string FirmwareVersion { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drive"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="firmware">The firmware.</param>
        /// <param name="idBase">The identifier base.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.Exception"></exception>
        public Drive(string name, string firmware, int idBase, string value)
        {
            Name = name;
            FirmwareVersion = firmware;

            var lines = value.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            DriveAttributeValues = new Interop.Models.Kernel32.SmartAttribute[lines.Length];
            var thresholds = new List<SmartThreshold>();

            for (int i = 0; i < lines.Length; i++)
            {
                var array = lines[i].Split([' '], StringSplitOptions.RemoveEmptyEntries);

                if (array.Length is not 4 and not 5)
                {
                    throw new Exception();
                }

                var v = new Interop.Models.Kernel32.SmartAttribute { Id = Convert.ToByte(array[0], idBase), RawValue = new byte[6] };

                for (int j = 0; j < 6; j++)
                {
                    v.RawValue[j] = Convert.ToByte(array[1].Substring(2 * j, 2), 16);
                }

                v.WorstValue = Convert.ToByte(array[2], 10);
                v.CurrentValue = Convert.ToByte(array[3], 10);

                DriveAttributeValues[i] = v;

                if (array.Length != 5) continue;
                var t = new SmartThreshold { Id = v.Id, Threshold = Convert.ToByte(array[4], 10) };
                thresholds.Add(t);
            }

            DriveThresholdValues = thresholds.ToArray();
        }
    }
}
