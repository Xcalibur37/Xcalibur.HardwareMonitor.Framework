using System;
using System.Runtime.InteropServices;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios;

/// <summary>
/// Firmware Table
/// </summary>
internal static class FirmwareTable
{
    /// <summary>
    /// Gets the table.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="table">The table.</param>
    /// <returns></returns>
    public static byte[] GetTable(Provider provider, string table)
    {
        int id = table[3] << 24 | table[2] << 16 | table[1] << 8 | table[0];
        return GetTable(provider, id);
    }

    /// <summary>
    /// Gets the table.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="table">The table.</param>
    /// <returns></returns>
    public static byte[] GetTable(Provider provider, int table)
    {
        int size;

        try
        {
            size = Interop.Kernel32.GetSystemFirmwareTable(provider, table, IntPtr.Zero, 0);
        }
        catch (Exception e) when (e is DllNotFoundException or EntryPointNotFoundException)
        {
            return null;
        }

        if (size <= 0) return null;

        IntPtr allocatedBuffer = IntPtr.Zero;
        try
        {
            allocatedBuffer = Marshal.AllocHGlobal(size);
            Interop.Kernel32.GetSystemFirmwareTable(provider, table, allocatedBuffer, size);
            if (Marshal.GetLastWin32Error() != 0) return null;

            byte[] buffer = new byte[size];
            Marshal.Copy(allocatedBuffer, buffer, 0, size);
            return buffer;
        }
        finally
        {
            if (allocatedBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(allocatedBuffer);
            }
        }
    }

    /// <summary>
    /// Enumerates the tables.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <returns></returns>
    public static string[] EnumerateTables(Provider provider)
    {
        int size;

        try
        {
            size = Interop.Kernel32.EnumSystemFirmwareTables(provider, IntPtr.Zero, 0);
        }
        catch (Exception e) when (e is DllNotFoundException or EntryPointNotFoundException)
        {
            return null;
        }

        IntPtr allocatedBuffer = IntPtr.Zero;

        try
        {
            allocatedBuffer = Marshal.AllocHGlobal(size);
            Interop.Kernel32.EnumSystemFirmwareTables(provider, allocatedBuffer, size);

            byte[] buffer = new byte[size];
            Marshal.Copy(allocatedBuffer, buffer, 0, size);

            string[] result = new string[size / 4];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Encoding.ASCII.GetString(buffer, 4 * i, 4);
            }
            return result;
        }
        finally
        {
            if (allocatedBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(allocatedBuffer);
            }
        }
    }
}
