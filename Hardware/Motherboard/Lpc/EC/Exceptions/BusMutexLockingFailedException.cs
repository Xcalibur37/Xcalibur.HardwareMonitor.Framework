namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions
{
    /// <summary>
    /// Bus Mutex Locking Failed Exception
    /// </summary>
    /// <seealso cref="IoException" />
    public class BusMutexLockingFailedException : IoException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusMutexLockingFailedException"/> class.
        /// </summary>
        public BusMutexLockingFailedException() : base("could not lock ISA bus mutex") { }
    }
}
