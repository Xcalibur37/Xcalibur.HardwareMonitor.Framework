using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.D3D;

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

        OpenAdapterFromDeviceName(out uint status, deviceIdentifier, out D3DkmtOpenadapterfromdevicename adapter);
        if (status != WinNt.StatusSuccess) return false;

        GetAdapterType(out status, adapter, out D3DkmtAdaptertype adapterType);
        if (status != WinNt.StatusSuccess) return false;
        if (!adapterType.Value.HasFlag(D3DkmtAdaptertypeFlags.SoftwareDevice)) return false;

        deviceInfo.Integrated = !adapterType.Value.HasFlag(D3DkmtAdaptertypeFlags.HybridIntegrated);
        GetQueryStatisticsAdapterInformation(out status, adapter, 
            out D3DkmtQuerystatisticsAdapterInformation adapterInformation);
        if (status != WinNt.StatusSuccess) return false;

        uint segmentCount = adapterInformation.NbSegments;
        uint nodeCount = adapterInformation.NodeCount;

        deviceInfo.Nodes = new D3dDeviceNodeInfo[nodeCount];

        DateTime queryTime = DateTime.Now;
        for (uint nodeId = 0; nodeId < nodeCount; nodeId++)
        {
            GetNodeMetaData(out status, adapter, nodeId, out D3DkmtNodemetadata nodeMetaData);
            if (status != WinNt.StatusSuccess) return false;

            GetQueryStatisticsNode(out status, adapter, nodeId, out D3DkmtQuerystatisticsNodeInformation nodeInformation);
            if (status != WinNt.StatusSuccess) return false;

            deviceInfo.Nodes[nodeId] = new D3dDeviceNodeInfo
            {
                Id = nodeId,
                Name = GetNodeEngineTypeString(nodeMetaData),
                RunningTime = nodeInformation.GlobalInformation.RunningTime.QuadPart,
                QueryTime = queryTime
            };
        }

        GetSegmentSize(out status, adapter, out D3DkmtSegmentsizeinfo segmentSizeInfo);
        if (status != WinNt.StatusSuccess) return false;

        deviceInfo.GpuSharedLimit = segmentSizeInfo.SharedSystemMemorySize;
        deviceInfo.GpuVideoMemoryLimit = segmentSizeInfo.DedicatedVideoMemorySize;
        deviceInfo.GpuDedicatedLimit = segmentSizeInfo.DedicatedSystemMemorySize;

        for (uint segmentId = 0; segmentId < segmentCount; segmentId++)
        {
            GetQueryStatisticsSegment(out status, adapter, segmentId, 
                out D3DkmtQuerystatisticsSegmentInformation segmentInformation);
            if (status != WinNt.StatusSuccess) return false;

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
        return status == WinNt.StatusSuccess;
    }

    /// <summary>
    /// Gets the node engine type string.
    /// </summary>
    /// <param name="nodeMetaData">The node meta data.</param>
    /// <returns></returns>
    private static string GetNodeEngineTypeString(D3DkmtNodemetadata nodeMetaData) =>
        nodeMetaData.NodeData.EngineType switch
        {
            DxgkEngineType.DxgkEngineTypeOther => 
                "D3D " + (!string.IsNullOrWhiteSpace(nodeMetaData.NodeData.FriendlyName) 
                    ? nodeMetaData.NodeData.FriendlyName : "Other"),
            DxgkEngineType.DxgkEngineType3D => "D3D 3D",
            DxgkEngineType.DxgkEngineTypeVideoDecode => "D3D Video Decode",
            DxgkEngineType.DxgkEngineTypeVideoEncode => "D3D Video Encode",
            DxgkEngineType.DxgkEngineTypeVideoProcessing => "D3D Video Processing",
            DxgkEngineType.DxgkEngineTypeSceneAssembly => "D3D Scene Assembly",
            DxgkEngineType.DxgkEngineTypeCopy => "D3D Copy",
            DxgkEngineType.DxgkEngineTypeOverlay => "D3D Overlay",
            DxgkEngineType.DxgkEngineTypeCrypto => "D3D Crypto",
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
        D3DkmtOpenadapterfromdevicename adapter,
        out D3DkmtSegmentsizeinfo sizeInformation)
    {
        IntPtr segmentSizePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D3DkmtSegmentsizeinfo)));
        sizeInformation = new D3DkmtSegmentsizeinfo();
        Marshal.StructureToPtr(sizeInformation, segmentSizePtr, true);

        var queryAdapterInfo = new D3DkmtQueryadapterinfo
        {
            hAdapter = adapter.hAdapter,
            Type = Kmtqueryadapterinfotype.KmtqaitypeGetsegmentsize,
            pPrivateDriverData = segmentSizePtr,
            PrivateDriverDataSize = Marshal.SizeOf(typeof(D3DkmtSegmentsizeinfo))
        };

        status = Gdi32.D3DKMTQueryAdapterInfo(ref queryAdapterInfo);
        sizeInformation = Marshal.PtrToStructure<D3DkmtSegmentsizeinfo>(segmentSizePtr);
        Marshal.FreeHGlobal(segmentSizePtr);
    }

    /// <summary>
    /// Gets the node meta data.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="nodeId">The node identifier.</param>
    /// <param name="nodeMetaDataResult">The node meta data result.</param>
    private static void GetNodeMetaData(out uint status, D3DkmtOpenadapterfromdevicename adapter, uint nodeId, 
        out D3DkmtNodemetadata nodeMetaDataResult)
    {
        IntPtr nodeMetaDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D3DkmtNodemetadata)));
        nodeMetaDataResult = new D3DkmtNodemetadata { NodeOrdinalAndAdapterIndex = nodeId };
        Marshal.StructureToPtr(nodeMetaDataResult, nodeMetaDataPtr, true);

        var queryAdapterInfo = new D3DkmtQueryadapterinfo
        {
            hAdapter = adapter.hAdapter,
            Type = Kmtqueryadapterinfotype.KmtqaitypeNodemetadata,
            pPrivateDriverData = nodeMetaDataPtr,
            PrivateDriverDataSize = Marshal.SizeOf(typeof(D3DkmtNodemetadata))
        };

        status = Gdi32.D3DKMTQueryAdapterInfo(ref queryAdapterInfo);
        nodeMetaDataResult = Marshal.PtrToStructure<D3DkmtNodemetadata>(nodeMetaDataPtr);
        Marshal.FreeHGlobal(nodeMetaDataPtr);
    }

    /// <summary>
    /// Gets the query statistics node.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    /// <param name="nodeId">The node identifier.</param>
    /// <param name="nodeInformation">The node information.</param>
    private static void GetQueryStatisticsNode(out uint status, D3DkmtOpenadapterfromdevicename adapter, uint nodeId, 
        out D3DkmtQuerystatisticsNodeInformation nodeInformation)
    {
        var queryElement = new D3DkmtQuerystatisticsQueryElement { QueryNode = { NodeId = nodeId } };
        var queryStatistics = new D3DkmtQuerystatistics
        {
            AdapterLuid = adapter.AdapterLuid, Type = D3DkmtQuerystatisticsType.D3DkmtQuerystatisticsNode, QueryElement = queryElement
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
        D3DkmtOpenadapterfromdevicename adapter,
        uint segmentId,
        out D3DkmtQuerystatisticsSegmentInformation segmentInformation)
    {
        var queryElement = new D3DkmtQuerystatisticsQueryElement { QuerySegment = { SegmentId = segmentId } };
        var queryStatistics = new D3DkmtQuerystatistics
        {
            AdapterLuid = adapter.AdapterLuid, Type = D3DkmtQuerystatisticsType.D3DkmtQuerystatisticsSegment, QueryElement = queryElement
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
        D3DkmtOpenadapterfromdevicename adapter,
        out D3DkmtQuerystatisticsAdapterInformation adapterInformation)
    {
        var queryStatistics = new D3DkmtQuerystatistics
        {
            AdapterLuid = adapter.AdapterLuid, 
            Type = D3DkmtQuerystatisticsType.D3DkmtQuerystatisticsAdapter
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
    private static void GetAdapterType(out uint status, D3DkmtOpenadapterfromdevicename adapter, 
        out D3DkmtAdaptertype adapterTypeResult)
    {
        IntPtr adapterTypePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D3DkmtAdaptertype)));
        var queryAdapterInfo = new D3DkmtQueryadapterinfo
        {
            hAdapter = adapter.hAdapter,
            Type = Kmtqueryadapterinfotype.KmtqaitypeAdaptertype,
            pPrivateDriverData = adapterTypePtr,
            PrivateDriverDataSize = Marshal.SizeOf(typeof(D3DkmtAdaptertype))
        };

        status = Gdi32.D3DKMTQueryAdapterInfo(ref queryAdapterInfo);
        adapterTypeResult = Marshal.PtrToStructure<D3DkmtAdaptertype>(adapterTypePtr);
        Marshal.FreeHGlobal(adapterTypePtr);
    }

    /// <summary>
    /// Opens the name of the adapter from device.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="displayDeviceName">Display name of the device.</param>
    /// <param name="adapter">The adapter.</param>
    private static void OpenAdapterFromDeviceName(out uint status, string displayDeviceName, 
        out D3DkmtOpenadapterfromdevicename adapter)
    {
        adapter = new D3DkmtOpenadapterfromdevicename { pDeviceName = displayDeviceName };
        status = Gdi32.D3DKMTOpenAdapterFromDeviceName(ref adapter);
    }

    /// <summary>
    /// Closes the adapter.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="adapter">The adapter.</param>
    private static void CloseAdapter(out uint status, D3DkmtOpenadapterfromdevicename adapter)
    {
        var closeAdapter = new D3DkmtCloseadapter { hAdapter = adapter.hAdapter };
        status = Gdi32.D3DKMTCloseAdapter(ref closeAdapter);
    }
}
