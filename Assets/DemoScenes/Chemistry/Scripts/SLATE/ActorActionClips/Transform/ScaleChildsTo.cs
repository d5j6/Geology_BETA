using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Transform")]
public class ScaleChildsTo : ActorActionClip
{
    public bool scaleAllChilds;
    public int startIndex = 0;
    public int endIndex = 0;
    public Vector3 scaleTo;
    public EaseType interpolation;

    private bool isValidOnRuntime = true;

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

    public override bool isValid
    {
        get
        {
            return startIndex >= 0;
        }
    }

    protected override void OnEnter()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        if(scaleAllChilds)
        {
            startIndex = 0;
            endIndex = actor.transform.childCount - 1;
        }

        if(actor.transform.childCount - 1 < endIndex)
        {
            Debug.LogError(string.Format("{0} object not have child with {1} index", actor.transform.name, endIndex));
            isValidOnRuntime = false;
        }
    }

    protected override void OnUpdate(float time)
    {
        if(!isValidOnRuntime || !Application.isPlaying)
        {
            return;
        }

        for(int i = startIndex; i <= endIndex; i++)
        {
            Transform child = actor.transform.GetChild(i);
            child.localScale = Easing.Ease(interpolation, child.localScale, scaleTo, time / _length);
        }
    }
}
