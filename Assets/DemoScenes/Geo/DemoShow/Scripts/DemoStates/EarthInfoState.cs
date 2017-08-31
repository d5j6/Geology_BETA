using System;
using UnityEngine;
using System.Collections.Generic;

public class EarthInfoState : IDemoShowState
{
    public DemoShowStuff demoShowStuff;

    Action onStageEnd;

    Dictionary<Language, Dictionary<string, float>> timings;

    public IDemoShowState NextState
    {
        get;
        set;
    }

    void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("showMoreInfo_1", 40f);
        timings[Language.Russian].Add("showMoreInfo_2", 25.5f);
        timings[Language.Russian].Add("getProfessorsFact_1", 8f);
        timings[Language.Russian].Add("startMagneticFieldAction_1", 27f);
        timings[Language.Russian].Add("startTemperatureAction_1", 27.75f);
        timings[Language.Russian].Add("startMassAction_1", 23f);

        timings[Language.English].Add("showMoreInfo_1", 40f);
        timings[Language.English].Add("showMoreInfo_2", 29.5f);
        timings[Language.English].Add("getProfessorsFact_1", 7.1f);
        timings[Language.English].Add("startMagneticFieldAction_1", 27f);
        timings[Language.English].Add("startTemperatureAction_1", 26.75f);
        timings[Language.English].Add("startMassAction_1", 24f);
    }

    public void enter(Action callback = null)
    {
        if (timings == null)
        {
            InitializeTimings();
        }
        onStageEnd = callback;
        showMoreInfo();
    }

    void showMoreInfo()
    {
        MoreInfoController.Instance.ShowMoreInfo();
        AudioSourceController.Instance.FollowGameObject(Camera.main.gameObject);
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_EarthMainInfo").gameObject);

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["showMoreInfo_1"], () =>
        {
            //AudioSourceController.Instance.StopPlaying();
        });

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["showMoreInfo_2"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            MoreInfoController.Instance.HideMoreInfo(() =>
            {
                getProfessorsFact();
            });
        });
    }

    public void getProfessorsFact()
    {
        demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().ShowByScalingAndStartTalking(() =>
        {
            AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_ByTheWayEarthInfo").gameObject);

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getProfessorsFact_1"], () =>
            {
                AudioSourceController.Instance.StopPlaying();
                demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().StopTalkingAndScaleToZero(startMagneticFieldAction);
            });
        });
    }

    void startMagneticFieldAction()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_MagneticField").gameObject);
        //SceneStateMachine.Instance.GoToMagneticFieldMeasuringState();
        EarthController.Instance.MeasureMagneticField();

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startMagneticFieldAction_1"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            startTemperatureAction();
        });
    }

    void startTemperatureAction()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_Temperature").gameObject);
        //SceneStateMachine.Instance.GoToTemperatureState();
        EarthController.Instance.MeasureTemperature();

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTemperatureAction_1"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            startMassAction();
        });
    }

    void startMassAction()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_Mass").gameObject);
        //SceneStateMachine.Instance.GoToMassMeasuringState();
        EarthController.Instance.MeasureMass();

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startMassAction_1"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            
            if (onStageEnd != null)
            {
                onStageEnd.Invoke();
                onStageEnd = null;
            }

            if (NextState != null)
            {
                DemoShowStateMachine.Instance.GotoState(NextState);
                //NextState = null;
            }
        });
    }

    public void exit(Action callback = null)
    {
        LeanTween.cancel(DemoShowStateMachine.Instance.gameObject);
        AudioSourceController.Instance.StopPlaying();
        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public void interrupt(Action callback = null)
    {
        //onInterruptionEnd = callback;

        LeanTween.cancel(DemoShowStateMachine.Instance.gameObject);

        demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().StopTalkingAndScaleToZero();

        AudioSourceController.Instance.StopPlaying(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }
}
