// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Xcalibur.Extensions.V2;
using Xcalibur.HardwareMonitor.Framework.Hardware.Battery;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu;
using Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.Intel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Gpu;
using Xcalibur.HardwareMonitor.Framework.Hardware.Memory;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard;
using Xcalibur.HardwareMonitor.Framework.Hardware.Network;
using Xcalibur.HardwareMonitor.Framework.Hardware.Psu.Corsair;
using Xcalibur.HardwareMonitor.Framework.Hardware.Storage;

namespace Xcalibur.HardwareMonitor.Framework.Hardware;

/// <summary>
/// Stores all hardware groups and decides which devices should be enabled and updated.
/// </summary>
public class Computer : IComputer
{
    #region Fields

    private readonly List<IGroup> _groups = [];
    private readonly object _lock = new();
    private readonly ISettings _settings;

    private bool _batteryEnabled;
    private bool _cpuEnabled;
    private bool _gpuEnabled;
    private bool _memoryEnabled;
    private bool _motherboardEnabled;
    private bool _networkEnabled;
    private bool _open;
    private bool _psuEnabled;
    private SMBios _smbios;
    private bool _storageEnabled;

    #endregion

    #region Properties

    /// <inheritdoc />
    public IList<IHardware> Hardware
    {
        get
        {
            lock (_lock)
            {
                List<IHardware> list = [];
                _groups.Apply(x => list.AddRange(x.Hardware));
                return list;
            }
        }
    }

    /// <inheritdoc />
    public bool IsBatteryEnabled
    {
        get => _batteryEnabled;
        set
        {
            if (_open && value != _batteryEnabled)
            {
                if (value)
                {
                    Add(new BatteryGroup(_settings));
                }
                else
                {
                    RemoveType<BatteryGroup>();
                }
            }

            _batteryEnabled = value;
        }
    }

    /// <inheritdoc />
    public bool IsCpuEnabled
    {
        get => _cpuEnabled;
        set
        {
            if (_open && value != _cpuEnabled)
            {
                if (value)
                {
                    Add(new CpuGroup(_settings));
                }
                else
                {
                    RemoveType<CpuGroup>();
                }
            }

            _cpuEnabled = value;
        }
    }

    /// <inheritdoc />
    public bool IsGpuEnabled
    {
        get => _gpuEnabled;
        set
        {
            if (_open && value != _gpuEnabled)
            {
                if (value)
                {
                    Add(new AmdGpuGroup(_settings));
                    Add(new NvidiaGroup(_settings));
                    Add(new IntelGpuGroup(GetIntelCpus(), _settings));
                }
                else
                {
                    RemoveType<AmdGpuGroup>();
                    RemoveType<NvidiaGroup>();
                    RemoveType<IntelGpuGroup>();
                }
            }

            _gpuEnabled = value;
        }
    }

    /// <inheritdoc />
    public bool IsMemoryEnabled
    {
        get => _memoryEnabled;
        set
        {
            if (_open && value != _memoryEnabled)
            {
                if (value)
                {
                    Add(new MemoryGroup(_settings));
                }
                else
                {
                    RemoveType<MemoryGroup>();
                }
            }

            _memoryEnabled = value;
        }
    }

    /// <inheritdoc />
    public bool IsMotherboardEnabled
    {
        get => _motherboardEnabled;
        set
        {
            if (_open && value != _motherboardEnabled)
            {
                if (value)
                {
                    Add(new MotherboardGroup(_smbios, _settings));
                }
                else
                {
                    RemoveType<MotherboardGroup>();
                }
            }

            _motherboardEnabled = value;
        }
    }

    /// <inheritdoc />
    public bool IsNetworkEnabled
    {
        get => _networkEnabled;
        set
        {
            if (_open && value != _networkEnabled)
            {
                if (value)
                {
                    Add(new NetworkGroup(_settings));
                }
                else
                {
                    RemoveType<NetworkGroup>();
                }
            }

            _networkEnabled = value;
        }
    }

