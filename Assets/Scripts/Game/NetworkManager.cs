using System.Collections.Generic;
using System.IO;
using System.Text;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Extensions.Meshing;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.BinarySerialization;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField sessionIdInputField;

    [SerializeField] private GameObject peerPoseIndicator;
    
    private IARNetworking arNetworking;
    private IMultipeerNetworking multipeerNetworking;
    private IARSession arSession;

    private Dictionary<IPeer, GameObject> poseIndiicators = new Dictionary<IPeer, GameObject>();
    
    public bool IsHost { get; private set; }
    private bool isStart = false;
    
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
        
        sessionIdInputField.text = $"{Random.Range(100, 1000)}";
    }

    void OnDestroy()
    {
        arSession?.Dispose();
        multipeerNetworking?.Dispose();
        arNetworking?.Dispose();
    }

    public void OnJoinButtonClicked()
    {
        var sessionId = sessionIdInputField.text;
        var sessionIdAsByte = Encoding.UTF8.GetBytes(sessionId);
        multipeerNetworking.Join(sessionIdAsByte);
    }

    public void OnSendDataToPeersButtonClicked()
    {
        using var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, "mojimojikun");
        multipeerNetworking.SendDataToPeers(0, stream.ToArray(), multipeerNetworking.OtherPeers, TransportType.UnreliableOrdered);
    }

    public void OnStartGameButtonClicked()
    {
        Debug.Log($"OnStartGameButtonClicked, {sessionIdInputField.text}");
        
        var hoge = int.Parse(sessionIdInputField.text);
        if (hoge == 0)
        {
            var arMesh = FindObjectOfType<ARMeshManager>();
            arMesh.UseInvisibleMaterial = true;
        }
        if (hoge == 1)
        {
            var arMesh = FindObjectOfType<ARMeshManager>();
            arMesh.UseInvisibleMaterial = false;
        }

        if (hoge == 10)
        {
            isStart = true;
        }
    }

    private void OnPeerDataReceived(PeerDataReceivedArgs args)
    {
        if (args.Tag == 0)
        {
            using var stream = new MemoryStream(args.CopyData());
            var str = (string)GlobalSerializer.Deserialize(stream);
            Debug.Log(str);
        }
    }

    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("START OnSessionRan");
    }

    private void OnNetworkedConnected(ConnectedArgs args)
    {
        Debug.Log($"START OnNetworkedConnected: peerID {args.Self}, isHost: {args.IsHost}");
        IsHost = args.IsHost;
    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        Debug.Log($"START OnPeerStateReceived: state: {args.State}");
    }

    private void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        // Debug.Log($"START OnPeerPoseReceived: pose: {args.Pose}");

        if (!IsHost) return;
        if (args.Peer.Identifier == multipeerNetworking.Host.Identifier) return;
        if (!isStart) return;
        
        if (!poseIndiicators.ContainsKey(args.Peer))
        {
            Debug.Log("Instantiate Apple");
            poseIndiicators.Add(args.Peer, Instantiate(peerPoseIndicator));
        }

        if (poseIndiicators.TryGetValue(args.Peer, out var poseIndicator))
        {
            poseIndicator.transform.position = args.Pose.ToPosition() + new Vector3(0, 0, -0.05f);
        }
    }
}
