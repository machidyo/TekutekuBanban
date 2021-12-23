using MessagePipe;
using UnityEngine;
using VContainer;

namespace HelloMessagePipe.Hello2
{
    class Player : MonoBehaviour
    {
        [Inject] private IPublisher<PlayerAttackData> AttackEvent { get; set; }

        [SerializeField] private int hp;
        [SerializeField] private int atk;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Player attack");
            
                AttackEvent.Publish(new PlayerAttackData
                {
                    Position = transform.position + transform.forward,
                    Radius = 1.6f,
                    Damage = atk
                });
            }
        }
    }
}
