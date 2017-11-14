using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Transform")]
public class LookToward : ActorActionClip
{
    public Transform target;
    public bool inverse;
    public bool isTextContainer = false;

    public override bool isValid
    {
        get
        {
            return target != null;
        }
    }

    protected override void OnEnter()
    {
        Quaternion lookRotation;

        if (isTextContainer)
        {
            lookRotation = Quaternion.LookRotation(-target.forward);
        }
        else
        {
            lookRotation = Quaternion.LookRotation(target.forward);
        }

        if(inverse)
        {
            lookRotation = Quaternion.Inverse(lookRotation);
        }

        actor.transform.rotation = lookRotation;
    }
}
