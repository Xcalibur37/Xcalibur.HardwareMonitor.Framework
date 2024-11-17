namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.Gigabyte;

/// <summary>
/// Gigabyte Controller: Interface
/// </summary>
internal interface IGigabyteController
{
    /// <summary>
    /// Enables the specified enabled.
    /// </summary>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <returns></returns>
    bool Enable(bool enabled);

    /// <summary>
    /// Restores this instance.
    /// </summary>
    void Restore();
}
