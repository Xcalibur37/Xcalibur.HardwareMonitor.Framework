using System;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu;

/// <summary>
/// CPU Id
/// </summary>
public class CpuId
{
    #region Fields

    // ReSharper disable InconsistentNaming
    #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public const uint CPUID_0 = 0;
    public const uint CPUID_EXT = 0x80000000;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    // ReSharper restore InconsistentNaming

    private const int ThreadLimit = 64;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the affinity.
    /// </summary>
    /// <value>
    /// The affinity.
    /// </value>
    public GroupAffinity Affinity { get; private set; }

    /// <summary>
    /// Gets the apic identifier.
    /// </summary>
    /// <value>
    /// The apic identifier.
    /// </value>
    public uint ApicId { get; private set; }

    /// <summary>
    /// Gets the brand string.
    /// </summary>
    /// <value>
    /// The brand string.
    /// </value>
    public string BrandString { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the core identifier.
    /// </summary>
    /// <value>
    /// The core identifier.
    /// </value>
    public uint CoreId { get; private set; }

    /// <summary>
    /// Gets the data.
    /// </summary>
    /// <value>
    /// The data.
    /// </value>
    public uint[,] Data { get; private set; } = new uint[0, 0];

    /// <summary>
    /// Gets the ext data.
    /// </summary>
    /// <value>
    /// The ext data.
    /// </value>
    public uint[,] ExtData { get; private set; } = new uint[0, 0];

    /// <summary>
    /// Gets the family.
    /// </summary>
    /// <value>
    /// The family.
    /// </value>
    public uint Family { get; private set; }

    /// <summary>
    /// Gets the group.
    /// </summary>
    /// <value>
    /// The group.
    /// </value>
    public int Group { get; private set; }

    /// <summary>
    /// Gets the model.
    /// </summary>
    /// <value>
    /// The model.
    /// </value>
    public uint Model { get; private set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the type of the PKG.
    /// </summary>
    /// <value>
    /// The type of the PKG.
    /// </value>
    public uint PkgType { get; private set; }

    /// <summary>
    /// Gets the processor identifier.
    /// </summary>
    /// <value>
    /// The processor identifier.
    /// </value>
    public uint ProcessorId { get; private set; }

    /// <summary>
    /// Gets the stepping.
    /// </summary>
    /// <value>
    /// The stepping.
    /// </value>
    public uint Stepping { get; private set; }

    /// <summary>
    /// Gets the thread.
    /// </summary>
    /// <value>
    /// The thread.
    /// </value>
    public int Thread { get; private set; }

    /// <summary>
    /// Gets the thread identifier.
    /// </summary>
    /// <value>
    /// The thread identifier.
    /// </value>
    public uint ThreadId { get; private set; }

    /// <summary>
    /// Gets the vendor.
    /// </summary>
    /// <value>
    /// The vendor.
    /// </value>
    public CpuVendor Vendor { get; private set; } = CpuVendor.Unknown;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CpuId" /> class.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="thread">The thread.</param>
    /// <param name="affinity">The affinity.</param>
    private CpuId(int group, int thread, GroupAffinity affinity)
    {
        Thread = thread;
        Group = group;
        Affinity = affinity;

        // Thread must not exceed limit
        if (thread >= ThreadLimit) return;

        // Set processor attributes
        SetProcessorAttributes();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the specified <see cref="CpuId" />.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="thread">The thread.</param>
    /// <returns><see cref="CpuId" />.</returns>
    public static CpuId Get(int group, int thread)
    {
        if (thread >= ThreadLimit) return null;
        var affinity = GroupAffinity.Single((ushort)group, thread);

        GroupAffinity previousAffinity = ThreadAffinity.Set(affinity);
        if (previousAffinity == GroupAffinity.Undefined) return null;
        try
        {
            return new CpuId(group, thread, affinity);
        }
        finally
        {
            ThreadAffinity.Set(previousAffinity);
        }
    }

    /// <summary>
    /// Appends the register.
    /// </summary>
    /// <param name="b">The b.</param>
    /// <param name="value">The value.</param>
    private static void AppendRegister(StringBuilder b, uint value)
    {
        b.Append((char)(value & 0xff));
        b.Append((char)((value >> 8) & 0xff));
        b.Append((char)((value >> 16) & 0xff));
        b.Append((char)((value >> 24) & 0xff));
    }

    /// <summary>
    /// Gets the maximum cpu identifier ext.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException">thread</exception>
    private static uint GetMaxCpuIdExt()
    {
        uint result = 0;
        try
        {
            if (OpCode.CpuId(CPUID_EXT, 0, out var eax, out _, out _, out _))
            {
                result = eax > CPUID_EXT ? eax - CPUID_EXT : 0;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            // Do Nothing
        }
        return result;
    }

    /// <summary>
    /// Gets the next Log
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    private static uint NextLog2(long x)
    {
        if (x <= 0) return 0;
        x--;
        uint count = 0;
        while (x > 0)
        {
            x >>= 1;
            count++;
        }
        return count;
    }

    /// <summary>
    /// Gets core and thread masks.
    /// </summary>
    /// <param name="maxCpuid">The maximum cpuid.</param>
    /// <param name="maxCpuidExt">The maximum cpuid ext.</param>
    /// <param name="threadMaskWith">The thread mask with.</param>
    /// <param name="coreMaskWith">The core mask with.</param>
    private void GetMasks(uint maxCpuid, uint maxCpuidExt, out uint threadMaskWith, out uint coreMaskWith)
    {
        switch (Vendor)
        {
            case CpuVendor.Intel:
                uint maxCoreAndThreadIdPerPackage = (Data[1, 1] >> 16) & 0xFF;
                var maxCoreIdPerPackage = maxCpuid >= 4 ? ((Data[4, 0] >> 26) & 0x3F) + 1 : (uint)1;
                threadMaskWith = NextLog2(maxCoreAndThreadIdPerPackage / maxCoreIdPerPackage);
                coreMaskWith = NextLog2(maxCoreIdPerPackage);
                break;
            case CpuVendor.Amd:
                var corePerPackage = maxCpuidExt >= 8 ? (ExtData[8, 2] & 0xFF) + 1 : (uint)1;
                threadMaskWith = 0;
                coreMaskWith = NextLog2(corePerPackage);

                if (Family is 0x17 or 0x19)
                {
                    // ApicIdCoreIdSize: APIC ID size.
                    // cores per DIE
                    // we need this for Ryzen 5 (4 cores, 8 threads) ans Ryzen 6 (6 cores, 12 threads)
                    // Ryzen 5: [core0][core1][dummy][dummy][core2][core3] (Core0 EBX = 00080800, Core2 EBX = 08080800)
                    coreMaskWith = ((ExtData[8, 2] >> 12) & 0xF) switch
                    {
                        0x04 => NextLog2(16), // Ryzen
                        0x05 => NextLog2(32), // Threadripper
                        0x06 => NextLog2(64), // Epic
                        _ => coreMaskWith
                    };
                }

                break;
            case CpuVendor.Unknown:
            default:
                threadMaskWith = 0;
                coreMaskWith = 0;
                break;
        }
    }

    /// <summary>
    /// Sets the data.
    /// </summary>
    /// <param name="eax">The eax.</param>
    /// <returns>Max CPU id</returns>
    private uint SetData(uint eax)
    {
        var maxCpuid = Math.Min(eax, 1024);
        Data = new uint[maxCpuid + 1, 4];
        for (uint i = 0; i < maxCpuid + 1; i++)
        {
            OpCode.CpuId(
                CPUID_0 + i,
                0,
                out Data[i, 0],
                out Data[i, 1],
                out Data[i, 2],
                out Data[i, 3]);
        }
        return maxCpuid;
    }

    /// <summary>
    /// Sets the extended data.
    /// </summary>
    /// <returns>Max extended CPU id</returns>
    private uint SetExtendedData()
    {
        var maxCpuidExt = Math.Min(GetMaxCpuIdExt(), 1024);

        ExtData = new uint[maxCpuidExt + 1, 4];
        for (uint i = 0; i < maxCpuidExt + 1; i++)
        {
            OpCode.CpuId(
                CPUID_EXT + i,
                0,
                out ExtData[i, 0],
                out ExtData[i, 1],
                out ExtData[i, 2],
                out ExtData[i, 3]);
        }
        return maxCpuidExt;
    }

    /// <summary>
    /// Sets the name and brand string.
    /// </summary>
    private void SetNameAndBrandString()
    {
        StringBuilder nameBuilder = new();
        for (uint i = 2; i <= 4; i++)
        {
            if (!OpCode.CpuId(
                    CPUID_EXT + i, 
                    0, 
                    out var eax, 
                    out var ebx, 
                    out var ecx, 
                    out var edx)) continue;
            AppendRegister(nameBuilder, eax);
            AppendRegister(nameBuilder, ebx);
            AppendRegister(nameBuilder, ecx);
            AppendRegister(nameBuilder, edx);
        }

        nameBuilder.Replace('\0', ' ');

        // Brand
        BrandString = nameBuilder.ToString().Trim();

        // Name
        nameBuilder.Replace("(R)", string.Empty);
        nameBuilder.Replace("(TM)", string.Empty);
        nameBuilder.Replace("(tm)", string.Empty);
        nameBuilder.Replace("CPU", string.Empty);
        nameBuilder.Replace("Dual-Core Processor", string.Empty);
        nameBuilder.Replace("Triple-Core Processor", string.Empty);
        nameBuilder.Replace("Quad-Core Processor", string.Empty);
        nameBuilder.Replace("Six-Core Processor", string.Empty);
        nameBuilder.Replace("Eight-Core Processor", string.Empty);
        nameBuilder.Replace("64-Core Processor", string.Empty);
        nameBuilder.Replace("32-Core Processor", string.Empty);
        nameBuilder.Replace("24-Core Processor", string.Empty);
        nameBuilder.Replace("16-Core Processor", string.Empty);
        nameBuilder.Replace("12-Core Processor", string.Empty);
        nameBuilder.Replace("8-Core Processor", string.Empty);
        nameBuilder.Replace("6-Core Processor", string.Empty);

        for (int i = 0; i < 10; i++)
        {
            nameBuilder.Replace("  ", " ");
        }

        var name = nameBuilder.ToString();
        if (name.Contains($"@"))
        {
            name = name.Remove(name.LastIndexOf('@'));
        }
        Name = name.Trim();
    }

    /// <summary>
    /// Sets the processor attributes.
    /// </summary>
    private void SetProcessorAttributes()
    {
        // Get CPU 0
        OpCode.CpuId(CPUID_0, 0, out uint eax, out uint ebx, out uint ecx, out uint edx);
        if (eax <= 0) return;

        // Set Data
        var maxCpuid = SetData(eax);

        // Set ExtData
        var maxCpuidExt = SetExtendedData();

        // Vendor
        SetVendor(ebx, ecx, edx);

        // Name and Brand
        SetNameAndBrandString();

        Family = ((Data[1, 0] & 0x0FF00000) >> 20) + ((Data[1, 0] & 0x0F00) >> 8);
        Model = ((Data[1, 0] & 0x0F0000) >> 12) + ((Data[1, 0] & 0xF0) >> 4);
        Stepping = Data[1, 0] & 0x0F;
        ApicId = (Data[1, 1] >> 24) & 0xFF;
        PkgType = (ExtData[1, 1] >> 28) & 0xFF;

        // Get Core and Thread Masks
        GetMasks(maxCpuid, maxCpuidExt, out uint threadMaskWith, out uint coreMaskWith);

        // Set processor, core, and thread
        ProcessorId = ApicId >> (int)(coreMaskWith + threadMaskWith);
        CoreId = (ApicId >> (int)threadMaskWith) - (ProcessorId << (int)coreMaskWith);
        ThreadId = ApicId - (ProcessorId << (int)(coreMaskWith + threadMaskWith)) - (CoreId << (int)threadMaskWith);
    }

    /// <summary>
    /// Sets the vendor.
    /// </summary>
    /// <param name="ebx">The ebx.</param>
    /// <param name="ecx">The ecx.</param>
    /// <param name="edx">The edx.</param>
    private void SetVendor(uint ebx, uint ecx, uint edx)
    {
        StringBuilder vendorBuilder = new();
        AppendRegister(vendorBuilder, ebx);
        AppendRegister(vendorBuilder, edx);
        AppendRegister(vendorBuilder, ecx);

        Vendor = vendorBuilder.ToString() switch
        {
            "GenuineIntel" => CpuVendor.Intel,
            "AuthenticAMD" => CpuVendor.Amd,
            _ => CpuVendor.Unknown
        };
    }

    #endregion
}
