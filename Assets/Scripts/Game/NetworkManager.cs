using System.IO;
using System.Text;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities.BinarySerialization;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour
{
    public bool IsHost { get; private set; }

    public readonly UnityEvent OnNetworkedConnectedEvent = new UnityEvent();
    public readonly UnityEvent<PeerDataReceivedArgs> OnPeerDataReceivedEvent = new UnityEvent<PeerDataReceivedArgs>();

    private IARNetworking arNetworking;
    private IMultipeerNetworking multipeerNetworking;
    private IARSession arSession;

    void Start()
    {
        arNetworking = ARNetworkingFactory.Create();
        multipeerNetworking = arNetworking.Networking;
        arSession = arNetworking.ARSession;

        var worldTrackingConfig = ARWorldTrackingConfigurationFactory.Create();
        worldTrackingConfig.WorldAlignment = WorldAlignment.Gravity;
        worldTrackingConfig.IsAutoFocusEnabled = true;
        worldTrackingConfig.IsSharedExperienceEnabled = true;

        arSession.Run(worldTrackingConfig);
        arSession.Ran += OnSessionRan;

        multipeerNetworking.Connected += OnNetworkedConnected;

        arNetworking.PeerStateReceived += OnPeerStateReceived;
        arNetworking.PeerPoseReceived += OnPeerPoseReceived;

        multipeerNetworking.PeerDataReceived += OnPeerDataReceived;
    }

    void OnDestroy()
    {
        arSession?.Dispose();
        multipeerNetworking?.Dispose();
        arNetworking?.Dispose();
    }

    public void Join(string sessionId)
    {
        var sessionIdAsByte = Encoding.UTF8.GetBytes(sessionId);
        multipeerNetworking.Join(sessionIdAsByte);
    }

    public void Ping(int index)
    {
        using var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, $"Ping {index}");
        multipeerNetworking.SendDataToPeers(0, stream.ToArray(), multipeerNetworking.OtherPeers, TransportType.UnreliableOrdered);
    }

    public void Fix()
    {
        using var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, "CanStart");
        multipeerNetworking.SendDataToPeers(1, stream.ToArray(), multipeerNetworking.OtherPeers, TransportType.UnreliableOrdered);
    }

    public void Question(int index)
    {
        using var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, index);
        multipeerNetworking.SendDataToPeers(2, stream.ToArray(), multipeerNetworking.OtherPeers, TransportType.UnreliableOrdered);
    }
    
    private void OnPeerDataReceived(PeerDataReceivedArgs args)
    {
        OnPeerDataReceivedEvent.Invoke(args);
    }

    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("START OnSessionRan");
    }

    private void OnNetworkedConnected(ConnectedArgs args)
    {
        Debug.Log($"START OnNetworkedConnected: peerID {args.Self}, isHost: {args.IsHost}");
        IsHost = args.IsHost;
        OnNetworkedConnectedEvent?.Invoke();
    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        Debug.Log($"START OnPeerStateReceived: state: {args.State}");
    }

    private void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        // Debug.Log($"START OnPeerPoseReceived: pose: {args.Pose}");
    }
}
