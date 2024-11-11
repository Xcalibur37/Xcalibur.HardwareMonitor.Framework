namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc
{
    /// <summary>
    /// Handler that will trigger the actions assigned to it when the event occurs.
    /// </summary>
    /// <param name="sensor">Component returned to the assigned action(s).</param>
    public delegate void SensorEventHandler(ISensor sensor);
}
