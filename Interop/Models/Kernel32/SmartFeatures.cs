namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    public enum SmartFeatures : byte
    {
        /// <summary>
        /// Read SMART data.
        /// </summary>
        SmartReadData = 0xD0,

        /// <summary>
        /// Read SMART thresholds.
        /// obsolete
        /// </summary>
        ReadThresholds = 0xD1,

        /// <summary>
        /// Autosave SMART data.
        /// </summary>
        EnableDisableAutosave = 0xD2,

        /// <summary>
        /// Save SMART attributes.
        /// </summary>
        SaveAttributeValues = 0xD3,

        /// <summary>
        /// Set SMART to offline immediately.
        /// </summary>
        ExecuteOfflineDiags = 0xD4,

        /// <summary>
        /// Read SMART log.
        /// </summary>
        SmartReadLog = 0xD5,

        /// <summary>
        /// Write SMART log.
        /// </summary>
        SmartWriteLog = 0xD6,

        /// <summary>
        /// Write SMART thresholds.
        /// obsolete
        /// </summary>
        WriteThresholds = 0xD7,

        /// <summary>
        /// Enable SMART.
        /// </summary>
        EnableSmart = 0xD8,

        /// <summary>
        /// Disable SMART.
        /// </summary>
        DisableSmart = 0xD9,

        /// <summary>
        /// Get SMART status.
        /// </summary>
        ReturnSmartStatus = 0xDA,

        /// <summary>
        /// Set SMART to offline automatically.
        /// </summary>
        EnableDisableAutoOffline = 0xDB /* obsolete */
    }
}
