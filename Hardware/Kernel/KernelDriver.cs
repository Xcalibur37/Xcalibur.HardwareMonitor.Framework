using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

/// <summary>
/// Kernel Driver
/// </summary>
internal class KernelDriver
{
    #region Fields

    private readonly string _driverId;
    private readonly string _serviceName;
    private SafeFileHandle _device;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether this instance is open.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
    /// </value>
    public bool IsOpen => _device != null;

    #endregion
    
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="KernelDriver"/> class.
    /// </summary>
    /// <param name="serviceName">Name of the service.</param>
    /// <param name="driverId">The driver identifier.</param>
    public KernelDriver(string serviceName, string driverId)
    {
        _serviceName = serviceName;
        _driverId = driverId;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public void Close()
    {
        if (_device == null) return;
        _device.Close();
        _device.Dispose();
        _device = null;
    }

    /// <summary>
    /// Deletes this instance.
    /// </summary>
    /// <returns></returns>
    public bool Delete()
    {
        nint manager = AdvApi32.OpenSCManager(null, null, ScManagerAccessMask.ScManagerConnect);
        if (manager == nint.Zero) return false;

        nint service = AdvApi32.OpenService(manager, _serviceName, ServiceAccessMask.ServiceAllAccess);
        if (service == nint.Zero)
        {
            AdvApi32.CloseServiceHandle(manager);
            return true;
        }

        ServiceStatus status = new();
        AdvApi32.ControlService(service, ServiceControl.ServiceControlStop, ref status);
        AdvApi32.DeleteService(service);
        AdvApi32.CloseServiceHandle(service);
        AdvApi32.CloseServiceHandle(manager);

        return true;
    }

    /// <summary>
    /// Devices the io control.
    /// </summary>
    /// <param name="ioControlCode">The io control code.</param>
    /// <param name="inBuffer">The in buffer.</param>
    /// <returns></returns>
    public bool DeviceIoControl(IoControlCode ioControlCode, object inBuffer) => 
        _device != null && Kernel32.DeviceIoControl(_device, ioControlCode, inBuffer, inBuffer == null 
            ? 0 
            : (uint)Marshal.SizeOf(inBuffer), null, 0, out _, nint.Zero);

    /// <summary>
    /// Devices the io control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ioControlCode">The io control code.</param>
    /// <param name="inBuffer">The in buffer.</param>
    /// <param name="outBuffer">The out buffer.</param>
    /// <returns></returns>
    public bool DeviceIoControl<T>(IoControlCode ioControlCode, object inBuffer, ref T outBuffer)
    {
        if (_device == null) return false;
        object boxedOutBuffer = outBuffer;
        bool b = Kernel32.DeviceIoControl(
            _device,
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

    /// <summary>
    /// Devices the io control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ioControlCode">The io control code.</param>
    /// <param name="inBuffer">The in buffer.</param>
    /// <param name="outBuffer">The out buffer.</param>
    /// <returns></returns>
    public bool DeviceIoControl<T>(IoControlCode ioControlCode, object inBuffer, ref T[] outBuffer)
    {
        if (_device == null) return false;
        object boxedOutBuffer = outBuffer;
        bool b = Kernel32.DeviceIoControl(
            _device,
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

    /// <summary>
    /// Installs the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <returns></returns>
    public bool Install(string path, out string errorMessage)
    {
        nint manager = AdvApi32.OpenSCManager(null, null, 
            ScManagerAccessMask.ScManagerCreateService);
        if (manager == nint.Zero)
        {
            int errorCode = Marshal.GetLastWin32Error();
            errorMessage = $"OpenSCManager returned the error code: {errorCode:X8}.";
            return false;
        }

        var service = AdvApi32.CreateService(
            manager,
            _serviceName,
            _serviceName,
            ServiceAccessMask.ServiceAllAccess,
            ServiceType.ServiceKernelDriver,
            ServiceStart.ServiceDemandStart,
            ServiceError.ServiceErrorNormal,
            path,
            null,
            null,
            null,
            null,
            null);

        if (service == nint.Zero)
        {
            int errorCode = Marshal.GetLastWin32Error();
            if (errorCode == Kernel32.ErrorServiceExists)
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
            if (errorCode != Kernel32.ErrorServiceAlreadyRunning)
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
            FileInfo fileInfo = new($@"\\.\{_driverId}");
#pragma warning disable CA1416
            FileSecurity fileSecurity = fileInfo.GetAccessControl();
            fileSecurity.SetSecurityDescriptorSddlForm("O:BAG:SYD:(A;;FA;;;SY)(A;;FA;;;BA)");
            fileInfo.SetAccessControl(fileSecurity);
#pragma warning restore CA1416
        }
        catch
        {
            // Do nothing
        }

        errorMessage = null;
        return true;
    }

    /// <summary>
    /// Opens this instance.
    /// </summary>
    /// <returns></returns>
    public bool Open()
    {
        nint fileHandle = Kernel32.CreateFile($@"\\.\{_driverId}", 0xC0000000, 
            FileShare.None, nint.Zero, FileMode.Open, 
            FileAttributes.Normal, nint.Zero);
        _device = new SafeFileHandle(fileHandle, true);
        if (_device.IsInvalid)
        {
            Close();
        }
        return _device != null;
    }

    #endregion
}
