using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Slate;

public class QuestionButton : MonoBehaviour, IInteractive
{
    public Cutscene nextCutscene;
    public Action onClickEvent;

    [SerializeField]
    private List<ActionType> _allowedActions;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private float _duration = 1f;

    private Sequence _fadeSeq;

    private TextMeshPro[] _texts;

    private bool _isActive;

    private BoxCollider _collider;

    private float liveTime;

    void Awake()
    {
        //_texts = GetComponentsInChildren<TextMeshPro>();
        //_collider = GetComponent<BoxCollider>();
        //_collider.enabled = false;

        //MakeAlphaZero();

        //_fadeSeq = DOTween.Sequence();

        //onClickEvent += PlayNextCutscene;
    }

    public List<ActionType> GetAllowedActions()
    {
        return _allowedActions;
    }

    public void OnGazeEnter()
    {
    }

    public void OnGazeLeave()
    {
    }

    public void OnGestureTap()
    {
        if(!_isActive)
        {
            return;
        }

        if(onClickEvent != null)
        {
            onClickEvent.Invoke();
        }
    }

    public void StopDrag()
    {
    }

    public bool TryToDrag()
    {
        return false;
    }

    //public void FadeIn()
    //{
    //    if(_isActive)
    //    {
    //        return;
    //    }

    //    _isActive = true;
    //    _collider.enabled = true;

    //    _fadeSeq.Kill();
    //    _fadeSeq = DOTween.Sequence();

    //    Color c = _meshRenderer.material.color;
    //    c.a = 1f;

    //    _fadeSeq.Append(_meshRenderer.material.DOColor(c, _duration));

    //    foreach(Transform text in transform)
    //    {
    //        _fadeSeq.Insert(0f, text.GetComponent<TextMeshPro>().DOColor(new Color(1f, 1f, 1f, 1f), _duration));
    //    }

    //    _fadeSeq.Play();
    //}

    //public void FadeOut()
    //{
    //    if(!_isActive)
    //    {
    //        return;
    //    }

    //    _isActive = false;
    //    _collider.enabled = false;

    //    _fadeSeq.Kill();
    //    _fadeSeq = DOTween.Sequence();

    //    Color c = _meshRenderer.material.color;
    //    c.a = 0f;

    //    _fadeSeq.Append(_meshRenderer.material.DOColor(c, _duration));

    //    foreach(Transform text in transform)
    //    {
    //        _fadeSeq.Insert(0f, text.GetComponent<TextMeshPro>().DOColor(new Color(1f, 1f, 1f, 0f), _duration));
    //    }

    //    _fadeSeq.Play();
    //}

    //private void MakeAlphaZero()
    //{
    //    Color c= _meshRenderer.material.color;
    //    c.a = 0f;
    //    _meshRenderer.material.color = c;

    //    foreach(TextMeshPro t in _texts)
    //    {
    //        Color tc = t.color;
    //        tc.a = 0;
    //        t.color = tc;
    //    }
    //}

    void Update()
    {
        switch (gameObject.activeSelf)
        {
            case true:
                _isActive = true;
                break;
            case false:
                _isActive = false;
                break;
        }

        if(!_isActive)
        {
            return;
        }

        liveTime += Time.deltaTime;

        if(liveTime >= .1f)
        {
            liveTime = 0f;

            //FadeOut();
            //if (CutsceneManager.Instance.BaseGid1SectionFound)
            //{
            //    CutsceneManager.Instance.PlayBaseGIDCutscene2("Groups and periods");
            //}
        }
    }

    private void PlayNextCutscene()
    {
        //FadeOut();
        //CutsceneManager.Instance.PlayBaseGIDCutscene2();
    }
    public bool TryToResize() { return false; }

}
