using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class BaseGIDButton : MonoBehaviour, IInteractive
{
    [SerializeField]
    private List<ActionType> _allowedTypes;

    private Tween _colorTween;

    [SerializeField]
    private TextMeshPro _text;

    [SerializeField]
    private Color _selfColor;

    [SerializeField]
    private Color _highlightColor = Color.white;

    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private PeriodicTable periodicTable;

    public List<ActionType> GetAllowedActions()
    {
        return _allowedTypes;
    }

    private CutsceneManager cutsceneManager;

    private void Start()
    {
        cutsceneManager = GameObject.FindObjectOfType<CutsceneManager>();
    }

    public void OnGazeEnter()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _highlightColor, _duration).Play();
    }

    public void OnGazeLeave()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _selfColor, _duration).Play();
    }

    public void OnGestureTap()
    {
        //periodicTable.DeselectElement(null);

            //CutsceneManager.Instance.PlayBaseGIDCutscene1();
            //PlayerManager.Instance.ChangeStateToDefault();
			cutsceneManager.PlaySectionNow(playDemo: true);
    }

    public void StopDrag() { }

    public bool TryToDrag() { return false; }
    public bool TryToResize() { return false; }
}
