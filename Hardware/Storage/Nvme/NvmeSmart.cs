using System;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// NVME: Smart Drive
/// </summary>
/// <seealso cref="IDisposable" />
public class NvmeSmart : IDisposable
{
    #region Fields

    private readonly int _driveNumber;
    private readonly SafeHandle _handle;

    #endregion

    #region Properties

    /// <summary>
    /// Returns true if ... is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid => _handle is { IsInvalid: false };

    /// <summary>
    /// Gets the nvme drive.
    /// </summary>
    /// <value>
    /// The nvme drive.
    /// </value>
    internal INvmeDrive NvmeDrive { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NvmeSmart"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    internal NvmeSmart(StorageInfo storageInfo)
    {
        _driveNumber = storageInfo.Index;
        NvmeDrive = null;
        var name = storageInfo.Name;

        // Test Samsung protocol.
        if (NvmeDrive == null && name.IndexOf("Samsung", StringComparison.OrdinalIgnoreCase) > -1)
        {
            _handle = NvmeSamsung.IdentifyDevice(storageInfo);
            if (_handle != null)
            {
                NvmeDrive = new NvmeSamsung();
                if (!NvmeDrive.IdentifyController(_handle, out _))
                {
                    NvmeDrive = null;
                }
            }
        }

        // Test Intel protocol.
        if (NvmeDrive == null && name.IndexOf("Intel", StringComparison.OrdinalIgnoreCase) > -1)
        {
            _handle = NvmeIntel.IdentifyDevice(storageInfo);
            if (_handle != null)
            {
                NvmeDrive = new NvmeIntel();
            }
        }

        // Test Intel raid protocol.
        if (NvmeDrive == null && name.IndexOf("Intel", StringComparison.OrdinalIgnoreCase) > -1)
        {
            _handle = NvmeIntelRst.IdentifyDevice(storageInfo);
            if (_handle != null)
            {
                NvmeDrive = new NvmeIntelRst();
            }
        }

        // Test Windows generic driver protocol.
        if (NvmeDrive == null)
        {
            _handle = NvmeWindows.IdentifyDevice(storageInfo);
            if (_handle != null)
            {
                NvmeDrive = new NvmeWindows();
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Closes this instance.
    /// </summary>
    public void Close()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Close();
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
    /// unmanaged resources.</param>
    protected void Dispose(bool disposing)
    {
        if (!disposing || _handle is not { IsClosed: false }) return;
        _handle.Close();
    }

    /// <summary>
    /// Gets the health information.
    /// </summary>
    /// <returns></returns>
    public NvmeHealthInfo GetHealthInfo()
    {
        if (_handle?.IsClosed != false) return null;

        bool valid = false;
        var data = new Kernel32.NVME_HEALTH_INFO_LOG();
        if (NvmeDrive != null)
        {
            valid = NvmeDrive.HealthInfoLog(_handle, out data);
        }

        if (!valid) return null;
        return new NvmeHealthInfo(data);
    }

    /// <summary>
    /// Gets the information.
    /// </summary>
    /// <returns></returns>
    public NvmeInfo GetInfo()
    {
        if (_handle?.IsClosed != false) return null;

        bool valid = false;
        var data = new Kernel32.NVME_IDENTIFY_CONTROLLER_DATA();
        if (NvmeDrive != null)
        {
            valid = NvmeDrive.IdentifyController(_handle, out data);
        }
        if (!valid) return null;
        return new NvmeInfo(_driveNumber, data);
    }

    #endregion
}