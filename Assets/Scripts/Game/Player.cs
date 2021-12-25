using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    async void Start()
    {
        for (var i = 0; i < 1000; i++)
        {
            await MoveForward();
        }
    }

    private async UniTask MoveForward()
    {
        var speedPerSec = 0.1f;
        transform.DOLocalMove(transform.position + transform.forward * speedPerSec, speedPerSec).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(speedPerSec));
    }

    /// <summary>
    /// for debug
    /// 今回に関して言えば、動きの質感的に MoveForward のほうがいい気がした。念のため取っておく。
    /// </summary>
    private async UniTask MoveByPath()
    {
        var poss = new[]
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 0),
        };
        transform
            .DOPath(poss, 15, PathType.Linear)
            .SetLookAt(0.1f, Vector3.forward);
        await UniTask.Delay(TimeSpan.FromSeconds(16));
    }
}
