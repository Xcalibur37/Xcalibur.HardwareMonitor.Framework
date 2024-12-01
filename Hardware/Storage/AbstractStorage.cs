using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;
using Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// Abstract storage
/// </summary>
/// <seealso cref="Hardware" />
public abstract class AbstractStorage : Hardware
{
    #region Fields

    private readonly AbstractPerformanceValue _perfTotal = new();
    private readonly AbstractPerformanceValue _perfWrite = new();
    private readonly AbstractPerformanceValue _perfRead = new();
    private readonly StorageInfo _storageInfo;
    private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(60);

    private ulong _lastReadCount;
    private long _lastTime;
    private DateTime _lastUpdate = DateTime.MinValue;
    private ulong _lastWriteCount;
    private Sensor _sensorDiskReadRate;
    private Sensor _sensorDiskTotalActivity;
    private Sensor _sensorDiskWriteActivity;
    private Sensor _sensorDiskReadActivity;
    private Sensor _sensorDiskWriteRate;
    private Sensor _usageSensor;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the drive infos.
    /// </summary>
    /// <value>
    /// The drive infos.
    /// </value>
    public DriveInfo[] DriveInfos { get; }

    /// <summary>
    /// Gets the firmware revision.
    /// </summary>
    /// <value>
    /// The firmware revision.
    /// </value>
    public string FirmwareRevision { get; }

    /// <summary>
    /// </summary>
    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.Storage;

