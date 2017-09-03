using UnityEngine;
using System.Collections;

public class EventManager : Singleton<EventManager>
{
    public SequenceCompleteEvent sequenceCompleteEvent = new SequenceCompleteEvent();
}
