using UnityEngine;
using System.Collections;
using System;

public abstract class EventPattern
{
    private event Action eventObject;

    public void Subscribe(Action listenerAction)
    {
        eventObject += listenerAction;
    }

    public void RemoveSubscriber(Action listenerAction)
    {
        eventObject -= listenerAction;
    }

    public void Publish()
    {
        eventObject.Invoke();
    }
}
