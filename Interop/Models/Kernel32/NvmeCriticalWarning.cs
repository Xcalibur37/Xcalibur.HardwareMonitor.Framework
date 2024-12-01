using System;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [Flags]
    public enum NvmeCriticalWarning
    {
        None = 0x00,

        /// <summary>
        /// If set to 1, then the available spare space has fallen below the threshold.
        /// </summary>
        AvailableSpaceLow = 0x01,

        /// <summary>
        /// If set to 1, then a temperature is above an over temperature threshold or below an under temperature threshold.
        /// </summary>
        TemperatureThreshold = 0x02,

        /// <summary>
        /// If set to 1, then the device reliability has been degraded due to significant media related errors or any public error that degrades device reliability.
        /// </summary>
        ReliabilityDegraded = 0x04,

        /// <summary>
        /// If set to 1, then the media has been placed in read only mode
        /// </summary>
        ReadOnly = 0x08,

        /// <summary>
        /// If set to 1, then the volatile memory backup device has failed. This field is only valid if the controller has a volatile memory backup solution.
        /// </summary>
        VolatileMemoryBackupDeviceFailed = 0x10
    }
}
