using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI walkingTime; 
    [SerializeField] private float walkingTimeKakuninyou;
    [SerializeField] private TextMeshProUGUI name; 
    
    [Header("DebugUI")]
    [SerializeField] private TextMeshProUGUI isGrounded; 
    [SerializeField] private TextMeshProUGUI currentStatus;


    void Start()
    {
        name.text = gameObject.name;
    }


    void Update()
    {
        walkingTimeKakuninyou = player.WalkingTime;
        walkingTime.text = $"{player.WalkingTime:F3}";

        isGrounded.text = "" + player.IsGrounded;
        currentStatus.text = "" + player.CurrentState;
    }
}
