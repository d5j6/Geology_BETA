using System;
using UnityEngine;

public class EarthState : DemoSceneState, IDemoSceneState
{
    public EarthState(StatesStuffContainer stuff) : base(stuff) { }

    bool IDemoSceneState.EarthState
    {
        get
        {
            return true;
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

    private Action cb;
    public void enter(System.Action callback = null)
    {
        cb = callback;
        if (StatesStuffObject.LastState.EarthState == false)
        {
            SmallInfoViewController.Instance.RemoveFromScene(() =>
            {
                Stone3DViewController.Instance.RemoveFromScene(() =>
                {
                    PiePolygon.Instance.HideFromTheScene(() =>
                    {
                        showEarth();
                    });
                });
            });
        }
        else
        {
            openEarth();
        }
    }

    public void exit(Action callback)
    {
        callback.Invoke();
    }

    void openEarth()
    {
        BigSimpleInfoPanelController.Instance.Show(
            DataController.Instance.GetMaxScrollPosFor(InformationsMockup.Earth),
            DataController.Instance.GetLabelFor(InformationsMockup.Earth),
            DataController.Instance.GetDescriptionFor(InformationsMockup.Earth));
        EarthController.Instance.Surface().Whole().Join().Go(() =>
        {
            if (cb != null)
            {
                cb.Invoke();
                cb = null;
            }
        });
    }

    void showEarth()
    {
        if (StatesStuffObject.LoadPrefabsDynamically)
        {
            Loader.Instance.LoadAndIntsntiateGOPrefab("OptimizedEarthComplex", onEarthInstantiated);
        }
        else
        {
            onEarthInstantiated(StatesStuffObject.Earth);
        }
    }

    void onEarthInstantiated(GameObject go)
    {
        StatesStuffObject.Earth = go;
        StatesStuffObject.Earth.SetActive(true);
        /*StatesStuffObject.Earth.transform.position = BoundaryVolume.Instance.transform.position;
        StatesStuffObject.Earth.transform.rotation = BoundaryVolume.Instance.transform.rotation;
        StatesStuffObject.Earth.transform.localScale = BoundaryVolume.Instance.transform.localScale;*/
        StatesStuffObject.Earth.transform.parent = StatesStuffObject.AllParent;
        StatesStuffObject.Earth.GetComponent<SlicedEarthPolygon>().HideImmediately();
        StatesStuffObject.Earth.GetComponent<SlicedEarthPolygon>().BringToTheScene();

        LeanTween.delayedCall(StatesStuffObject.Earth.GetComponent<SlicedEarthPolygon>().GetShowingTime(), () =>
        {
            loadBigInfoPanel();
        });
    }

    void loadBigInfoPanel()
    {
        if (StatesStuffObject.LoadPrefabsDynamically)
        {
            Loader.Instance.LoadAndIntsntiateGOPrefab("HitechInfoPanel", onBigInfoPanelInstantiated);
        }
        else
        {
            onBigInfoPanelInstantiated(StatesStuffObject.BigInfoPanel);
        }
    }

    void onBigInfoPanelInstantiated(GameObject go)
    {
        StatesStuffObject.BigInfoPanel = go;
        StatesStuffObject.BigInfoPanel.SetActive(true);

        //StatesStuffObject.BigInfoPanel.transform.position = StatesStuffObject.Earth.transform.position + StatesStuffObject.Earth.transform.right * -1.15f;

        StatesStuffObject.BigInfoPanel.transform.parent = StatesStuffObject.AllParent;
        StatesStuffObject.BigInfoPanel.GetComponent<BigSimpleInfoPanelController>().HideImmediately();
        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Earth);

        //LeanTween.scale(StatesStuffObject.BigInfoPanel, Vector3.one, 0.8f);

        if (cb != null)
        {
            cb.Invoke();
            cb = null;
        }
    }
}
