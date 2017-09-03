using UnityEngine;
using System;
using System.Collections.Generic;

public class ClickNextForContinuingState: IDemoShowState
{
    public DemoShowStuff demoShowStuff;

    System.Action onStageEnd;

    Dictionary<Language, Dictionary<string, float>> timings;

    //bool pause = true;

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

        timings[Language.Russian].Add("womanVoice_1", 5f);

        timings[Language.English].Add("womanVoice_1", 5f);
    }

    public void enter(Action callback = null)
    {
        onStageEnd = callback;
        if (timings == null)
        {
            InitializeTimings();
        }
        womanVoice();
        DemoNextActionHandler.Instance.PrepareNextAction(NextState);
        //NextState = null;
    }

    void womanVoice()
    {
        AudioSourceController.Instance.FollowGameObject(Camera.main.gameObject);
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_NextOrClickForContinuing").gameObject);

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["womanVoice_1"], () =>
        {
            if (onStageEnd != null)
            {
                onStageEnd.Invoke();
                onStageEnd = null;
            }

            /*if (NextState != null)
            {
                DemoShowStateMachine.Instance.GotoState(NextState);
                //NextState = null;
            }*/
        });
    }

    public void exit(Action callback = null)
    {
        LeanTween.cancel(DemoShowStateMachine.Instance.gameObject);
        //onStageEnd = null;
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
