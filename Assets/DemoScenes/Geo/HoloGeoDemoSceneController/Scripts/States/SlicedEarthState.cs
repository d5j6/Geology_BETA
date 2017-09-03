using System;
using UnityEngine;

public class SlicedEarthState : DemoSceneState, IDemoSceneState
{
    private Action cb;

    public SlicedEarthState(StatesStuffContainer stuff) : base(stuff) { }

    public bool EarthState
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

    public void enter(Action callback = null)
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

    private void openEarth()
    {
        BigSimpleInfoPanelController.Instance.Show(
            DataController.Instance.GetMaxScrollPosFor(InformationsMockup.InnerCore), 
            DataController.Instance.GetLabelFor(InformationsMockup.InnerCore), 
            DataController.Instance.GetDescriptionFor(InformationsMockup.InnerCore));
        EarthController.Instance.OpenEarthWidely(() =>
        {
            if (cb != null)
            {
                cb.Invoke();
                cb = null;
            }
        });
    }

    private void showEarth()
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

    private void onEarthInstantiated(GameObject go)
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

    private void loadBigInfoPanel()
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

    private void onBigInfoPanelInstantiated(GameObject go)
    {
        StatesStuffObject.BigInfoPanel = go;
        StatesStuffObject.BigInfoPanel.SetActive(true);
        //StatesStuffObject.BigInfoPanel.transform.position = StatesStuffObject.Earth.transform.position + StatesStuffObject.Earth.transform.right * -1.15f;
        StatesStuffObject.BigInfoPanel.transform.parent = StatesStuffObject.AllParent;
        StatesStuffObject.BigInfoPanel.GetComponent<BigSimpleInfoPanelController>().HideImmediately();
        //LeanTween.scale(StatesStuffObject.BigInfoPanel, Vector3.one, 0.8f);

        openEarth();
    }
}
