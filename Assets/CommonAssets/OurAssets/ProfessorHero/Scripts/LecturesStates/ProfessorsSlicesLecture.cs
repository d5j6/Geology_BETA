using UnityEngine;
using System.Collections.Generic;
using System;

public class ProfessorsSlicesLecture : IProfessorLectureState
{
    private Dictionary<Language, Dictionary<string, float>> timings;

    private Action stageEnd;

    private void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("ProfessorsSlicesLecture_1", 10f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_12", 4f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_2", 12f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_3", 10f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_4", 59f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_5", 10f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_6", 20f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_7", 30f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_71", 54f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_72", 63f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_8", 88f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_9", 6f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_10", 18f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_101", 26f);
        timings[Language.Russian].Add("ProfessorsSlicesLecture_11", 55f);

        timings[Language.English].Add("ProfessorsSlicesLecture_1", 13f);
        timings[Language.English].Add("ProfessorsSlicesLecture_12", 4f);
        timings[Language.English].Add("ProfessorsSlicesLecture_2", 12f);
        timings[Language.English].Add("ProfessorsSlicesLecture_3", 10f);
        timings[Language.English].Add("ProfessorsSlicesLecture_4", 44f);
        timings[Language.English].Add("ProfessorsSlicesLecture_5", 10f);
        timings[Language.English].Add("ProfessorsSlicesLecture_6", 20f);
        timings[Language.English].Add("ProfessorsSlicesLecture_7", 28f);
        timings[Language.English].Add("ProfessorsSlicesLecture_71", 54f);
        timings[Language.English].Add("ProfessorsSlicesLecture_72", 63f);
        timings[Language.English].Add("ProfessorsSlicesLecture_8", 74f);
        timings[Language.English].Add("ProfessorsSlicesLecture_9", 6f);
        timings[Language.English].Add("ProfessorsSlicesLecture_10", 18f);
        timings[Language.English].Add("ProfessorsSlicesLecture_101", 26f);
        timings[Language.English].Add("ProfessorsSlicesLecture_11", 56f);
    }

    public void enter(Action callback = null)
    {
        stageEnd = callback;

        if (timings == null)
        {
            InitializeTimings();
        }

        ProfessorOnPlatform.Instance.ShowByFlyingInAndStartTalking(() =>
        {
            goToSlicedView();
        });
    }

    private void goToSlicedView()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_CoreLecture").gameObject);
        SceneStateMachine.Instance.GoToSlicedEarthState();

        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_1"], () =>
        {
            startTalkingAboutCore();
        });
    }

    private void startTalkingAboutCore()
    {
        EarthController.Instance.OuterCore().Whole().Go();

        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_12"], () =>
        {
            EarthController.Instance.Extend().Go();
        });

        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_2"], () =>
        {
            EarthController.Instance.InnerCore().Go();
            LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_3"], () =>
            {
                EarthController.Instance.Join().Go();
            });
        });

        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_4"], () =>
        {
            AudioSourceController.Instance.StopPlaying();

            startTalkingAboutMantle();
        });
    }

    private void startTalkingAboutMantle()
    {
        Debug.Log("startTalkingAboutMantle");
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_MantleLecture").gameObject);

        EarthController.Instance.UpperMantle().Go();
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_5"], () =>
        {
            EarthController.Instance.Extend().Go();
            /*LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_6"], () =>
            {
                SceneStateMachine.Instance.ToggleSettingState();
            });*/
        });
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_7"], () =>
        {
            EarthController.Instance.LowerMantle().Go();
        });
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_71"], () =>
        {
            EarthController.Instance.Join().Go();
        });
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_72"], () =>
        {
            EarthController.Instance.OuterCore().Go();
        });
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_8"], () =>
        {
            AudioSourceController.Instance.StopPlaying();
            startTalkingAboutCrust();
        });
    }

    private void startTalkingAboutCrust()
    {
        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_CrustLecture").gameObject);

        EarthController.Instance.Crust().Go();
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_9"], () =>
        {
            EarthController.Instance.Extend().Go();
            LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_10"], () =>
            {
                EarthController.Instance.Join().Go();
            });
        });

        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_101"], () =>
        {
            EarthController.Instance.Slice().Go();
        });

        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsSlicesLecture_11"], () =>
        {
            AudioSourceController.Instance.StopPlaying(() =>
            {
                ProfessorOnPlatform.Instance.StopTalkingAndScaleToZero(() =>
                {
                    if (stageEnd != null)
                    {
                        stageEnd.Invoke();
                        stageEnd = null;
                    }
                });
            });
        });
    }

    public void exit(Action callback = null)
    {
        LeanTween.cancel(ProfessorsLecturesController.Instance.gameObject);
        AudioSourceController.Instance.StopPlaying();
        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public bool tryInterrupt(Action callback = null)
    {
        LeanTween.cancel(ProfessorsLecturesController.Instance.gameObject);

        ProfessorOnPlatform.Instance.StopTalkingAndScaleToZero();

        AudioSourceController.Instance.StopPlaying(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }

            if (stageEnd != null)
            {
                stageEnd.Invoke();
                stageEnd = null;
            }
        });

        return true;
    }
}
