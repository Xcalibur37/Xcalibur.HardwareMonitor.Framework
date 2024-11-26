using System;
using Xcalibur.HardwareMonitor.Framework.Interop;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Storage.Nvme;

/// <summary>
/// NVME Information
/// </summary>
public class NvmeInfo
{
    #region Properties

    /// <summary>
    /// Gets or sets the controller identifier.
    /// </summary>
    /// <value>
    /// The controller identifier.
    /// </value>
    public ushort ControllerId { get; protected set; }

    /// <summary>
    /// Gets or sets the ieee.
    /// </summary>
    /// <value>
    /// The ieee.
    /// </value>
    public byte[] IEEE { get; protected set; }

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    /// <value>
    /// The index.
    /// </value>
    public int Index { get; protected set; }

    /// <summary>
    /// Gets or sets the model.
    /// </summary>
    /// <value>
    /// The model.
    /// </value>
    public string Model { get; protected set; }

    /// <summary>
    /// Gets or sets the number namespaces.
    /// </summary>
    /// <value>
    /// The number namespaces.
    /// </value>
    public uint NumberNamespaces { get; protected set; }

    /// <summary>
    /// Gets or sets the revision.
    /// </summary>
    /// <value>
    /// The revision.
    /// </value>
    public string Revision { get; protected set; }

    /// <summary>
    /// Gets or sets the serial.
    /// </summary>
    /// <value>
    /// The serial.
    /// </value>
    public string Serial { get; protected set; }

    /// <summary>
    /// Gets or sets the ssvid.
    /// </summary>
    /// <value>
    /// The ssvid.
    /// </value>
    public ushort SSVID { get; protected set; }

    /// <summary>
    /// Gets or sets the total capacity.
    /// </summary>
    /// <value>
    /// The total capacity.
    /// </value>
    public ulong TotalCapacity { get; protected set; }

    /// <summary>
    /// Gets or sets the unallocated capacity.
    /// </summary>
    /// <value>
    /// The unallocated capacity.
    /// </value>
    public ulong UnallocatedCapacity { get; protected set; }

    /// <summary>
    /// Gets or sets the vid.
    /// </summary>
    /// <value>
    /// The vid.
    /// </value>
    public ushort VID { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NvmeInfo"/> class.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="data">The data.</param>
    public NvmeInfo(int index, Kernel32.NVME_IDENTIFY_CONTROLLER_DATA data)
    {
        Index = index;
        VID = data.VID;
        SSVID = data.SSVID;
        Serial = NvmeHelper.GetString(data.SN);
        Model = NvmeHelper.GetString(data.MN);
        Revision = NvmeHelper.GetString(data.FR);
        IEEE = data.IEEE;
        TotalCapacity = BitConverter.ToUInt64(data.TNVMCAP, 0); // 128bit little endian
        UnallocatedCapacity = BitConverter.ToUInt64(data.UNVMCAP, 0);
        ControllerId = data.CNTLID;
        NumberNamespaces = data.NN;
    }

    #endregion
}