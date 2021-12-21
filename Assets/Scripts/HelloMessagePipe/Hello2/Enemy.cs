using System;
using MessagePipe;
using UnityEngine;
using VContainer;

class Enemy : MonoBehaviour
{
    [Inject] private ISubscriber<PlayerAttackData> OnAttacked { get; set; }

    [SerializeField] private int hp;

    private IDisposable disposable;

    void Awake()
    {
        var d = DisposableBag.CreateBuilder();

        OnAttacked.Subscribe(attack =>
        {
            if (Vector3.Distance(transform.position, attack.Position) <= attack.Radius)
            {
                hp -= attack.Damage;
                Debug.Log($"Enemy has {attack.Damage} damage.");

                if (hp <= 0)
                {
                    Debug.Log("Enemy is death.");
                    Destroy(gameObject);
                }
            }
        });

        disposable = d.Build();
    }

    void OnDestroy()
    {
        disposable.Dispose();
    }
}
