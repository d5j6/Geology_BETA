using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;
using TMPro;

[Category("Gemeleon/TextMeshPro")]
public class SetText : ActorActionClip
{
    public string newText;

    public bool applyToChild;
    public int childIndex;

    public override string info
    {
        get
        {
            return string.Format("Set Text \"{0}\"", newText);
        }
    }

    protected override void OnEnter()
    {
        if(applyToChild)
        {
            actor.transform.GetChild(childIndex).GetComponent<TextMeshPro>().text = newText;
        }
        else
        {
            actor.GetComponent<TextMeshPro>().text = newText;
        }
    }
}
