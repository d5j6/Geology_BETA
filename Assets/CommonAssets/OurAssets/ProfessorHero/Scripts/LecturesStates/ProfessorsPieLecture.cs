using System.Collections.Generic;
using System;

public class ProfessorsPieLecture : IProfessorLectureState
{
    private Dictionary<Language, Dictionary<string, float>> timings;

    private Action stageEnd;

    private void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("ProfessorsPieLecture_1", 1f);
        timings[Language.Russian].Add("ProfessorsPieLecture_2", 84f);
        timings[Language.Russian].Add("ProfessorsPieLecture_3", 49f);
        timings[Language.Russian].Add("ProfessorsPieLecture_4", 48f);
        timings[Language.Russian].Add("ProfessorsPieLecture_5", 47f);

        timings[Language.English].Add("ProfessorsPieLecture_1", 1f);
        timings[Language.English].Add("ProfessorsPieLecture_2", 71f);
        timings[Language.English].Add("ProfessorsPieLecture_3", 47f);
        timings[Language.English].Add("ProfessorsPieLecture_4", 32f);
        timings[Language.English].Add("ProfessorsPieLecture_5", 45f);
    }

    public void enter(Action callback = null)
    {
        stageEnd = callback;

        if (timings == null)
        {
            InitializeTimings();
        }

        SceneStateMachine.Instance.GoToPieState();
        ProfessorOnPlatform.Instance.ShowByFlyingInAndStartTalking(() =>
        {
            gotoPie();
        });
    }


    private void gotoPie()
    {
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsPieLecture_1"], () =>
        {
            AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_CrustModelLecture").gameObject);

            LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsPieLecture_2"], () =>
            {
                AudioSourceController.Instance.StopPlaying(() =>
                {
                    GotoSedimentary();
                });
            });
        });
    }

    private void GotoSedimentary()
    {
        PieController.Instance.ShowSedimentaryLayer();

        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_SedimentaryLayerLecture").gameObject);
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsPieLecture_3"], () =>
        {
            AudioSourceController.Instance.StopPlaying(() =>
            {
                GotoGranit();
            });
        });
    }

    private void GotoGranit()
    {
        PieController.Instance.ShowGranitLayer();

        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_GranitLayerLecture").gameObject);
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsPieLecture_4"], () =>
        {
            AudioSourceController.Instance.StopPlaying(() =>
            {
                GotoBasalt();
            });
        });
    }

    private void GotoBasalt()
    {
        PieController.Instance.ShowBasaltLayer();

        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_BasaltLayerLecture").gameObject);
        LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsPieLecture_5"], () =>
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
