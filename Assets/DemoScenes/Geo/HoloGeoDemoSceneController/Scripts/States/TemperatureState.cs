using System;
using UnityEngine;

public class TemperatureState : DemoSceneState, IDemoSceneState
{
    Action cb;

    public TemperatureState(StatesStuffContainer stuff) : base(stuff) { }

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
        Loader.Instance.LoadAndIntsntiateGOPrefab("TemperatureMeasurer", onMagneticMeasurerInstantiated);
    }

    private void onMagneticMeasurerInstantiated(GameObject go)
    {
        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Temperature);

        go.GetComponent<TemperatureMeasurer>().Planet = EarthController.Instance.gameObject;
        go.GetComponent<TemperatureMeasurer>().PlanetSurface = EarthController.Instance.GetSurfaceGO();
        Action<Action> a;
        if (DemoShowStateMachine.playing)
        {
            a = go.GetComponent<TemperatureMeasurer>().MeasureInDemoMode;
        }
        else if (ProfessorsLecturesController.Instance.LecturePlaying)
        {
            a = go.GetComponent<TemperatureMeasurer>().MeasureInLectureMode;
        }
        else
        {
            a = go.GetComponent<TemperatureMeasurer>().Measure;
        }
        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Temperature);
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
