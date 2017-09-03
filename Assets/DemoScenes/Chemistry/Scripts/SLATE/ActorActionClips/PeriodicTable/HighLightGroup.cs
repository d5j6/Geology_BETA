using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/PeriodicTable")]
public class HighLightGroup : ActorActionClip
{
    public int groupIndex;
    public int loopsCount;

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

    public override string info
    {
        get
        {
            return string.Format("Highlight Group {0}", groupIndex);
        }
    }

    protected override void OnEnter()
    {
        actor.GetComponent<PeriodicTable>().HighLightGroup(groupIndex, _length / loopsCount, loopsCount);
    }

    protected override void OnExit()
    {
        actor.GetComponent<PeriodicTable>().HighLightGroup(groupIndex, _length / loopsCount, loopsCount, true);
    }
}
