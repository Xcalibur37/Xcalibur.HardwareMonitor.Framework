using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NvmePowerStateDesc
    {
        /// <summary>
        /// bit 0:15 Maximum  Power (MP) in centiwatts
        /// </summary>
        public ushort MP;

        /// <summary>
        /// bit 16:23
        /// </summary>
        public byte Reserved0;

        /// <summary>
        /// bit 24 Max Power Scale (MPS), bit 25 Non-Operational State (NOPS)
        /// </summary>
        public byte MPS_NOPS;

        /// <summary>
        /// bit 32:63 Entry Latency (ENLAT) in microseconds
        /// </summary>
        public uint ENLAT;

        /// <summary>
        /// bit 64:95 Exit Latency (EXLAT) in microseconds
        /// </summary>
        public uint EXLAT;

        /// <summary>
        /// bit 96:100 Relative Read Throughput (RRT)
        /// </summary>
        public byte RRT;

        /// <summary>
        /// bit 104:108 Relative Read Latency (RRL)
        /// </summary>
        public byte RRL;

        /// <summary>
        /// bit 112:116 Relative Write Throughput (RWT)
        /// </summary>
        public byte RWT;

        /// <summary>
        /// bit 120:124 Relative Write Latency (RWL)
        /// </summary>
        public byte RWL;

        /// <summary>
        /// bit 128:143 Idle Power (IDLP)
        /// </summary>
        public ushort IDLP;

        /// <summary>
        /// bit 150:151 Idle Power Scale (IPS)
        /// </summary>
        public byte IPS;

        /// <summary>
        /// bit 152:159
        /// </summary>
        public byte Reserved7;

        /// <summary>
        /// bit 160:175 Active Power (ACTP)
        /// </summary>
        public ushort ACTP;

        /// <summary>
        /// bit 176:178 Active Power Workload (APW), bit 182:183  Active Power Scale (APS)
        /// </summary>
        public byte APW_APS;

        /// <summary>
        /// bit 184:255.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] Reserved9;
    }
}
