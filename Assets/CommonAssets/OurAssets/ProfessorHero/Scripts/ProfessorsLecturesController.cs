using UnityEngine;
using HoloToolkit.Unity;
using System;

public class ProfessorsLecturesController : Singleton<ProfessorsLecturesController>
{
    public Action LectureStarted;
    public Action LectureEnded;

    public bool LecturePlaying = false;
    private IProfessorLectureState lecture;
    private ProfessorsEarthLecture professorsEarthLecture;
    private ProfessorsSlicesLecture professorsSlicesLecture;
    private ProfessorsPieLecture professorsPieLecture;

    private bool _enabled = false;

    private void Awake()
    {
        initLectures();
    }

    private void initLectures()
    {
        professorsEarthLecture = new ProfessorsEarthLecture();
        professorsSlicesLecture = new ProfessorsSlicesLecture();
        professorsPieLecture = new ProfessorsPieLecture();
    }

    private void GiveMeLecture()
    {
        if ((!DemoShowStateMachine.playing) && (!LecturePlaying))
        {
            if (SceneStateMachine.Instance.IsEarthState())
            {
                GotoLecture(professorsEarthLecture);
            }
            else if (SceneStateMachine.Instance.IsPieState())
            {
                GotoLecture(professorsPieLecture);
            }
            else
            {
                GotoLecture(professorsSlicesLecture);
            }
        }
    }

    private void StopLecturing(Action callback = null)
    {
        if (lecture.tryInterrupt(callback))
        {
            lecture = null;
            LecturePlaying = false;
        }
    }

    public void ToggleLecturing()
    {
        if (LecturePlaying)
        {
            StopLecturing();
        }
        else
        {
            GiveMeLecture();
        }
    }

    private void GotoLecture(IProfessorLectureState newLecture)
    {
        LecturePlaying = true;

        if (LectureStarted != null)
        {
            LectureStarted.Invoke();
        }

        if (lecture != null)
        {
            lecture.exit(() =>
            {
                lecture = newLecture;
                lecture.enter(() =>
                {
                    onLectureEnded();
                });
            });
        }
        else
        {
            lecture = newLecture;
            lecture.enter(() =>
            {
                onLectureEnded();
            });
        }
    }

    private void onLectureEnded()
    {
        if (LectureEnded != null)
        {
            LectureEnded.Invoke();
        }
    }
}
