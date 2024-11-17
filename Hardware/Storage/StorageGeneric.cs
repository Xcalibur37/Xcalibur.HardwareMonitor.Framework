





using System.Text;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

internal sealed class StorageGeneric : AbstractStorage
{
    private StorageGeneric(StorageInfo storageInfo, string name, string firmwareRevision, int index, ISettings settings)
        : base(storageInfo, name, firmwareRevision, "hdd", index, settings)
    {
        CreateSensors();
    }

    public static AbstractStorage CreateInstance(StorageInfo info, ISettings settings)
    {
        string name = string.IsNullOrEmpty(info.Name) ? "Generic Hard Disk" : info.Name;
        string firmwareRevision = string.IsNullOrEmpty(info.Revision) ? "Unknown" : info.Revision;
        return new StorageGeneric(info, name, firmwareRevision, info.Index, settings);
    }

    protected override void UpdateSensors() { }
}