using System.Collections.Generic;
using System;

public class PieDemoState : IDemoShowState
{
    public DemoShowStuff demoShowStuff;

    public Action StageEnd;

    private Dictionary<Language, Dictionary<string, float>> timings;

    public IDemoShowState NextState
    {
        get;

        set;
    }

    private void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("gotoPie_1", 3.5f);
        timings[Language.Russian].Add("gotoPie_2", 34f);
        timings[Language.Russian].Add("gotoPie_3", 50.65f);
        timings[Language.Russian].Add("getInterestingFactAboutSandstone_1", 0.2f);
        timings[Language.Russian].Add("getInterestingFactAboutSandstone_2", 10f);
        timings[Language.Russian].Add("getInterestingFactAboutSandstone_3", 0.75f);
        timings[Language.Russian].Add("startTalkingAboutGranit_1", 13.0f);
        timings[Language.Russian].Add("startTalkingAboutBasalt_1", 20f); 
         timings[Language.Russian].Add("goodbye_1", 9f);

        timings[Language.English].Add("gotoPie_1", 3.5f);
        timings[Language.English].Add("gotoPie_2", 41f);
        timings[Language.English].Add("gotoPie_3", 53f);
        timings[Language.English].Add("getInterestingFactAboutSandstone_1", 0.2f);
        timings[Language.English].Add("getInterestingFactAboutSandstone_2", 7.5f);
        timings[Language.English].Add("getInterestingFactAboutSandstone_3", 0.35f);
        timings[Language.English].Add("startTalkingAboutGranit_1", 13.0f);
        timings[Language.English].Add("startTalkingAboutBasalt_1", 20f);
        timings[Language.English].Add("goodbye_1", 9f);
    }

    public void enter(Action callback = null)
    {
        if (timings == null)
        {
            InitializeTimings();
        }
        StageEnd = callback;
        gotoPie();
    }


    private void gotoPie()
    {
        SceneStateMachine.Instance.GoToPieState();

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["gotoPie_1"], () =>
        {
            AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_ContinentalOceanicSedimentary").gameObject);

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["gotoPie_2"], () =>
            {
                PieController.Instance.ShowSedimentaryLayer();
            });

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["gotoPie_3"], () =>
            {
                getInterestingFactAboutSandstone();
            });
        });

        /*AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("51").gameObject);

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, 34f, () =>
        {
            RocksRadioButtonGroupController.Instance.TryToClick(0);
        });

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, 54f, () =>
        {
            getInterestingFactAboutSandstone();
        });*/
    }

    private void getInterestingFactAboutSandstone()
    {
        demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().ShowByScalingAndStartTalking(() =>
        {
            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutSandstone_1"], () =>
            {
                AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_ByTheWaySandstone").gameObject);
            });

            LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutSandstone_2"], () =>
            {
                AudioSourceController.Instance.StopPlaying();
                demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().StopTalkingAndScaleToZero();
                LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["getInterestingFactAboutSandstone_3"], () =>
                {
                    startTalkingAboutGranit();
                });
            });
        });
    }

    private void startTalkingAboutGranit()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_Granit").gameObject);
        PieController.Instance.ShowGranitLayer();

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutGranit_1"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            startTalkingAboutBasalt();
        });
    }

    private void startTalkingAboutBasalt()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_Basalt").gameObject);
        PieController.Instance.ShowBasaltLayer();

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["startTalkingAboutBasalt_1"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            goodbye();
        });
    }

    private void goodbye()
    {
        SceneStateMachine.Instance.GoToEarthState();
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_Goodbye").gameObject);

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["goodbye_1"], () =>
        {
            if (StageEnd != null)
            {
                StageEnd.Invoke();
                StageEnd = null;
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
