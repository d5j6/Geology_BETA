using UnityEngine;
using System.Collections.Generic;
using System;

public class ProbabilityManager : MonoBehaviour
{
    private bool _isInitialize;

    private List<List<AtomFormula>> _leveledFormula;

    private List<ProbabilityLevel> _probabilityesLevels;

    //TODO: formula can be simplified with json-data correction - need bake correct formula 
    //      structure in json and convert it from json into class when started
    public void Initialize(AtomInformation atomInformation)
    {
        if (_isInitialize)
        {
            return;
        }

        _isInitialize = true;

        string[] splittedFormula = atomInformation.formula.Split(' ');

        AtomFormula[] formula = new AtomFormula[splittedFormula.Length];

        for (int i = 0; i < splittedFormula.Length; i++)
        {
            int levelSymbolIndex = splittedFormula[i].IndexOfAny(new char[] { 's', 'p', 'd', 'f' });

            int level = int.Parse(splittedFormula[i].Substring(0, levelSymbolIndex));
            ProbabilityForm form = (ProbabilityForm)Enum.Parse(typeof(ProbabilityForm), splittedFormula[i].Substring(levelSymbolIndex, 1).ToUpper());
            int electronsCount = int.Parse(splittedFormula[i].Substring(levelSymbolIndex + 1, splittedFormula[i].Length - 1 - levelSymbolIndex));

            formula[i] = new AtomFormula(level, form, electronsCount);
        }

        int maxLevel = 0;
        foreach (AtomFormula part in formula)
        {
            if (part.level > maxLevel)
            {
                maxLevel = part.level;
            }
        }

        _leveledFormula = new List<List<AtomFormula>>();
        for (int i = 0; i < maxLevel; i++)
        {
            _leveledFormula.Add(new List<AtomFormula>());
        }

        foreach (AtomFormula part in formula)
        {
            if (part.form == ProbabilityForm.D && atomInformation.name == "Titanium")
            {
                _leveledFormula[part.level].Add(part);
            }
            else if (part.form == ProbabilityForm.F && atomInformation.name == "Lutetium")
            {
                _leveledFormula[part.level + 1].Add(part);
            }
            else
            {
                _leveledFormula[part.level - 1].Add(part);
            }
        }

        GenerateProbabilityes();
    }

    private void GenerateProbabilityes()
    {
        //TODO: block below was commented becouse need only one last level generation of probability model

        //for(int i = 0; i < _leveledFormula.Count; i++)
        //{
        //    ProbabilityLevel pLevel = new GameObject(string.Format("LevelN{0}", i + 1)).AddComponent<ProbabilityLevel>();
        //    pLevel.transform.SetParent(transform);
        //    pLevel.transform.localPosition = Vector3.zero;
        //    pLevel.transform.localRotation = Quaternion.identity;
        //    pLevel.Initialize(i + 1, _leveledFormula[i]);
        //}

        ProbabilityLevel pLevel = new GameObject(string.Format("LevelN{0}", _leveledFormula.Count)).AddComponent<ProbabilityLevel>();
        pLevel.transform.SetParent(transform);
        pLevel.transform.localPosition = Vector3.zero;
        pLevel.transform.localRotation = Quaternion.identity;
        pLevel.Initialize(_leveledFormula.Count, _leveledFormula[_leveledFormula.Count - 1]);
    }
}

public enum ProbabilityForm { S, P, D, F }

public struct AtomFormula
{
    public int level;
    public ProbabilityForm form;
    public int electronsCount;

    public AtomFormula(int level, ProbabilityForm form, int electronsCount)
    {
        this.level = level;
        this.form = form;
        this.electronsCount = electronsCount;
    }
}