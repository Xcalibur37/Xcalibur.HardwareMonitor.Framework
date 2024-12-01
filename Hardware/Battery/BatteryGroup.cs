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
    private readonly List<Battery> _hardware = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BatteryGroup"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public unsafe BatteryGroup(ISettings settings)
    {
        // No implementation for battery information on Unix systems
        if (Software.OperatingSystem.IsUnix) return;

        IntPtr hdev = SetupApi.SetupDiGetClassDevs(ref SetupApi.GuidDeviceBattery, IntPtr.Zero, IntPtr.Zero, SetupApi.DigcfPresent | SetupApi.DigcfDeviceinterface);
        if (hdev == SetupApi.InvalidHandleValue) return;
        for (uint i = 0; ; i++)
        {
            SpDeviceInterfaceData did = default;
            did.cbSize = (uint)Marshal.SizeOf(typeof(SpDeviceInterfaceData));

            if (!SetupApi.SetupDiEnumDeviceInterfaces(hdev,
                    IntPtr.Zero,
                    ref SetupApi.GuidDeviceBattery,
                    i,
                    ref did))
            {
                if (Marshal.GetLastWin32Error() == SetupApi.ErrorNoMoreItems) break;
            }
            else
            {
                SetupApi.SetupDiGetDeviceInterfaceDetail(hdev,
                    did,
                    IntPtr.Zero,
                    0,
                    out uint cbRequired,
                    IntPtr.Zero);

                if (Marshal.GetLastWin32Error() != SetupApi.ErrorInsufficientBuffer) continue;
                IntPtr pdidd = Kernel32.LocalAlloc(Kernel32.Lptr, cbRequired);
                Marshal.WriteInt32(pdidd, Environment.Is64BitProcess ? 8 : 4 + Marshal.SystemDefaultCharSize); // cbSize.

                if (SetupApi.SetupDiGetDeviceInterfaceDetail(hdev,
                        did,
                        pdidd,
                        cbRequired,
                        out _,
                        IntPtr.Zero))
                {
                    string devicePath = new((char*)(pdidd + 4));

                    SafeFileHandle battery = Kernel32.CreateFile(devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);
                    if (!battery.IsInvalid)
                    {
                        BatteryQueryInformation bqi = default;

                        uint dwWait = 0;
                        if (Kernel32.DeviceIoControl(battery,
                                IoCtl.IOCTL_BATTERY_QUERY_TAG,
                                ref dwWait,
                                Marshal.SizeOf(dwWait),
                                ref bqi.BatteryTag,
                                Marshal.SizeOf(bqi.BatteryTag),
                                out _,
                                IntPtr.Zero))
                        {
                            BatteryInformation bi = default;
                            bqi.InformationLevel = BatteryQueryInformationLevel.BatteryInformation;

                            if (Kernel32.DeviceIoControl(battery,
                                    IoCtl.IOCTL_BATTERY_QUERY_INFORMATION,
                                    ref bqi,
                                    Marshal.SizeOf(bqi),
                                    ref bi,
                                    Marshal.SizeOf(bi),
                                    out _,
                                    IntPtr.Zero))
                            {
                                // Only batteries count.
                                if (bi.Capabilities.HasFlag(BatteryCapabilities.BATTERY_SYSTEM_BATTERY))
                                {
                                    const int maxLoadString = 100;

                                    IntPtr ptrDevName = Marshal.AllocCoTaskMem(maxLoadString);
                                    bqi.InformationLevel = BatteryQueryInformationLevel.BatteryDeviceName;

                                    if (Kernel32.DeviceIoControl(battery,
                                            IoCtl.IOCTL_BATTERY_QUERY_INFORMATION,
                                            ref bqi,
                                            Marshal.SizeOf(bqi),
                                            ptrDevName,
                                            maxLoadString,
                                            out _,
                                            IntPtr.Zero))
                                    {
                                        IntPtr ptrManName = Marshal.AllocCoTaskMem(maxLoadString);
                                        bqi.InformationLevel = BatteryQueryInformationLevel.BatteryManufactureName;

                                        if (Kernel32.DeviceIoControl(battery,
                                                IoCtl.IOCTL_BATTERY_QUERY_INFORMATION,
                                                ref bqi,
                                                Marshal.SizeOf(bqi),
                                                ptrManName,
                                                maxLoadString,
                                                out _,
                                                IntPtr.Zero))
                                        {
                                            string name = Marshal.PtrToStringUni(ptrDevName);
                                            string manufacturer = Marshal.PtrToStringUni(ptrManName);

                                            _hardware.Add(new Battery(name, manufacturer, battery, bi, bqi.BatteryTag, settings));
                                        }

                                        Marshal.FreeCoTaskMem(ptrManName);
                                    }

                                    Marshal.FreeCoTaskMem(ptrDevName);
                                }
                            }
                        }
                    }
                }

                Kernel32.LocalFree(pdidd);
            }
        }

        SetupApi.SetupDiDestroyDeviceInfoList(hdev);
    }

    /// <inheritdoc />
    public IReadOnlyList<IHardware> Hardware => _hardware;

    /// <inheritdoc />
    public void Close()
    {
        _hardware.Apply(x => x.Close());
    }
}
