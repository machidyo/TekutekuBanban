using UnityEngine;

public class Operation : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject markerGameObject;

    public GameObject Marker { get; private set; }
    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            SetMarker();
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
            SetMarker();
        }
    }

    private void SetMarker()
    {
        Debug.Log($"START SetMarker (true, false) {networkManager.IsHost}, {networkManager.CanStart}");
        if (!networkManager.IsHost) return;
        if (networkManager.CanStart) return;
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100))
        {
            Debug.Log($"hit.collider.gameObject.layer = {hit.collider.gameObject.layer}");
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                var pos = hit.point + Vector3.up * 0.1f;
                Debug.Log($"pos = {pos}, Marker is null = {Marker == null}");
                if (Marker == null)
                {
                    Marker = Instantiate(markerGameObject, pos, Quaternion.identity);
                }
                else
                {
                    Marker.transform.position = pos;
                }
            }
        }
    }}
