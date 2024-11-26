using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel.Models;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

/// <summary>
/// Protection Ring 0 is accessible to the kernel, which is a central part of most operating systems and can access everything.
/// Code running here is said to be running in kernel mode. Processes running in kernel mode can affect the
/// entire system; if anything fails here, it will probably result in a system shutdown. This ring has direct access
/// to the CPU and the system memory, so any instructions requiring the use of either will be executed here.
/// https://www.baeldung.com/cs/os-rings
/// </summary>
internal static class Ring0
{
    #region Fields

    private static KernelDriver _driver;
    private static string _filePath;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether this instance is open.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
    /// </value>
    public static bool IsOpen => _driver != null;

    #endregion

    #region Methods
    
    /// <summary>
    /// Closes this instance.
    /// </summary>
    public static void Close()
    {
        if (_driver != null)
        {
            uint refCount = 0;
            _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_GET_REFCOUNT, null, ref refCount);
            _driver.Close();

            if (refCount <= 1)
                _driver.Delete();

            _driver = null;
        }

        // try to delete temporary driver file again if failed during open
        Delete();
    }
    
    /// <summary>
    /// Gets the pci address.
    /// </summary>
    /// <param name="bus">The bus.</param>
    /// <param name="device">The device.</param>
    /// <param name="function">The function.</param>
    /// <returns></returns>
    public static uint GetPciAddress(byte bus, byte device, byte function) =>
        (uint)(((bus & 0xFF) << 8) | ((device & 0x1F) << 3) | (function & 7));

    /// <summary>
    /// Opens this instance.
    /// </summary>
    public static void Open()
    {
        // no implementation for unix systems
        if (Software.OperatingSystem.IsUnix) return;

        if (_driver is not null) return;
        _driver = new KernelDriver(GetServiceName(), "WinRing0_1_2_0");
        _driver.Open();

        if (!_driver.IsOpen)
        {
            LoadDriver();
        }

        if (_driver.IsOpen) return;
        _driver = null;
    }

    /// <summary>
    /// Reads the io port.
    /// </summary>
    /// <param name="port">The port.</param>
    /// <returns></returns>
    public static byte ReadIoPort(uint port)
    {
        if (_driver == null) return 0;

        uint value = 0;
        _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_READ_IO_PORT_BYTE, port, ref value);
        return (byte)(value & 0xFF);
    }
    
    /// <summary>
    /// Reads the memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address">The address.</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    public static bool ReadMemory<T>(ulong address, ref T buffer)
    {
        if (_driver == null) return false;
        ReadMemoryInput input = new() { Address = address, UnitSize = 1, Count = (uint)Marshal.SizeOf(buffer) };
        return _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_READ_MEMORY, input, ref buffer);
    }

    /// <summary>
    /// Reads the memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address">The address.</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    public static bool ReadMemory<T>(ulong address, ref T[] buffer)
    {
        if (_driver == null) return false;
        ReadMemoryInput input = new() { Address = address, UnitSize = (uint)Marshal.SizeOf(typeof(T)), Count = (uint)buffer.Length };
        return _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_READ_MEMORY, input, ref buffer);
    }

    /// <summary>
    /// Reads the MSR.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="eax">The eax.</param>
    /// <param name="edx">The edx.</param>
    /// <returns></returns>
    public static bool ReadMsr(uint index, out uint eax, out uint edx)
    {
        if (_driver == null)
        {
            eax = 0;
            edx = 0;
            return false;
        }

        ulong buffer = 0;
        bool result = _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_READ_MSR, index, ref buffer);
        edx = (uint)((buffer >> 32) & 0xFFFFFFFF);
        eax = (uint)(buffer & 0xFFFFFFFF);
        return result;
    }

    /// <summary>
    /// Reads the MSR.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="eax">The eax.</param>
    /// <param name="edx">The edx.</param>
    /// <param name="affinity">The affinity.</param>
    /// <returns></returns>
    public static bool ReadMsr(uint index, out uint eax, out uint edx, GroupAffinity affinity)
    {
        GroupAffinity previousAffinity = ThreadAffinity.Set(affinity);
        bool result = ReadMsr(index, out eax, out edx);
        ThreadAffinity.Set(previousAffinity);
        return result;
    }

    /// <summary>
    /// Reads the pci configuration.
    /// </summary>
    /// <param name="pciAddress">The pci address.</param>
    /// <param name="regAddress">The reg address.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool ReadPciConfig(uint pciAddress, uint regAddress, out uint value)
    {
        if (_driver == null || (regAddress & 3) != 0)
        {
            value = 0;
            return false;
        }

        ReadPciConfigInput input = new() { PciAddress = pciAddress, RegAddress = regAddress };

        value = 0;
        return _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_READ_PCI_CONFIG, input, ref value);
    }

    /// <summary>
    /// Writes the io port.
    /// </summary>
    /// <param name="port">The port.</param>
    /// <param name="value">The value.</param>
    public static void WriteIoPort(uint port, byte value)
    {
        if (_driver == null)
            return;

        WriteIoPortInput input = new() { PortNumber = port, Value = value };
        _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_WRITE_IO_PORT_BYTE, input);
    }

    /// <summary>
    /// Writes the MSR.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="eax">The eax.</param>
    /// <param name="edx">The edx.</param>
    /// <returns></returns>
    public static bool WriteMsr(uint index, uint eax, uint edx)
    {
        if (_driver == null)
            return false;

        WriteMsrInput input = new() { Register = index, Value = ((ulong)edx << 32) | eax };
        return _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_WRITE_MSR, input);
    }

    /// <summary>
    /// Writes the pci configuration.
    /// </summary>
    /// <param name="pciAddress">The pci address.</param>
    /// <param name="regAddress">The reg address.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool WritePciConfig(uint pciAddress, uint regAddress, uint value)
    {
        if (_driver == null || (regAddress & 3) != 0) return false;
        WritePciConfigInput input = new() { PciAddress = pciAddress, RegAddress = regAddress, Value = value };
        return _driver.DeviceIOControl(Interop.Ring0.IOCTL_OLS_WRITE_PCI_CONFIG, input);
    }

    /// <summary>
    /// Determines whether this instance can create the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>
    ///   <c>true</c> if this instance can create the specified path; otherwise, <c>false</c>.
    /// </returns>
    private static bool CanCreate(string path)
    {
        try
        {
            using (File.Create(path, 1, FileOptions.DeleteOnClose))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes this instance.
    /// </summary>
    private static void Delete()
    {
        try
        {
            // Try to delete the driver file
            if (_filePath != null && File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            _filePath = null;
        }
        catch
        {
            // Ignored.
        }
    }

    /// <summary>
    /// Extracts the specified file path.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns></returns>
    private static bool Extract(string filePath)
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        string resourceName = $"{assemblyName}.Resources.{(Software.OperatingSystem.Is64Bit ? "WinRing0x64.gz" : "WinRing0.gz")}";

        Assembly assembly = typeof(Ring0).Assembly;
        long requiredLength = 0;

        try
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (FileStream target = new(filePath, FileMode.Create))
                    {
                        stream.Position = 1; // Skip first byte.

                        using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            gzipStream.CopyTo(target);
                        }

                        requiredLength = target.Length;
                    }
                }
            }
        }
        catch
        {
            return false;
        }

        if (HasValidFile(filePath, requiredLength)) return true;

        // Ensure the file is actually written to the file system.
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        while (stopwatch.ElapsedMilliseconds < 2000)
        {
            if (HasValidFile(filePath, requiredLength)) return true;
            Thread.Yield();
        }
        return false;


    }

    /// <summary>
    /// Gets the file path.
    /// </summary>
    /// <returns></returns>
    private static string GetFilePath()
    {
        string filePath = null;

        try
        {
            ProcessModule processModule = Process.GetCurrentProcess().MainModule;
            if (!string.IsNullOrEmpty(processModule?.FileName))
            {
                filePath = Path.ChangeExtension(processModule.FileName, ".sys");
                if (CanCreate(filePath))
                {
                    return filePath;
                }
            }
        }
        catch
        {
            // Continue with the other options.
        }

        string previousFilePath = filePath;
        filePath = GetPathFromAssembly(Assembly.GetExecutingAssembly());
        if (previousFilePath != filePath && !string.IsNullOrEmpty(filePath) && CanCreate(filePath))
        {
            return filePath;
        }

        previousFilePath = filePath;
        filePath = GetPathFromAssembly(typeof(Ring0).Assembly);
        if (previousFilePath != filePath && !string.IsNullOrEmpty(filePath) && CanCreate(filePath))
        {
            return filePath;
        }

        try
        {
            filePath = Path.GetTempFileName();
            if (!string.IsNullOrEmpty(filePath))
            {
                filePath = Path.ChangeExtension(filePath, ".sys");
                if (CanCreate(filePath))
                {
                    return filePath;
                }
            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    private static string GetName(string name) => $"R0{name}".Replace(" ", string.Empty).Replace(".", "_");

    /// <summary>
    /// Gets the name from assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns></returns>
    private static string GetNameFromAssembly(Assembly assembly) => assembly?.GetName().Name;
    
    /// <summary>
    /// Gets the path from assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns></returns>
    private static string GetPathFromAssembly(Assembly assembly)
    {
        try
        {
            string location = assembly?.Location;
            return !string.IsNullOrEmpty(location) ? Path.ChangeExtension(location, ".sys") : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the name of the service.
    /// </summary>
    /// <returns></returns>
    private static string GetServiceName()
    {
        string name;

        try
        {
            var processModule = Process.GetCurrentProcess().MainModule;
            if (!string.IsNullOrEmpty(processModule?.FileName))
            {
                name = Path.GetFileNameWithoutExtension(processModule.FileName);
                if (!string.IsNullOrEmpty(name))
                {
                    return GetName(name);
                }
            }
        }
        catch
        {
            // Continue with the other options.
        }

        name = GetNameFromAssembly(Assembly.GetExecutingAssembly());
        if (!string.IsNullOrEmpty(name))
        {
            return GetName(name);
        }

        name = GetNameFromAssembly(typeof(Ring0).Assembly);
        if (!string.IsNullOrEmpty(name))
        {
            return GetName(name);
        }

        name = Assembly.GetExecutingAssembly().GetName().Name;
        return GetName(name);
    }

    /// <summary>
    /// Determines whether [has valid file] [the specified file path].
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="requiredLength">Length of the required.</param>
    /// <returns>
    ///   <c>true</c> if [has valid file] [the specified file path]; otherwise, <c>false</c>.
    /// </returns>
    private static bool HasValidFile(string filePath, long requiredLength)
    {
        try
        {
            return File.Exists(filePath) && new FileInfo(filePath).Length == requiredLength;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Loads the driver.
    /// </summary>
    private static void LoadDriver()
    {
        // driver is not loaded, try to install and open
        _filePath = GetFilePath();
        if (_filePath == null || !Extract(_filePath)) return;
        if (_driver.Install(_filePath, out _))
        {
            _driver.Open();
        }
        else
        {
            // install failed, try to delete and reinstall
            _driver.Delete();

            // wait a short moment to give the OS a chance to remove the driver
            Thread.Sleep(2000);

            if (_driver.Install(_filePath, out _))
            {
                _driver.Open();
            }
        }
        if (_driver.IsOpen) return;

        // Delete
        _driver.Delete();
        Delete();
    }

    #endregion
}
