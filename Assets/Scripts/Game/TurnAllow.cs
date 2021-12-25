using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAllow : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider collider)
    {
        var diff = transform.parent.transform.rotation.eulerAngles.y - collider.transform.rotation.eulerAngles.y;
        collider.gameObject.transform.Rotate(Vector3.up, diff);
        Debug.Log("OnTriggerEnter");
    }
}
