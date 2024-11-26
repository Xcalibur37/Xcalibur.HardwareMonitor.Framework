using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios.Information
{
    /// <summary>
    /// Processor information obtained from the SMBIOS table.
    /// </summary>
    public class ProcessorInformation : InformationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorInformation"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="strings">The strings.</param>
        internal ProcessorInformation(byte[] data, IList<string> strings) : base(data, strings)
        {
            SocketDesignation = GetString(0x04).Trim();
            ManufacturerName = GetString(0x07).Trim();
            Version = GetString(0x10).Trim();
            CoreCount = GetByte(0x23) != 255 ? GetByte(0x23) : GetWord(0x2A);
            CoreEnabled = GetByte(0x24) != 255 ? GetByte(0x24) : GetWord(0x2C);
            ThreadCount = GetByte(0x25) != 255 ? GetByte(0x25) : GetWord(0x2E);
            ExternalClock = GetWord(0x12);
            MaxSpeed = GetWord(0x14);
            CurrentSpeed = GetWord(0x16);
            Serial = GetString(0x20).Trim();
            Id = GetQword(0x08);
            Handle = GetWord(0x02);

            byte characteristics1 = GetByte(0x26);
            byte characteristics2 = GetByte(0x27);

            Characteristics = ProcessorCharacteristics.None;
            if (IsBitSet(characteristics1, 2))
            {
                Characteristics |= ProcessorCharacteristics._64BitCapable;
            }

            if (IsBitSet(characteristics1, 3))
            {
                Characteristics |= ProcessorCharacteristics.MultiCore;
            }

            if (IsBitSet(characteristics1, 4))
            {
                Characteristics |= ProcessorCharacteristics.HardwareThread;
            }

            if (IsBitSet(characteristics1, 5))
            {
                Characteristics |= ProcessorCharacteristics.ExecuteProtection;
            }

            if (IsBitSet(characteristics1, 6))
            {
                Characteristics |= ProcessorCharacteristics.EnhancedVirtualization;
            }

            if (IsBitSet(characteristics1, 7))
            {
                Characteristics |= ProcessorCharacteristics.PowerPerformanceControl;
            }

            if (IsBitSet(characteristics2, 0))
            {
                Characteristics |= ProcessorCharacteristics._128BitCapable;
            }

            ProcessorType = (ProcessorType)GetByte(0x05);
            Socket = (ProcessorSocket)GetByte(0x19);

            int family = GetByte(0x06);
            Family = (ProcessorFamily)(family == 254 ? GetWord(0x28) : family);

            L1CacheHandle = GetWord(0x1A);
            L2CacheHandle = GetWord(0x1C);
            L3CacheHandle = GetWord(0x1E);
        }

        /// <summary>
        /// Gets the characteristics of the processor.
        /// </summary>
        public ProcessorCharacteristics Characteristics { get; }

        /// <summary>
        /// Gets the value that represents the number of cores per processor socket.
        /// </summary>
        public ushort CoreCount { get; }

        /// <summary>
        /// Gets the value that represents the number of enabled cores per processor socket.
        /// </summary>
        public ushort CoreEnabled { get; }

        /// <summary>
        /// Gets the value that represents the current processor speed (in MHz).
        /// </summary>
        public ushort CurrentSpeed { get; }

        /// <summary>
        /// Gets the external Clock Frequency, in MHz. If the value is unknown, the field is set to 0.
        /// </summary>
        public ushort ExternalClock { get; }

        /// <summary>
        /// Gets <inheritdoc cref="ProcessorFamily" />
        /// </summary>
        public ProcessorFamily Family { get; }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <value>The handle.</value>
        public ushort Handle { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// Gets the L1 cache handle.
        /// </summary>
        public ushort L1CacheHandle { get; }

        /// <summary>
        /// Gets the L2 cache handle.
        /// </summary>
        public ushort L2CacheHandle { get; }

        /// <summary>
        /// Gets the L3 cache handle.
        /// </summary>
        public ushort L3CacheHandle { get; }

        /// <summary>
        /// Gets the string number of Processor Manufacturer.
        /// </summary>
        public string ManufacturerName { get; }

        /// <summary>
        /// Gets the value that represents the maximum processor speed (in MHz) supported by the system for this processor socket.
        /// </summary>
        public ushort MaxSpeed { get; }

        /// <summary>
        /// Gets <inheritdoc cref="Framework.Hardware.SmBios.ProcessorType" />
        /// </summary>
        public ProcessorType ProcessorType { get; }

        /// <summary>
        /// Gets the value that represents the string number for the serial number of this processor.
        /// <para>This value is set by the manufacturer and normally not changeable.</para>
        /// </summary>
        public string Serial { get; }

        /// <summary>
        /// Gets <inheritdoc cref="ProcessorSocket" />
        /// </summary>
        public ProcessorSocket Socket { get; }

        /// <summary>
        /// Gets the string number for Reference Designation.
        /// </summary>
        public string SocketDesignation { get; }

        /// <summary>
        /// Gets the value that represents the number of threads per processor socket.
        /// </summary>
        public ushort ThreadCount { get; }

        /// <summary>
        /// Gets the value that represents the string number describing the Processor.
        /// </summary>
        public string Version { get; }


        /// <summary>
        /// Determines whether [is bit set] [the specified b].
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="pos">The position.</param>
        /// <returns>
        ///   <c>true</c> if [is bit set] [the specified b]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsBitSet(byte b, int pos) => (b & (1 << pos)) != 0;
    }
}