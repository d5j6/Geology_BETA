using UnityEngine;
using System.Collections.Generic;

public class GreetingsAndProfessorState : IDemoShowState {

    public DemoShowStuff demoShowStuff;

    System.Action onStageEnd;

    Dictionary<Language, Dictionary<string, float>> timings;

    void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("womanVoice_1", 13f);
        timings[Language.Russian].Add("professorVoice_0", 3.6f);
        timings[Language.Russian].Add("professorVoice_1", 16.6f);
        timings[Language.Russian].Add("professorVoice_2", 1f);
        timings[Language.Russian].Add("professorVoice_3", 18f);

        timings[Language.English].Add("womanVoice_1", 12.3f);
        timings[Language.English].Add("professorVoice_0", 5f);
        timings[Language.English].Add("professorVoice_1", 18.5f);
        timings[Language.English].Add("professorVoice_2", 1f);
        timings[Language.English].Add("professorVoice_3", 19f);
    }

    public IDemoShowState NextState
    {
        get;

        set;
    }

    public void enter(System.Action callback = null)
    {
        onStageEnd = callback;
        if (timings == null)
        {
            InitializeTimings();
        }
        SceneStateMachine.Instance.goToStartState(womanVoice);
        //womanVoice();
    }

    void womanVoice()
    {
        AudioSourceController.Instance.FollowGameObject(Camera.main.gameObject);
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_IntroduceProfessor").gameObject);
        if (!SceneStateMachine.Instance.IsEarthState())
        {
            SceneStateMachine.Instance.GoToDefaultState();
        }
        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["womanVoice_1"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            professorVoice();
        });
    }

    void professorVoice()
    {
        GameObject StandsForHERO = new GameObject();
        Loader.Instance.LoadAndIntsntiateGOPrefab("StandsForHERO", (GameObject go) =>
        {
            Object.Destroy(StandsForHERO);
            StandsForHERO = go;
            Vector3 pos = ProfessorOnPlatform.Instance.transform.position;
            pos.y += 0.478f;
            StandsForHERO.transform.position = pos;
            StandsForHERO.transform.localScale = Vector3.one * 0.15f;
        });

        LeanTween.delayedCall(timings[LanguageManager.Instance.CurrentLanguage]["professorVoice_0"], () =>
        {
            StandsForHERO.GetComponent<StandsForHEROController>().ShowOnProfessor(new float[5] { 0f, 0.6f, 1.2f, 1.8f, 2.4f }, () =>
            {
                Object.Destroy(StandsForHERO);
            });
        });

        //demoShowStuff.Professor.transform.position = ;
        AudioSourceController.Instance.FollowGameObject(demoShowStuff.Professor);
        //Debug.Log("SlicedEarthPolygon.Instance.transform.localPosition = " + (SlicedEarthPolygon.Instance.transform.localPosition));
        //Debug.Log("Positioning profeesor at " + (SlicedEarthPolygon.Instance.transform.localPosition + new Vector3(-0.479f, -0.176f, 1.4999f)));ShowByScalingAndStartTalking
        //demoShowStuff.Professor.transform.localPosition = SlicedEarthPolygon.Instance.transform.localPosition + new Vector3(-0.42315884f, -0.15854688f, -0.500144f);
        demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().ShowByFlyingInAndStartTalking(() =>
        {
            LeanTween.delayedCall(timings[LanguageManager.Instance.CurrentLanguage]["professorVoice_1"], () =>
            {
                demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().StopTalkingAndScaleToZero();
            });
        });
        //demoShowStuff.Professor.GetComponent<ProfessorOnPlatform>().ShowIntroWithBubbles(SlicedEarthPolygon.Instance.transform.localPosition + new Vector3(-0.42315884f, -0.15854688f, -0.500144f));

        LeanTween.delayedCall(timings[LanguageManager.Instance.CurrentLanguage]["professorVoice_2"], () =>
        {
            AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_Greetings").gameObject);
        });

        LeanTween.delayedCall(DemoShowStateMachine.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["professorVoice_3"], () =>
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
    }

    public void exit(System.Action callback = null)
    {
        if (LeanTween.isTweening(DemoShowStateMachine.Instance.gameObject))
        {
            LeanTween.cancel(DemoShowStateMachine.Instance.gameObject);
        }
        AudioSourceController.Instance.StopPlaying();
        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public void interrupt(System.Action callback = null)
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
