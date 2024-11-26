using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios.Information
{
    /// <summary>
    /// System enclosure obtained from the SMBIOS table.
    /// </summary>
    public class SystemEnclosure : InformationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemEnclosure"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="strings">The strings.</param>
        internal SystemEnclosure(byte[] data, IList<string> strings) : base(data, strings)
        {
            ManufacturerName = GetString(0x04).Trim();
            Version = GetString(0x06).Trim();
            SerialNumber = GetString(0x07).Trim();
            AssetTag = GetString(0x08).Trim();
            RackHeight = GetByte(0x11);
            PowerCords = GetByte(0x12);
            SKU = GetString(0x15).Trim();
            LockDetected = (GetByte(0x05) & 128) == 128;
            Type = (SystemEnclosureType)(GetByte(0x05) & 127);
            BootUpState = (SystemEnclosureState)GetByte(0x09);
            PowerSupplyState = (SystemEnclosureState)GetByte(0x0A);
            ThermalState = (SystemEnclosureState)GetByte(0x0B);
            SecurityStatus = (SystemEnclosureSecurityStatus)GetByte(0x0C);
        }

        /// <summary>
        /// Gets the asset tag associated with the enclosure or chassis.
        /// </summary>
        public string AssetTag { get; }

        /// <summary>
        /// Gets <inheritdoc cref="SystemEnclosureState" />
        /// </summary>
        public SystemEnclosureState BootUpState { get; }

        /// <summary>
        /// Gets or sets the system enclosure lock.
        /// </summary>
        /// <returns>System enclosure lock is present if <see langword="true" />. Otherwise, either a lock is not present or it is unknown if the enclosure has a lock.</returns>
        public bool LockDetected { get; set; }

        /// <summary>
        /// Gets the string describing the chassis or enclosure manufacturer name.
        /// </summary>
        public string ManufacturerName { get; }

        /// <summary>
        /// Gets the number of power cords associated with the enclosure or chassis.
        /// </summary>
        public byte PowerCords { get; }

        /// <summary>
        /// Gets the state of the enclosure’s power supply (or supplies) when last booted.
        /// </summary>
        public SystemEnclosureState PowerSupplyState { get; }

        /// <summary>
        /// Gets the height of the enclosure, in 'U's. A U is a standard unit of measure for the height of a rack or rack-mountable component and is equal to 1.75 inches or 4.445 cm. A value of <c>0</c>
        /// indicates that the enclosure height is unspecified.
        /// </summary>
        public byte RackHeight { get; }

        /// <summary>
        /// Gets the physical security status of the enclosure when last booted.
        /// </summary>
        public SystemEnclosureSecurityStatus SecurityStatus { get; set; }

        /// <summary>
        /// Gets the string describing the chassis or enclosure serial number.
        /// </summary>
        public string SerialNumber { get; }

        /// <summary>
        /// Gets the string describing the chassis or enclosure SKU number.
        /// </summary>
        public string SKU { get; }

        /// <summary>
        /// Gets the thermal state of the enclosure when last booted.
        /// </summary>
        public SystemEnclosureState ThermalState { get; }

        /// <summary>
        /// Gets <inheritdoc cref="Type" />
        /// </summary>
        public SystemEnclosureType Type { get; }

        /// <summary>
        /// Gets the number of null-terminated string representing the chassis or enclosure version.
        /// </summary>
        public string Version { get; }
    }
}
