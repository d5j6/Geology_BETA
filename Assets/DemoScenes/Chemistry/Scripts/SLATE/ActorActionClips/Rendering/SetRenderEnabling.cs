using UnityEngine;
using System.Collections;
using Slate;
using Slate.ActionClips;

[Category("Gemeleon/Rendering")]
[Description("Change active state of MeshRenderer or SkinedMeshRender componens")]
public class SetRenderEnabling : ActorActionClip
{
    public ActiveState state;

    public override string info
    {
        get
        {
            return string.Format("Set Render Active State To {0}", state.ToString());
        }
    }

    protected override void OnEnter()
    {
        Renderer rend = actor.GetComponent<Renderer>();

        if(rend == null)
        {
            Debug.LogError("Actor {0} don't have Renderer component attached to");
            return;
        }

        switch(state)
        {
            case ActiveState.Disable:
                rend.enabled = false;
                break;
            case ActiveState.Enable:
                rend.enabled = true;
                break;
            case ActiveState.Toggle:
                rend.enabled = !rend.enabled;
                break;
        }
    }
}
