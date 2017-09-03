using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Atom")]
public class SetAtomShellMaterial : ActorActionClip
{
    public Color toColor;

    public bool applyToChild;
    public int childIndex;

    public EaseType interpolation;

    [SerializeField] [HideInInspector]
    private float _length = 1f;

    private Material _material;

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

    public override string info
    {
        get
        {
            return string.Format("Set Atom Shell Material Color To {0}", toColor.ToString());
        }
    }

    protected override void OnEnter()
    {
        if(applyToChild)
        {
            _material = actor.transform.GetChild(childIndex).GetComponent<Atom>().shell.material;
        }
        else
        {
            _material = actor.GetComponent<Atom>().shell.material;
        }
    }

    protected override void OnUpdate(float time)
    {
        _material.color = Easing.Ease(interpolation, _material.color, toColor, time / _length);
    }
}
