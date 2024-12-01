using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    [StructLayout(LayoutKind.Sequential)]
public struct NvmeIdentifyControllerData
{
    /// <summary>
    /// byte 0:1 M - PCI Vendor ID (VID)
    /// </summary>
    public ushort VID;

    /// <summary>
    /// byte 2:3 M - PCI Subsystem Vendor ID (SSVID)
    /// </summary>
    public ushort SSVID;

    /// <summary>
    /// byte 4: 23 M - Serial Number (SN)
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] SN;

    /// <summary>
    /// byte 24:63 M - Model Number (MN)
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
    public byte[] MN;

    /// <summary>
    /// byte 64:71 M - Firmware Revision (FR)
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] FR;

    /// <summary>
    /// byte 72 M - Recommended Arbitration Burst (RAB)
    /// </summary>
    public byte RAB;

    /// <summary>
    /// byte 73:75 M - IEEE OUI Identifier (IEEE). Controller Vendor code.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public byte[] IEEE;

    /// <summary>
    /// byte 76 O - Controller Multi-Path I/O and Namespace Sharing Capabilities (CMIC)
    /// </summary>
    public byte CMIC;

    /// <summary>
    /// byte 77 M - Maximum Data Transfer Size (MDTS)
    /// </summary>
    public byte MDTS;

    /// <summary>
    /// byte 78:79 M - Controller ID (CNTLID)
    /// </summary>
    public ushort CNTLID;

    /// <summary>
    /// byte 80:83 M - Version (VER)
    /// </summary>
    public uint VER;

    /// <summary>
    /// byte 84:87 M - RTD3 Resume Latency (RTD3R)
    /// </summary>
    public uint RTD3R;

    /// <summary>
    /// byte 88:91 M - RTD3 Entry Latency (RTD3E)
    /// </summary>
    public uint RTD3E;

    /// <summary>
    /// byte 92:95 M - Optional Asynchronous Events Supported (OAES)
    /// </summary>
    public uint OAES;

    /// <summary>
    /// byte 96:239.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144)]
    public byte[] Reserved0;

    /// <summary>
    /// byte 240:255.  Refer to the NVMe Management Interface Specification for definition.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] ReservedForManagement;

    /// <summary>
    /// byte 256:257 M - Optional Admin Command Support (OACS)
    /// </summary>
    public ushort OACS;

    /// <summary>
    /// byte 258 M - Abort Command Limit (ACL)
    /// </summary>
    public byte ACL;

    /// <summary>
    /// byte 259 M - Asynchronous Event Request Limit (AERL)
    /// </summary>
    public byte AERL;

    /// <summary>
    /// byte 260 M - Firmware Updates (FRMW)
    /// </summary>
    public byte FRMW;

    /// <summary>
    /// byte 261 M - Log Page Attributes (LPA)
    /// </summary>
    public byte LPA;

    /// <summary>
    /// byte 262 M - Error Log Page Entries (ELPE)
    /// </summary>
    public byte ELPE;

    /// <summary>
    /// byte 263 M - Number of Power States Support (NPSS)
    /// </summary>
    public byte NPSS;

    /// <summary>
    /// byte 264 M - Admin Vendor Specific Command Configuration (AVSCC)
    /// </summary>
    public byte AVSCC;

    /// <summary>
    /// byte 265 O - Autonomous Power State Transition Attributes (APSTA)
    /// </summary>
    public byte APSTA;

    /// <summary>
    /// byte 266:267 M - Warning Composite Temperature Threshold (WCTEMP)
    /// </summary>
    public ushort WCTEMP;

    /// <summary>
    /// byte 268:269 M - Critical Composite Temperature Threshold (CCTEMP)
    /// </summary>
    public ushort CCTEMP;

    /// <summary>
    /// byte 270:271 O - Maximum Time for Firmware Activation (MTFA)
    /// </summary>
    public ushort MTFA;

    /// <summary>
    /// byte 272:275 O - Host Memory Buffer Preferred Size (HMPRE)
    /// </summary>
    public uint HMPRE;

    /// <summary>
    /// byte 276:279 O - Host Memory Buffer Minimum Size (HMMIN)
    /// </summary>
    public uint HMMIN;

    /// <summary>
    /// byte 280:295 O - Total NVM Capacity (TNVMCAP)
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] TNVMCAP;

    /// <summary>
    /// byte 296:311 O - Unallocated NVM Capacity (UNVMCAP)
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] UNVMCAP;

    /// <summary>
    /// byte 312:315 O - Replay Protected Memory Block Support (RPMBS)
    /// </summary>
    public uint RPMBS;

    /// <summary>
    /// byte 316:511
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 196)]
    public byte[] Reserved1;

    /// <summary>
    /// byte 512 M - Submission Queue Entry Size (SQES)
    /// </summary>
    public byte SQES;

    /// <summary>
    /// byte 513 M - Completion Queue Entry Size (CQES)
    /// </summary>
    public byte CQES;

    /// <summary>
    /// byte 514:515
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public byte[] Reserved2;

    /// <summary>
    /// byte 516:519 M - Number of Namespaces (NN)
    /// </summary>
    public uint NN;

    /// <summary>
    /// byte 520:521 M - Optional NVM Command Support (ONCS)
    /// </summary>
    public ushort ONCS;

    /// <summary>
    /// byte 522:523 M - Fused Operation Support (FUSES)
    /// </summary>
    public ushort FUSES;

    /// <summary>
    /// byte 524 M - Format NVM Attributes (FNA)
    /// </summary>
    public byte FNA;

    /// <summary>
    /// byte 525 M - Volatile Write Cache (VWC)
    /// </summary>
    public byte VWC;

    /// <summary>
    /// byte 526:527 M - Atomic Write Unit Normal (AWUN)
    /// </summary>
    public ushort AWUN;

    /// <summary>
    /// byte 528:529 M - Atomic Write Unit Power Fail (AWUPF)
    /// </summary>
    public ushort AWUPF;

    /// <summary>
    /// byte 530 M - NVM Vendor Specific Command Configuration (NVSCC)
    /// </summary>
    public byte NVSCC;

    /// <summary>
    /// byte 531
    /// </summary>
    public byte Reserved3;

    /// <summary>
    /// byte 532:533 O - Atomic Compare and Write Unit (ACWU)
    /// </summary>
    public ushort ACWU;

    /// <summary>
    /// byte 534:535
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public byte[] Reserved4;

    /// <summary>
    /// byte 536:539 O - SGL Support (SGLS)
    /// </summary>
    public uint SGLS;

    /// <summary>
    /// byte 540:703
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 164)]
    public byte[] Reserved5;

    /// <summary>
    /// byte 704:2047
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1344)]
    public byte[] Reserved6;

    /// <summary>
    /// byte 2048:3071 Power State Descriptors
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public NvmePowerStateDesc[] PDS;

    /// <summary>
    /// byte 3072:4095 Vendor Specific
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
    public byte[] VS;
}
}
