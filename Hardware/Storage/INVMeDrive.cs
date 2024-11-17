﻿




using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

internal interface INVMeDrive
{
    SafeHandle Identify(StorageInfo storageInfo);

    bool IdentifyController(SafeHandle hDevice, out Kernel32.NVME_IDENTIFY_CONTROLLER_DATA data);

    bool HealthInfoLog(SafeHandle hDevice, out Kernel32.NVME_HEALTH_INFO_LOG data);
}