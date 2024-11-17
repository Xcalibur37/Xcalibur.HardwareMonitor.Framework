using System.Collections.Generic;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// Windows Embedded Controller.
/// </summary>
/// <seealso cref="EmbeddedControllerBase" />
public class WindowsEmbeddedController : EmbeddedControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsEmbeddedController"/> class.
    /// </summary>
    /// <param name="sources">The sources.</param>
    /// <param name="settings">The settings.</param>
    public WindowsEmbeddedController(IEnumerable<EmbeddedControllerSource> sources, ISettings settings) : 
        base(sources, settings) { }

    /// <summary>
    /// Acquires the io interface.
    /// </summary>
    /// <returns></returns>
    protected override IEmbeddedControllerIo AcquireIoInterface() => new WindowsEmbeddedControllerIo();
}