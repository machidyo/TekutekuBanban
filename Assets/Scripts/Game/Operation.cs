using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Operation : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
            }
            else
            {
                Respawn().Forget();
            }
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
            // CurrentInputAction = InputAction.Tap;
        }
    }
    
    private async UniTask Respawn()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                // var particle = Instantiate(arPlaneTouch, hit.point, Quaternion.identity);
                // particle.transform.position += new Vector3(0, 0.01f * avatarAdjuster.MagnificationForInitSize, 0);
                // particle.transform.localScale *= avatarAdjuster.MagnificationForInitSize;
                player.SetTappedPoint(hit.point);
            }
        }
    }
}