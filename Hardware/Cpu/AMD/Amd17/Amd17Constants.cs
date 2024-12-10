namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD.Amd17
{
    /// <summary>
    /// AMD 17-Series Constants
    /// </summary>
    internal class Amd17Constants
    {
        internal const uint CofvidStatus = 0xC0010071;
        internal const uint F17HM01HSvi = 0x0005A000;
        internal const uint F17HM01HThmTconCurTmp = 0x00059800;
        internal const uint F17HM70HCcd1Temp = 0x00059954;
        internal const uint F17HM61HCcd1Temp = 0x00059b08;
        internal const uint F17HTempOffsetFlag = 0x80000;
        internal const uint Family17HPciControlRegister = 0x60;
        internal const uint Hwcr = 0xC0010015;
        internal const uint MsrCoreEnergyStat = 0xC001029A;
        internal const uint MsrHardwarePstateStatus = 0xC0010293;
        internal const uint MsrPkgEnergyStat = 0xC001029B;
        internal const uint MsrPstate0 = 0xC0010064;
        internal const uint MsrPwrUnit = 0xC0010299;
        internal const uint PerfCtl0 = 0xC0010000;
        internal const uint PerfCtr0 = 0xC0010004;
    }
}
