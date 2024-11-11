using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions
{
    /// <summary>
    /// Multiple Board Records Found Exception
    /// </summary>
    /// <seealso cref="Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions.BadConfigurationException" />
    public class MultipleBoardRecordsFoundException : BadConfigurationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleBoardRecordsFoundException"/> class.
        /// </summary>
        /// <param name="model"></param>
        public MultipleBoardRecordsFoundException(string model) : 
            base($"Multiple board records refer to the same model '{model}'") { }
    }
}
