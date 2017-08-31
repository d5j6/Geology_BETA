using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon")]
public class PlayCutscene : DirectorActionClip
{
    public Cutscene cutscene;

    public override string info
    {
        get
        {
            return string.Format("Play Cutscene \"{0}\"", cutscene == null ? "null" : cutscene.name);
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
        cutscene.Play();
    }
}
