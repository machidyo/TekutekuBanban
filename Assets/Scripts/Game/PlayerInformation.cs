using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI isGrounded; 
    [SerializeField] private TextMeshProUGUI currentStatus; 
    [SerializeField] private TextMeshProUGUI name; 

    void Start()
    {
        name.text = gameObject.name;
    }


    void Update()
    {
        isGrounded.text = "" + player.IsGrounded;
        currentStatus.text = "" + player.CurrentState;
    }
}
