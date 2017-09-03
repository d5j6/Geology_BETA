using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class SubSequence
{
    private string _name;
    public string name { get { return _name; } }

    public int nextSubSequenceIndex { get; set; }

    public Sequence subSequence = DOTween.Sequence();

    private bool _isInitialized;

    public SubSequence(string name)
    {
        _name = name;
    }
}
