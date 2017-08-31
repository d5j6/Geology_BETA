using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Atom")]
public class RotateOrbitBy : ActorActionClip
{
    public int orbitIndex;
    public Vector3 angles;

    public bool applyToChilds;
    public int startIndex;
    public int endIndex;

    public override string info
    {
        get
        {
            return string.Format("Rotate Orbit By {0} angles", angles);
        }
    }

    protected override void OnEnter()
    {
        if(applyToChilds)
        {
            for(int i = startIndex; i <= endIndex; i++)
            {
                actor.transform.GetChild(i).GetComponent<Atom>().RotateOrbitBy(orbitIndex, angles);
            }
        }
        else
        {
            actor.GetComponent<Atom>().RotateOrbitBy(orbitIndex, angles);
        }
    }
}
