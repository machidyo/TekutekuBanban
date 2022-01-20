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
using Niantic.ARDK.Utilities.BinarySerialization;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> questions;
    
    [Header("UI")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject statePanel;
    [SerializeField] private TMP_InputField sessionIdInputField;

    public bool IsHost { get; private set; }
    public bool CanStart { get; private set; }

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

    private bool isVisibleMesh = true;
    public void OnSwitchMeshButtonClicked()
    {
        isVisibleMesh = !isVisibleMesh;

        var arMesh = FindObjectOfType<ARMeshManager>();
        arMesh.UseInvisibleMaterial = isVisibleMesh;
    }

    public void OnQuestionButtonClicked(int index)
    {
        if (IsHost)
        {
            Ping(index);
        }
        else
        {
            Question(index);
        }
    }

    private void Ping(int index)
    {
        using var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, $"Ping {index}");
        multipeerNetworking.SendDataToPeers(0, stream.ToArray(), multipeerNetworking.OtherPeers, TransportType.UnreliableOrdered);
    }

    private void Question(int index)
    {
        using var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, index);
        multipeerNetworking.SendDataToPeers(2, stream.ToArray(), multipeerNetworking.OtherPeers, TransportType.UnreliableOrdered);
    }

    public void OnFixButtonClicked()
    {
        using var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, "CanStart");
        multipeerNetworking.SendDataToPeers(1, stream.ToArray(), multipeerNetworking.OtherPeers, TransportType.UnreliableOrdered);
    }
    
    private GameObject temp;
    private void OnPeerDataReceived(PeerDataReceivedArgs args)
    {
        if (args.Tag == 0)
        {
            using var stream = new MemoryStream(args.CopyData());
            var str = (string)GlobalSerializer.Deserialize(stream);
            Debug.Log(str);
        }

        if (args.Tag == 1)
        {
            using var stream = new MemoryStream(args.CopyData());
            var str = (string)GlobalSerializer.Deserialize(stream);
            CanStart = true;
            OnSwitchMeshButtonClicked();
            Debug.Log(str);
        }

        if (args.Tag == 2)
        {
            using var stream = new MemoryStream(args.CopyData());
            var index = (int)GlobalSerializer.Deserialize(stream);
            Debug.Log($"index = {index}");

            if (temp == null)
            {
                var marker = FindObjectOfType<Operation>().Marker;
                temp = Instantiate(questions[index], marker.transform.position, Quaternion.identity);
                marker.SetActive(false);
            }
            else
            {
                var pos = temp.transform.position;
                Destroy(temp);
                temp = Instantiate(questions[index], pos, Quaternion.identity);
            }
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

        questionPanel.SetActive(!IsHost);
        statePanel.SetActive(!IsHost);
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
