using UnityEngine;
using System.Collections;
using System;

public class ChartSliceController : MonoBehaviour, IButton3D, IFadeble
{
    private Material _material;

    public Color emissionColor;

    public float duration = 1f;

    private bool _isEmissioned;
    private bool _isShow;

    public bool EnabledByCollider
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public bool EnabledByFunctionality
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    public void OnGazeLeave()
    {
        ActivateEmission();
    }

    public void OnGazeOver(RaycastHit hitInfo)
    {
        DeactivateEmission();
    }

    public void OnTap(RaycastHit hitInfo)
    {
        throw new NotImplementedException();
    }

    public void ActivateEmission()
    {
        if (!_isEmissioned)
        {
            return;
        }

        LeanTween.cancel(gameObject);

        _isEmissioned = false;

        LeanTween.value(gameObject, _material.GetColor("_Emission"), Color.black, duration).setOnUpdate((Color value) =>
        {
            _material.SetColor("_Emission", value);
        });
    }

    public void DeactivateEmission()
    {
        if (_isEmissioned)
        {
            return;
        }

        LeanTween.cancel(gameObject);

        _isEmissioned = true;

        LeanTween.value(gameObject, _material.GetColor("_Emission"), emissionColor, duration).setOnUpdate((Color value) =>
        {
            _material.SetColor("_Emission", value);
        });
    }

    public void FadeIn()
    {
        if (_isShow)
        {
            return;
        }

        _isShow = true;

        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, _material.color.a, 1f, duration).setOnUpdate((float value) =>
        {
            Color newColor = _material.color;
            newColor.a = value;

            _material.color = newColor;
        });
    }

    public void FadeOut()
    {
        if (!_isShow)
        {
            return;
        }

        _isShow = false;

        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, _material.color.a, 0f, duration).setOnUpdate((float value) =>
        {
            Color newColor = _material.color;
            newColor.a = value;

            _material.color = newColor;
        });
    }
}
