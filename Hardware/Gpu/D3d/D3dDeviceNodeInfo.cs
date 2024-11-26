using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.D3d
{
    /// <summary>
    /// D3D Device Node Info
    /// </summary>
    public struct D3dDeviceNodeInfo
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public ulong Id;

        /// <summary>
        /// The name
        /// </summary>
        public string Name;

        /// <summary>
        /// The running time
        /// </summary>
        public long RunningTime;

        /// <summary>
        /// The query time
        /// </summary>
        public DateTime QueryTime;
    }
}
