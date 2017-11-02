using UnityEngine;
using HoloToolkit.Unity;
using System;

public class PiePolygon : Singleton<PiePolygon>
{
    public Action Hided;
    public Action Showed;
    //private PieManipulator pieManipulator;

    public GameObject MainPieGameObject;
    public GameObject DotPlane;
    private Material dotsPlaneMaterial;

    public float targetPieScale = 0.5f;
    public float timeOfDotsShowing = 2.5f;
    public float timeOfDotsHiding = 1.5f;
    public float timeOfPieShowing = 1.5f;
    public float timeOfPieHiding = 1.0f;

    private float frequencyDeviations = 15.85f;
    private float targetFrequency = 35.2f;

    public Vector3 polygonScale = Vector3.one;

    public float GetShowingTime()
    {
        return timeOfDotsShowing;
    }

    public void BringToTheScene(System.Action callback = null)
    {
        if (Showed != null)
        {
            Showed.Invoke();
        }

        Render();

        LeanTween.value(gameObject, 0f, Mathf.PI*2f, timeOfDotsShowing).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            dotsPlaneMaterial.SetFloat("_DotsFadingOuterRange", (val / (Mathf.PI * 2f)) * 0.378f);
            dotsPlaneMaterial.SetFloat("_DotsFrequency", targetFrequency - Mathf.Sin(val)* frequencyDeviations);
        });

        LeanTween.value(gameObject, 0f, targetPieScale * polygonScale.x, timeOfPieShowing).setDelay(timeOfDotsShowing - timeOfPieShowing).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            MainPieGameObject.transform.localScale = Vector3.one * val;
        }).setOnComplete(() =>
        {
            PieController.Instance.StartShowingLabels();
        });
        LeanTween.rotateAroundLocal(MainPieGameObject, Vector3.up, 360.0f, timeOfPieShowing).setDelay(timeOfDotsShowing - timeOfPieShowing).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            SmallInfoViewController.Instance.Show(InformationsMockup.Pie);
            /*SmallInfoViewController.Instance.BringToScene(Vector3.one * BoundaryVolume.Instance.transform.localScale.y, () =>
            {
                SmallInfoViewController.Instance.Show(InformationsMockup.Pie);
            });*/

            PieController.Instance.ShowPieDivider(() =>
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        });
    }

    public void HideFromTheScene(System.Action callback = null)
    {
        if (MainPieGameObject.transform.localScale == Vector3.zero)
        {
            if (callback != null)
            {
                callback.Invoke();
            }

            if (Hided != null)
            {
                Hided.Invoke();
            }
        }
        else
        {
            PieController.Instance.HidePieDivider();

            LeanTween.value(gameObject, Mathf.PI * 2f, 0f, timeOfDotsHiding).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                dotsPlaneMaterial.SetFloat("_DotsFadingOuterRange", (val / (Mathf.PI * 2f)) * 0.378f);
                dotsPlaneMaterial.SetFloat("_DotsFrequency", targetFrequency - Mathf.Sin(val));
            });

            LeanTween.rotateAroundLocal(MainPieGameObject, Vector3.up, -360.0f, timeOfPieHiding).setDelay(timeOfDotsHiding - timeOfPieHiding).setEase(LeanTweenType.easeInOutCubic);
            
            LeanTween.value(gameObject, targetPieScale * polygonScale.x, 0f, timeOfPieHiding).setDelay(timeOfDotsHiding - timeOfPieHiding).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                MainPieGameObject.transform.localScale = Vector3.one * val;
            }).setOnComplete(() =>
            {
                Unrender();

                if (callback != null)
                {
                    callback.Invoke();
                }

                if (Hided != null)
                {
                    Hided.Invoke();
                }
            });
        }
    }

    public void HideImmediately()
    {
        PieController.Instance.StopShowingLabels();
        Unrender();
        dotsPlaneMaterial.SetFloat("_DotsFadingOuterRange", 0f);
        dotsPlaneMaterial.SetFloat("_DotsFadingInnerRange", 0f);
        MainPieGameObject.transform.localScale = Vector3.zero;
    }

    private void Unrender()
    {
        setRenderers(false);
    }

    private void Render()
    {
        setRenderers(true);
    }

    private void setRenderers(bool enabled)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = enabled;
        }
    }

    private void Awake()
    {
        //pieManipulator = MainPieGameObject.GetComponent<PieManipulator>();
        dotsPlaneMaterial = DotPlane.GetComponent<Renderer>().material;
        HideImmediately();
    }
}