    /// <summary>
    /// Gets the index.
    /// </summary>
    /// <value>
    /// The index.
    /// </value>
    public int Index { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractStorage"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="index">The index.</param>
    /// <param name="settings">The settings.</param>
    internal AbstractStorage(StorageInfo storageInfo, string name, string firmwareRevision, string id, int index, ISettings settings)
        : base(name, new Identifier(id, index.ToString(CultureInfo.InvariantCulture)), settings)
    {
        _storageInfo = storageInfo;
        FirmwareRevision = firmwareRevision;
        Index = index;

        string[] logicalDrives = WindowsStorage.GetLogicalDrives(index);
        var driveInfoList = new List<DriveInfo>(logicalDrives.Length);

        foreach (string logicalDrive in logicalDrives)
        {
            try
            {
                var di = new DriveInfo(logicalDrive);
                if (di.TotalSize <= 0) continue;
                driveInfoList.Add(new DriveInfo(logicalDrive));
            }
            catch (ArgumentException)
            {
                // Do nothing
            }
            catch (IOException)
            {
                // Do nothing
            }
            catch (UnauthorizedAccessException)
            {
                // Do nothing
            }
        }

        DriveInfos = driveInfoList.ToArray();
    }

    #endregion

    /// <inheritdoc />
    public override void Close()
    {
        _storageInfo.Handle?.Close();
        base.Close();
    }

    /// <summary>
    /// Creates the instance.
    /// </summary>
    /// <param name="deviceId">The device identifier.</param>
    /// <param name="driveNumber">The drive number.</param>
    /// <param name="diskSize">Size of the disk.</param>
    /// <param name="scsiPort">The scsi port.</param>
    /// <param name="settings">The settings.</param>
    /// <returns></returns>
    public static AbstractStorage CreateInstance(string deviceId, uint driveNumber, ulong diskSize, int scsiPort, ISettings settings)
    {
        var info = WindowsStorage.GetStorageInfo(deviceId, driveNumber);
        if (info == null || info.Removable || 
            info.BusType is StorageBusType.BusTypeVirtual or StorageBusType.BusTypeFileBackedVirtual)
            return null;

        info.DiskSize = diskSize;
        info.DeviceId = deviceId;
        info.Handle = Kernel32.OpenDevice(deviceId);
        info.Scsi = $@"\\.\SCSI{scsiPort}:";
        
        //fallback, when it is not possible to read out with the nvme implementation,
        //try it with the sata smart implementation
        if (info.BusType == StorageBusType.BusTypeNvme)
        {
            AbstractStorage x = NvmeGeneric.CreateInstance(info, settings);
            if (x != null) return x;
        }

        return info.BusType is StorageBusType.BusTypeAta or StorageBusType.BusTypeSata or StorageBusType.BusTypeNvme
            ? AtaStorage.CreateInstance(info, settings)
            : StorageGeneric.CreateInstance(info, settings);
    }

    /// <summary>
    /// Creates the sensors.
    /// </summary>
    /// <returns></returns>
    protected virtual void CreateSensors()
    {
        if (DriveInfos.Length > 0)
        {
            // Used Space
            _usageSensor = new Sensor("Used Space", 0, SensorType.Load, this, Settings);
            ActivateSensor(_usageSensor);
        }

        // Read Activity
        _sensorDiskReadActivity = new Sensor("Read Activity", 31, SensorType.Load, this, Settings);
        ActivateSensor(_sensorDiskReadActivity);

        // Write Activity
        _sensorDiskWriteActivity = new Sensor("Write Activity", 32, SensorType.Load, this, Settings);
        ActivateSensor(_sensorDiskWriteActivity);

        // Total Activity
        _sensorDiskTotalActivity = new Sensor("Total Activity", 33, SensorType.Load, this, Settings);
        ActivateSensor(_sensorDiskTotalActivity);

        // Read Rate
        _sensorDiskReadRate = new Sensor("Read Rate", 34, SensorType.Throughput, this, Settings);
        ActivateSensor(_sensorDiskReadRate);

        // Write Rate
        _sensorDiskWriteRate = new Sensor("Write Rate", 35, SensorType.Throughput, this, Settings);
        ActivateSensor(_sensorDiskWriteRate);
    }

    /// <summary>
    /// Updates the sensors.
    /// </summary>
    /// <returns></returns>
    protected abstract void UpdateSensors();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc />
    public override void Update()
    {
        // Update statistics.
        if (_storageInfo != null)
        {
            try
            {
                UpdatePerformanceSensors();
            }
            catch
            {
                // Do nothing
            }
        }

        // Read out at update interval.
        var tDiff = DateTime.UtcNow - _lastUpdate;
        if (tDiff <= _updateInterval) return;
        _lastUpdate = DateTime.UtcNow;

        // Update sensors
        UpdateSensors();

        if (_usageSensor == null) return;
        long totalSize = 0;
        long totalFreeSpace = 0;

        // Determine totals
        foreach (var t in DriveInfos)
        {
            if (!t.IsReady) continue;

            try
            {
                totalSize += t.TotalSize;
                totalFreeSpace += t.TotalFreeSpace;
            }
            catch (IOException)
            {
                // Do nothing
            }
            catch (UnauthorizedAccessException)
            {
                // Do nothing
            }
        }

        // Set
        _usageSensor.Value = totalSize > 0 ? 100.0f - (100.0f * totalFreeSpace / totalSize) : null;
    }

    /// <summary>
    /// Updates the performance sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdatePerformanceSensors()
    {
        if (!Kernel32.DeviceIoControl(_storageInfo.Handle,
                                      IoCtl.IOCTL_DISK_PERFORMANCE,
                                      IntPtr.Zero,
                                      0,
                                      out DiskPerformance diskPerformance,
                                      Marshal.SizeOf<DiskPerformance>(),
                                      out _,
                                      IntPtr.Zero)) return;

        _perfRead.Update(diskPerformance.ReadTime, diskPerformance.QueryTime);
        _sensorDiskReadActivity.Value = (float)_perfRead.Result;

        _perfWrite.Update(diskPerformance.WriteTime, diskPerformance.QueryTime);
        _sensorDiskWriteActivity.Value = (float)_perfWrite.Result;

        _perfTotal.Update(diskPerformance.IdleTime, diskPerformance.QueryTime);
        _sensorDiskTotalActivity.Value = (float)(100 - _perfTotal.Result);

        ulong readCount = diskPerformance.BytesRead;
        ulong readDiff = readCount - _lastReadCount;
        _lastReadCount = readCount;

        ulong writeCount = diskPerformance.BytesWritten;
        ulong writeDiff = writeCount - _lastWriteCount;
        _lastWriteCount = writeCount;

        long currentTime = Stopwatch.GetTimestamp();
        if (_lastTime != 0)
        {
            double timeDeltaSeconds = TimeSpan.FromTicks(currentTime - _lastTime).TotalSeconds;

            double writeSpeed = writeDiff * (1 / timeDeltaSeconds);
            _sensorDiskWriteRate.Value = (float)writeSpeed;

            double readSpeed = readDiff * (1 / timeDeltaSeconds);
            _sensorDiskReadRate.Value = (float)readSpeed;
        }

        _lastTime = currentTime;
    }

    /// <summary>
    /// </summary>
    /// <param name="visitor"></param>
    /// <returns></returns>
    /// <inheritdoc />
    public override void Traverse(IVisitor visitor)
    {
        Sensors.Apply(x => x.Accept(visitor));
    }
}