    /// <inheritdoc />
    public bool IsPsuEnabled
    {
        get => _psuEnabled;
        set
        {
            if (_open && value != _psuEnabled)
            {
                if (value)
                {
                    Add(new CorsairPsuGroup(_settings));
                }
                else
                {
                    RemoveType<CorsairPsuGroup>();
                }
            }

            _psuEnabled = value;
        }
    }

    /// <inheritdoc />
    public bool IsStorageEnabled
    {
        get => _storageEnabled;
        set
        {
            if (_open && value != _storageEnabled)
            {
                if (value)
                {
                    Add(new StorageGroup(_settings));
                }
                else
                {
                    RemoveType<StorageGroup>();
                }
            }

            _storageEnabled = value;
        }
    }

    /// <summary>
    /// Contains computer information table read in accordance with <see href="https://www.dmtf.org/standards/smbios">System Management BIOS (SMBIOS) Reference Specification</see>.
    /// </summary>
    public SMBios SMBios
    {
        get
        {
            if (!_open)
            {
                throw new InvalidOperationException("SMBIOS cannot be accessed before opening.");
            }
            return _smbios;
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new <see cref="IComputer" /> instance with basic initial <see cref="Settings" />.
    /// </summary>
    public Computer()
    {
        _settings = new Settings();
    }

    /// <summary>
    /// Creates a new <see cref="IComputer" /> instance with additional <see cref="ISettings" />.
    /// </summary>
    /// <param name="settings">Computer settings that will be transferred to each <see cref="IHardware" />.</param>
    public Computer(ISettings settings)
    {
        _settings = settings ?? new Settings();
    }

    #endregion

    #region Methods
    
    //// <inheritdoc />
    /// <summary>
    /// Triggers the <see cref="IVisitor.VisitComputer" /> method for the given observer.
    /// </summary>
    /// <param name="visitor">Observer who call to devices.</param>
    public void Accept(IVisitor visitor)
    {
        if (visitor != null)
        {
            visitor.VisitComputer(this);
        }
        else
        {
            throw new ArgumentNullException(nameof(visitor));
        }
    }

    /// <summary>
    /// If opened before, removes all <see cref="IGroup" /> and triggers <see cref="OpCode.Close" />, <see cref="InpOut.Close" /> and <see cref="Ring0.Close" />.
    /// </summary>
    public void Close()
    {
        if (!_open) return;

        lock (_lock)
        {
            while (_groups.Count > 0)
            {
                Remove(_groups[^1]);
            }
        }

        OpCode.Close();
        InpOut.Close();
        Ring0.Close();
        Mutexes.Close();

        _smbios = null;
        _open = false;
    }

    /// <summary>
    /// If this hasn't been opened before, opens <see cref="SMBios" />, <see cref="Ring0" />, <see cref="OpCode" /> and triggers the private <see cref="AddGroups" /> method depending on which categories are
    /// enabled.
    /// </summary>
    public void Open()
    {
        if (_open) return;

        _smbios = new SMBios();

        Ring0.Open();
        Mutexes.Open();
        OpCode.Open();

        AddGroups();

        _open = true;
    }

    /// <summary>
    /// If opened before, removes all <see cref="IGroup" /> and recreates it.
    /// </summary>
    public void Reset()
    {
        if (!_open) return;

        RemoveGroups();
        AddGroups();
    }

    /// <summary>
    /// Triggers the <see cref="IElement.Accept" /> method with the given visitor for each device in each group.
    /// </summary>
    /// <param name="visitor">Observer who call to devices.</param>
    public void Traverse(IVisitor visitor)
    {
        lock (_lock)
        {
            // Use a for-loop instead of foreach to avoid a collection modified exception after sleep, even though everything is under a lock.
            for (int i = 0; i < _groups.Count; i++)
            {
                IGroup group = _groups[i];
                for (int j = 0; j < group.Hardware.Count; j++)
                {
                    group.Hardware[j].Accept(visitor);
                }
            }
        }
    }

    /// <summary>
    /// Adds the specified group.
    /// </summary>
    /// <param name="group">The group.</param>
    private void Add(IGroup group)
    {
        if (group == null) return;

        lock (_lock)
        {
            if (_groups.Contains(group)) return;
            _groups.Add(group);

            if (group is IHardwareChanged hardwareChanged)
            {
                hardwareChanged.HardwareAdded += HardwareAddedEvent;
                hardwareChanged.HardwareRemoved += HardwareRemovedEvent;
            }
        }

        if (HardwareAdded == null) return;
        group.Hardware.Apply(x => HardwareAdded(x));
    }

    /// <summary>
    /// Gets the intel cpus.
    /// </summary>
    /// <returns></returns>
    private List<IntelCpu> GetIntelCpus()
    {
        // Create a temporary cpu group if one has not been added.
        lock (_lock)
        {
            IGroup cpuGroup = _groups.Find(x => x is CpuGroup) ?? new CpuGroup(_settings);
            return cpuGroup.Hardware.Select(x => x as IntelCpu).ToList();
        }
    }

    /// <summary>
    /// Removes the specified group.
    /// </summary>
    /// <param name="group">The group.</param>
    private void Remove(IGroup group)
    {
        lock (_lock)
        {
            if (!_groups.Contains(group)) return;
            _groups.Remove(group);

            if (group is IHardwareChanged hardwareChanged)
            {
                hardwareChanged.HardwareAdded -= HardwareAddedEvent;
                hardwareChanged.HardwareRemoved -= HardwareRemovedEvent;
            }
        }

        if (HardwareRemoved is not null)
        {
            group.Hardware.Apply(x => HardwareRemoved(x));
        }

        group.Close();
    }

    /// <summary>
    /// Adds the groups.
    /// </summary>
    private void AddGroups()
    {
        if (_motherboardEnabled)
        {
            Add(new MotherboardGroup(_smbios, _settings));
        }

        if (_cpuEnabled)
        {
            Add(new CpuGroup(_settings));
        }

        if (_memoryEnabled)
        {
            Add(new MemoryGroup(_settings));
        }

        if (_gpuEnabled)
        {
            Add(new AmdGpuGroup(_settings));
            Add(new NvidiaGroup(_settings));
            Add(new IntelGpuGroup(GetIntelCpus(), _settings));
        }

        if (_storageEnabled)
        {
            Add(new StorageGroup(_settings));
        }

        if (_networkEnabled)
        {
            Add(new NetworkGroup(_settings));
        }

        if (_psuEnabled)
        {
            Add(new CorsairPsuGroup(_settings));
        }

        if (_batteryEnabled)
        {
            Add(new BatteryGroup(_settings));
        }
    }

    /// <summary>
    /// Removes the groups.
    /// </summary>
    private void RemoveGroups()
    {
        lock (_lock)
        {
            while (_groups.Count > 0)
            {
                Remove(_groups[^1]);
            }
        }
    }

    /// <summary>
    /// Removes the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void RemoveType<T>() where T : IGroup
    {
        List<T> list = [];
        lock (_lock)
        {
            _groups
                .Where(x => x is T)
                .Apply(y => list.Add((T)y));
        }
        list.Apply(x => Remove(x));
    }

    #endregion

    #region Events

    /// <inheritdoc />
    public event HardwareEventHandler HardwareAdded;

    /// <summary>
    /// "Hardware added" event.
    /// </summary>
    /// <param name="hardware">The hardware.</param>
    private void HardwareAddedEvent(IHardware hardware)
    {
        HardwareAdded?.Invoke(hardware);
    }

    /// <inheritdoc />
    public event HardwareEventHandler HardwareRemoved;

    /// <summary>
    /// "Hardware removed" event.
    /// </summary>
    /// <param name="hardware">The hardware.</param>
    private void HardwareRemovedEvent(IHardware hardware)
    {
        HardwareRemoved?.Invoke(hardware);
    }

    #endregion
}
