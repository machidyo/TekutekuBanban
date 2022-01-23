using UnityEngine;

public class Operation : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;
    [SerializeField] private NetworkManager networkManager;
    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            gameMaster.SetMarker();
        }
#elif UNITY_IOS || UNITY_ANDROID
        UpdateIfMobile();
#endif
    }

    private void UpdateIfMobile()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Ended)
        {
            gameMaster.SetMarker();
        }
    }
}
