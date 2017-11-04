using UnityEngine;
using System.Collections.Generic;
using System;

// Simplified Factory Method pattern
public class AtomFactory : IAtomFactory
{
    public static Atom newAtom;

    public static AtomInformation atomInfo;

    public Atom CreateAtom(string atomName, int[] holes = null, int holesOffset = 0, List<List<float>> overridedPositions = null)
    {
        Debug.Log("Creating atom's projection... Step #2");

        atomInfo = DataManager.Instance.GetAtominfo(atomName);

        // Atom newAtom = GameObject.Instantiate(PrefabManager.Instance.atomPrefab).GetComponent<Atom>();
        newAtom = GameObject.Instantiate(PrefabManager.Instance.atomPrefab,
                                         ProjectorController.Instance._spawnPlace).GetComponent<Atom>();

        
        newAtom.Initialize(atomInfo, holes, holesOffset, overridedPositions);

        int orbitsCount = atomInfo.electrons.Length - 1;
        float scaleFactor = orbitsCount / 7f * 0.05f;
        Vector3 scale = new Vector3(0.1f - scaleFactor, 0.1f - scaleFactor, 0.1f - scaleFactor);

        // Это команда не выполняется
        newAtom.transform.localScale = scale;

        return newAtom;
    }
}


public interface IAtomFactory
{
    Atom CreateAtom(string atomName, int[] holes = null, int holesOffset = 0, List<List<float>> overridedPositions = null);
}