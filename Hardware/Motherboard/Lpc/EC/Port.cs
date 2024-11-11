using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC
{
    /// <summary>
    /// EC Port
    /// </summary>
    internal enum Port : byte
    {
        Command = 0x66,
        Data = 0x62
    }
}
