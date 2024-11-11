namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC
{
    /// <summary>
    /// Embedded Controller Sensor
    /// </summary>
    public enum ECSensor
    {
        /// <summary>
        /// Chipset temperature [℃]
        /// </summary>
        TempChipset,

        /// <summary>
        /// CPU temperature [℃]
        /// </summary>
        TempCPU,

        /// <summary>
        /// motherboard temperature [℃]
        /// </summary>
        TempMB,

        /// <summary>
        /// "T_Sensor" temperature sensor reading [℃]
        /// </summary>
        TempTSensor,

        /// <summary>
        /// "T_Sensor 2" temperature sensor reading [℃]
        /// </summary>
        TempTSensor2,

        /// <summary>
        /// VRM temperature [℃]
        /// </summary>
        TempVrm,

        /// <summary>
        /// CPU Core voltage [mV]
        /// </summary>
        VoltageCPU,

        /// <summary>
        /// CPU_Opt fan [RPM]
        /// </summary>
        FanCPUOpt,

        /// <summary>
        /// VRM heat sink fan [RPM]
        /// </summary>
        FanVrmHS,

        /// <summary>
        /// Chipset fan [RPM]
        /// </summary>
        FanChipset,

        /// <summary>
        /// Water Pump [RPM]
        /// </summary>
        FanWaterPump,

        /// <summary>
        /// Water flow sensor reading [RPM]
        /// </summary>
        FanWaterFlow,

        /// <summary>
        /// CPU current [A]
        /// </summary>
        CurrCPU,

        /// <summary>
        /// "Water_In" temperature sensor reading [℃]
        /// </summary>
        TempWaterIn,

        /// <summary>
        /// "Water_Out" temperature sensor reading [℃]
        /// </summary>
        TempWaterOut,

        /// <summary>
        /// Water block temperature sensor reading [℃]
        /// </summary>
        TempWaterBlockIn,

        /// <summary>
        /// Determines the maximum of the parameters.
        /// </summary>
        Max
    }
}
