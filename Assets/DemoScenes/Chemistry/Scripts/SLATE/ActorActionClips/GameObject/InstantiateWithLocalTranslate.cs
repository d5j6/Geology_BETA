using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/GameObject")]
public class InstantiateWithLocalTranslate : ActorActionClip
{
    public GameObject target;
    public int count = 1;
    public Vector3 localTranslate;
    public Transform optionalParent;

    public override bool isValid
    {
        get
        {
            return target != null;
        }
    }

    protected override void OnEnter()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        for(int i = 0; i < count; i++)
        {
            GameObject copy = Instantiate(target);

            if(optionalParent != null)
            {
                copy.transform.SetParent(optionalParent);
            }

            copy.transform.position = target.transform.position;
            copy.transform.rotation = target.transform.rotation;
            copy.transform.Translate(localTranslate * i);
        }
    }
}
