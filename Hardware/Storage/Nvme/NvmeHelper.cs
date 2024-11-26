using System;
using System.Text;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme
{
    /// <summary>
    /// NVME Helper
    /// </summary>
    internal class NvmeHelper
    {
        /// <summary>
        /// Delegate: Gets the sensor value
        /// </summary>
        /// <param name="health">The health.</param>
        /// <returns></returns>
        internal delegate float GetSensorValue(NvmeHealthInfo health);

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        internal static string GetString(byte[] s) => Encoding.ASCII.GetString(s).Trim('\t', '\n', '\r', ' ', '\0');

        /// <summary>
        /// Kelvins to celsius.
        /// </summary>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        internal static short KelvinToCelsius(ushort k) => (short)(k > 0 ? k - 273 : short.MinValue);

        /// <summary>
        /// Kelvins to celsius.
        /// </summary>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        internal static short KelvinToCelsius(byte[] k) => KelvinToCelsius(BitConverter.ToUInt16(k, 0));
    }
}
