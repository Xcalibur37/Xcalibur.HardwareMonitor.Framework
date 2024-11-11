namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.SuperIo
{
    /// <summary>
    /// Super I/O Constants
    /// </summary>
    internal static class SuperIoConstants
    {
        // Voltages
        internal static string V18Volts = "+1.8V";
        internal static string V3StandbyVolts = "3V Standby";
        internal static string V3vccVolts = "+3VCC";
        internal static string V33Volts = "+3.3V";
        internal static string V33StandbyVolts = "+3.3V Standby";
        internal static string V50Volts = "+5V";
        internal static string V50StandbyVolts = "+5V Standby";
        internal static string V120Volts = "+12V";
        
        internal static string Alw105Volts = "+1.05V ALW";
        internal static string AuxTinVoltNumber = "AUXTIN{0}";
        internal static string AvStandbyVolts = "AV Standby";
        internal static string AvccVolts = "AVCC";
        internal static string Avcc3Volts = "AVCC3";
        internal static string BatteryVolts = "Battery";
        internal static string ChipsetVolts = "Chipset";
        internal static string Chipset082Volts = "Chipset 0.82V";
        internal static string Chipset100Volts = "Chipset 1.0V";
        internal static string Chipset105Volts = "Chipset 1.05V";
        internal static string CmosBatteryVolts = "CMOS Battery";
        internal static string Cpu105Volts = "CPU 1.05V";
        internal static string CpuGraphicsVolts = "CPU Graphics";
        internal static string CpuImcVolts = "Memory Controller";
        internal static string CpuInputAuxVolts = "CPU Input Auxiliary";
        internal static string CpuIoVolts = "CPU I/O";
        internal static string CpuL2CacheVolts = "CPU L2 Cache";
        internal static string CpuNorthbridgeSocVolts = "Northbridge / SOC";
        internal static string CpuSocVolts = "CPU SOC";
        internal static string CpuSocPremVolts = "PREM CPU SOC";
        internal static string CpuSaVolts = "CPU System Agent";
        internal static string CpuTerminationVolts = "CPU Termination";
        internal static string CpuVccioVolts = "CPU VCCIO";
        internal static string CpuVccioImcVolts = "CPU VCCIO / IMC";
        internal static string CpuVddioImcVolts = "CPU VDDIO / IMC";
        internal static string CpuVddpVolts = "CPU VDDP";
        internal static string CpuWeightedVolts = "CPU (Weighted)";
        internal static string DimmTerminationVolts = "DIMM Termination";
        internal static string DimmABVolts = "DIMM A/B";
        internal static string DimmCDVolts = "DIMM C/D";
        internal static string DimmVolts = "DIMM";
        internal static string DimmWriteVolts = "DIMM Write";
        internal static string DramVolts = "DRAM";
        internal static string IGpuVolts = "iGPU";
        internal static string IohVcoreVolts = "IOH Vcore";
        internal static string IvrAtomL2ClusterVoltNumber = "IVR Atom L2 Cluster #{0}";
        internal static string PchCoreVolts = "PCH Core";
        internal static string Pch105Volts = "PCH 1.05V";
        internal static string PchVolts = "PCH";
        internal static string PhaseLockedLoopVolts = "Phase Locked Loop";
        internal static string ReservedVolts = "Reserved";
        internal static string SysTinVolts = "SYSTIN";
        internal static string VBatVolts = "VBat";
        internal static string VcoreRefVolts = "VcoreRef";
        internal static string VcoreRefinVolts = "VCore Refin";
        internal static string VcoreVolts = "VCore";
        internal static string VcoreMiscVolts = "VCore Misc";
        internal static string VcoreSocVolts = "VCore SOC";
        internal static string VddcrSocVolts = "VDDCR SOC";
        internal static string VinVoltNumber = "VIN{0}";
        internal static string VoltageVoltNumber = "Voltage #{0}";
        internal static string VrefVolts = "VRef";
        internal static string Vsb3vVolts = "VSB3V";
        internal static string Vcc3vVolts = "VCC3V";
        internal static string VppmVolts = "VPPM";
        internal static string VsbVolts = "VSB";
        internal static string VttVolts = "VTT";
        
        // Temps
        internal static string AuxTempNumber = "AUX {0}";
        internal static string AuxiliaryTemp = "Auxiliary";
        internal static string AuxTinTempNumber = "AUXTIN{0}";
        internal static string ChipsetTemp = "Chipset";
        internal static string CoreSocTemp = "Core SOC";
        internal static string CoreVrmTemp = "Core VRM";
        internal static string CpuCoreTemp = "CPU Core";
        internal static string CpuPackageTemp = "CPU Package";
        internal static string CpuPeciTemp = "CPU (PECI)";
        internal static string CpuSocketTemp = "CPU Socket";
        internal static string CpuTemp = "CPU";
        internal static string DimmAgentNumbersTemp = "Agent {0}, DIMM {1}";
        internal static string DeviceTempNumber = "Device {0}";
        internal static string M21Temp = "M2_1";
        internal static string MotherboardTemp = "Motherboard";
        internal static string MosTemp = "MOS";
        internal static string PchChipCpuMaxTemp = "PCH Chip CPU Max";
        internal static string PchChipTemp = "PCH Chip";
        internal static string PciEx1Temp = "PCI-E x1";
        internal static string PchCpuTemp = "PCH CPU";
        internal static string PchMchTemp = "PCH MCH";
        internal static string PchTemp = "PCH";
        internal static string PeciCalibratedTempNumber = "PECI {0} Calibrated";
        internal static string PeciTempNumber = "PECI {0}";
        internal static string ProbeTemp = "Probe";
        internal static string SmbusTempNumber = "SMBus {0}";
        internal static string SouthbridgeTemp = "Southbridge";
        internal static string SystemTemp = "System";
        internal static string T1Temp = "T1";
        internal static string T2Temp = "T2";
        internal static string TempNumber = "Temperature #{0}";
        internal static string TSensorTemp = "T-Sensor";
        internal static string VirtualTemp = "Virtual";
        internal static string VregTemp = "VREG";
        internal static string VrmTemp = "VRM";
        internal static string VrmMosTemp = "VRM MOS";
        
        // Fans
        internal static string AioPumpFan = "AIO Pump";
        internal static string AuxiliaryFan = "Auxiliary Fan";
        internal static string AuxiliaryFanNumber = "Auxiliary Fan #{0}";
        internal static string ChassisFan = "Chassis Fan";
        internal static string ChassisFanNumber = "Chassis Fan #{0}";
        internal static string ChipsetFan = "Chipset Fan";
        internal static string CpuFan = "CPU Fan";
        internal static string CpuFanNumber = "CPU Fan #{0}";
        internal static string CpuOptionalFan = "CPU Optional Fan";
        internal static string FanNumber = "Fan #{0}";
        internal static string HighAmpFan = "High Amp Fan";
        internal static string MosFanNumber = "MOS Fan #{0}";
        internal static string PowerFan = "Power Fan";
        internal static string PumpFan = "Pump Fan";
        internal static string RadiatorFanNumber = "Radiator Fan #{0}";
        internal static string SouthbridgeFan = "Southbridge Fan";
        internal static string SystemFan = "System Fan";
        internal static string SystemFanNumber = "System Fan #{0}";
        internal static string WaterPumpFan = "Water Pump";
        internal static string WaterPumpFanNumber = "Water Pump #{0}";

        // Controls
        internal static string FanControlNumber = "Fan Control #{0}";
    }
}
