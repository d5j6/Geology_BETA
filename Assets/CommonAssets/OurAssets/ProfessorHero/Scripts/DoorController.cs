using UnityEngine;
using System.Collections.Generic;

public class DoorController : MonoBehaviour {

    public GameObject Body;

    Material[] mats;
    Color[] cols;

    void Awake()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        mats = new Material[renderers.Length];
        cols = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            mats[i] = renderers[i].material;
            cols[i] = mats[i].color;
        }
    }

	public void ShowUp(Vector3 showingPosition, System.Action callback = null, float duration = 1.6f)
    {
        transform.position = showingPosition + Vector3.up * 2;
        LeanTween.move(gameObject, showingPosition, duration).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.value(gameObject, 0, 1, duration).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            for (int i = 0; i < mats.Length; i++)
            {
                cols[i].a = val;
                mats[i].color = cols[i];
            }
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void Hide(System.Action callback = null, float duration = 1.6f)
    {
        Vector3 targetPosition = transform.position + Vector3.up * 2;
        LeanTween.move(gameObject, targetPosition, duration).setEase(LeanTweenType.easeInCubic);
        LeanTween.value(gameObject, 1, 0, duration).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            for (int i = 0; i < mats.Length; i++)
            {
                cols[i].a = val;
                mats[i].color = cols[i];
            }
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void FadeIn(float duration = 1.3f, System.Action callback = null)
    {
        LeanTween.value(gameObject, 0, 1, duration).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            for (int i = 0; i < mats.Length; i++)
            {
                cols[i].a = val;
                mats[i].color = cols[i];
            }
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void FadeOut(float duration = 1.3f, System.Action callback = null)
    {
        LeanTween.value(gameObject, 1, 0, duration).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            for (int i = 0; i < mats.Length; i++)
            {
                cols[i].a = val;
                mats[i].color = cols[i];
            }
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void Open(System.Action callback = null, float t = 1.14f)
    {
        LeanTween.rotateAroundLocal(Body, Vector3.up, 80f, t).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void Close(float t = 1.02f, System.Action callback = null)
    {
        LeanTween.rotateAroundLocal(Body, Vector3.up, -80f, t).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }
}
