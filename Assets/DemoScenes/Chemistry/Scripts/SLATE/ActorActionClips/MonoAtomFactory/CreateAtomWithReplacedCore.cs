using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;
using Gemeleon.Extensions;

[Category("Gemeleon/MonoAtomFactory")]
public class CreateAtomWithReplacedCore : ActorActionClip
{
    public string atomName;
    public float atomSize = 0.02f;
    public Core corePrefab;
    public AtomState state = AtomState.Classic2D;
    public Transform parentObject;
    public Vector3 localStartPos;

    public override string info
    {
        get
        {
            if(parentObject != null)
            {
                return string.Format("Create atom With Replaced Core And Parent To {0}", parentObject.name);
            }
            else
            {
                return base.info;
            }
        }
    }

    public override bool isValid
    {
        get
        {
            return !string.IsNullOrEmpty(name) && parentObject != null && corePrefab != null;
        }
    }

    protected override void OnEnter()
    {
        if(Application.isPlaying)
        {
            MonoAtomFactory mfactory = actor.GetComponent<MonoAtomFactory>();

            if(mfactory == null)
            {
                Debug.LogError(string.Format("Actor {0} don't have MonoAtomFactory component attached to", actor.name));
                return;
            }

             mfactory.CreateAtomWithReplacedCore(atomName, atomSize, corePrefab, state, parentObject, localStartPos);
        }
    }
}
