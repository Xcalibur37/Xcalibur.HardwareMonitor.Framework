using System.Runtime.InteropServices;

namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// Reserved for system use. Do not use.
    /// https://learn.microsoft.com/en-my/windows-hardware/drivers/ddi/d3dkmthk/ns-d3dkmthk-d3dkmt_querystatistics_preemption_information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct D3DkmtQuerystatisticsPreemptionInformation
    {
        public uint PreemptionCounter;
        public uint PreemptionCounter1;
        public uint PreemptionCounter2;
        public uint PreemptionCounter3;
        public uint PreemptionCounter4;
        public uint PreemptionCounter5;
        public uint PreemptionCounter6;
        public uint PreemptionCounter7;
        public uint PreemptionCounter8;
        public uint PreemptionCounter9;
        public uint PreemptionCounter10;
        public uint PreemptionCounter11;
        public uint PreemptionCounter12;
        public uint PreemptionCounter13;
        public uint PreemptionCounter14;
        public uint PreemptionCounter15;
    }
}
