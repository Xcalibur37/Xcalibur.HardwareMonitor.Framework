using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.D3d;

/// <summary>
/// D3D Display Device
/// </summary>
internal static class D3dDisplayDevice
{
    /// <summary>
    /// Gets the device identifiers.
    /// </summary>
    /// <returns></returns>
    public static string[] GetDeviceIdentifiers()
    {
        if (CfgMgr32.CM_Get_Device_Interface_List_Size(out uint size, ref CfgMgr32.GUID_DISPLAY_DEVICE_ARRIVAL, null, 
                CfgMgr32.CM_GET_DEVICE_INTERFACE_LIST_PRESENT) != CfgMgr32.CR_SUCCESS)
        {
            return null;
        }
        var data = new char[size];
        return CfgMgr32.CM_Get_Device_Interface_List(ref CfgMgr32.GUID_DISPLAY_DEVICE_ARRIVAL, null, data, (uint)data.Length, 
            CfgMgr32.CM_GET_DEVICE_INTERFACE_LIST_PRESENT) == CfgMgr32.CR_SUCCESS 
            ? new string(data).Split('\0').Where(m => !string.IsNullOrEmpty(m)).ToArray() 
            : null;
    }

    /// <summary>
    /// Gets the actual device identifier.
    /// </summary>
    /// <param name="deviceIdentifier">The device identifier.</param>
    /// <returns></returns>
    public static string GetActualDeviceIdentifier(string deviceIdentifier)
    {
        if (string.IsNullOrEmpty(deviceIdentifier)) return deviceIdentifier;

        // For example:
        // \\?\ROOT#BasicRender#0000#{1ca05180-a699-450a-9a0c-de4fbe3ddd89}  -->  ROOT\BasicRender\0000
        // \\?\PCI#VEN_1002&DEV_731F&SUBSYS_57051682&REV_C4#6&e539058&0&00000019#{1ca05180-a699-450a-9a0c-de4fbe3ddd89}  -->  PCI\VEN_1002&DEV_731F&SUBSYS_57051682&REV_C4\6&e539058&0&00000019

        if (deviceIdentifier.StartsWith(@"\\?\"))
        {
            deviceIdentifier = deviceIdentifier[4..];
        }

        if (deviceIdentifier.Length <= 0 || deviceIdentifier[^1] != '}') return deviceIdentifier.Replace('#', '\\');
        int lastIndex = deviceIdentifier.LastIndexOf('{');
        if (lastIndex <= 0) return deviceIdentifier.Replace('#', '\\');
        deviceIdentifier = deviceIdentifier.Substring(0, lastIndex - 1);

        return deviceIdentifier.Replace('#', '\\');
    }

    /// <summary>
    /// Gets the device information by identifier.
    /// </summary>
    /// <param name="deviceIdentifier">The device identifier.</param>
    /// <param name="deviceInfo">The device information.</param>
    /// <returns></returns>
    public static bool GetDeviceInfoByIdentifier(string deviceIdentifier, out D3dDeviceInfo deviceInfo)
    {
        deviceInfo = new D3dDeviceInfo();

        OpenAdapterFromDeviceName(out uint status, deviceIdentifier, out D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter);
        if (status != WinNt.STATUS_SUCCESS) return false;

        GetAdapterType(out status, adapter, out D3dkmth.D3DKMT_ADAPTERTYPE adapterType);
        if (status != WinNt.STATUS_SUCCESS) return false;
        if (!adapterType.Value.HasFlag(D3dkmth.D3DKMT_ADAPTERTYPE_FLAGS.SoftwareDevice)) return false;

        deviceInfo.Integrated = !adapterType.Value.HasFlag(D3dkmth.D3DKMT_ADAPTERTYPE_FLAGS.HybridIntegrated);
        GetQueryStatisticsAdapterInformation(out status, adapter, 
            out D3dkmth.D3DKMT_QUERYSTATISTICS_ADAPTER_INFORMATION adapterInformation);
        if (status != WinNt.STATUS_SUCCESS) return false;

        uint segmentCount = adapterInformation.NbSegments;
        uint nodeCount = adapterInformation.NodeCount;

        deviceInfo.Nodes = new D3dDeviceNodeInfo[nodeCount];

        DateTime queryTime = DateTime.Now;
        for (uint nodeId = 0; nodeId < nodeCount; nodeId++)
        {
            GetNodeMetaData(out status, adapter, nodeId, out D3dkmth.D3DKMT_NODEMETADATA nodeMetaData);
            if (status != WinNt.STATUS_SUCCESS) return false;

            GetQueryStatisticsNode(out status, adapter, nodeId, out D3dkmth.D3DKMT_QUERYSTATISTICS_NODE_INFORMATION nodeInformation);
            if (status != WinNt.STATUS_SUCCESS) return false;

            deviceInfo.Nodes[nodeId] = new D3dDeviceNodeInfo
            {
                Id = nodeId,
                Name = GetNodeEngineTypeString(nodeMetaData),
                RunningTime = nodeInformation.GlobalInformation.RunningTime.QuadPart,
                QueryTime = queryTime
            };
        }

        GetSegmentSize(out status, adapter, out D3dkmth.D3DKMT_SEGMENTSIZEINFO segmentSizeInfo);
        if (status != WinNt.STATUS_SUCCESS) return false;

        deviceInfo.GpuSharedLimit = segmentSizeInfo.SharedSystemMemorySize;
        deviceInfo.GpuVideoMemoryLimit = segmentSizeInfo.DedicatedVideoMemorySize;
        deviceInfo.GpuDedicatedLimit = segmentSizeInfo.DedicatedSystemMemorySize;

        for (uint segmentId = 0; segmentId < segmentCount; segmentId++)
        {
            GetQueryStatisticsSegment(out status, adapter, segmentId, 
                out D3dkmth.D3DKMT_QUERYSTATISTICS_SEGMENT_INFORMATION segmentInformation);
            if (status != WinNt.STATUS_SUCCESS) return false;

            ulong bytesResident = segmentInformation.BytesResident;
            ulong bytesCommitted = segmentInformation.BytesCommitted;

            if (segmentInformation.Aperture == 1)
            {
                deviceInfo.GpuSharedUsed += bytesResident;
                deviceInfo.GpuSharedMax += bytesCommitted;
            }
            else
            {
                deviceInfo.GpuDedicatedUsed += bytesResident;
                deviceInfo.GpuDedicatedMax += bytesCommitted;
            }
        }

        CloseAdapter(out status, adapter);
        return status == WinNt.STATUS_SUCCESS;
    }

    /// <summary>
    /// Gets the node engine type string.
    /// </summary>
    /// <param name="nodeMetaData">The node meta data.</param>
    /// <returns></returns>
    private static string GetNodeEngineTypeString(D3dkmth.D3DKMT_NODEMETADATA nodeMetaData) =>
        nodeMetaData.NodeData.EngineType switch
        {
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_OTHER => 
                "D3D " + (!string.IsNullOrWhiteSpace(nodeMetaData.NodeData.FriendlyName) 
                    ? nodeMetaData.NodeData.FriendlyName : "Other"),
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_3D => "D3D 3D",
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_VIDEO_DECODE => "D3D Video Decode",
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_VIDEO_ENCODE => "D3D Video Encode",
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_VIDEO_PROCESSING => "D3D Video Processing",
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_SCENE_ASSEMBLY => "D3D Scene Assembly",
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_COPY => "D3D Copy",
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_OVERLAY => "D3D Overlay",
            D3dkmdt.DXGK_ENGINE_TYPE.DXGK_ENGINE_TYPE_CRYPTO => "D3D Crypto",
            _ => "D3D Unknown"
        };

    /// <summary>
    /// Gets the size of the segment.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="sizeInformation">The size information.</param>
    private static void GetSegmentSize
    (
        out uint status,
        D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter,
        out D3dkmth.D3DKMT_SEGMENTSIZEINFO sizeInformation)
    {
        IntPtr segmentSizePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D3dkmth.D3DKMT_SEGMENTSIZEINFO)));
        sizeInformation = new D3dkmth.D3DKMT_SEGMENTSIZEINFO();
        Marshal.StructureToPtr(sizeInformation, segmentSizePtr, true);

