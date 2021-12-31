using System.Collections.Generic;
using System.Text;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
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

        sessionIdInputField.text = "12345";
        var sessionId = sessionIdInputField.text;
        var sessionIdAsByte = Encoding.UTF8.GetBytes(sessionId);

        multipeerNetworking.Join(sessionIdAsByte);
        multipeerNetworking.Connected += OnNetworkedConnected;

        arNetworking.PeerStateReceived += OnPeerStateReceived;
        arNetworking.PeerPoseReceived += OnPeerPoseReceived;
    }

    void OnDestroy()
    {
        arSession?.Dispose();
        multipeerNetworking?.Dispose();
        arNetworking?.Dispose();
    }

    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("START OnSessionRan");
    }

    private void OnNetworkedConnected(ConnectedArgs args)
    {
        Debug.Log($"START OnNetworkedConnected: peerID {args.Self}, isHost: {args.IsHost}");
    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        Debug.Log($"START OnPeerStateReceived: state: {args.State}");
    }

    private void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        Debug.Log($"START OnPeerPoseReceived: pose: {args.Pose}");

        if (!poseIndiicators.ContainsKey(args.Peer))
        {
            poseIndiicators.Add(args.Peer, Instantiate(peerPoseIndicator));
        }

        if (poseIndiicators.TryGetValue(args.Peer, out var poseIndicator))
        {
            poseIndicator.transform.position = args.Pose.ToPosition() + new Vector3(0, 0, -0.5f);
        }
    }
}
