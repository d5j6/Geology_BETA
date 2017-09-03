using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Atom")]
public class StopRotating : ActorActionClip
{
    public bool applyToChild;
    public int childStartIndex;
    public int childEndIndex;


    [SerializeField] [HideInInspector]
    private float _length = 1f;

    public override float length
    {
        get
        {
            return _length;
        }

        set
        {
            _length = value;
        }
    }

    protected override void OnEnter()
    {
        if(applyToChild)
        {
            for(int i = childStartIndex; i <= childEndIndex; i++)
            {
                actor.transform.GetChild(i).GetComponent<Atom>().StopRotating(_length);
            }
        }
        else
        {
            actor.GetComponent<Atom>().StopRotating(_length);
        }
    }
}
