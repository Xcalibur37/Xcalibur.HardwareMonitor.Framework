using System;
using System.Collections.Generic;
using System.Linq;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Interop;
using Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Gpu.AMD;

/// <summary>
/// AMD GPU Group
/// </summary>
/// <seealso cref="IGroup" />
internal class AmdGpuGroup : IGroup
{
    #region Fields

    private readonly List<AmdGpu> _hardware = [];
    private IntPtr _context = IntPtr.Zero;
    private AdlStatus _status;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _hardware;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AmdGpuGroup" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public AmdGpuGroup(ISettings settings)
    {
        SetHardware(settings);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Stop updating this group in the future.
    /// </summary>
    public void Close()
    {
        try
        {
            _hardware.Apply(x => x.Close());
            if (_status != AdlStatus.AdlOk || _context == IntPtr.Zero) return;
            AtiAdlxx.ADL2_Main_Control_Destroy(_context);
        }
        catch (Exception)
        {
            // Do Nothing
        }
    }

    /// <summary>
    /// Determines whether [is already added] [the specified bus number].
    /// </summary>
    /// <param name="busNumber">The bus number.</param>
    /// <param name="deviceNumber">The device number.</param>
    /// <returns>
    ///   <c>true</c> if [is already added] [the specified bus number]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsAlreadyAdded(int busNumber, int deviceNumber) 
        => _hardware.Any(g => g.BusNumber == busNumber && g.DeviceNumber == deviceNumber);

    /// <summary>
    /// Sets the hardware.
    /// </summary>
    /// <param name="settings">The settings.</param>
    private void SetHardware(ISettings settings)
    {
        try
        {
            List<AmdGpu> potentialHardware = [];

            // ADL status
            _status = AtiAdlxx.ADL2_Main_Control_Create(AtiAdlxx.MainMemoryAlloc, 1, ref _context);
            if (_status != AdlStatus.AdlOk) return;

            // Get ADL adapter count
            int numberOfAdapters = 0;
            AtiAdlxx.ADL2_Adapter_NumberOfAdapters_Get(_context, ref numberOfAdapters);
            if (numberOfAdapters <= 0) return;

            // ADL Status: OK
            // Process adapters
            var adapterInfo = new AdlAdapterInfo[numberOfAdapters];
            if (AtiAdlxx.ADL2_Adapter_AdapterInfo_Get(ref _context, adapterInfo) == AdlStatus.AdlOk)
            {
                for (int i = 0; i < numberOfAdapters; i++)
                {
                    uint device = 0;
                    var currentAdapter = adapterInfo[i];
                    AdlGcnInfo gcnInfo = new();
                    AdlpmLogSupportInfo pmLogSupportInfo = new();
                    AtiAdlxx.ADL2_Adapter_Active_Get(_context, currentAdapter.AdapterIndex, out _);

                    // Get ASIC GCN architecture
                    if (AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_GcnAsicInfo_Get)))
                    {
                        AtiAdlxx.ADL2_GcnAsicInfo_Get(_context, currentAdapter.AdapterIndex, ref gcnInfo);
                    }


                    var hasSensorsSupported = false;
                    if (AtiAdlxx.UsePmLogForFamily(gcnInfo.ASICFamilyId) &&
                        AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Adapter_PMLog_Support_Get)) &&
                        AtiAdlxx.ADL_Method_Exists(nameof(AtiAdlxx.ADL2_Device_PMLog_Device_Create)))
                    {
                        if (AdlStatus.AdlOk ==
                            AtiAdlxx.ADL2_Device_PMLog_Device_Create(_context, currentAdapter.AdapterIndex,
                                ref device) &&
                            AdlStatus.AdlOk == AtiAdlxx.ADL2_Adapter_PMLog_Support_Get(_context,
                                currentAdapter.AdapterIndex, ref pmLogSupportInfo))
                        {
                            // Has supported sensors
                            hasSensorsSupported = pmLogSupportInfo.usSensors.Count(x =>
                                x != (ushort)AdlpmLogSensors.AdlSensorMaxtypes) > 0;
                        }

                        // Destroy device
                        if (device != 0)
                        {
                            AtiAdlxx.ADL2_Device_PMLog_Device_Destroy(_context, device);
                        }
                    }

                    // No UDID, not ATI, or is already added, exit
                    if (string.IsNullOrEmpty(currentAdapter.UDID) ||
                        currentAdapter.VendorID != AtiAdlxx.AtiVendorId ||
                        IsAlreadyAdded(currentAdapter.BusNumber, currentAdapter.DeviceNumber)) continue;

                    // Evaluate which list to add new AMD GPU based on supported sensors
                    var amdGpu = new AmdGpu(_context, currentAdapter, gcnInfo, settings);
                    if (hasSensorsSupported)
                    {
                        _hardware.Add(amdGpu);
                    }
                    else
                    {
                        potentialHardware.Add(amdGpu);
                    }
                }
            }

            // Add potential hardware to the primary hardware list
            foreach (var amdGpus in potentialHardware.GroupBy(x => $"{x.BusNumber}-{x.DeviceNumber}"))
            {
                AmdGpu amdGpu = amdGpus.OrderByDescending(x => x.Sensors.Length).FirstOrDefault();
                if (amdGpu == null || IsAlreadyAdded(amdGpu.BusNumber, amdGpu.DeviceNumber)) continue;
                _hardware.Add(amdGpu);
            }
        }
        catch (DllNotFoundException)
        {
            // Do Nothing
        }
        catch (EntryPointNotFoundException e)
        {
            // Do Nothing
        }

        #endregion
    }
}
