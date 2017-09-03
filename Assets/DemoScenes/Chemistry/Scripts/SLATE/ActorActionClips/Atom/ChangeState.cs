using UnityEngine;
using System.Collections.Generic;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Atom")]
public class ChangeState : ActorActionClip
{
    public AtomState toState;

    public bool applyToChilds;
    public int startIndex;
    public int endIndex;

    public override string info
    {
        get
        {
            return string.Format("Change State To {0}", toState);
        }
    }

    protected override void OnEnter()
    {
        List<Atom> atoms = new List<Atom>();

        if(applyToChilds)
        {
            for(int i = startIndex; i <= endIndex; i++)
            {
                atoms.Add(actor.transform.GetChild(i).GetComponent<Atom>());
            }
        }
        else
        {
            atoms.Add(actor.GetComponent<Atom>());
        }

        foreach(Atom atom in atoms)
        {
            switch(toState)
            {
                case AtomState.None:
                    atom.ChangeStateToNone();
                    break;
                case AtomState.Classic2D:
                    atom.ChangeStateToClassic2D();
                    break;
                case AtomState.Classic3D:
                    atom.ChangeStateToClassic3D();
                    break;
                case AtomState.Real:
                    atom.ChangeStateToReal();
                    break;
            }
        }
    }
}
