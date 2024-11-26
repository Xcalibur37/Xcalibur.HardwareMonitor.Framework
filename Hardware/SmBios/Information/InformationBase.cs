using System;
using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios.Information
{
    /// <summary>
    /// Information Base
    /// </summary>
    public abstract class InformationBase
    {
        private readonly byte[] _data;
        private readonly IList<string> _strings;

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationBase" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="strings">The strings.</param>
        protected InformationBase(byte[] data, IList<string> strings)
        {
            _data = data;
            _strings = strings;
        }

        /// <summary>
        /// Gets the byte.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns><see cref="byte" />.</returns>
        protected byte GetByte(int offset)
        {
            if (offset < _data.Length && offset >= 0)
                return _data[offset];

            return 0;
        }

        /// <summary>
        /// Gets the word.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns><see cref="ushort" />.</returns>
        protected ushort GetWord(int offset)
        {
            if (offset + 1 < _data.Length && offset >= 0)
            {
                return BitConverter.ToUInt16(_data, offset);
            }

            return 0;
        }

        /// <summary>
        /// Gets the dword.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns><see cref="ushort" />.</returns>
        protected uint GetDword(int offset)
        {
            if (offset + 3 < _data.Length && offset >= 0)
            {
                return BitConverter.ToUInt32(_data, offset);
            }

            return 0;
        }

        /// <summary>
        /// Gets the qword.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns><see cref="ulong" />.</returns>
        protected ulong GetQword(int offset)
        {
            if (offset + 7 < _data.Length && offset >= 0)
            {
                return BitConverter.ToUInt64(_data, offset);
            }

            return 0;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns><see cref="string" />.</returns>
        protected string GetString(int offset)
        {
            if (offset < _data.Length && _data[offset] > 0 && _data[offset] <= _strings.Count)
                return _strings[_data[offset] - 1];

            return string.Empty;
        }
    }
}
