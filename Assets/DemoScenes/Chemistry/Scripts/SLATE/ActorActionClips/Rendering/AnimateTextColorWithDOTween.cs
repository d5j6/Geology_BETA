using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;
using DG.Tweening;
using TMPro;

[Category("Gemeleon/Rendering/Text")]
public class AnimateTextColorWithDOTween : ActorActionClip
{
    public Color toColor;
    //public float halfPeriod = 0.4f;
    public int loopsCount = 2;
    public LoopType loopType;

    [SerializeField]
    [HideInInspector]
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

    public override string info
    {
        get
        {
            return string.Format("Animate Color Of {0}", actor.name);
        }
    }

    public override bool isValid
    {
        get
        {
            return _length > 0f && loopsCount > 0;
        }
    }

    protected override void OnEnter()
    {
        if(Application.isPlaying)
        {
            TextMeshPro text = actor.GetComponent<TextMeshPro>();

            if(text == null)
            {
                Debug.LogError("Actor {0} don't have Renderer component attached to");
                return;
            }

            text.DOColor(toColor, _length / loopsCount).SetLoops(loopsCount, loopType).Play();
        }
    }
}
