using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions
{
    /// <summary>
    /// Bad Configuration Exception
    /// </summary>
    /// <seealso cref="Exception" />
    public class BadConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BadConfigurationException(string message) : base(message) { }
    }
}
