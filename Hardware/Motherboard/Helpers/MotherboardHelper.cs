using System.Linq;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Helpers
{
    /// <summary>
    /// Motherboard Helper
    /// </summary>
    public static class MotherboardHelper
    {
        /// <summary>
        /// Determines whether [is intel series700] [the specified model].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isDdr4">if set to <c>true</c> [is DDR4].</param>
        /// <returns>
        ///   <c>true</c> if [is intel series700] [the specified model]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIntelSeries700(this MotherboardModel model, bool isDdr4 = false)
            => model.IsIntelSeriesBoard(["B760", "Z790"], isDdr4);

        /// <summary>
        /// Determines whether [is intel series600] [the specified model].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isDdr4">if set to <c>true</c> [is DDR4].</param>
        /// <returns>
        ///   <c>true</c> if [is intel series600] [the specified model]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIntelSeries600(this MotherboardModel model, bool isDdr4 = false)
            => model.IsIntelSeriesBoard(["H610", "B660", "W680", "Z690"], isDdr4);

        /// <summary>
        /// Determines whether [is intel series500] [the specified model].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        ///   <c>true</c> if [is intel series500] [the specified model]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIntelSeries500(this MotherboardModel model)
            => model.IsIntelSeriesBoard(["H510", "B560", "Z590"]);

        /// <summary>
        /// Determines whether [is intel series400] [the specified model].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        ///   <c>true</c> if [is intel series400] [the specified model]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIntelSeries400(this MotherboardModel model)
            => model.IsIntelSeriesBoard(["H410", "B460", "Z490"]);

        /// <summary>
        /// Determines whether [is intel series board] [the specified model].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="boards">The boards.</param>
        /// <param name="isDdr4">if set to <c>true</c> [is DDR4].</param>
        /// <returns>
        ///   <c>true</c> if [is intel series board] [the specified model]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsIntelSeriesBoard(this MotherboardModel model, string[] boards, bool isDdr4 = false)
        {
            var modelName = model.ToString();
            string[] ddr4 = ["_D4", "_DDR4"];
            return !isDdr4
                ? boards.Any(modelName.Contains)
                : boards.Any(modelName.Contains) && ddr4.Any(modelName.Contains);
        }
    }
}
