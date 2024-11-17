using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xcalibur.Extensions.V2;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Network;

/// <summary>
/// Network group.
/// </summary>
/// <seealso cref="IGroup" />
/// <seealso cref="IHardwareChanged" />
internal class NetworkGroup : IGroup, IHardwareChanged
{
    #region Fields

    private readonly Dictionary<string, Network> _networks = new();
    private readonly object _scanLock = new();
    private readonly ISettings _settings;
    private readonly List<Network> _hardware = [];

    #endregion

    #region Properties

    /// <summary>
    /// Gets a list that stores information about <see cref="IHardware" /> in a given group.
    /// </summary>
    public IReadOnlyList<IHardware> Hardware => _hardware;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkGroup"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public NetworkGroup(ISettings settings)
    {
        _settings = settings;
        UpdateNetworkInterfaces(settings);

        NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAddressChanged;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Close open devices.
    /// </summary>
    public void Close()
    {
        NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
        NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAddressChanged;
        _hardware.Apply(x => x.Close());
    }

    /// <summary>
    /// Gets the network interfaces.
    /// </summary>
    /// <returns></returns>
    private static IOrderedEnumerable<NetworkInterface> GetNetworkInterfaces()
    {
        var retry = 0;
        while (retry++ < 5)
        {
            try
            {
                return NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(GetDesiredNetworkTypes)
                    .OrderBy(x => x.Name);
            }
            catch (NetworkInformationException)
            {
                // Disabling IPv4 while running can cause a NetworkInformationException: The pipe is being closed.
                // This can be retried.
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the desired network types.
    /// </summary>
    /// <param name="nic">The nic.</param>
    /// <returns></returns>
    private static bool GetDesiredNetworkTypes(NetworkInterface nic) =>
        nic.NetworkInterfaceType is not (
            NetworkInterfaceType.Loopback or
            NetworkInterfaceType.Tunnel or
            NetworkInterfaceType.Unknown);

    /// <summary>
    /// Updates the network interfaces.
    /// </summary>
    /// <param name="settings">The settings.</param>
    private void UpdateNetworkInterfaces(ISettings settings)
    {
        // When multiple events fire concurrently, we don't want threads interfering
        // with others as they manipulate non-thread safe state.
        lock (_scanLock)
        {
            // Get current interfaces
            IOrderedEnumerable<NetworkInterface> networkInterfaces = GetNetworkInterfaces();
            if (networkInterfaces is null) return;
            var foundInterfaces = networkInterfaces.ToDictionary(x => x.Id, x => x);

            // Remove network interfaces that no longer exist.
            Span<string> removeKeys =
                (from networkInterfacePair in _networks
                    where !foundInterfaces.ContainsKey(networkInterfacePair.Key)
                    select networkInterfacePair.Key).ToArray();

            // Remove
            foreach (var key in removeKeys)
            {
                var network = _networks[key];
                network.Close();
                _networks.Remove(key);
                _hardware.Remove(network);
                HardwareRemoved?.Invoke(network);
            }

            // Add new network interfaces
            foreach (var networkInterfacePair in foundInterfaces.Where(networkInterfacePair => !_networks.ContainsKey(networkInterfacePair.Key)))
            {
                var key = networkInterfacePair.Key;
                _networks.Add(key, new Network(networkInterfacePair.Value, settings));
                var adapter = _networks[key];
                _hardware.Add(adapter);
                HardwareAdded?.Invoke(adapter);
            }
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when [hardware added].
    /// </summary>
    public event HardwareEventHandler HardwareAdded;

    /// <summary>
    /// Occurs when [hardware removed].
    /// </summary>
    public event HardwareEventHandler HardwareRemoved;

    /// <summary>
    /// Handles the NetworkAddressChanged event of the NetworkChange control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
    {
        UpdateNetworkInterfaces(_settings);
    }

    #endregion
}
