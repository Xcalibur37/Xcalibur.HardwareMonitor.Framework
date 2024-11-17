using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Embedded Controller I/O - Interface
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IEmbeddedControllerIo : IDisposable
{
    /// <summary>
    /// Reads the specified registers.
    /// </summary>
    /// <param name="registers">The registers.</param>
    /// <param name="data">The data.</param>
    void Read(ushort[] registers, byte[] data);
}