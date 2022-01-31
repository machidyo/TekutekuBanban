using UnityEngine;
using UnityEngine.Events;

public class Operation : MonoBehaviour
{
    public readonly UnityEvent OnTouch = new UnityEvent();
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnTouch.Invoke();
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
            OnTouch.Invoke();
        }
    }
}
