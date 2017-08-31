using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/GameObject")]
public class SetActive : ActorActionClip
{
    public bool activeState;

    public override string info
    {
        get
        {
            return string.Format("Set Active To {0}", activeState);
        }
    }

    protected override void OnEnter()
    {
        actor.SetActive(activeState);
    }
}
