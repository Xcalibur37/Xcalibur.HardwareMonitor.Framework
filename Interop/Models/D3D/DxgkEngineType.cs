namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D
{
    /// <summary>
    /// The DXGK_ENGINE_TYPE enumeration indicates the type of engine on a GPU node.
    /// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dkmdt/ne-d3dkmdt-dxgk_engine_type
    /// </summary>
    internal enum DxgkEngineType
    {
        DxgkEngineTypeOther = 0,
        DxgkEngineType3D = 1,
        DxgkEngineTypeVideoDecode = 2,
        DxgkEngineTypeVideoEncode = 3,
        DxgkEngineTypeVideoProcessing = 4,
        DxgkEngineTypeSceneAssembly = 5,
        DxgkEngineTypeCopy = 6,
        DxgkEngineTypeOverlay = 7,
        DxgkEngineTypeCrypto,
        DxgkEngineTypeMax
    }
}
