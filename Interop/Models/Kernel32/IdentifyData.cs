using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IdentifyData
    {
        public ushort GeneralConfiguration;
        public ushort NumberOfCylinders;
        public ushort Reserved1;
        public ushort NumberOfHeads;
        public ushort UnformattedBytesPerTrack;
        public ushort UnformattedBytesPerSector;
        public ushort SectorsPerTrack;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] VendorUnique;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] SerialNumber;

        public ushort BufferType;
        public ushort BufferSectorSize;
        public ushort NumberOfEccBytes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] FirmwareRevision;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] ModelNumber;

        public byte MaximumBlockTransfer;
        public byte VendorUnique2;
        public ushort DoubleWordIo;
        public ushort Capabilities;
        public ushort Reserved2;
        public byte VendorUnique3;
        public byte PioCycleTimingMode;
        public byte VendorUnique4;
        public byte DmaCycleTimingMode;
        public ushort TranslationFieldsValid;
        public ushort NumberOfCurrentCylinders;
        public ushort NumberOfCurrentHeads;
        public ushort CurrentSectorsPerTrack;
        public uint CurrentSectorCapacity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 197)]
        public ushort[] Reserved3;
    }
}
