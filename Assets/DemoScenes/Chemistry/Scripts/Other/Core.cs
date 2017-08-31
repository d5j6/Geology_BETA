using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Billboard))]
public class Core : MonoBehaviour
{
    private bool _isInitialized;

    private Material _material;

    private Tween _colorTween;

    public void Initialize(AtomInformation atominformation)
    {
        if(_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        _material = GetComponent<MeshRenderer>().material;
        
        _colorTween = _material.DOColor(Color.white, 1f).SetLoops(-1, LoopType.Yoyo).Play();
    }

    void OnDestroy()
    {
        _colorTween.Kill();

        if(_material != null)
        {
            _material.color = Color.red;
        }
    }
}
