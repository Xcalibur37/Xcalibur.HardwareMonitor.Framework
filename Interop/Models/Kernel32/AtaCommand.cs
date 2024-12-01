namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    public enum AtaCommand : byte
    {
        /// <summary>
        /// SMART data requested.
        /// </summary>
        AtaSmart = 0xB0,

        /// <summary>
        /// Identify data is requested.
        /// </summary>
        AtaIdentifyDevice = 0xEC
    }
}
