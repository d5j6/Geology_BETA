using UnityEngine;
using System.Collections.Generic;
using Slate;
using Slate.ActionClips;
using DG.Tweening;

[Category("Gemeleon/Rendering")]
public class AnimateColorWithDOTween : ActorActionClip
{
    public bool animateInChilds;
    public int childStartIndex;
    public int childEndIndex;

    [Space(4)]
    public Color toColor;
    public int loopsCount = 2;
    public LoopType loopType;

    [SerializeField]
    [HideInInspector]
    private float _length = 1f;

    List<Renderer> renderers;
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
        if(!Application.isPlaying)
        {
            return;
        }

        renderers = new List<Renderer>();

        if(animateInChilds)
        {
            for(int i = childStartIndex; i <= childEndIndex; i++)
            {
                renderers.Add(actor.transform.GetChild(i).GetComponent<Renderer>());
            }
        }
        else
        {
            renderers.Add(actor.GetComponent<Renderer>());
        }

        if(renderers.Count == 0)
        {
            Debug.LogError(string.Format("Actor {0} (or he childs) don't have Renderer component attached to", actor.name));
            return;
        }

        foreach(Renderer rend in renderers)
        {
            rend.material.DOColor(toColor, _length / loopsCount).SetLoops(loopsCount, loopType).Play();
        }
    }

    protected override void OnExit()
    {
        foreach (Renderer rend in renderers)
        {
            rend.material.DOComplete();
        }
    }

    //protected override void OnReverse()
    //{
    //    Debug.Log("OnReverseeeeeeeeeeeeee");
    //}

    //protected override void OnReverseEnter()
    //{
    //    Debug.Log("OnReverseeeeeeeeeeeeeeEntttteeeeeeeeeer");
    //}

}
