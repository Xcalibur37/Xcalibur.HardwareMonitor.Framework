namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Embedded Controller Reader
/// </summary>
/// <param name="ecIo">The ec io.</param>
/// <param name="register">The register.</param>
/// <returns></returns>
public delegate float EmbeddedControllerReader(IEmbeddedControllerIo ecIo, ushort register);