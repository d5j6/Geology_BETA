using UnityEngine;
using System.Collections.Generic;
using Slate;
using Slate.ActionClips;
using DG.Tweening;

[Category("Gemeleon/Atom")]
public class HighlightHoles : ActorActionClip
{
    public bool applyToChild;
    public int childIndex;

    [Space(4)]
    public int orbitIndex;
    public Vector3 scaleTo;
    public int loopsCount;
    public EaseType interpolation;

    private Vector3 _startLocalScale;
    private List<Transform> _holes;
    private float _timeSegment;

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
        if(!Application.isPlaying)
        {
            return;
        }

        if(applyToChild)
        {
            _holes = actor.transform.GetChild(childIndex).GetComponent<Atom>().GetOrbitHoles(orbitIndex);
        }
        else
        {
            _holes = actor.GetComponent<Atom>().GetLastOrbitElectrons();
        }

        _startLocalScale = _holes[0].localScale;

        _timeSegment = _length / loopsCount;
    }

    protected override void OnUpdate(float time)
    {
        if(!Application.isPlaying)
        {
            return;
        }

        float localTimeScale = time % _timeSegment;
        
        if((int)(time / _timeSegment) % 2 == 0)
        {            
            foreach(Transform hole in _holes)
            {
                hole.localScale = Easing.Ease(interpolation, _startLocalScale, scaleTo, localTimeScale / _timeSegment);
            }
        }
        else
        {
            foreach(Transform hole in _holes)
            {
                hole.localScale = Easing.Ease(interpolation, scaleTo, _startLocalScale, localTimeScale / _timeSegment);
            }
        }
    }
}