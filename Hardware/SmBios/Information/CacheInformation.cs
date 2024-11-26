using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios.Information
{
    /// <summary>
    /// Cache information obtained from the SMBIOS table.
    /// </summary>
    public class CacheInformation : InformationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheInformation"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="strings">The strings.</param>
        internal CacheInformation(byte[] data, IList<string> strings) : base(data, strings)
        {
            Handle = GetWord(0x02);
            Designation = GetCacheDesignation();
            Associativity = (CacheAssociativity)GetByte(0x12);
            Size = GetWord(0x09);
        }

        /// <summary>
        /// Gets <inheritdoc cref="CacheAssociativity" />
        /// </summary>
        public CacheAssociativity Associativity { get; }

        /// <summary>
        /// Gets <inheritdoc cref="CacheDesignation" />
        /// </summary>
        public CacheDesignation Designation { get; }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public ushort Handle { get; }

        /// <summary>
        /// Gets the value that represents the installed cache size.
        /// </summary>
        public ushort Size { get; }

        /// <summary>
        /// Gets the cache designation.
        /// </summary>
        /// <returns><see cref="CacheDesignation" />.</returns>
        private CacheDesignation GetCacheDesignation()
        {
            string rawCacheType = GetString(0x04);

            if (rawCacheType.Contains("L1"))
            {
                return CacheDesignation.L1;
            }

            if (rawCacheType.Contains("L2"))
            {
                return CacheDesignation.L2;
            }

            if (rawCacheType.Contains("L3"))
            {
                return CacheDesignation.L3;
            }

            return CacheDesignation.Other;
        }
    }
}
