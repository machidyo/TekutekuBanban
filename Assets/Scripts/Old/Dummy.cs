using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    async void Start()
    {
        for (var i = 0; i < 10; i++)
        {
            await MoveByPath();
        }
    }

    void Update()
    {
        
    }
    
    /// <summary>
    /// for debug
    /// 今回に関して言えば、動きの質感的に MoveForward のほうがいい気がした。念のため取っておく。
    /// </summary>
    private async UniTask MoveByPath()
    {
        var poss = new[]
        {
            transform.position + new Vector3(0, 0, 1),
            transform.position,
        };
        transform
            .DOPath(poss, 15, PathType.Linear)
            .SetLookAt(0.1f, Vector3.forward);
        await UniTask.Delay(TimeSpan.FromSeconds(16));
    }
}
