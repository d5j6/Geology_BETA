using UnityEngine;
using System.Collections.Generic;
using Slate;
using Slate.ActionClips;
using Gemeleon.Extensions;

[Category("Gemeleon/MonoAtomFactory")]
public class CreateAtomWithThreeOrbitOverrides : ActorActionClip
{
    public string atomName;
    public float atomSize = 0.02f;
    public AtomState state;
    public int[] holesPerOrbit;
    public int holesOffset;
    public Transform parentObject;
    public Vector3 localStartPos;

    public float[] overrides1;
    public float[] overrides2;
    public float[] overrides3;

    protected override void OnEnter()
    {
        List<List<float>> overrides = new List<List<float>>();
        overrides.Add(new List<float>(overrides1));
        overrides.Add(new List<float>(overrides2));
        overrides.Add(new List<float>(overrides3));

        MonoAtomFactory factory = actor.GetComponent<MonoAtomFactory>();
        factory.CreateAtomWithOverridesAndParentTo(atomName, atomSize, state, holesPerOrbit.Length == 0 ? null : holesPerOrbit, holesOffset, overrides, parentObject, localStartPos);
    }
}
