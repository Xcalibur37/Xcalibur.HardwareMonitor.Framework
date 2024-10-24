// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Battery;

internal class BatteryGroup : IGroup
{
    private readonly List<Battery> _hardware = new();

    public unsafe BatteryGroup(ISettings settings)
    {
        // No implementation for battery information on Unix systems
        if (Software.OperatingSystem.IsUnix)
            return;

        IntPtr hdev = SetupApi.SetupDiGetClassDevs(ref SetupApi.GUID_DEVICE_BATTERY, IntPtr.Zero, IntPtr.Zero, SetupApi.DIGCF_PRESENT | SetupApi.DIGCF_DEVICEINTERFACE);
        if (hdev != SetupApi.INVALID_HANDLE_VALUE)
        {
            for (uint i = 0; ; i++)
            {
                SetupApi.SP_DEVICE_INTERFACE_DATA did = default;
                did.cbSize = (uint)Marshal.SizeOf(typeof(SetupApi.SP_DEVICE_INTERFACE_DATA));

                if (!SetupApi.SetupDiEnumDeviceInterfaces(hdev,
                                                          IntPtr.Zero,
                                                          ref SetupApi.GUID_DEVICE_BATTERY,
                                                          i,
                                                          ref did))
                {
                    if (Marshal.GetLastWin32Error() == SetupApi.ERROR_NO_MORE_ITEMS)
                        break;
                }
                else
                {
                    SetupApi.SetupDiGetDeviceInterfaceDetail(hdev,
                                                             did,
                                                             IntPtr.Zero,
                                                             0,
                                                             out uint cbRequired,
                                                             IntPtr.Zero);

                    if (Marshal.GetLastWin32Error() == SetupApi.ERROR_INSUFFICIENT_BUFFER)
                    {
                        IntPtr pdidd = Kernel32.LocalAlloc(Kernel32.LPTR, cbRequired);
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
                                Kernel32.BATTERY_QUERY_INFORMATION bqi = default;

                                uint dwWait = 0;
                                if (Kernel32.DeviceIoControl(battery,
                                                             Kernel32.IOCTL.IOCTL_BATTERY_QUERY_TAG,
                                                             ref dwWait,
                                                             Marshal.SizeOf(dwWait),
                                                             ref bqi.BatteryTag,
                                                             Marshal.SizeOf(bqi.BatteryTag),
                                                             out _,
                                                             IntPtr.Zero))
                                {
                                    Kernel32.BATTERY_INFORMATION bi = default;
                                    bqi.InformationLevel = Kernel32.BATTERY_QUERY_INFORMATION_LEVEL.BatteryInformation;

                                    if (Kernel32.DeviceIoControl(battery,
                                                                 Kernel32.IOCTL.IOCTL_BATTERY_QUERY_INFORMATION,
                                                                 ref bqi,
                                                                 Marshal.SizeOf(bqi),
                                                                 ref bi,
                                                                 Marshal.SizeOf(bi),
                                                                 out _,
                                                                 IntPtr.Zero))
                                    {
                                        // Only batteries count.
                                        if (bi.Capabilities.HasFlag(Kernel32.BatteryCapabilities.BATTERY_SYSTEM_BATTERY))
                                        {
                                            const int maxLoadString = 100;

                                            IntPtr ptrDevName = Marshal.AllocCoTaskMem(maxLoadString);
                                            bqi.InformationLevel = Kernel32.BATTERY_QUERY_INFORMATION_LEVEL.BatteryDeviceName;

                                            if (Kernel32.DeviceIoControl(battery,
                                                                         Kernel32.IOCTL.IOCTL_BATTERY_QUERY_INFORMATION,
                                                                         ref bqi,
                                                                         Marshal.SizeOf(bqi),
                                                                         ptrDevName,
                                                                         maxLoadString,
                                                                         out _,
                                                                         IntPtr.Zero))
                                            {
                                                IntPtr ptrManName = Marshal.AllocCoTaskMem(maxLoadString);
                                                bqi.InformationLevel = Kernel32.BATTERY_QUERY_INFORMATION_LEVEL.BatteryManufactureName;

                                                if (Kernel32.DeviceIoControl(battery,
                                                                             Kernel32.IOCTL.IOCTL_BATTERY_QUERY_INFORMATION,
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
            }

            SetupApi.SetupDiDestroyDeviceInfoList(hdev);
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<IHardware> Hardware => _hardware;

    /// <inheritdoc />
    public void Close()
    {
        foreach (Battery battery in _hardware)
            battery.Close();
    }
}
