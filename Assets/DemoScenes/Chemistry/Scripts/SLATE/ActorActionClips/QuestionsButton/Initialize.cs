using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Scripts/QuestionButton")]
public class Initialize : ActorActionClip
{
    public Cutscene cutscene;

    public override string info
    {
        get
        {
            return "Initialize Button";
        }
    }

    public override bool isValid
    {
        get
        {
            return cutscene != null;
        }
    }

    protected override void OnEnter()
    {
        if(Application.isPlaying)
        {
            //actor.GetComponent<QuestionButton>().Initialize(cutscene);
        }
    }
}
