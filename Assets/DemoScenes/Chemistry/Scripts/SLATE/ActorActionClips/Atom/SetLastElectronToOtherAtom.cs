using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Atom")]
public class SetLastElectronToOtherAtom : ActorActionClip
{
    public int fromAtomIndex;
    public int toAtomIndex;

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
        actor.transform.GetChild(fromAtomIndex).GetComponent<Atom>().SetLastElectronToOtherAtom(actor.transform.GetChild(toAtomIndex).GetComponent<Atom>(), _length);
    }
}
