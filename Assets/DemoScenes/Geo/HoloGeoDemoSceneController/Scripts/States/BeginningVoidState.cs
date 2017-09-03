using System;
using UnityEngine;

public class BeginningVoidState : DemoSceneState, IDemoSceneState
{
    public BeginningVoidState(StatesStuffContainer stuff) : base(stuff)
    {

    }

    public bool RecursiveState
    {
        get
        {
            return false;
        }
    }

    bool IDemoSceneState.EarthState
    {
        get
        {
            return true;
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
        Debug.Log("Beginning Void");

        cb = callback;
        if ((StatesStuffObject.LastState != null) && (StatesStuffObject.LastState.EarthState))
        {
            if (StatesStuffObject.Earth != null)
            {
                BoundaryVolume.Instance.transform.position = StatesStuffObject.Earth.transform.position;
                BoundaryVolume.Instance.transform.rotation = StatesStuffObject.Earth.transform.rotation;
                BoundaryVolume.Instance.transform.localScale = StatesStuffObject.Earth.transform.localScale;
            }
            /*if (BigSimpleInfoPanelController.Instance != null)
            {
                BoundaryVolume.Instance.InfoVolume.transform.position = BigSimpleInfoPanelController.Instance.transform.position;
                BoundaryVolume.Instance.InfoVolume.transform.rotation = BigSimpleInfoPanelController.Instance.transform.rotation;
                BoundaryVolume.Instance.InfoVolume.transform.localScale = BigSimpleInfoPanelController.Instance.transform.localScale * BoundaryVolume.Instance.transform.localScale.y;
            }
            if (BigSimpleInfoPanelController.Instance != null)
            {
                BoundaryVolume.Instance.MenuVolume.transform.position = HoloStudyDemoGeoMenuController.Instance.transform.position;
                BoundaryVolume.Instance.MenuVolume.transform.rotation = HoloStudyDemoGeoMenuController.Instance.transform.rotation;
                BoundaryVolume.Instance.MenuVolume.transform.localScale = HoloStudyDemoGeoMenuController.Instance.transform.localScale * BoundaryVolume.Instance.transform.localScale.y;
            }*/

            /*if (BigSimpleInfoPanelController.Instance != null)
            {
                StartPositioning.Instance.InfoPosAdding = BigSimpleInfoPanelController.Instance.transform.localPosition;
                StartPositioning.Instance.InfoPosScaling = BigSimpleInfoPanelController.Instance.transform.localScale;
            }
            if (HoloStudyDemoGeoMenuController.Instance != null)
            {
                StartPositioning.Instance.MenuPosAdding = HoloStudyDemoGeoMenuController.Instance.transform.localPosition;
                StartPositioning.Instance.MenuPosScaling = HoloStudyDemoGeoMenuController.Instance.transform.localScale;
            }*/
        }
        else
        {
            if (StatesStuffObject.Pie != null)
            {
                BoundaryVolume.Instance.transform.position = StatesStuffObject.Pie.transform.position;
                BoundaryVolume.Instance.transform.rotation = StatesStuffObject.Pie.transform.rotation;
                BoundaryVolume.Instance.transform.localScale = StatesStuffObject.Pie.transform.localScale;
            }
        }
        checkClosedProfessor();
    }

    void checkClosedProfessor()
    {
        if (ProfessorOnPlatform.Instance.ProfessorVisible())
        {
            ProfessorOnPlatform.Instance.StopTalkingAndScaleToZero(checkClosedMoreInfo);
        }
        else
        {
            checkClosedMoreInfo();
        }
    }

    void checkClosedMoreInfo()
    {
        MoreInfoController.Instance.onHide += checkClosedEarth;
        MoreInfoController.Instance.HideMoreInfo();
    }

    void checkClosedEarth()
    {
        MoreInfoController.Instance.onHide -= checkClosedEarth;
        if (StatesStuffObject.Earth != null)
        {
            StatesStuffObject.Earth.GetComponent<SlicedEarthPolygon>().HideFromTheScene(() =>
            {
                checkBigInfoPanelOpened();
            });
        }
        else
        {
            checkBigInfoPanelOpened();
        }
    }

    void checkBigInfoPanelOpened()
    {
        //BigSimpleInfoPanelController.Instance.Hide(checkClosedPie);
        BigSimpleInfoPanelController.Instance.RemoveFromScene(checkClosedPie);
    }

    void checkClosedPie()
    {
        if (StatesStuffObject.Pie != null)
        {
            if (StatesStuffObject.Pie.activeSelf)
            {
                StatesStuffObject.Pie.GetComponent<PiePolygon>().HideFromTheScene(checkRockInfoPanelClosed);
            }
            else
            {
                checkRockInfoPanelClosed();
            }
        }
        else
        {
            checkRockInfoPanelClosed();
        }
    }

    void checkRockInfoPanelClosed()
    {
        //SmallInfoViewController.Instance.Hide(checkRock3D);
        SmallInfoViewController.Instance.RemoveFromScene(checkRock3D);
    }

    void checkRock3D()
    {
        //tone3DViewController.Instance.Hide(checkMenuClosed);
        Stone3DViewController.Instance.RemoveFromScene(checkMenuClosed);
    }

    void checkMenuClosed()
    {
        HoloStudyDemoGeoMenuController.Instance.Hide(onCheckedAllClosed);
    }

    void onCheckedAllClosed()
    {
        if (cb != null)
        {
            cb.Invoke();
            cb = null;
        }
    }

    public void exit(Action callback)
    {
        callback.Invoke();
    }
}
