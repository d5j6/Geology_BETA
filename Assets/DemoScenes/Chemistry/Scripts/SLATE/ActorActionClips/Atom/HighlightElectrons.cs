using UnityEngine;
using System.Collections.Generic;
using Slate;
using Slate.ActionClips;
using System.Linq;

[Category("Gemeleon/Atom")]
public class HighlightElectrons : ActorActionClip
{
    public bool applyToChild;
    public int childIndex;

    [Space(4)]
    public Vector3 scaleTo;
    public Color colorTo = Color.white;
    public int loopsCount;
    public EaseType interpolation;

    private Vector3 _startLocalScale;
    private Color _startColor;

    private List<Transform> _electrons;
    private Material[] _electronMaterials;

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
            _electrons = actor.transform.GetChild(childIndex).GetComponent<Atom>().GetLastOrbitElectrons();
        }
        else
        {
            _electrons = actor.GetComponent<Atom>().GetLastOrbitElectrons();
        }

        _electronMaterials = (from i in _electrons select i.GetComponent<MeshRenderer>().material).ToArray();

        _startLocalScale = _electrons[0].localScale;
        _startColor = _electronMaterials[0].color;

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
            //foreach(Transform electron in _electrons)
            //{
            //    electron.localScale = Easing.Ease(interpolation, _startLocalScale, scaleTo, localTimeScale / _timeSegment);
            //}

            for(int i = 0; i < _electrons.Count; i++)
            {
                _electrons[i].localScale = Easing.Ease(interpolation, _startLocalScale, scaleTo, localTimeScale / _timeSegment);
                _electronMaterials[i].color = Easing.Ease(interpolation, _startColor, colorTo, localTimeScale / _timeSegment);
            }
        }
        else
        {
            //foreach(Transform electron in _electrons)
            //{
            //    electron.localScale = Easing.Ease(interpolation, scaleTo, _startLocalScale, localTimeScale / _timeSegment);
            //}

            for(int i = 0; i < _electrons.Count; i++)
            {
                _electrons[i].localScale = Easing.Ease(interpolation, scaleTo, _startLocalScale, localTimeScale / _timeSegment);
                _electronMaterials[i].color = Easing.Ease(interpolation, colorTo, _startColor, localTimeScale / _timeSegment);
            }
        }
    }
}