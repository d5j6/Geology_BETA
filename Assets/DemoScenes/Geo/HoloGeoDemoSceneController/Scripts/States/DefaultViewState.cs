using System;
using UnityEngine;

public class DefaultViewState : DemoSceneState, IDemoSceneState
{
    public DefaultViewState(StatesStuffContainer stuff) : base(stuff) { }

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
        get;
        set;
    }

    private Action cb;
    public void enter(Action callback = null)
    {
        Debug.Log("Defaul view state");
        cb = callback;
        showEarth();
    }

    public void exit(Action callback)
    {
        callback.Invoke();
    }

    private void showEarth()
    {
/*#if !UNITY_EDITOR
        initSpeechCommands();
#endif*/
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
        if (/*StartPositioning.Instance.positioned*/false)
        {
            StatesStuffObject.Earth.transform.position = BoundaryVolume.Instance.transform.position;
            StatesStuffObject.Earth.transform.rotation = BoundaryVolume.Instance.transform.rotation;
            StatesStuffObject.Earth.transform.localScale = BoundaryVolume.Instance.transform.localScale;
        }
        else
        {
            Vector3 pos = Camera.main.transform.forward * 2.5f;
            pos.y = Camera.main.transform.position.y/* + 0.27f*/;
            StatesStuffObject.Earth.transform.position = pos;
            StatesStuffObject.Earth.transform.rotation = BoundaryVolume.Instance.transform.rotation;
            //StatesStuffObject.Earth.transform.localScale = BoundaryVolume.Instance.transform.localScale;
        }
        //StatesStuffObject.Earth.transform.position = BoundaryVolume.Instance.transform.position;
        StatesStuffObject.Earth.transform.parent = StatesStuffObject.AllParent;
        StatesStuffObject.Earth.GetComponent<SlicedEarthPolygon>().HideImmediately();
        //Debug.Log("Bringing to scene");
        StatesStuffObject.Earth.GetComponent<SlicedEarthPolygon>().BringToTheScene();

        LeanTween.delayedCall(StatesStuffObject.Earth.GetComponent<SlicedEarthPolygon>().GetShowingTime(), () =>
        {
            loadBigInfoPanel();
            showMenu();
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

        if (/*StartPositioning.Instance.positioned*/false)
        {
            /*StatesStuffObject.BigInfoPanel.transform.position = StartPositioning.Instance.InfoHolo.position;
            StatesStuffObject.BigInfoPanel.transform.localScale = StartPositioning.Instance.InfoPosScaling * BoundaryVolume.Instance.transform.localScale.y;*/
        }
        else
        {
            StatesStuffObject.BigInfoPanel.transform.position = StatesStuffObject.Earth.transform.position + Camera.main.transform.rotation * StatesStuffObject.Earth.transform.right * -1.15f;
        }
        StatesStuffObject.BigInfoPanel.transform.parent = StatesStuffObject.AllParent;
        /*StatesStuffObject.BigInfoPanel.GetComponent<BigSimpleInfoPanelController>().HideImmediately();
        
        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Earth);*/
        
        BigSimpleInfoPanelController.Instance.RemoveFromSceneImmediately();
        BigSimpleInfoPanelController.Instance.BringToScene();
    }

    private void showMenu()
    {
        if (StatesStuffObject.LoadPrefabsDynamically)
        {
            Loader.Instance.LoadAndIntsntiateGOPrefab("Menu", onTopMenuInstantiated);
        }
        else
        {
            onTopMenuInstantiated(StatesStuffObject.Menu);
        }
    }

    private void onTopMenuInstantiated(GameObject go)
    {
        StatesStuffObject.Menu = go;

        if (/*StartPositioning.Instance.positioned*/false)
        {
            /*StatesStuffObject.Menu.transform.localScale = StartPositioning.Instance.MenuPosScaling * BoundaryVolume.Instance.transform.localScale.y;
            StatesStuffObject.Menu.GetComponent<HoloStudyDemoGeoMenuController>().Show(StartPositioning.Instance.MenuHolo.position, cb);*/
        }
        else
        {
            StatesStuffObject.Menu.GetComponent<HoloStudyDemoGeoMenuController>().Show(StatesStuffObject.Earth.transform.position + Camera.main.transform.rotation * StatesStuffObject.Earth.transform.right + Vector3.up * -0.5f, () =>
            {
                BoundaryVolume.Instance.transform.localScale = StatesStuffObject.Earth.transform.localScale;
                BoundaryVolume.Instance.transform.localPosition = StatesStuffObject.Earth.transform.localPosition;
                BoundaryVolume.Instance.transform.localRotation = StatesStuffObject.Earth.transform.localRotation;

                StatesStuffObject.Pie.transform.position = BoundaryVolume.Instance.transform.position;
                StatesStuffObject.Pie.transform.rotation = BoundaryVolume.Instance.transform.rotation;
                StatesStuffObject.Pie.transform.localScale = BoundaryVolume.Instance.transform.localScale;
                StatesStuffObject.Pie.GetComponent<PiePolygon>().polygonScale = BoundaryVolume.Instance.transform.localScale;

                Stone3DViewController.Instance.transform.position = StatesStuffObject.Pie.transform.position + Camera.main.transform.rotation * new Vector3(0.336f, 0.331f, 0) * BoundaryVolume.Instance.transform.localScale.y;
                SmallInfoViewController.Instance.transform.position = StatesStuffObject.Pie.transform.position + Camera.main.transform.rotation * new Vector3(-0.987f, 0.144f, 0) * BoundaryVolume.Instance.transform.localScale.y;

                /*StartPositioning.Instance.InfoPosAdding = BigSimpleInfoPanelController.Instance.transform.localPosition / BoundaryVolume.Instance.transform.localScale.y - StatesStuffObject.Earth.transform.localPosition;
                StartPositioning.Instance.InfoPosScaling = BigSimpleInfoPanelController.Instance.transform.localScale / BoundaryVolume.Instance.transform.localScale.y;

                StartPositioning.Instance.MenuPosAdding = HoloStudyDemoGeoMenuController.Instance.transform.localPosition / BoundaryVolume.Instance.transform.localScale.y - StatesStuffObject.Earth.transform.localPosition;
                StartPositioning.Instance.MenuPosScaling = HoloStudyDemoGeoMenuController.Instance.transform.localScale / BoundaryVolume.Instance.transform.localScale.y;*/

                if (cb != null)
                {
                    cb.Invoke();
                    cb = null;
                }
            });
        }

    }
}
