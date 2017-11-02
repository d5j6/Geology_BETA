using UnityEngine;

public class CommonRingedAndTintedButton : RingedAndTintedButton
{

    /*
     * Логика этой кнопки такая - если активны режимы:
     * демо, лекция, мореинфо
     * то кнопка неактивна,
     * иначе кнопка активна, если нет переходов между состояниями
     * иначе кнопка неактивна
     */

    protected override void Awake()
    {
        base.Awake();

        ProfessorsLecturesController.Instance.LectureStarted += onLectureStarted;
        ProfessorsLecturesController.Instance.LectureEnded += onLectureEnded;

        DemoShowStateMachine.Instance.DemoShowStartedPlaying += onDemoShowStartedPlaying;
        DemoShowStateMachine.Instance.DemoShowEndedPlaying += onDemoShowEndedPlaying;

        MoreInfoController.Instance.MoreInfoShowed += onMoreInfoShowed;
        MoreInfoController.Instance.MoreInfoHided += onMoreInfoHided;

        SceneStateMachine.Instance.StateChangeStarted += onStateChangeStarted;
        SceneStateMachine.Instance.StateChangeEnded += onStateChangeEnded;

        EarthController.Instance.ChangesStarted += onEarthChangesStarted;
        EarthController.Instance.ChangesEnded += onEarthChangesEnded;

        EarthController.Instance.MeasuringStarted += onEarthMeasuringStarted;
        EarthController.Instance.MeasuringEnded += onEarthMeasuringEnded;

        PieController.Instance.ChangesStarted += onPieChangesStarted;
        PieController.Instance.ChangesEnded += onPieChangesEnded;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (ProfessorsLecturesController.Instance != null)
        {
            ProfessorsLecturesController.Instance.LectureStarted -= onLectureStarted;
            ProfessorsLecturesController.Instance.LectureEnded -= onLectureEnded;
        }

        if (DemoShowStateMachine.Instance != null)
        {
            DemoShowStateMachine.Instance.DemoShowStartedPlaying -= onDemoShowStartedPlaying;
            DemoShowStateMachine.Instance.DemoShowEndedPlaying -= onDemoShowEndedPlaying;
        }

        if (MoreInfoController.Instance != null)
        {
            MoreInfoController.Instance.MoreInfoShowed -= onMoreInfoShowed;
            MoreInfoController.Instance.MoreInfoHided -= onMoreInfoHided;
        }

        if (SceneStateMachine.Instance != null)
        {
            SceneStateMachine.Instance.StateChangeStarted -= onStateChangeStarted;
            SceneStateMachine.Instance.StateChangeEnded -= onStateChangeEnded;
        }

        if (EarthController.Instance != null)
        {
            EarthController.Instance.ChangesStarted -= onEarthChangesStarted;
            EarthController.Instance.ChangesEnded -= onEarthChangesEnded;
            EarthController.Instance.MeasuringStarted -= onEarthMeasuringStarted;
            EarthController.Instance.MeasuringEnded -= onEarthMeasuringEnded;
        }

        if (PieController.Instance != null)
        {
            PieController.Instance.ChangesStarted -= onPieChangesStarted;
            PieController.Instance.ChangesEnded -= onPieChangesEnded;
        }
    }

    private void onLectureStarted()
    {
        reactingOnOthers = true;
        disable();
    }

    private void onLectureEnded()
    {
        reactingOnOthers = false;
        enable();
    }

    private void onDemoShowStartedPlaying()
    {
        reactingOnOthers = true;
        disable();
    }

    private void onDemoShowEndedPlaying()
    {
        reactingOnOthers = false;
        enable();
    }

    private void onMoreInfoShowed()
    {
        reactingOnOthers = true;
        disable();
    }

    private void onMoreInfoHided()
    {
        reactingOnOthers = false;
        enable();
    }

    //Меньшие раздражители

    private bool reactingOnOthers = false;

    private void onStateChangeStarted()
    {
        if (!reactingOnOthers)
        {
            disable();
        }
    }

    private void onStateChangeEnded()
    {
        if (!reactingOnOthers)
        {
            enable();
        }
    }

    protected virtual void onEarthChangesStarted()
    {
        if (!reactingOnOthers)
        {
            disable();
        }
    }

    protected virtual void onEarthChangesEnded()
    {
        if (!reactingOnOthers)
        {
            enable();
        }
    }

    protected virtual void onPieChangesStarted()
    {
        if (!reactingOnOthers)
        {
            disable();
        }
    }

    protected virtual void onPieChangesEnded()
    {
        if (!reactingOnOthers)
        {
            enable();
        }
    }

    private void onEarthMeasuringStarted()
    {
        if (!reactingOnOthers)
        {
            disable();
        }
    }

    private void onEarthMeasuringEnded()
    {
        if (!reactingOnOthers)
        {
            enable();
        }
    }
}