        var queryAdapterInfo = new D3dkmth.D3DKMT_QUERYADAPTERINFO
        {
            hAdapter = adapter.hAdapter,
            Type = D3dkmth.KMTQUERYADAPTERINFOTYPE.KMTQAITYPE_GETSEGMENTSIZE,
            pPrivateDriverData = segmentSizePtr,
            PrivateDriverDataSize = Marshal.SizeOf(typeof(D3dkmth.D3DKMT_SEGMENTSIZEINFO))
        };

        status = Gdi32.D3DKMTQueryAdapterInfo(ref queryAdapterInfo);
        sizeInformation = Marshal.PtrToStructure<D3dkmth.D3DKMT_SEGMENTSIZEINFO>(segmentSizePtr);
        Marshal.FreeHGlobal(segmentSizePtr);
    }

    /// <summary>
    /// Gets the node meta data.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="nodeId">The node identifier.</param>
    /// <param name="nodeMetaDataResult">The node meta data result.</param>
    private static void GetNodeMetaData(out uint status, D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter, uint nodeId, 
        out D3dkmth.D3DKMT_NODEMETADATA nodeMetaDataResult)
    {
        IntPtr nodeMetaDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D3dkmth.D3DKMT_NODEMETADATA)));
        nodeMetaDataResult = new D3dkmth.D3DKMT_NODEMETADATA { NodeOrdinalAndAdapterIndex = nodeId };
        Marshal.StructureToPtr(nodeMetaDataResult, nodeMetaDataPtr, true);

        var queryAdapterInfo = new D3dkmth.D3DKMT_QUERYADAPTERINFO
        {
            hAdapter = adapter.hAdapter,
            Type = D3dkmth.KMTQUERYADAPTERINFOTYPE.KMTQAITYPE_NODEMETADATA,
            pPrivateDriverData = nodeMetaDataPtr,
            PrivateDriverDataSize = Marshal.SizeOf(typeof(D3dkmth.D3DKMT_NODEMETADATA))
        };

        status = Gdi32.D3DKMTQueryAdapterInfo(ref queryAdapterInfo);
        nodeMetaDataResult = Marshal.PtrToStructure<D3dkmth.D3DKMT_NODEMETADATA>(nodeMetaDataPtr);
        Marshal.FreeHGlobal(nodeMetaDataPtr);
    }

    /// <summary>
    /// Gets the query statistics node.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="nodeId">The node identifier.</param>
    /// <param name="nodeInformation">The node information.</param>
    private static void GetQueryStatisticsNode(out uint status, D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter, uint nodeId, 
        out D3dkmth.D3DKMT_QUERYSTATISTICS_NODE_INFORMATION nodeInformation)
    {
        var queryElement = new D3dkmth.D3DKMT_QUERYSTATISTICS_QUERY_ELEMENT { QueryNode = { NodeId = nodeId } };
        var queryStatistics = new D3dkmth.D3DKMT_QUERYSTATISTICS
        {
            AdapterLuid = adapter.AdapterLuid, Type = D3dkmth.D3DKMT_QUERYSTATISTICS_TYPE.D3DKMT_QUERYSTATISTICS_NODE, QueryElement = queryElement
        };
        status = Gdi32.D3DKMTQueryStatistics(ref queryStatistics);
        nodeInformation = queryStatistics.QueryResult.NodeInformation;
    }

    /// <summary>
    /// Gets the query statistics segment.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="segmentId">The segment identifier.</param>
    /// <param name="segmentInformation">The segment information.</param>
    private static void GetQueryStatisticsSegment
    (
        out uint status,
        D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter,
        uint segmentId,
        out D3dkmth.D3DKMT_QUERYSTATISTICS_SEGMENT_INFORMATION segmentInformation)
    {
        var queryElement = new D3dkmth.D3DKMT_QUERYSTATISTICS_QUERY_ELEMENT { QuerySegment = { SegmentId = segmentId } };
        var queryStatistics = new D3dkmth.D3DKMT_QUERYSTATISTICS
        {
            AdapterLuid = adapter.AdapterLuid, Type = D3dkmth.D3DKMT_QUERYSTATISTICS_TYPE.D3DKMT_QUERYSTATISTICS_SEGMENT, QueryElement = queryElement
        };
        status = Gdi32.D3DKMTQueryStatistics(ref queryStatistics);
        segmentInformation = queryStatistics.QueryResult.SegmentInformation;
    }

    /// <summary>
    /// Gets the query statistics adapter information.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="adapterInformation">The adapter information.</param>
    private static void GetQueryStatisticsAdapterInformation
    (
        out uint status,
        D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter,
        out D3dkmth.D3DKMT_QUERYSTATISTICS_ADAPTER_INFORMATION adapterInformation)
    {
        var queryStatistics = new D3dkmth.D3DKMT_QUERYSTATISTICS
        {
            AdapterLuid = adapter.AdapterLuid, 
            Type = D3dkmth.D3DKMT_QUERYSTATISTICS_TYPE.D3DKMT_QUERYSTATISTICS_ADAPTER
        };
        status = Gdi32.D3DKMTQueryStatistics(ref queryStatistics);
        adapterInformation = queryStatistics.QueryResult.AdapterInformation;
    }

    /// <summary>
    /// Gets the type of the adapter.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="adapterTypeResult">The adapter type result.</param>
    private static void GetAdapterType(out uint status, D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter, 
        out D3dkmth.D3DKMT_ADAPTERTYPE adapterTypeResult)
    {
        IntPtr adapterTypePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D3dkmth.D3DKMT_ADAPTERTYPE)));
        var queryAdapterInfo = new D3dkmth.D3DKMT_QUERYADAPTERINFO
        {
            hAdapter = adapter.hAdapter,
            Type = D3dkmth.KMTQUERYADAPTERINFOTYPE.KMTQAITYPE_ADAPTERTYPE,
            pPrivateDriverData = adapterTypePtr,
            PrivateDriverDataSize = Marshal.SizeOf(typeof(D3dkmth.D3DKMT_ADAPTERTYPE))
        };

        status = Gdi32.D3DKMTQueryAdapterInfo(ref queryAdapterInfo);
        adapterTypeResult = Marshal.PtrToStructure<D3dkmth.D3DKMT_ADAPTERTYPE>(adapterTypePtr);
        Marshal.FreeHGlobal(adapterTypePtr);
    }

    /// <summary>
    /// Opens the name of the adapter from device.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="displayDeviceName">Display name of the device.</param>
    /// <param name="adapter">The adapter.</param>
    private static void OpenAdapterFromDeviceName(out uint status, string displayDeviceName, 
        out D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter)
    {
        adapter = new D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME { pDeviceName = displayDeviceName };
        status = Gdi32.D3DKMTOpenAdapterFromDeviceName(ref adapter);
    }

    /// <summary>
    /// Closes the adapter.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    private static void CloseAdapter(out uint status, D3dkmth.D3DKMT_OPENADAPTERFROMDEVICENAME adapter)
    {
        var closeAdapter = new D3dkmth.D3DKMT_CLOSEADAPTER { hAdapter = adapter.hAdapter };
        status = Gdi32.D3DKMTCloseAdapter(ref closeAdapter);
    }
}
