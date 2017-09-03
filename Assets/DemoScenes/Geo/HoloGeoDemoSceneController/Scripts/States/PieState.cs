using System;
using UnityEngine;

public class PieState : DemoSceneState, IDemoSceneState
{
    public bool EarthState
    {
        get
        {
            return false;
        }
    }

    public bool RecursiveState
    {
        get
        {
            return false;
        }
    }

    public Fader ButtonFader
    {
        get;
        set;
    }

    public RingsSelectingController ButtonRings
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

    public void exit(Action callback)
    {
        callback.Invoke();
    }

    public PieState(StatesStuffContainer stuff) : base(stuff) { }

    private Action cb;
    public void enter(Action callback = null)
    {
        cb = callback;
        if (StatesStuffObject.LastState.EarthState == true)
        {
            CloseEarthPolygon(() =>
            {
                ShowPiePolygon();
            });
        }
        else
        {
            ShowPiePolygon();
        }
    }

    private void CloseEarthPolygon(Action callback = null)
    {
        EarthController.Instance.Surface().Whole().Join().Go(() =>
        {
            closeEarth(callback);
        });
    }

    private void closeEarth(Action callback = null)
    {
        StatesStuffObject.BigInfoPanel.GetComponent<BigSimpleInfoPanelController>().Hide(() =>
        {
            SlicedEarthPolygon.Instance.HideFromTheScene(callback);
        });

        /*Vector3 earthScale = Vector3.one;
        Vector3 bigInfoPanelScale = Vector3.one;
        LeanTween.value(StatesStuffObject.Earth, 0, 1, 1f).setEase(LeanTweenType.easeInOutQuad).setOnStart(() =>
        {
            earthScale = StatesStuffObject.Earth.transform.localScale;
            bigInfoPanelScale = StatesStuffObject.BigInfoPanel.transform.localScale;
        }).setOnUpdate((float val) =>
        {
            StatesStuffObject.Earth.transform.localScale = Vector3.Lerp(earthScale, Vector3.zero, val);
            StatesStuffObject.BigInfoPanel.transform.localScale = Vector3.Lerp(bigInfoPanelScale, Vector3.zero, val);
        }).setOnComplete(() =>
        {
            StatesStuffObject.Earth.SetActive(false);
            StatesStuffObject.BigInfoPanel.SetActive(false);

            if (callback != null)
            {
                callback.Invoke();
            }
        });*/
    }

    private void ShowPiePolygon()
    {
        if (StatesStuffObject.LoadPrefabsDynamically)
        {
            Loader.Instance.LoadAndIntsntiateGOPrefab("PieComplex", onPieComplexInstantiated);
        }
        else
        {
            onPieComplexInstantiated(StatesStuffObject.Pie);
        }
    }

    private void onPieComplexInstantiated(GameObject go)
    {
        StatesStuffObject.Pie = go;
        StatesStuffObject.Pie.SetActive(true);
        /*StatesStuffObject.Pie.transform.position = BoundaryVolume.Instance.transform.position;
        StatesStuffObject.Pie.transform.rotation = BoundaryVolume.Instance.transform.rotation;
        StatesStuffObject.Pie.transform.localScale = BoundaryVolume.Instance.transform.localScale;
        StatesStuffObject.Pie.GetComponent<PiePolygon>().polygonScale = BoundaryVolume.Instance.transform.localScale;*/
        LeanTween.delayedCall(0.2f, () =>
        {
            StatesStuffObject.Pie.transform.parent = StatesStuffObject.AllParent;
            StatesStuffObject.Pie.GetComponent<PiePolygon>().BringToTheScene(() =>
            {
                SmallInfoViewController.Instance.BringToScene(Vector3.one * BoundaryVolume.Instance.transform.localScale.y, () =>
                {
                    Stone3DViewController.Instance.BringToScene(Vector3.one * 0.5f * BoundaryVolume.Instance.transform.localScale.y, () =>
                    {
                        if (cb != null)
                        {
                            cb.Invoke();
                            cb = null;
                        }
                    });
                });
            });
        });
    }
}
