using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class MenuDragScript : MonoBehaviour, IInteractive
{
    [SerializeField]
    private GameObject _chaptersMenu;

    [SerializeField]
    private SpriteRenderer _sprite;

    [SerializeField]
    private Color _selfColor;

    [SerializeField]
    private Color _highlightColor = Color.white;

    [SerializeField]
    private float _duration = 1f;

    private Tween _colorTween;

    private int _oldLayer;

    private Coroutine _dragCoroutine;

    [SerializeField]
    private List<ActionType> _allowedTypes;

    private bool isSharingFirstTime;
    public bool IsSharing { get; set; }

    private void Awake()
    {
        isSharingFirstTime = true;
        IsSharing = false;
    }

    public List<ActionType> GetAllowedActions()
    {
        return _allowedTypes;
    }

    private void Start()
    {
        _chaptersMenu = transform.parent.gameObject;
    }

    public void OnGazeEnter()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _sprite.color; }, (c) => { _sprite.color = c; }, _highlightColor, _duration).Play();
    }

    public void OnGazeLeave()
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _sprite.color; }, (c) => { _sprite.color = c; }, _selfColor, _duration).Play();
    }

    public void OnGestureTap() { }

    public bool TryToDrag()
    {
        Debug.Log("sd");
        _oldLayer = _chaptersMenu.layer;

        ChangeLayerRecursively(_chaptersMenu, LayerMask.NameToLayer("Ignore Raycast"));

        _dragCoroutine = StartCoroutine(DragCoroutine());

        return true;
    }

    public bool TryToResize() { return false; }
    IEnumerator DragCoroutine()
    {
        while (true)
        {
            _chaptersMenu.transform.position = Vector3.Lerp(_chaptersMenu.transform.position, OwnCursorManager.Instance.cursor.position, Time.deltaTime * 8f);
            //_chaptersMenu.transform.rotation = Quaternion.Slerp(_chaptersMenu.transform.rotation, OwnCursorManager.Instance.cursor.rotation, Time.deltaTime * 8f);
            _chaptersMenu.transform.rotation = Quaternion.LookRotation(-OwnGazeManager.Instance.PointNormal);
            //Vector3 eulerAngle = _chaptersMenu.transform.rotation.eulerAngles;
            //Vector3 test = Vector3.Lerp(_chaptersMenu.transform.rotation.eulerAngles, OwnCursorManager.Instance.cursor.rotation.eulerAngles, Time.deltaTime);
            //test.y *= -1;
            //_chaptersMenu.transform.rotation = Quaternion.Euler(test);

            if (isSharingFirstTime || IsSharing)
            {
                isSharingFirstTime = false;
                SV_Sharing.Instance.SendTransform(
                    _chaptersMenu.transform.position,
                    _chaptersMenu.transform.rotation,
                    _chaptersMenu.transform.localScale,
                    "menu_pos");
            }

            yield return null;
        }
    }

    public void StopDrag()
    {
        StopCoroutine(_dragCoroutine);

        _chaptersMenu.transform.position = OwnGazeManager.Instance.HitPoint + new Vector3(0f,0f,-0.05f);
        _chaptersMenu.transform.rotation = Quaternion.LookRotation(-OwnGazeManager.Instance.PointNormal);

        ChangeLayerRecursively(_chaptersMenu, _oldLayer);
    }

    private void ChangeLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;

        foreach (Transform child in go.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }
}

