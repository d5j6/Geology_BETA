using UnityEngine;
using System.Collections.Generic;
using Slate;
using Slate.ActionClips;
using DG.Tweening;
using System.Linq;

[Category("Gemeleon/Atom")]
public class HighlightProtons : ActorActionClip
{
    public bool applyToChild;
    public int childIndex;

    [Space(4)]
    public Vector3 toScale;
    public Color colorTo;
    public int loopsCount;
    public EaseType interpolation;

    private Vector3 _startScale;
    private Color _startColor;

    private Material[] _protonsMaterials;
    private Proton[] _protons;

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
            //_protonsMaterials = (from i in actor.transform.GetChild(childIndex).GetComponent<Atom>().GetProtons() select i.GetComponent<MeshRenderer>().material).ToArray();
            _protons = actor.transform.GetChild(childIndex).GetComponent<Atom>().GetProtons();
        }
        else
        {
            //_protonsMaterials = (from i in actor.GetComponent<Atom>().GetProtons() select i.GetComponent<MeshRenderer>().material).ToArray();
            _protons = actor.GetComponent<Atom>().GetProtons();
        }

        _protonsMaterials = (from i in _protons select i.GetComponent<MeshRenderer>().material).ToArray();

        _startScale = _protons[0].transform.localScale;
        _startColor = _protonsMaterials[0].color;

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
            //foreach(Material material in _protonsMaterials)
            //{
            //    material.color = Easing.Ease(interpolation, _startColor, colorTo, localTimeScale / _timeSegment);
            //}

            for(int i = 0; i < _protons.Length; i++)
            {
                _protons[i].transform.localScale = Easing.Ease(interpolation, _startScale, toScale, localTimeScale / _timeSegment);
                _protonsMaterials[i].color = Easing.Ease(interpolation, _startColor, colorTo, localTimeScale / _timeSegment);
            }
        }
        else
        {
            //foreach(Material material in _protonsMaterials)
            //{
            //    material.color = Easing.Ease(interpolation, colorTo, _startColor, localTimeScale / _timeSegment);
            //}

            for(int i = 0; i < _protons.Length; i++)
            {
                _protons[i].transform.localScale = Easing.Ease(interpolation, toScale, _startScale, localTimeScale / _timeSegment);
                _protonsMaterials[i].color = Easing.Ease(interpolation, colorTo, _startColor, localTimeScale / _timeSegment);
            }
        }
    }
}