using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IoControlCode
    {
        /// <summary>
        /// Gets the resulting IO control code.
        /// </summary>
        public uint Code { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoControlCode" /> struct.
        /// </summary>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="function">The function.</param>
        /// <param name="access">The access.</param>
        public IoControlCode(uint deviceType, uint function, Access access) : this(deviceType, function, Method.Buffered, access)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoControlCode" /> struct.
        /// </summary>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="function">The function.</param>
        /// <param name="method">The method.</param>
        /// <param name="access">The access.</param>
        public IoControlCode(uint deviceType, uint function, Method method, Access access)
        {
            Code = (deviceType << 16) | ((uint)access << 14) | (function << 2) | (uint)method;
        }
    }
}
