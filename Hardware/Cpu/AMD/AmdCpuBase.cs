﻿using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD;

/// <summary>
/// AMD CPU Base
/// </summary>
/// <seealso cref="CpuBase" />
internal abstract class AmdCpuBase : CpuBase
{
    // ReSharper disable InconsistentNaming
    private const ushort AMD_VENDOR_ID = 0x1022;
    private const byte DEVICE_VENDOR_ID_REGISTER = 0;
    private const byte PCI_BASE_DEVICE = 0x18;
    private const byte PCI_BUS = 0;
    // ReSharper restore InconsistentNaming

    /// <summary>
    /// Initializes a new instance of the <see cref="AmdCpuBase"/> class.
    /// </summary>
    /// <param name="processorIndex">Index of the processor.</param>
    /// <param name="cpuId">The cpu identifier.</param>
    /// <param name="settings">The settings.</param>
    protected AmdCpuBase(int processorIndex, CpuId[][] cpuId, ISettings settings) : base(processorIndex, cpuId, settings) { }

    /// <summary>
    /// Gets the PCI address.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="deviceId">The device identifier.</param>
    /// <returns></returns>
    protected uint GetPciAddress(byte function, ushort deviceId)
    {
        // Assemble the pci address
        uint address = Ring0.GetPciAddress(PCI_BUS, (byte)(PCI_BASE_DEVICE + Index), function);

        // Verify that we have the correct bus, device and function
        if (!Ring0.ReadPciConfig(address, DEVICE_VENDOR_ID_REGISTER, out uint deviceVendor))
            return Interop.Ring0.InvalidPciAddress;

        // Get address and return
        return deviceVendor != (deviceId << 16 | AMD_VENDOR_ID)
            ? Interop.Ring0.InvalidPciAddress
            : address;
    }
}
