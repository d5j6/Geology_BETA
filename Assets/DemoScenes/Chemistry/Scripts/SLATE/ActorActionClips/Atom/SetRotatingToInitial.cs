using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Atom")]
public class SetRotatingToInitial : ActorActionClip
{
    public int startIndex;
    public int endIndex;

    [SerializeField] [HideInInspector]
    private float _length;

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
        for(int i = startIndex; i <= endIndex; i++)
        {
            actor.transform.GetChild(i).GetComponent<Atom>().SetRotatingToInitial(_length);
        }
    }
}