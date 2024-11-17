using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo.Nuvoton
{
    /// <summary>
    /// Temperature Source Data
    /// </summary>
    public readonly struct TemperatureSourceData
    {
        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public Enum Source { get; }

        /// <summary>
        /// Gets the register.
        /// </summary>
        /// <value>
        /// The register.
        /// </value>
        public ushort Register { get; }

        /// <summary>
        /// Gets the half register.
        /// </summary>
        /// <value>
        /// The half register.
        /// </value>
        public ushort HalfRegister { get; }

        /// <summary>
        /// Gets the half bit.
        /// </summary>
        /// <value>
        /// The half bit.
        /// </value>
        public int HalfBit { get; }

        /// <summary>
        /// Gets the source register.
        /// </summary>
        /// <value>
        /// The source register.
        /// </value>
        public ushort SourceRegister { get; }

        /// <summary>
        /// Gets the alternate register.
        /// </summary>
        /// <value>
        /// The alternate register.
        /// </value>
        public ushort? AlternateRegister { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperatureSourceData"/> struct.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="register">The register.</param>
        /// <param name="halfRegister">The half register.</param>
        /// <param name="halfBit">The half bit.</param>
        /// <param name="sourceRegister">The source register.</param>
        /// <param name="alternateRegister">The alternate register.</param>
        public TemperatureSourceData(Enum source, ushort register, ushort halfRegister = 0, int halfBit = -1, ushort sourceRegister = 0, ushort? alternateRegister = null)
        {
            Source = source;
            Register = register;
            HalfRegister = halfRegister;
            HalfBit = halfBit;
            SourceRegister = sourceRegister;
            AlternateRegister = alternateRegister;
        }
    }
}
