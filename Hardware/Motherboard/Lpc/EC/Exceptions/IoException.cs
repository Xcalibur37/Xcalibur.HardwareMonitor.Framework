namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions
{
    /// <summary>
    /// IO Exception
    /// </summary>
    /// <seealso cref="System.IO.IOException" />
    public class IoException : System.IO.IOException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IoException"/> class.
        /// </summary>
        /// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
        public IoException(string message) : base($"ACPI embedded controller I/O error: {message}") { }
    }
}
