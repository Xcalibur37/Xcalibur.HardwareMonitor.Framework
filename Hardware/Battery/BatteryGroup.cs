using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Battery;

/// <summary>
/// Battery Group
/// </summary>
/// <seealso cref="IGroup" />
internal class BatteryGroup : IGroup
{
    private readonly ISettings _settings;
    private readonly List<Battery> _hardware = [];

    /// <inheritdoc />
    public IReadOnlyList<IHardware> Hardware => _hardware;

    /// <summary>
    /// Initializes a new instance of the <see cref="BatteryGroup"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public BatteryGroup(ISettings settings)
    {
        _settings = settings;

        // No implementation for battery information on Unix systems
        if (Software.OperatingSystem.IsUnix) return;

        // Process batteries
        ProcessBatteries();
    }

    /// <inheritdoc />
    public void Close()
    {
        _hardware.Apply(x => x.Close());
    }

    private unsafe void ProcessBatteries()
    {
        // Get handle
        var handle = SetupApi.SetupDiGetClassDevs(ref SetupApi.GuidDeviceBattery, IntPtr.Zero,
            IntPtr.Zero, SetupApi.DigcfPresent | SetupApi.DigcfDeviceinterface);
        if (handle == SetupApi.InvalidHandleValue) return;

        // Cycle through each device
        for (uint i = 0; ; i++)
        {
            SpDeviceInterfaceData interfaceData = default;
            interfaceData.cbSize = (uint)Marshal.SizeOf(typeof(SpDeviceInterfaceData));

            // Get device
            if (!SetupApi.SetupDiEnumDeviceInterfaces(handle,
                    IntPtr.Zero,
                    ref SetupApi.GuidDeviceBattery,
                    i,
                    ref interfaceData))
            {
                if (Marshal.GetLastWin32Error() == SetupApi.ErrorNoMoreItems) break;
            }
            else
            {
                // Get details
                SetupApi.SetupDiGetDeviceInterfaceDetail(handle,
                    interfaceData,
                    IntPtr.Zero,
                    0,
                    out uint interfaceDetailDataSize,
                    IntPtr.Zero);
                if (Marshal.GetLastWin32Error() != SetupApi.ErrorInsufficientBuffer) continue;

                // Interface detail data
                var interfaceDetailData = Kernel32.LocalAlloc(Kernel32.Lptr, interfaceDetailDataSize);
                Marshal.WriteInt32(interfaceDetailData, Environment.Is64BitProcess ? 8 : 4 + Marshal.SystemDefaultCharSize);

                // Device interface details
                if (SetupApi.SetupDiGetDeviceInterfaceDetail(handle,
                    interfaceData,
                    interfaceDetailData,
                    interfaceDetailDataSize,
                    out _,
                    IntPtr.Zero))
                {
                    // Get battery file handle
                    string devicePath = new((char*)(interfaceDetailData + 4));
                    var batteryFileHandle = Kernel32.CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite,
                        IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);

                    if (!batteryFileHandle.IsInvalid)
                    {
                        BatteryQueryInformation queryInfo = default;

                        // Get battery tag
                        uint inputBuffer = 0;
                        if (Kernel32.DeviceIoControl(batteryFileHandle,
                            IoCtl.IoctlBatteryQueryTag,
                            ref inputBuffer,
                            Marshal.SizeOf(inputBuffer),
                            ref queryInfo.BatteryTag,
                            Marshal.SizeOf(queryInfo.BatteryTag),
                            out _,
                            IntPtr.Zero))
                        {
                            BatteryInformation batteryInfo = default;
                            queryInfo.InformationLevel = BatteryQueryInformationLevel.BatteryInformation;

                            // Get battery information
                            if (Kernel32.DeviceIoControl(batteryFileHandle,
                                IoCtl.IoctlBatteryQueryInformation,
                                ref queryInfo,
                                Marshal.SizeOf(queryInfo),
                                ref batteryInfo,
                                Marshal.SizeOf(batteryInfo),
                                out _,
                                IntPtr.Zero))
                            {
                                // Get valid batteries
                                if (batteryInfo.Capabilities.HasFlag(BatteryCapabilities.BATTERY_SYSTEM_BATTERY))
                                {
                                    const int maxLoadString = 100;
                                    queryInfo.InformationLevel = BatteryQueryInformationLevel.BatteryDeviceName;

                                    var name = GetDeviceName(batteryFileHandle, queryInfo, maxLoadString);
                                    var manufacturer = GetManufacturerName(batteryFileHandle, queryInfo, maxLoadString);

                                    _hardware.Add(new Battery(name, manufacturer, batteryFileHandle, batteryInfo, queryInfo.BatteryTag, _settings));
                                }
                            }
                        }
                    }
                }

                // Free interface detail data
                Kernel32.LocalFree(interfaceDetailData);
            }
        }

        // Free main handle
        SetupApi.SetupDiDestroyDeviceInfoList(handle);
    }

    /// <summary>
    /// Gets the name of the device.
    /// </summary>
    /// <param name="batteryFileHandle">The battery file handle.</param>
    /// <param name="queryInfo">The query information.</param>
    /// <param name="maxLoadString">The maximum load string.</param>
    /// <returns></returns>
    private string GetDeviceName(SafeFileHandle batteryFileHandle, BatteryQueryInformation queryInfo, int maxLoadString)
    {
        string name = "";
        IntPtr pointer = Marshal.AllocCoTaskMem(maxLoadString);

        try
        {
            queryInfo.InformationLevel = BatteryQueryInformationLevel.BatteryDeviceName;

            // Get device name
            if (Kernel32.DeviceIoControl(batteryFileHandle,
                IoCtl.IoctlBatteryQueryInformation,
                ref queryInfo,
                Marshal.SizeOf(queryInfo),
                pointer,
                maxLoadString,
                out _,
                IntPtr.Zero))
            {
                name = Marshal.PtrToStringUni(pointer);
            }
        }
        catch
        {
            // Do nothing
        }
        finally
        {
            // Free manufacturer pointer
            Marshal.FreeCoTaskMem(pointer);
        }

        return name;
    }

    /// <summary>
    /// Gets the name of the manufacturer.
    /// </summary>
    /// <param name="batteryFileHandle">The battery file handle.</param>
    /// <param name="queryInfo">The query information.</param>
    /// <param name="maxLoadString">The maximum load string.</param>
    /// <returns></returns>
    private string GetManufacturerName(SafeFileHandle batteryFileHandle, BatteryQueryInformation queryInfo, int maxLoadString)
    {
        string manufacturer = "";
        IntPtr pointer = Marshal.AllocCoTaskMem(maxLoadString);

        try
        {
            queryInfo.InformationLevel = BatteryQueryInformationLevel.BatteryManufactureName;

            // Get manufacturer name
            if (Kernel32.DeviceIoControl(batteryFileHandle,
                    IoCtl.IoctlBatteryQueryInformation,
                    ref queryInfo,
                    Marshal.SizeOf(queryInfo),
                    pointer,
                    maxLoadString,
                    out _,
                    IntPtr.Zero))
            {
                manufacturer = Marshal.PtrToStringUni(pointer);

            }
        }
        catch
        {
            // Do nothing
        }
        finally
        {
            // Free manufacturer pointer
            Marshal.FreeCoTaskMem(pointer);
        }

        return manufacturer;
    }
}
