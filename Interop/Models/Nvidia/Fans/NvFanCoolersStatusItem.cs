using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Nvidia.Fans
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    internal struct NvFanCoolersStatusItem : IEquatable<NvFanCoolersStatusItem>
    {
        public uint CoolerId;
        public uint CurrentRpm;
        public uint CurrentMinLevel;
        public uint CurrentMaxLevel;
        public uint CurrentLevel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U4)]
        private readonly uint[] _reserved;

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(NvFanCoolersStatusItem other) => 
            CoolerId == other.CoolerId && 
            CurrentRpm == other.CurrentRpm && 
            CurrentMinLevel == other.CurrentMinLevel && 
            CurrentMaxLevel == other.CurrentMaxLevel && 
            CurrentLevel == other.CurrentLevel && 
            Equals(_reserved, other._reserved);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) 
            => obj is NvFanCoolersStatusItem other && Equals(other);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() 
            => HashCode.Combine(CoolerId, CurrentRpm, CurrentMinLevel, CurrentMaxLevel, CurrentLevel, _reserved);
    }
}
