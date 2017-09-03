using System;
using System.Collections.Generic;

public class SlicesState : IDemoShowState
{
    public DemoShowStuff demoShowStuff;

    System.Action onStageEnd;
    System.Action onInterruptionEnd;

    Dictionary<Language, Dictionary<string, float>> timings;

    public IDemoShowState NextState
    {
        get;
        set;
    }

    public void enter(Action callback = null)
    {
        if (timings == null)
        {
            InitializeTimings();
        }
        onStageEnd = callback;
        goToSlicedView();
    }

    void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("goToSlicedView_1", 14f);
        timings[Language.Russian].Add("startTalkingAboutCore_1", 5f);
        timings[Language.Russian].Add("startTalkingAboutCore_2", 15f);
        timings[Language.Russian].Add("startTalkingAboutCore_3", 23.5f);
        timings[Language.Russian].Add("getInterestingFactAboutCore_1", 1f);
        timings[Language.Russian].Add("getInterestingFactAboutCore_2", 12.5f);
        timings[Language.Russian].Add("getInterestingFactAboutCore_3", 0.7f);
        timings[Language.Russian].Add("startTalkingAboutSlices_1", 5f);
        timings[Language.Russian].Add("startTalkingAboutSlices_2", 15f);
        timings[Language.Russian].Add("startTalkingAboutSlices_3", 10f);
        timings[Language.Russian].Add("startTalkingAboutSlices_4", 16f);
        timings[Language.Russian].Add("startTalkingAboutSlices_5", 64.15f);
        timings[Language.Russian].Add("getInterestingFactAboutCrust_1", 1f);
        timings[Language.Russian].Add("getInterestingFactAboutCrust_2", 15.5f);
        timings[Language.Russian].Add("getInterestingFactAboutCrust_3", 14f);
        timings[Language.Russian].Add("getInterestingFactAboutCrust_4", 0.7f);
        timings[Language.Russian].Add("endStage_1", 12.2f);
        timings[Language.Russian].Add("endStage_2", 0.8f);

        timings[Language.English].Add("goToSlicedView_1", 14f);
        timings[Language.English].Add("startTalkingAboutCore_1", 5f);
        timings[Language.English].Add("startTalkingAboutCore_2", 15f);
        timings[Language.English].Add("startTalkingAboutCore_3", 27.5f);
        timings[Language.English].Add("getInterestingFactAboutCore_1", 0.1f);
        timings[Language.English].Add("getInterestingFactAboutCore_2", 10.0f);
        timings[Language.English].Add("getInterestingFactAboutCore_3", 0.7f);
        timings[Language.English].Add("startTalkingAboutSlices_1", 5f);
        timings[Language.English].Add("startTalkingAboutSlices_2", 15f);
        timings[Language.English].Add("startTalkingAboutSlices_3", 16f);
        timings[Language.English].Add("startTalkingAboutSlices_4", 24f);
        timings[Language.English].Add("startTalkingAboutSlices_5", 76f);
        timings[Language.English].Add("getInterestingFactAboutCrust_1", 1f);
        timings[Language.English].Add("getInterestingFactAboutCrust_2", 10.5f);
        timings[Language.English].Add("getInterestingFactAboutCrust_3", 9.2f);
        timings[Language.English].Add("getInterestingFactAboutCrust_4", 0.1f);
        timings[Language.English].Add("endStage_1", 14.2f);
        timings[Language.English].Add("endStage_2", 0.8f);
    }

    void goToSlicedView()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_Core").gameObject);
        SceneStateMachine.Instance.GoToSlicedEarthState();

        /*LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, 13f, () =>
        {
            SceneStateMachineFacade.Instance.GotoSlices();
        });*/
        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["goToSlicedView_1"], () =>
        {
            //AudioSourceController.Instance.StopPlaying();

            startTalkingAboutCore();
        });
    }

    void startTalkingAboutCore()
    {
        EarthController.Instance.InnerCore().Whole().Go();
        //AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("415").gameObject);

        LeanTween.delayedCall(timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutCore_1"], () =>
        {
            EarthController.Instance.Extend().Go();
            LeanTween.delayedCall(timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutCore_2"], () =>
            {
                EarthController.Instance.Join().Go();
            });
        });

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutCore_3"], () =>
        {
            AudioSourceController.Instance.StopPlaying();

            getInterestingFactAboutCore();
        });
    }

    void getInterestingFactAboutCore()
    {
        demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().ShowByScalingAndStartTalking(() =>
        {
            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutCore_1"], () =>
            {
                AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_ByTheWayCore").gameObject);
            });

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutCore_2"], () =>
            {
                AudioSourceController.Instance.StopPlaying();
                demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().StopTalkingAndScaleToZero();
                LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutCore_3"], () =>
                {
                    startTalkingAboutSlices();
                });
            });
        });
    }

    void startTalkingAboutSlices()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_MantleAndCrust").gameObject);

        EarthController.Instance.LowerMantle().Go();
        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutSlices_1"], () =>
        {
            EarthController.Instance.Extend().Go();
            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutSlices_2"], () =>
            {
                EarthController.Instance.Join().Go();
                LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutSlices_3"], () =>
                {
                    EarthController.Instance.UpperMantle().Go();
                    LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutSlices_4"], () =>
                    {
                        EarthController.Instance.Crust().Go();
                    });
                });
            });
        });

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutSlices_5"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            getInterestingFactAboutCrust();
        });
    }

    void getInterestingFactAboutCrust()
    {
        demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().ShowByScalingAndStartTalking(() =>
        {
            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutCrust_1"], () =>
            {
                AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_ByTheWayCrust").gameObject);
            });

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutCrust_2"], () =>
            {
                EarthController.Instance.Surface().Go();
            });

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutCrust_3"], () =>
            {
                AudioSourceController.Instance.StopPlaying();
                demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().StopTalkingAndScaleToZero();

                LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutCrust_4"], () =>
                {
                    endStage();
                });
            });
        });
    }

    void endStage()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_PreparePie").gameObject);

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["endStage_1"], () =>
        {
            AudioSourceController.Instance.StopPlaying();

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["endStage_2"], () =>
            {
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
