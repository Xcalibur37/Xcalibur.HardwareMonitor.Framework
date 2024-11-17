using System.Globalization;
using System.IO;
using System.Text;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc
{
    /// <summary>
    /// LM Chip
    /// </summary>
    /// <seealso cref="ISuperIo" />
    public class LmChip : ISuperIo
    {
        #region Fields   

        private readonly FileStream[] _fanStreams;
        private readonly FileStream[] _temperatureStreams;
        private readonly FileStream[] _voltageStreams;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the chip.
        /// </summary>
        /// <value>
        /// The chip.
        /// </value>
        public Chip Chip { get; }

        /// <summary>
        /// Gets the controls.
        /// </summary>
        /// <value>
        /// The controls.
        /// </value>
        public float?[] Controls { get; }

        /// <summary>
        /// Gets the fans.
        /// </summary>
        /// <value>
        /// The fans.
        /// </value>
        public float?[] Fans { get; }

        /// <summary>
        /// Gets the temperatures.
        /// </summary>
        /// <value>
        /// The temperatures.
        /// </value>
        public float?[] Temperatures { get; }

        /// <summary>
        /// Gets the voltages.
        /// </summary>
        /// <value>
        /// The voltages.
        /// </value>
        public float?[] Voltages { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LmChip"/> class.
        /// </summary>
        /// <param name="chip">The chip.</param>
        /// <param name="path">The path.</param>
        public LmChip(Chip chip, string path)
        {
            Chip = chip;

            string[] voltagePaths = Directory.GetFiles(path, "in*_input");
            Voltages = new float?[voltagePaths.Length];
            _voltageStreams = new FileStream[voltagePaths.Length];
            for (int i = 0; i < voltagePaths.Length; i++)
            {
                _voltageStreams[i] = new FileStream(voltagePaths[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            string[] temperaturePaths = Directory.GetFiles(path, "temp*_input");
            Temperatures = new float?[temperaturePaths.Length];
            _temperatureStreams = new FileStream[temperaturePaths.Length];
            for (int i = 0; i < temperaturePaths.Length; i++)
            {
                _temperatureStreams[i] = new FileStream(temperaturePaths[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            string[] fanPaths = Directory.GetFiles(path, "fan*_input");
            Fans = new float?[fanPaths.Length];
            _fanStreams = new FileStream[fanPaths.Length];
            for (int i = 0; i < fanPaths.Length; i++)
            {
                _fanStreams[i] = new FileStream(fanPaths[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            Controls = [];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            _voltageStreams.Apply(x => x.Close());
            _temperatureStreams.Apply(x => x.Close());
            _fanStreams.Apply(x => x.Close());
        }

        /// <summary>
        /// Reads the gpio.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public byte? ReadGpio(int index) => null;

        /// <summary>
        /// Sets the control.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void SetControl(int index, byte? value) { }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < Voltages.Length; i++)
            {
                string s = ReadFirstLine(_voltageStreams[i]);
                try
                {
                    Voltages[i] = 0.001f *
                                  long.Parse(s, CultureInfo.InvariantCulture);
                }
                catch
                {
                    Voltages[i] = null;
                }
            }

            for (int i = 0; i < Temperatures.Length; i++)
            {
                string s = ReadFirstLine(_temperatureStreams[i]);
                try
                {
                    Temperatures[i] = 0.001f *
                                      long.Parse(s, CultureInfo.InvariantCulture);
                }
                catch
                {
                    Temperatures[i] = null;
                }
            }

            for (int i = 0; i < Fans.Length; i++)
            {
                string s = ReadFirstLine(_fanStreams[i]);
                try
                {
                    Fans[i] = long.Parse(s, CultureInfo.InvariantCulture);
                }
                catch
                {
                    Fans[i] = null;
                }
            }
        }

        /// <summary>
        /// Writes the gpio.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void WriteGpio(int index, byte value) { }

        /// <summary>
        /// Reads the first line.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        private static string ReadFirstLine(Stream stream)
        {
            StringBuilder sb = new();
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                int b = stream.ReadByte();
                while (b is not -1 and not 10)
                {
                    sb.Append((char)b);
                    b = stream.ReadByte();
                }
            }
            catch
            {
                // Do nothing
            }

            return sb.ToString();
        }

        #endregion
    }
}
