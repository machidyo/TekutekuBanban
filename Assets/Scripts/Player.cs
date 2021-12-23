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
        for (var i = 0; i < 10; i++)
        {
            Debug.Log($"{i + 1}回目");
            await MoveForward();
        }

        for (var i = 0; i < 10; i++)
        {
            Debug.Log($"{i + 1}回目");
            await MoveByPath();
        }
    }

    private async UniTask MoveForward()
    {
        transform.DOLocalMove(transform.position + transform.forward, 4).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(4));
        transform.Rotate(Vector3.up, 90);
    }

    // 今回に関して言えば、動きの質感的に MoveForward のほうがいい気がした。念のため取っておく。
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
