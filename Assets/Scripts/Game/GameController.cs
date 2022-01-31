using System.IO;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities.BinarySerialization;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameController : IStartable
{
    private readonly Operation operation;
    private readonly Question question;
    private readonly NetworkManager networkManager;
    private readonly ScreenViewer screenViewer;

    [Inject]
    public GameController(Operation o, Question gm, NetworkManager nm, ScreenViewer sv)
    {
        operation = o;
        question = gm;
        networkManager = nm;
        screenViewer = sv;
    }

    public void Start()
    {
        Debug.Log("START Start in GameController");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        operation.OnTouch.AddListener(SetMarker);
        
        networkManager.OnNetworkedConnectedEvent.AddListener(OnNetworkedConnected);
        networkManager.OnPeerDataReceivedEvent.AddListener(OnDataReceived);
    }

    private void SetMarker()
    {
        if (!networkManager.IsHost) return;
        question.SetMarker();
    }

    private void OnDataReceived(PeerDataReceivedArgs args)
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
            question.FixMarker();
            screenViewer.SwitchButton.onClick.Invoke();
            Debug.Log(str);
        }

        if (args.Tag == 2)
        {
            using var stream = new MemoryStream(args.CopyData());
            var index = (int)GlobalSerializer.Deserialize(stream);
            Debug.Log($"index = {index}");
            question.SetQuestion(index);
        }
    }
    
    private void OnNetworkedConnected()
    {
        Debug.Log("START OnNetworkedConnected in GameController");
        // ARMesh がなんらかの不具合で共有できないことから、ホストが回答者で、参加者が質問者という歪な形をとっている
        var isAdmin = !networkManager.IsHost;
        screenViewer.ShowAdminUI(isAdmin);
    }
}
