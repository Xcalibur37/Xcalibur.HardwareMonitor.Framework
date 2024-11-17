





using System;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

public interface ISmart : IDisposable
{
    bool IsValid { get; }

    void Close();

    bool EnableSmart();

    Kernel32.SMART_ATTRIBUTE[] ReadSmartData();

    Kernel32.SMART_THRESHOLD[] ReadSmartThresholds();

    bool ReadNameAndFirmwareRevision(out string name, out string firmwareRevision);
}