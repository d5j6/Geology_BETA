using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;
using Gemeleon.Extensions;

[Category("Gemeleon/MonoAtomFactory")]
public class CreateAtomsAndParentTo : ActorActionClip
{
    public string[] atomNames = new string[0];
    public float atomSize = 0.02f;
    public AtomState state = AtomState.Classic2D;
    public Transform parentObject;
    public Vector3 localStartPos;
    public Vector3 localOffset;

    public override string info
    {
        get
        {
            if(parentObject != null)
            {
                return string.Format("Create atoms And Parent To {0}", parentObject.name);
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
            bool isValidate = true;

            if(atomNames.Length == 0)
            {
                isValidate = false;
            }

            foreach(string name in atomNames)
            {
                if(string.IsNullOrEmpty(name))
                {
                    isValidate = false;
                }
            }

            if(parentObject == null)
            {
                isValidate = false;
            }

            return isValidate;
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

            mfactory.CreateAtomsAndParentTo(atomNames, atomSize, state, parentObject, localStartPos, localOffset);
        }
    }
}
