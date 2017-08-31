using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/TextMeshPro")]
public class CreateText : ActorActionClip
{
    public string objectName;
    public string text;
    public float width = 1f;
    public float height = 1f;
    public float fontSize = 1f;
    public Vector3 localPos;
    public Vector3 localRot;
    public Vector3 localScale;
    public Transform parentObject;

    public override string info
    {
        get
        {
            return string.Format("Create text \"{0}\"", objectName);
        }
    }

    protected override void OnEnter()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        actor.GetComponent<MonoTextFactory>().CreateText(objectName, text, width, height, fontSize, localScale, localPos, Quaternion.Euler(localRot), parentObject);
    }
}
