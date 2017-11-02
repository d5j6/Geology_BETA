using UnityEngine;
using HoloToolkit.Unity;

public class SlicedEarthPolygon : Singleton<SlicedEarthPolygon>
{
    public event System.Action OnHided;
    public GameObject Planet;

    public float showingTime = 0.9f;
    //public float earthInitialScale = 0.9f;

    public void BringToTheScene(System.Action callback = null)
    {
        LeanTween.scale(Planet, Vector3.one, showingTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public float GetShowingTime()
    {
        return showingTime;
    }

    public void HideFromTheScene(System.Action callback = null)
    {
        if (Planet.transform.localScale == Vector3.zero)
        {
            if (callback != null)
            {
                callback.Invoke();
            }
            if (OnHided != null)
            {
                OnHided();
                OnHided = null;
            }
        }
        else
        {
            LeanTween.scale(Planet, Vector3.zero, showingTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
            {
                if (callback != null)
                {
                    callback.Invoke();
                }

                if (OnHided != null)
                {
                    OnHided();
                    OnHided = null;
                }
            });
        }
    }

    public void HideImmediately()
    {
        Planet.transform.localScale = Vector3.zero;
    }

    private void Awake()
    {
        HideImmediately();
    }
}