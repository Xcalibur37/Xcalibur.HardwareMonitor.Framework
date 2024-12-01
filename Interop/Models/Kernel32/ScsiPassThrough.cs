using System;
using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScsiPassThrough
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort Length;

        public byte ScsiStatus;
        public byte PathId;
        public byte TargetId;
        public byte Lun;
        public byte CdbLength;
        public byte SenseInfoLength;
        public byte DataIn;
        public uint DataTransferLength;
        public uint TimeOutValue;
        public IntPtr DataBufferOffset;
        public uint SenseInfoOffset;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Cdb;
    }
}
