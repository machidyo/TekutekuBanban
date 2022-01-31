using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenViewer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject statePanel;
    [SerializeField] private TMP_InputField sessionIdInputField;

    [Header("Button")]
    [SerializeField] private Button joinButton;
    [SerializeField] private Button fixButton;
    [SerializeField] private Button switchButton;
    [SerializeField] private List<Button> questionButtons;
    
    public string SessionId => sessionIdInputField.text;
    public Button JoinButton => joinButton;
    public Button FixButton => fixButton;
    public Button SwitchButton => switchButton;
    public List<Button> QuestionButtons => questionButtons;

    public void Start()
    {
        sessionIdInputField.text = $"{Random.Range(100, 1000)}";
    }

    public void ShowAdminUI(bool isActive)
    {
        questionPanel.SetActive(isActive);
        statePanel.SetActive(isActive);
    }
}
