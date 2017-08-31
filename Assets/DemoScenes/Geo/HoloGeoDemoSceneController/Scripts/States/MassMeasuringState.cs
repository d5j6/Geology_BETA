using System;
using UnityEngine;

public class MassMeasuringState : DemoSceneState, IDemoSceneState
{
    Action cb;

    public MassMeasuringState(StatesStuffContainer stuff) : base(stuff) { }

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

    public void enter(System.Action callback = null)
    {
        cb = callback;
        startMeasuringMass();
    }

    void startMeasuringMass()
    {
        Loader.Instance.LoadAndIntsntiateGOPrefab("CurvedSpace", onMassMeasurerInstantiated);
    }

    void onMassMeasurerInstantiated(GameObject go)
    {
        BigSimpleInfoPanelController.Instance.Show(
            DataController.Instance.GetMaxScrollPosFor(InformationsMockup.Mass),
            DataController.Instance.GetLabelFor(InformationsMockup.Mass),
            DataController.Instance.GetDescriptionFor(InformationsMockup.Mass));

        go.GetComponent<MassMeasurer>().Planet = EarthController.Instance.gameObject;
        Action<Action> a;
        if (DemoShowStateMachine.playing)
        {
            a = go.GetComponent<MassMeasurer>().MeasureInDemoMode;
        }
        else if (ProfessorsLecturesController.Instance.LecturePlaying)
        {
            a = go.GetComponent<MassMeasurer>().MeasureInLectureMode;
        }
        else
        {
            a = go.GetComponent<MassMeasurer>().Measure;
        }
        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Mass);
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
