namespace Xcalibur.HardwareMonitor.Framework.Hardware.Sensors
{
    /// <summary>
    /// Category of what type the selected sensor is.
    /// </summary>
    public enum SensorType
    {
        Voltage, // V
        Current, // A
        Power, // W
        Clock, // MHz
        Temperature, // °C
        Load, // %
        Frequency, // Hz
        Fan, // RPM
        Flow, // L/h
        Control, // %
        Level, // %
        Factor, // 1
        Data, // GB = 2^30 Bytes
        SmallData, // MB = 2^20 Bytes
        Throughput, // B/s
        TimeSpan, // Seconds 
        Energy, // milliwatt-hour (mWh)
        Noise, // dBA
        Conductivity, // µS/cm
        Humidity // %
    }
}
