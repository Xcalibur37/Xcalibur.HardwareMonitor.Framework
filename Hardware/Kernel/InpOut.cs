using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;

/// <summary>
/// InpOut32 is a windows DLL and Driver to give direct access to hardware ports.
/// </summary>
internal static class InpOut
{
    #region Fields

    private static string _filePath;
    private static IntPtr _libraryHandle;
    private static Interop.InpOut.MapPhysToLinDelegate _mapPhysToLin;
    private static Interop.InpOut.UnmapPhysicalMemoryDelegate _unmapPhysicalMemory;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether this instance is open.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
    /// </value>
    public static bool IsOpen { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public static void Close()
    {
        if (_libraryHandle != IntPtr.Zero)
        {
            Kernel32.FreeLibrary(_libraryHandle);
            Delete();

            _libraryHandle = IntPtr.Zero;
        }

        IsOpen = false;
    }

    /// <summary>
    /// Opens this instance.
    /// </summary>
    /// <returns></returns>
    public static bool Open()
    {
        if (Software.OperatingSystem.IsUnix) return false;

        if (IsOpen) return true;

        _filePath = GetFilePath();
        if (_filePath != null && (File.Exists(_filePath) || Extract(_filePath)))
        {
            _libraryHandle = Kernel32.LoadLibrary(_filePath);
            if (_libraryHandle != IntPtr.Zero)
            {
                IntPtr mapPhysToLinAddress = Kernel32.GetProcAddress(_libraryHandle, "MapPhysToLin");
                IntPtr unmapPhysicalMemoryAddress = Kernel32.GetProcAddress(_libraryHandle, "UnmapPhysicalMemory");

                if (mapPhysToLinAddress != IntPtr.Zero)
                {
                    _mapPhysToLin = Marshal.GetDelegateForFunctionPointer<Interop.InpOut.MapPhysToLinDelegate>(mapPhysToLinAddress);
                }

                if (unmapPhysicalMemoryAddress != IntPtr.Zero)
                {
                    _unmapPhysicalMemory = Marshal.GetDelegateForFunctionPointer<Interop.InpOut.UnmapPhysicalMemoryDelegate>(unmapPhysicalMemoryAddress);
                }

                IsOpen = true;
            }
        }

        if (!IsOpen)
        {
            Delete();
        }

        return IsOpen;
    }

    /// <summary>
    /// Maps the memory.
    /// </summary>
    /// <param name="baseAddress">The base address.</param>
    /// <param name="size">The size.</param>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    public static IntPtr MapMemory(IntPtr baseAddress, uint size, out IntPtr handle)
    {
        if (_mapPhysToLin != null) return _mapPhysToLin(baseAddress, size, out handle);
        handle = IntPtr.Zero;
        return IntPtr.Zero;

    }

    /// <summary>
    /// Reads the memory.
    /// </summary>
    /// <param name="baseAddress">The base address.</param>
    /// <param name="size">The size.</param>
    /// <returns></returns>
    public static byte[] ReadMemory(IntPtr baseAddress, uint size)
    {
        if (_mapPhysToLin == null || _unmapPhysicalMemory == null) return null;
        IntPtr pdwLinAddr = _mapPhysToLin(baseAddress, size, out IntPtr pPhysicalMemoryHandle);
        if (pdwLinAddr == IntPtr.Zero) return null;

        byte[] bytes = new byte[size];
        Marshal.Copy(pdwLinAddr, bytes, 0, bytes.Length);
        _unmapPhysicalMemory(pPhysicalMemoryHandle, pdwLinAddr);

        return bytes;

    }

    /// <summary>
    /// Unmaps the memory.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <param name="address">The address.</param>
    /// <returns></returns>
    public static bool UnmapMemory(IntPtr handle, IntPtr address) => _unmapPhysicalMemory != null && _unmapPhysicalMemory(handle, address);

    /// <summary>
    /// Writes the memory.
    /// </summary>
    /// <param name="baseAddress">The base address.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static bool WriteMemory(IntPtr baseAddress, byte value)
    {
        if (_mapPhysToLin == null || _unmapPhysicalMemory == null) return false;

        IntPtr pdwLinAddr = _mapPhysToLin(baseAddress, 1, out IntPtr pPhysicalMemoryHandle);
        if (pdwLinAddr == IntPtr.Zero) return false;

        Marshal.WriteByte(pdwLinAddr, value);
        _unmapPhysicalMemory(pPhysicalMemoryHandle, pdwLinAddr);

        return true;
    }

    /// <summary>
    /// Deletes this instance.
    /// </summary>
    private static void Delete()
    {
        try
        {
            // try to delete the DLL
            if (_filePath != null && File.Exists(_filePath))
                File.Delete(_filePath);

            _filePath = null;
        }
        catch
        {
            // Do nothing
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
        string resourceName = $"{assemblyName}.Resources.{(Software.OperatingSystem.Is64Bit ? "inpoutx64.gz" : "inpout32.gz")}";

        var assembly = typeof(InpOut).Assembly;
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
                            requiredLength = target.Length;
                        }
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
        string filePath;

        try
        {
            filePath = Path.GetTempFileName();
            if (!string.IsNullOrEmpty(filePath))
            {
                return Path.ChangeExtension(filePath, ".dll");
            }
        }
        catch (IOException)
        {
            // Do nothing
        }

        const string fileName = "inpout.dll";

        try
        {
            ProcessModule processModule = Process.GetCurrentProcess().MainModule;
            if (!string.IsNullOrEmpty(processModule?.FileName))
            {
                return Path.Combine(Path.GetDirectoryName(processModule.FileName) ?? string.Empty, fileName);
            }
        }
        catch
        {
            // Continue with the other options.
        }

        filePath = GetPathFromAssembly(Assembly.GetExecutingAssembly());
        if (!string.IsNullOrEmpty(filePath))
        {
            return Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, fileName);
        }

        filePath = GetPathFromAssembly(typeof(InpOut).Assembly);
        return !string.IsNullOrEmpty(filePath) ? Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, fileName) : null;

        static string GetPathFromAssembly(Assembly assembly)
        {
            try
            {
                string location = assembly?.Location;
                return !string.IsNullOrEmpty(location) ? location : null;
            }
            catch
            {
                return null;
            }
        }
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

    #endregion
}
