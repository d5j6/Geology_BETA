using System.Collections.Generic;
using System;

public class ProfessorsEarthLecture : IProfessorLectureState
{
    private Dictionary<Language, Dictionary<string, float>> timings;

    private Action stageEnd;

    private void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("ProfessorsEarthLecture_1", 99f);
        timings[Language.Russian].Add("ProfessorsEarthLecture_2", 52f);
        timings[Language.Russian].Add("ProfessorsEarthLecture_3", 54f);
        timings[Language.Russian].Add("ProfessorsEarthLecture_4", 50f);

        timings[Language.English].Add("ProfessorsEarthLecture_1", 92f);
        timings[Language.English].Add("ProfessorsEarthLecture_2", 43f);
        timings[Language.English].Add("ProfessorsEarthLecture_3", 42f);
        timings[Language.English].Add("ProfessorsEarthLecture_4", 43f);
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
            AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_EarthLecture").gameObject);
            BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Earth);

            LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsEarthLecture_1"], () =>
            {
                AudioSourceController.Instance.StopPlaying(() =>
                {
                    SceneStateMachine.Instance.GoToMagneticFieldMeasuringState();
                    //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.MagneticField), DataController.Instance.GetDescriptionFor(InformationsMockup.MagneticField));
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.MagneticField);
                    AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_MagneticFieldLecture").gameObject);

                    LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsEarthLecture_2"], () =>
                    {
                        AudioSourceController.Instance.StopPlaying(() =>
                        {
                            //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.Temperature), DataController.Instance.GetDescriptionFor(InformationsMockup.Temperature));
                            BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Temperature);
                            AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_TemperatureLecture").gameObject);
                            SceneStateMachine.Instance.GoToTemperatureState();

                            LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsEarthLecture_3"], () =>
                            {
                                AudioSourceController.Instance.StopPlaying(() =>
                                {
                                    //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.Mass), DataController.Instance.GetDescriptionFor(InformationsMockup.Mass));
                                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Mass);
                                    AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Professor_MassLecture").gameObject);
                                    SceneStateMachine.Instance.GoToMassMeasuringState();
                                    LeanTween.delayedCall(ProfessorsLecturesController.Instance.gameObject, timings[LanguageManager.Instance.CurrentLanguage]["ProfessorsEarthLecture_4"], () =>
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
                                });
                            });
                        });
                    });
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
        if ((MassMeasurer.Instance != null && MassMeasurer.Instance.CurrentlyMeasuring) || (MagneticFieldMeasurer.Instance != null && MagneticFieldMeasurer.Instance.CurrentlyMeasuring) || (TemperatureMeasurer.Instance != null && TemperatureMeasurer.Instance.CurrentlyMeasuring))
        {
            if (callback != null)
            {
                callback.Invoke();
            }

            return false;
        }
        else
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
}
