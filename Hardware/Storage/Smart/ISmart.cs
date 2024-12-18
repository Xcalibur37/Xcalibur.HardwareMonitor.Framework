﻿using System;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Smart;

/// <summary>
/// Smart interface
/// </summary>
/// <seealso cref="IDisposable" />
public interface ISmart : IDisposable
{
    /// <summary>
    /// Returns true if ... is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
    /// </value>
    bool IsValid { get; }

    /// <summary>
    /// Closes this instance.
    /// </summary>
    void Close();

    /// <summary>
    /// Enables the smart.
    /// </summary>
    /// <returns></returns>
    bool EnableSmart();

    /// <summary>
    /// Reads the name and firmware revision.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="firmwareRevision">The firmware revision.</param>
    /// <returns></returns>
    bool ReadNameAndFirmwareRevision(out string name, out string firmwareRevision);

    /// <summary>
    /// Reads the smart data.
    /// </summary>
    /// <returns></returns>
    Interop.Models.Kernel32.SmartAttribute[] ReadSmartData();

    /// <summary>
    /// Reads the smart thresholds.
    /// </summary>
    /// <returns></returns>
    Interop.Models.Kernel32.SmartThreshold[] ReadSmartThresholds();
}