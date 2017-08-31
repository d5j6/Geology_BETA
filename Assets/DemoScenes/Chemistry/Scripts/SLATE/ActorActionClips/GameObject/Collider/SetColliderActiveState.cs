using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/GameObject/Collider")]
public class SetColliderActiveState : ActorActionClip
{
    public bool activeState;

    public override string info
    {
        get
        {
            return string.Format("Set Collider Active State To {0}", activeState);
        }
    }

    protected override void OnEnter()
    {
        actor.GetComponent<Collider>().enabled = activeState;
    }
}
