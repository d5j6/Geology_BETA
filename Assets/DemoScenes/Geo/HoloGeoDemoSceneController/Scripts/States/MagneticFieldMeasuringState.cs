using System;
using UnityEngine;

public class MagneticFieldMeasuringState : DemoSceneState, IDemoSceneState
{
    System.Action cb;

    public MagneticFieldMeasuringState(StatesStuffContainer stuff) : base(stuff) { }

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

    public void enter(Action callback = null)
    {
        cb = callback;
        Loader.Instance.LoadAndIntsntiateGOPrefab("MagneticMeasurer", onMagneticMeasurerInstantiated);
    }

    void onMagneticMeasurerInstantiated(GameObject go)
    {
        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.MagneticField);

        go.GetComponent<MagneticFieldMeasurer>().Planet = EarthController.Instance.gameObject;
        Action<Action> a;
        if (DemoShowStateMachine.playing)
        {
            a = go.GetComponent<MagneticFieldMeasurer>().MeasureInDemoMode;
        }
        else if (ProfessorsLecturesController.Instance.LecturePlaying)
        {
            a = go.GetComponent<MagneticFieldMeasurer>().MeasureInLectureMode;
        }
        else
        {
            a = go.GetComponent<MagneticFieldMeasurer>().Measure;
        }
        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.MagneticField);
        a(() =>
        {
            MonoBehaviour.Destroy(go);
            if (cb != null)
            {
                cb.Invoke();
                cb = null;
            }
        });
    }

    public void exit(System.Action callback)
    {
        callback.Invoke();
    }
}
