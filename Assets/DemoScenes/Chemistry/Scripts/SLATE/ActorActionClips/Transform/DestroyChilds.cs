using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Transform")]
public class DestroyChilds : ActorActionClip
{
    public bool destroyAllChilds;
    public int startIndex = 0;
    public int endIndex = 0;

    public override bool isValid
    {
        get
        {
            return startIndex >= 0 && endIndex >= startIndex;
        }
    }

    protected override void OnEnter()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        if(destroyAllChilds)
        {
            startIndex = 0;
            endIndex = actor.transform.childCount - 1;
        }

        for(int i = startIndex; i <= endIndex; i++)
        {
            Destroy(actor.transform.GetChild(i).gameObject);
        }
    }
}
