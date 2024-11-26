using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xcalibur.HardwareMonitor.Framework.Hardware.SmBios.Information;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.SmBios;

/// <summary>
/// Reads and processes information encoded in an SMBIOS table.
/// </summary>
public class SmBios
{
    private readonly Version _version;

    /// <summary>
    /// Gets <inheritdoc cref="BiosInformation" />
    /// </summary>
    public BiosInformation Bios { get; }

    /// <summary>
    /// Gets <inheritdoc cref="BaseBoardInformation" />
    /// </summary>
    public BaseBoardInformation Board { get; }

    /// <summary>
    /// Gets <inheritdoc cref="MemoryDevice" />
    /// </summary>
    public MemoryDevice[] MemoryDevices { get; }

    /// <summary>
    /// Gets <inheritdoc cref="CacheInformation" />
    /// </summary>
    public CacheInformation[] ProcessorCaches { get; }

    /// <summary>
    /// Gets <inheritdoc cref="ProcessorInformation" />
    /// </summary>
    public ProcessorInformation[] Processors { get; }

    /// <summary>
    /// Gets <inheritdoc cref="SystemInformation" />
    /// </summary>
    public SystemInformation System { get; }

    /// <summary>
    /// Gets <inheritdoc cref="Information.SystemEnclosure" />
    /// </summary>
    public SystemEnclosure SystemEnclosure { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmBios" /> class.
    /// </summary>
    public SmBios()
    {
        byte[] raw;
        if (Software.OperatingSystem.IsUnix)
        {
            raw = null;

            string boardVendor = ReadSysFs("/sys/class/dmi/id/board_vendor");
            string boardName = ReadSysFs("/sys/class/dmi/id/board_name");
            string boardVersion = ReadSysFs("/sys/class/dmi/id/board_version");
            Board = new BaseBoardInformation(boardVendor, boardName, boardVersion, null);

            string systemVendor = ReadSysFs("/sys/class/dmi/id/sys_vendor");
            string productName = ReadSysFs("/sys/class/dmi/id/product_name");
            string productVersion = ReadSysFs("/sys/class/dmi/id/product_version");
            System = new SystemInformation(systemVendor, productName, productVersion, null, null);

            string biosVendor = ReadSysFs("/sys/class/dmi/id/bios_vendor");
            string biosVersion = ReadSysFs("/sys/class/dmi/id/bios_version");
            string biosDate = ReadSysFs("/sys/class/dmi/id/bios_date");
            Bios = new BiosInformation(biosVendor, biosVersion, biosDate);

            MemoryDevices = [];
            ProcessorCaches = [];
        }
        else
        {
            List<MemoryDevice> memoryDeviceList = [];
            List<CacheInformation> processorCacheList = [];
            List<ProcessorInformation> processorInformationList = [];

            string[] tables = FirmwareTable.EnumerateTables(Kernel32.Provider.RSMB);
            if (tables is { Length: > 0 })
            {
                raw = FirmwareTable.GetTable(Kernel32.Provider.RSMB, tables[0]);
                if (raw == null || raw.Length == 0) return;

                byte majorVersion = raw[1];
                byte minorVersion = raw[2];

                if (majorVersion > 0 || minorVersion > 0)
                {
                    _version = new Version(majorVersion, minorVersion);
                }

                if (raw is { Length: > 0 })
                {
                    int offset = 8;
                    byte type = raw[offset];

                    while (offset + 4 < raw.Length && type != 127)
                    {
                        type = raw[offset];
                        int length = raw[offset + 1];
                        if (offset + length > raw.Length) break;

                        byte[] data = new byte[length];
                        Array.Copy(raw, offset, data, 0, length);
                        offset += length;

                        List<string> strings = new();
                        if (offset < raw.Length && raw[offset] == 0)
                        {
                            offset++;
                        }

                        while (offset < raw.Length && raw[offset] != 0)
                        {
                            StringBuilder stringBuilder = new();

                            while (offset < raw.Length && raw[offset] != 0)
                            {
                                stringBuilder.Append((char)raw[offset]);
                                offset++;
                            }

                            offset++;

                            strings.Add(stringBuilder.ToString());
                        }

                        offset++;
                        switch (type)
                        {
                            case 0x00:
                                Bios = new BiosInformation(data, strings);
                                break;
                            case 0x01:
                                System = new SystemInformation(data, strings);
                                break;
                            case 0x02:
                                Board = new BaseBoardInformation(data, strings);
                                break;
                            case 0x03:
                                SystemEnclosure = new SystemEnclosure(data, strings);
                                break;
                            case 0x04:
                                processorInformationList.Add(new ProcessorInformation(data, strings));
                                break;
                            case 0x07:
                                processorCacheList.Add(new CacheInformation(data, strings));
                                break;
                            case 0x11:
                                memoryDeviceList.Add(new MemoryDevice(data, strings));
                                break;
                        }
                    }
                }
            }

            MemoryDevices = memoryDeviceList.ToArray();
            ProcessorCaches = processorCacheList.ToArray();
            Processors = processorInformationList.ToArray();
        }
    }

    /// <summary>
    /// Reads the system fs.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    private static string ReadSysFs(string path)
    {
        try
        {
            if (!File.Exists(path)) return string.Empty;
            using (StreamReader reader = new(path))
            {
                return reader.ReadLine();
            }

        }
        catch
        {
            return string.Empty;
        }
    }
}
