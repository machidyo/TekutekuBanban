using System;
using System.Collections;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MessagePipeTester : MonoBehaviour
{
    [Inject] IPublisher<int> IndexPublisher { get; set; }
    [Inject] ISubscriber<int> IndexSubscriber { get; set; }

    private IDisposable dispose;
    
    void Start()
    {
        var d = DisposableBag.CreateBuilder();

        IndexSubscriber
            .Subscribe(index => Debug.Log(index))
            .AddTo(d);
        
        IndexPublisher.Publish(1);
        IndexPublisher.Publish(4);
        IndexPublisher.Publish(7);

        dispose = d.Build();
    }

    void OnDestroy()
    {
        dispose.Dispose();
    }
}
