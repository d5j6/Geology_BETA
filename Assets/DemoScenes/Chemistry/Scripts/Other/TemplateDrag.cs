using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(MeshRenderer))]
public class TemplateDrag : MonoBehaviour, IInteractive
{
    [SerializeField]
    private List<ActionType> _allowedTypes;

    private Coroutine _dragCoroutine;

    private int _oldLayer;

    public List<ActionType> GetAllowedActions()
    {
        return _allowedTypes;
    }

    private Material _material;

    [SerializeField]
    private float _fadeDuration = 1f;

    [SerializeField]
    private GameObject _rootObject;

    public void OnGazeEnter() { }

    public void OnGazeLeave() { }

    public void OnGestureTap() { }

    public void StopDrag()
    {
        StopCoroutine(_dragCoroutine);

        _rootObject.transform.position = OwnGazeManager.Instance.HitPoint;
        _rootObject.transform.rotation = Quaternion.LookRotation(OwnGazeManager.Instance.PointNormal);
        
        ChangeLayerRecursively(_rootObject, _oldLayer);
    }

    public bool TryToDrag()
    {
        //_oldLayer = gameObject.layer;

        //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        //_dragCoroutine = StartCoroutine(DragCoroutine());

        //return true;

        _oldLayer = _rootObject.layer;

        ChangeLayerRecursively(_rootObject, LayerMask.NameToLayer("Ignore Raycast"));

        _dragCoroutine = StartCoroutine(DragCoroutine());

        return true;
    }

    IEnumerator DragCoroutine()
    {
        while(true)
        {
             _rootObject.transform.position = Vector3.Lerp(_rootObject.transform.position, OwnCursorManager.Instance.cursor.position, Time.deltaTime * 8f);
            _rootObject.transform.rotation = OwnCursorManager.Instance.cursor.rotation;

            yield return null;
        }
    }

    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;

        Color targetColor = _material.color;
        targetColor.a = 0.4f;

        _material.DOColor(targetColor, _fadeDuration).SetLoops(-1, LoopType.Yoyo).Play();
    }

    private void ChangeLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;

        foreach(Transform child in go.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }

    public bool TryToResize() { return false; }
}
