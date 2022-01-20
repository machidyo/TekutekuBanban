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

        // var particle = Instantiate(arPlaneTouch, hit.point, Quaternion.identity);
        // particle.transform.position += new Vector3(0, 0.01f * avatarAdjuster.MagnificationForInitSize, 0);
        // particle.transform.localScale *= avatarAdjuster.MagnificationForInitSize;
        Destroy(gameObject, 2);
    }
}
