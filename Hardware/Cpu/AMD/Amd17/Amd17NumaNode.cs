using System.Collections.Generic;
using Xcalibur.Extensions.V2;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD.Amd17
{
    /// <summary>
    /// AMD 17-series Numa Node
    /// </summary>
    internal class Amd17NumaNode
    {
        #region Fields

        private readonly Amd17Cpu _cpu;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cores.
        /// </summary>
        /// <value>
        /// The cores.
        /// </value>
        public List<Amd17Core> Cores { get; }

        /// <summary>
        /// Gets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Amd17NumaNode"/> class.
        /// </summary>
        /// <param name="cpu">The cpu.</param>
        /// <param name="id">The identifier.</param>
        public Amd17NumaNode(Amd17Cpu cpu, int id)
        {
            Cores = new List<Amd17Core>();
            NodeId = id;
            _cpu = cpu;
        }

        /// <summary>
        /// Appends the thread.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="coreId">The core identifier.</param>
        public void AppendThread(CpuId thread, int coreId)
        {
            Amd17Core core = null;
            foreach (Amd17Core c in Cores)
            {
                if (c.CoreId == coreId)
                    core = c;
            }

            if (core == null)
            {
                core = new Amd17Core(_cpu, coreId);
                Cores.Add(core);
            }

            if (thread != null)
                core.Threads.Add(thread);
        }

        /// <summary>
        /// Updates the sensors.
        /// </summary>
        public void UpdateSensors()
        {
            Cores.Apply(c => c.UpdateSensors());
        }
    }
}
