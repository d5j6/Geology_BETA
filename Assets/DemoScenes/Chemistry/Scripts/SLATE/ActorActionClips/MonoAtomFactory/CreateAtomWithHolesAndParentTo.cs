using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;
using Gemeleon.Extensions;

[Category("Gemeleon/MonoAtomFactory")]
public class CreateAtomWithHolesAndParentTo : ActorActionClip
{
    public string atomName;
    public float atomSize = 0.02f;
    public AtomState state = AtomState.Classic2D;
    public int[] holes;
    public Transform parentObject;
    public Vector3 localStartPos;
    public Vector3 localOffset;

    public override string info
    {
        get
        {
            if(parentObject != null)
            {
                return string.Format("Create Atoms With Holes And Parent To {0}", parentObject.name);
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
            return !string.IsNullOrEmpty(name) && parentObject != null && holes.Length > 0;
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

            mfactory.CreateAtomWithHolesAndParentTo(atomName, atomSize, state, holes, parentObject, localStartPos);
        }
    }
}
