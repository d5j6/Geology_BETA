using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;
using DG.Tweening;

[Category("Gemeleon/Transform")]
public class LocalTranslate : ActorActionClip
{
    public Vector3 translateVector;

    public bool applyToChild;
    public int childIndex;

    [SerializeField] [HideInInspector]
    private float _length;

    private Vector3 _startLocalPosition;

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
        if(_length == 0f)
        {
            if(applyToChild)
            {
                actor.transform.GetChild(childIndex).Translate(translateVector);
            }
            else
            {
                actor.transform.Translate(translateVector);
            }

            return;
        }

        if(applyToChild)
        {
            _startLocalPosition = actor.transform.GetChild(childIndex).localPosition;
        }
        else
        {
            _startLocalPosition = actor.transform.localPosition;
        }
    }

    protected override void OnUpdate(float time)
    {
        if(_length == 0)
        {
            return;
        }

        if(applyToChild)
        {
            actor.transform.GetChild(childIndex).localPosition = Easing.Ease(EaseType.QuadraticInOut, _startLocalPosition, _startLocalPosition + translateVector, time / _length);
        }
        else
        {
            actor.transform.localPosition = Easing.Ease(EaseType.QuadraticInOut, _startLocalPosition, _startLocalPosition + translateVector, time / _length);
        }
    }
}
