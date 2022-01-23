using Niantic.ARDK.Extensions.Meshing;
using TMPro;
using UnityEngine;

public class ScreenViewer : MonoBehaviour
{
    [SerializeField] private ARMeshManager arMeshManager;
    [SerializeField] private NetworkManager networkManager;

    [Header("UI")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject statePanel;
    [SerializeField] private TMP_InputField sessionIdInputField;

    void Start()
    {
        sessionIdInputField.text = $"{Random.Range(100, 1000)}";
    }

    public void OnJoinButtonClicked()
    {
        var sessionId = sessionIdInputField.text;
        networkManager.Join(sessionId);
    }
    
    public void OnFixButtonClicked()
    {
        networkManager.Fix();
    }

    public void OnSwitchMeshButtonClicked()
    {
        arMeshManager.UseInvisibleMaterial = !arMeshManager.UseInvisibleMaterial;
    }

    public void OnQuestionButtonClicked(int index)
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

    public void ShowAdminUI()
    {
        questionPanel.SetActive(true);
        statePanel.SetActive(true);
    }
}
