using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class AtomUIStateButton : MonoBehaviour, IInteractive
{
    [SerializeField]
    private List<ActionType> _allowedTypes;

    private bool _isSelected;

    [SerializeField]
    private Color _selfColor = Color.gray;

    [SerializeField]
    private Color _highlightColor = new Color(0.75f, 0.75f, 0.75f);

    [SerializeField]
    private Color _selectedColor = Color.white;

    private Material _material;

    Tween _colorTween;

    [SerializeField]
    private float _duration = 1f;

    private AtomUIStateController _atomUIState;

    [SerializeField]
    private UnityEvent onTapSignal;

    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _atomUIState = GetComponentInParent<AtomUIStateController>();
    }

    public List<ActionType> GetAllowedActions()
    {
        return _allowedTypes;
    }

    public void OnGazeEnter()
    {
        

        if(_isSelected)
        {
            return;
        }

        HighlightColor();
    }

    public void HighlightColor()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _material.color; }, (c) => { _material.color = c; }, _highlightColor, _duration).Play();
    }

    public void OnGazeLeave()
    {
        if(_isSelected)
        {
            return;
        }

        CanselHighlightColor();
    }

    public void CanselHighlightColor()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _material.color; }, (c) => { _material.color = c; }, _selfColor, _duration).Play();
    }

    public void OnGestureTap()
    {
        onTapSignal.Invoke();
    }

    public void Select()
    {
        _colorTween.Kill();

        _material.color = _selectedColor;
        _isSelected = true;
    }

    public void Deselect()
    {
        _material.color = _selfColor;
        _isSelected = false;
    }

    public void StopDrag() { }

    public bool TryToDrag() { return false; }
    public bool TryToResize() { return false; }

    public void ChangeViewToClassic3D()
    {
        ChangeState(_atomUIState.ChangeViewToClassic3D);
    }

    public void ChangeViewToClassic2D()
    {
        ChangeState(_atomUIState.ChangeViewToClassic2D);
    }

    public void ChangeViewToReal()
    {
        ChangeState(_atomUIState.ChangeViewToReal);
    }

    private void ChangeState(Action chamgeStateMethod)
    {
        if(_isSelected)
        {
            return;
        }

        Select();
        chamgeStateMethod();
    }
}
