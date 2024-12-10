namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

/// <summary>
/// Generic Storage
/// </summary>
/// <seealso cref="AbstractStorage" />
internal sealed class StorageGeneric : AbstractStorage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StorageGeneric"/> class.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <param name="index">The index.</param>
    /// <param name="settings">The settings.</param>
    private StorageGeneric(StorageInfo storageInfo, string name, string firmwareRevision, int index, ISettings settings)
        : base(storageInfo, name, firmwareRevision, "hdd", index, settings)
    {
        CreateSensors();
    }

    /// <summary>
    /// Creates the instance.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="settings">The settings.</param>
    /// <returns></returns>
    public static AbstractStorage CreateInstance(StorageInfo info, ISettings settings)
    {
        var name = string.IsNullOrEmpty(info.Name) ? "Generic Hard Disk" : info.Name;
        var firmwareRevision = string.IsNullOrEmpty(info.Revision) ? "Unknown" : info.Revision;
        return new StorageGeneric(info, name, firmwareRevision, info.Index, settings);
    }

    /// <summary>
    /// Updates the sensors.
    /// </summary>
    protected override void UpdateSensors() { }
}