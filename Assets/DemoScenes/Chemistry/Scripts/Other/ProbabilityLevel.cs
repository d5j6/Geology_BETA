using UnityEngine;
using System.Collections.Generic;

public class ProbabilityLevel : MonoBehaviour
{
    private int _level;

    private List<ProbabilitySubLevel> _subLevels = new List<ProbabilitySubLevel>();

    private bool _isInitialized;

    public void Initialize(int level, List<AtomFormula> formulaParts)
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        _level = level;

        foreach (AtomFormula formulaPart in formulaParts)
        {
            ProbabilitySubLevel subLevel = new GameObject(string.Format("{0}{1}{2}", formulaPart.level, formulaPart.form, formulaPart.electronsCount)).AddComponent<ProbabilitySubLevel>();

            subLevel.transform.SetParent(transform);
            subLevel.transform.localPosition = Vector3.zero;
            subLevel.transform.localRotation = Quaternion.identity;
            subLevel.Initialize(formulaPart);

            _subLevels.Add(subLevel);
        }
    }
}
