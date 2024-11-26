﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

internal class KernelDriver
{
    private readonly string _driverId;
    private readonly string _serviceName;
    private SafeFileHandle _device;

    public KernelDriver(string serviceName, string driverId)
    {
        _serviceName = serviceName;
        _driverId = driverId;
    }

    public bool IsOpen => _device != null;

    public bool Install(string path, out string errorMessage)
    {
        nint manager = AdvApi32.OpenSCManager(null, null, AdvApi32.SC_MANAGER_ACCESS_MASK.SC_MANAGER_CREATE_SERVICE);
        if (manager == nint.Zero)
        {
            int errorCode = Marshal.GetLastWin32Error();
            errorMessage = $"OpenSCManager returned the error code: {errorCode:X8}.";
            return false;
        }

        nint service = AdvApi32.CreateService(manager,
                                                _serviceName,
                                                _serviceName,
                                                AdvApi32.SERVICE_ACCESS_MASK.SERVICE_ALL_ACCESS,
                                                AdvApi32.SERVICE_TYPE.SERVICE_KERNEL_DRIVER,
                                                AdvApi32.SERVICE_START.SERVICE_DEMAND_START,
                                                AdvApi32.SERVICE_ERROR.SERVICE_ERROR_NORMAL,
                                                path,
                                                null,
                                                null,
                                                null,
                                                null,
                                                null);

        if (service == nint.Zero)
        {
            int errorCode = Marshal.GetLastWin32Error();
            if (errorCode == Kernel32.ERROR_SERVICE_EXISTS)
            {
                errorMessage = "Service already exists";
                return false;
            }

            errorMessage = $"CreateService returned the error code: {errorCode:X8}.";
            AdvApi32.CloseServiceHandle(manager);
            return false;
        }

        if (!AdvApi32.StartService(service, 0, null))
        {
            int errorCode = Marshal.GetLastWin32Error();
            if (errorCode != Kernel32.ERROR_SERVICE_ALREADY_RUNNING)
            {
                errorMessage = $"StartService returned the error code: {errorCode:X8}.";
                AdvApi32.CloseServiceHandle(service);
                AdvApi32.CloseServiceHandle(manager);
                return false;
            }
        }

        AdvApi32.CloseServiceHandle(service);
        AdvApi32.CloseServiceHandle(manager);

        try
        {
            // restrict the driver access to system (SY) and builtin admins (BA)
            // TODO: replace with a call to IoCreateDeviceSecure in the driver
            FileInfo fileInfo = new(@"\\.\" + _driverId);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();
            fileSecurity.SetSecurityDescriptorSddlForm("O:BAG:SYD:(A;;FA;;;SY)(A;;FA;;;BA)");
            fileInfo.SetAccessControl(fileSecurity);
        }
        catch
        { }

        errorMessage = null;
        return true;
    }

    public bool Open()
    {
        nint fileHandle = Kernel32.CreateFile(@"\\.\" + _driverId, 0xC0000000, FileShare.None, nint.Zero, FileMode.Open, FileAttributes.Normal, nint.Zero);

        _device = new SafeFileHandle(fileHandle, true);
        if (_device.IsInvalid)
            Close();

        return _device != null;
    }

    public bool DeviceIOControl(Kernel32.IOControlCode ioControlCode, object inBuffer)
    {
        return _device != null && Kernel32.DeviceIoControl(_device, ioControlCode, inBuffer, inBuffer == null ? 0 : (uint)Marshal.SizeOf(inBuffer), null, 0, out uint _, nint.Zero);
    }

    public bool DeviceIOControl<T>(Kernel32.IOControlCode ioControlCode, object inBuffer, ref T outBuffer)
    {
        if (_device == null)
            return false;

        object boxedOutBuffer = outBuffer;
        bool b = Kernel32.DeviceIoControl(_device,
                                          ioControlCode,
                                          inBuffer,
                                          inBuffer == null ? 0 : (uint)Marshal.SizeOf(inBuffer),
                                          boxedOutBuffer,
                                          (uint)Marshal.SizeOf(boxedOutBuffer),
                                          out uint _,
                                          nint.Zero);

        outBuffer = (T)boxedOutBuffer;
        return b;
    }

    public bool DeviceIOControl<T>(Kernel32.IOControlCode ioControlCode, object inBuffer, ref T[] outBuffer)
    {
        if (_device == null)
            return false;

        object boxedOutBuffer = outBuffer;
        bool b = Kernel32.DeviceIoControl(_device,
                                          ioControlCode,
                                          inBuffer,
                                          inBuffer == null ? 0 : (uint)Marshal.SizeOf(inBuffer),
                                          boxedOutBuffer,
                                          (uint)(Marshal.SizeOf(typeof(T)) * outBuffer.Length),
                                          out uint _,
                                          nint.Zero);

        outBuffer = (T[])boxedOutBuffer;
        return b;
    }

    public void Close()
    {
        if (_device != null)
        {
            _device.Close();
            _device.Dispose();
            _device = null;
        }
    }

    public bool Delete()
    {
        nint manager = AdvApi32.OpenSCManager(null, null, AdvApi32.SC_MANAGER_ACCESS_MASK.SC_MANAGER_CONNECT);
        if (manager == nint.Zero)
            return false;

        nint service = AdvApi32.OpenService(manager, _serviceName, AdvApi32.SERVICE_ACCESS_MASK.SERVICE_ALL_ACCESS);
        if (service == nint.Zero)
        {
            AdvApi32.CloseServiceHandle(manager);
            return true;
        }

        AdvApi32.SERVICE_STATUS status = new();
        AdvApi32.ControlService(service, AdvApi32.SERVICE_CONTROL.SERVICE_CONTROL_STOP, ref status);
        AdvApi32.DeleteService(service);
        AdvApi32.CloseServiceHandle(service);
        AdvApi32.CloseServiceHandle(manager);

        return true;
    }
}
