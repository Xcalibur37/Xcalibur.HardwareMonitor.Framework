using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DiskPerformance
    {
        /// <nodoc />
        public ulong BytesRead;

        /// <nodoc />
        public ulong BytesWritten;

        /// <nodoc />
        public ulong ReadTime;

        /// <nodoc />
        public ulong WriteTime;

        /// <nodoc />
        public ulong IdleTime;

        /// <nodoc />
        public uint ReadCount;

        /// <nodoc />
        public uint WriteCount;

        /// <nodoc />
        public uint QueueDepth;

        /// <nodoc />
        public uint SplitCount;

        /// <nodoc />
        public ulong QueryTime;

        /// <nodoc />
        public int StorageDeviceNumber;

        /// <nodoc />
        public long StorageManagerName0;

        /// <nodoc />
        public long StorageManagerName1;
    }
}
