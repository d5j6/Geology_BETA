using UnityEngine;
using System.Collections.Generic;

public class MonoAtomFactory : MonoBehaviour
{
    AtomFactory factory;

    void Awake()
    {
        factory = new AtomFactory();
    }

    public Atom Create(string name, int[] holes = null, int holesOffset = 0, List<List<float>> overridedPositions = null)
    {
        return factory.CreateAtom(name, holes, holesOffset, overridedPositions);
    }
}
