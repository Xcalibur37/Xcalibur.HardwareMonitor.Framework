using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// NVME Drive: Interface
/// </summary>
internal interface INvmeDrive
{
    /// <summary>
    /// Health information log.
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    bool HealthInfoLog(SafeHandle hDevice, out NvmeHealthInfoLog data);

    /// <summary>
    /// Identifies the specified storage information.
    /// </summary>
    /// <param name="storageInfo">The storage information.</param>
    /// <returns></returns>
    SafeHandle Identify(StorageInfo storageInfo);

    /// <summary>
    /// Identifies the controller.
    /// </summary>
    /// <param name="hDevice">The h device.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    bool IdentifyController(SafeHandle hDevice, out NvmeIdentifyControllerData data);
}