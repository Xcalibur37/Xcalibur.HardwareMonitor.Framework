﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.Gigabyte;

/// <summary>
/// This is a controller present on some Gigabyte motherboards for both Intel and AMD, that is in custom firmware
/// loaded onto the 2nd ITE EC.
/// It can be accessed by using memory mapped IO, mapping its internal RAM onto main RAM via the ISA Bridge.
/// This class can disable it so that the regular IT87XX code can drive the fans.
/// </summary>
internal class IsaBridgeGigabyteController : IGigabyteController
{
    #region Fields

    private const uint ControllerAddressRange = 0xFF;
    private const int ControllerEnableRegister = 0x47;
    private const uint ControllerFanControlArea = 0x900;

    /// <summary>
    /// Base address in PCI RAM that maps to the EC's RAM
    /// </summary>
    private readonly uint _controllerBaseAddress;
    private readonly CpuVendor _vendor;
    private bool? _initialState;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsaBridgeGigabyteController"/> class.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <param name="vendor">The vendor.</param>
    public IsaBridgeGigabyteController(uint address, CpuVendor vendor)
    {
        _controllerBaseAddress = address;
        _vendor = vendor;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Enable/Disable Fan Control
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns>true on success</returns>
    public bool Enable(bool enabled)
    {
        // TODO: Intel
        return _vendor switch
        {
            CpuVendor.Amd => AmdEnable(enabled),
            _ => false
        };
    }

    /// <summary>
    /// Restore settings back to initial values
    /// </summary>
    public void Restore()
    {
        if (!_initialState.HasValue) return;
        Enable(_initialState.Value);
    }

    /// <summary>
    /// Enables AMD-related device
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <returns></returns>
    private bool AmdEnable(bool enabled)
    {
        if (!Mutexes.WaitPciBus(10)) return false;

        // see D14F3x https://www.amd.com/system/files/TechDocs/55072_AMD_Family_15h_Models_70h-7Fh_BKDG.pdf
        uint amdIsaBridgeAddress = Ring0.GetPciAddress(0x0, 0x14, 0x3);

        const uint ioOrMemoryPortDecodeEnableRegister = 0x48;
        const uint memoryRangePortEnableMask = 0x1 << 5;
        const uint pciMemoryAddressForLpcTargetCyclesRegister = 0x60;
        const uint romAddressRange2Register = 0x6C;

        uint controllerFanControlAddress = _controllerBaseAddress + ControllerFanControlArea;

        uint pciAddressStart = _controllerBaseAddress >> 0x10;
        uint pciAddressEnd = pciAddressStart + 1;

        uint enabledPciMemoryAddressRegister = pciAddressEnd << 0x10 | pciAddressStart;
        uint enabledRomAddressRegister = 0xFFFFU << 0x10 | pciAddressEnd;

        Ring0.ReadPciConfig(amdIsaBridgeAddress, ioOrMemoryPortDecodeEnableRegister, out uint originalDecodeEnableRegister);
        Ring0.ReadPciConfig(amdIsaBridgeAddress, pciMemoryAddressForLpcTargetCyclesRegister, out uint originalPciMemoryAddressRegister);
        Ring0.ReadPciConfig(amdIsaBridgeAddress, romAddressRange2Register, out uint originalRomAddressRegister);

        bool originalMmIoEnabled = (originalDecodeEnableRegister & memoryRangePortEnableMask) != 0 &&
                                   originalPciMemoryAddressRegister == enabledPciMemoryAddressRegister &&
                                   originalRomAddressRegister == enabledRomAddressRegister;

        if (!originalMmIoEnabled)
        {
            Ring0.WritePciConfig(amdIsaBridgeAddress, ioOrMemoryPortDecodeEnableRegister, originalDecodeEnableRegister | memoryRangePortEnableMask);
            Ring0.WritePciConfig(amdIsaBridgeAddress, pciMemoryAddressForLpcTargetCyclesRegister, enabledPciMemoryAddressRegister);
            Ring0.WritePciConfig(amdIsaBridgeAddress, romAddressRange2Register, enabledRomAddressRegister);
        }

        bool result = Enable(enabled, new IntPtr(controllerFanControlAddress));

        // Restore previous values
        if (!originalMmIoEnabled)
        {
            Ring0.WritePciConfig(amdIsaBridgeAddress, ioOrMemoryPortDecodeEnableRegister, originalDecodeEnableRegister);
            Ring0.WritePciConfig(amdIsaBridgeAddress, pciMemoryAddressForLpcTargetCyclesRegister, originalPciMemoryAddressRegister);
            Ring0.WritePciConfig(amdIsaBridgeAddress, romAddressRange2Register, originalRomAddressRegister);
        }

        Mutexes.ReleasePciBus();

        return result;
    }

    /// <summary>
    /// Enables a device as the specified address.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <param name="pciMmIoBaseAddress">The pci mm io base address.</param>
    /// <returns></returns>
    private bool Enable(bool enabled, IntPtr pciMmIoBaseAddress)
    {
        // Map PCI memory to this process memory
        if (!InpOut.Open()) return false;

        IntPtr mapped = InpOut.MapMemory(pciMmIoBaseAddress, ControllerAddressRange, out IntPtr handle);
        if (mapped == IntPtr.Zero) return false;

        bool current = Convert.ToBoolean(Marshal.ReadByte(mapped, ControllerEnableRegister));
        _initialState ??= current;

        // Update Controller State
        if (current != enabled)
        {
            Marshal.WriteByte(mapped, ControllerEnableRegister, Convert.ToByte(enabled));
            // Give it some time to see the change
            Thread.Sleep(500);
        }

        InpOut.UnmapMemory(handle, mapped);
        return true;
    }

    #endregion
}
