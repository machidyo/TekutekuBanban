using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    [SerializeField] private GameObject markerGameObject;
    [SerializeField] private List<GameObject> questions;

    private GameObject marker;
    private bool canStart;
    private GameObject question;

    public void SetMarker()
    {
        if (!networkManager.IsHost) return;
        if (canStart) return;
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                var pos = hit.point + Vector3.up * 0.1f;
                if (marker == null)
                {
                    marker = Instantiate(markerGameObject, pos, Quaternion.identity);
                }
                else
                {
                    marker.transform.position = pos;
                }
            }
        }
    }

    public void ReadyToStart()
    {
        canStart = true;
    }

    public void SetQuestion(int index)
    {
        if (question == null)
        {
            question = Instantiate(questions[index], marker.transform.position, Quaternion.identity);
            marker.SetActive(false);
        }
        else
        {
            var pos = question.transform.position;
            Destroy(question);
            question = Instantiate(questions[index], pos, Quaternion.identity);
        }
    }
}
