using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon")]
public class JumpToSection : DirectorActionClip
{
    public Cutscene cutscene;
    public string sectionName;

    public override string info
    {
        get
        {
            return string.Format("Jump To Section {0}", sectionName);
        }
    }

    public override bool isValid
    {
        get
        {
            return !string.IsNullOrEmpty(sectionName) && !(cutscene == null);
        }
    }

    protected override void OnEnter()
    {
        cutscene.JumpToSection(sectionName);
    }
}
