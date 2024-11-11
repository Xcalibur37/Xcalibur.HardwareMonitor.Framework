namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo
{
    /// <summary>
    /// Super I/O Delegates
    /// </summary>
    internal class SuperIoDelegates
    {
        /// <summary>
        /// Read Value: Delegate
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        internal delegate float? ReadValueDelegate(int index);

        /// <summary>
        /// Update: Delegate
        /// </summary>
        internal delegate void UpdateDelegate();
    }
}
