using Niantic.ARDK.Extensions.Meshing;
using VContainer;
using VContainer.Unity;

public class ScreenController : IStartable
{
    private readonly ScreenViewer screenViewer;
    private readonly ARMeshManager arMeshManager;
    private readonly NetworkManager networkManager;

    [Inject]
    public ScreenController(ScreenViewer sv, ARMeshManager arm, NetworkManager nm)
    {
        screenViewer = sv;
        arMeshManager = arm;
        networkManager = nm;
    }

    public void Start()
    {
        screenViewer.JoinButton.onClick.AddListener(OnJoinButtonClicked);
        screenViewer.FixButton.onClick.AddListener(OnFixButtonClicked);
        screenViewer.SwitchButton.onClick.AddListener(OnSwitchMeshButtonClicked);
        foreach (var questionButton in screenViewer.QuestionButtons)
        {
            questionButton.onClick.AddListener(() =>
            {
                var index = int.Parse(questionButton.gameObject.name);
                OnQuestionButtonClicked(index);
            });
        }
    }

    public void ShowAdminUI(bool isActive)
    {
        screenViewer.ShowAdminUI(isActive);
    }

    public void SwitchMeshDisplay()
    {
        OnSwitchMeshButtonClicked();
    }

    private void OnJoinButtonClicked()
    {
        var sessionId = screenViewer.SessionId;
        networkManager.Join(sessionId);
    }
    
    private void OnFixButtonClicked()
    {
        networkManager.Fix();
    }

    private void OnSwitchMeshButtonClicked()
    {
        arMeshManager.UseInvisibleMaterial = !arMeshManager.UseInvisibleMaterial;
    }

    private void OnQuestionButtonClicked(int index)
    {
        if (networkManager.IsHost)
        {
            networkManager.Ping(index);
        }
        else
        {
            networkManager.Question(index);
        }
    }
}
