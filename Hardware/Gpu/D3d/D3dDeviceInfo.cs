namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.D3d
{
    /// <summary>
    /// D3D Device Info
    /// </summary>
    public struct D3dDeviceInfo
    {
        /// <summary>
        /// The GPU shared limit
        /// </summary>
        public ulong GpuSharedLimit;

        /// <summary>
        /// The GPU dedicated limit
        /// </summary>
        public ulong GpuDedicatedLimit;

        /// <summary>
        /// The GPU video memory limit
        /// </summary>
        public ulong GpuVideoMemoryLimit;

        /// <summary>
        /// The GPU shared used
        /// </summary>
        public ulong GpuSharedUsed;

        /// <summary>
        /// The GPU dedicated used
        /// </summary>
        public ulong GpuDedicatedUsed;

        /// <summary>
        /// The GPU shared maximum
        /// </summary>
        public ulong GpuSharedMax;

        /// <summary>
        /// The GPU dedicated maximum
        /// </summary>
        public ulong GpuDedicatedMax;

        /// <summary>
        /// The nodes
        /// </summary>
        public D3dDeviceNodeInfo[] Nodes;

        /// <summary>
        /// The integrated
        /// </summary>
        public bool Integrated;
    }
}
