// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD.Amd17;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.Intel;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu;

/// <summary>
/// CPU Group
/// </summary>
/// <seealso cref="IGroup" />
internal class CpuGroup : IGroup
{
    #region Fields

    private readonly List<CpuBase> _hardware = [];
    private CpuId[][][] _threads;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _hardware;

    #endregion

    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CpuGroup"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public CpuGroup(ISettings settings)
    {
        // Build processors
        BuildProcessors(settings);
    }

    #endregion

    #region Methods
    
    /// <summary>
    /// Stop updating this group in the future.
    /// </summary>
    public void Close()
    {
        _hardware.Apply(x => x.Close());
    }
    
    /// <summary>
    /// Gets the processor threads.
    /// </summary>
    /// <returns></returns>
    private static CpuId[][] GetProcessorThreads()
    {
        List<CpuId> threads = [];

        for (int i = 0; i < ThreadAffinity.ProcessorGroupCount; i++)
        {
            for (int j = 0; j < 192; j++)
            {
                try
                {
                    var cpuid = CpuId.Get(i, j);
                    if (cpuid == null) continue;
                    threads.Add(cpuid);
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Continue...
                }
            }
        }

        SortedDictionary<uint, List<CpuId>> processors = new();
        foreach (CpuId thread in threads)
        {
            processors.TryGetValue(thread.ProcessorId, out List<CpuId> list);
            if (list == null)
            {
                list = new List<CpuId>();
                processors.Add(thread.ProcessorId, list);
            }
            list.Add(thread);
        }

        var processorThreads = new CpuId[processors.Count][];
        int index = 0;
        foreach (var list in processors.Values)
        {
            processorThreads[index] = list.ToArray();
            index++;
        }

        return processorThreads;
    }

    /// <summary>
    /// Groups the threads by core.
    /// </summary>
    /// <param name="threads">The threads.</param>
    /// <returns></returns>
    private static CpuId[][] GroupThreadsByCore(IEnumerable<CpuId> threads)
    {
        SortedDictionary<uint, List<CpuId>> cores = new();
        foreach (CpuId thread in threads)
        {
            cores.TryGetValue(thread.CoreId, out var coreList);
            if (coreList == null)
            {
                coreList = [];
                cores.Add(thread.CoreId, coreList);
            }
            coreList.Add(thread);
        }

        var coreThreads = new CpuId[cores.Count][];
        int index = 0;
        foreach (var list in cores.Values)
        {
            coreThreads[index] = list.ToArray();
            index++;
        }

        return coreThreads;
    }

    /// <summary>
    /// Builds the processors.
    /// </summary>
    /// <param name="settings">The settings.</param>
    private void BuildProcessors(ISettings settings)
    {
        CpuId[][] processorThreads = GetProcessorThreads();
        _threads = new CpuId[processorThreads.Length][][];

        // Process each thread
        int index = 0;
        foreach (CpuId[] threads in processorThreads)
        {
            if (threads.Length == 0) continue;

            // Core threads
            var coreThreads = GroupThreadsByCore(threads);

            // Map to current thread
            _threads[index] = coreThreads;

            // Add processor to hardware collection
            AddProcessorToHardware(threads[0], index, coreThreads, settings);

            // Next
            index++;
        }
    }

    /// <summary>
    /// Adds processor
    /// </summary>
    /// <param name="thread">The thread.</param>
    /// <param name="index">The index.</param>
    /// <param name="coreThreads">The core threads.</param>
    /// <param name="settings">The settings.</param>
    private void AddProcessorToHardware(CpuId thread, int index, CpuId[][] coreThreads, ISettings settings)
    {
        // Map processor to hardware collection
        switch (thread.Vendor)
        {
            case CpuVendor.Intel:
                _hardware.Add(new IntelCpu(index, coreThreads, settings));
                break;
            case CpuVendor.Amd:
                switch (thread.Family)
                {
                    case 0x0F:
                        _hardware.Add(new Amd0FCpu(index, coreThreads, settings));
                        break;
                    case 0x10 or 0x11 or 0x12 or 0x14 or 0x15 or 0x16:
                        _hardware.Add(new Amd10Cpu(index, coreThreads, settings));
                        break;
                    case 0x17 or 0x19 or 0x1A:
                        _hardware.Add(new Amd17Cpu(index, coreThreads, settings));
                        break;
                    default:
                        _hardware.Add(new CpuBase(index, coreThreads, settings));
                        break;
                }

                break;
            case CpuVendor.Unknown:
            default:
                _hardware.Add(new CpuBase(index, coreThreads, settings));
                break;
        }
    }

    #endregion
}
