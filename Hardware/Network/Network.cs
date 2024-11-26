using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Network;

/// <summary>
/// Network hardware
/// </summary>
/// <seealso cref="Hardware" />
public sealed class Network : Hardware
{
    #region Fields

    private readonly Sensor _dataDownloaded;
    private readonly Sensor _dataUploaded;
    private readonly Sensor _downloadSpeed;
    private readonly Sensor _networkUtilization;
    private readonly Sensor _uploadSpeed;
    private long _bytesDownloaded;
    private long _bytesUploaded;
    private long _lastTick;

    #endregion

    #region Properties

    /// <inheritdoc />
    public override HardwareType HardwareType => HardwareType.Network;

    /// <summary>
    /// Gets the network interface.
    /// </summary>
    /// <value>
    /// The network interface.
    /// </value>
    public NetworkInterface NetworkInterface { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Network"/> class.
    /// </summary>
    /// <param name="networkInterface">The network interface.</param>
    /// <param name="settings">The settings.</param>
    public Network(NetworkInterface networkInterface, ISettings settings)
        : base(networkInterface.Name, new Identifier("nic", networkInterface.Id), settings)
    {
        NetworkInterface = networkInterface;
        
        // Data Uploaded
        _dataUploaded = new Sensor("Data Uploaded", 2, SensorType.Data, this, settings);
        ActivateSensor(_dataUploaded);

        // Data Downloaded
        _dataDownloaded = new Sensor("Data Downloaded", 3, SensorType.Data, this, settings);
        ActivateSensor(_dataDownloaded);

        // Upload Speed
        _uploadSpeed = new Sensor("Upload Speed", 7, SensorType.Throughput, this, settings);
        ActivateSensor(_uploadSpeed);

        // Download Speed
        _downloadSpeed = new Sensor("Download Speed", 8, SensorType.Throughput, this, settings);
        ActivateSensor(_downloadSpeed);

        // Network Utilization
        _networkUtilization = new Sensor("Network Utilization", 1, SensorType.Load, this, settings);
        ActivateSensor(_networkUtilization);

        // Stats
        var stats = NetworkInterface.GetIPStatistics();
        _bytesUploaded = stats.BytesSent;
        _bytesDownloaded = stats.BytesReceived;
        
        // Timestamp
        _lastTick = Stopwatch.GetTimestamp();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void Update()
    {
        try
        {
            if (NetworkInterface == null) return;

            long newTick = Stopwatch.GetTimestamp();
            double dt = new TimeSpan(newTick - _lastTick).TotalSeconds;
            IPv4InterfaceStatistics interfaceStats = NetworkInterface.GetIPv4Statistics();

            // Report out the number of GB (2^30 Bytes) that this interface has up/downloaded. Note
            // that these values can reset back at zero (eg: after waking from sleep).
            _dataUploaded.Value = (float)(interfaceStats.BytesSent / (double)0x40000000);
            _dataDownloaded.Value = (float)(interfaceStats.BytesReceived / (double)0x40000000);

            // Detect a reset in interface stats if the new total is less than what was previously
            // seen. While setting the previous values to zero doesn't encapsulate the value the
            // instant before the reset, it is the best approximation we have.
            if (interfaceStats.BytesSent < _bytesUploaded || interfaceStats.BytesReceived < _bytesDownloaded)
            {
                _bytesUploaded = 0;
                _bytesDownloaded = 0;
            }

            long bytesUploaded = interfaceStats.BytesSent - _bytesUploaded;
            long bytesDownloaded = interfaceStats.BytesReceived - _bytesDownloaded;

            // Upload and download speeds are reported as the number of bytes transferred over the
            // time difference since the last report. In this way, the values represent the average
            // number of bytes up/downloaded in a second.
            _uploadSpeed.Value = (float)(bytesUploaded / dt);
            _downloadSpeed.Value = (float)(bytesDownloaded / dt);

            // Network speed is in bits per second, so when calculating the load on the NIC we first
            // grab the total number of bits up/downloaded
            long dbits = (bytesUploaded + bytesDownloaded) * 8;

            // Converts the ratio of total bits transferred over time, over theoretical max bits
            // transfer rate into a percentage load
            double load = (dbits / dt / NetworkInterface.Speed) * 100;

            // Finally clamp the value between 0% and 100% to avoid reporting nonsensical numbers
            _networkUtilization.Value = (float)Math.Min(Math.Max(load, 0), 100);

            // Store the recorded values and time, so they can be used in the next update
            _bytesUploaded = interfaceStats.BytesSent;
            _bytesDownloaded = interfaceStats.BytesReceived;
            _lastTick = newTick;
        }
        catch (NetworkInformationException networkInformationException) when (unchecked(networkInformationException.HResult == (int)0x80004005))
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (!networkInterface.Id.Equals(NetworkInterface?.Id)) continue;
                NetworkInterface = networkInterface;
                break;
            }
        }
    }

    #endregion
}
