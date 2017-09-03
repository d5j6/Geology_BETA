public class ProfessorButton : RingedAndTintedButton
{
    /*
     * Кнопка профессора имеет следующую логику - она становится неактивной, когда идет демо-режим, показано MoreInfo или когда Земля меняется и активной во всех остальных случаях
     * Когда лекция профессора запущена она выделена, иначе - не выделена
     */

    private void Srart()
    {
        ProfessorsLecturesController.Instance.LectureStarted += onLectureStarted;
        ProfessorsLecturesController.Instance.LectureEnded += onLectureEnded;

        DemoShowStateMachine.Instance.DemoShowStartedPlaying += onDemoShowStartedPlaying;
        DemoShowStateMachine.Instance.DemoShowEndedPlaying += onDemoShowEndedPlaying;

        MoreInfoController.Instance.MoreInfoShowed += onMoreInfoShowed;
        MoreInfoController.Instance.MoreInfoHided += onMoreInfoHided;

        EarthController.Instance.ChangesStarted += onEarthChangeStarted;
        EarthController.Instance.ChangesEnded += onEarthChangesEnded;
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

        if (EarthController.Instance != null)
        {
            EarthController.Instance.ChangesStarted -= onEarthChangeStarted;
            EarthController.Instance.ChangesEnded -= onEarthChangesEnded;
        }
    }

    private bool lecturing = false;

    protected override void singleTapAction()
    {
        base.singleTapAction();

        ProfessorsLecturesController.Instance.ToggleLecturing();
    }

    private void onLectureStarted()
    {
        reactingOnEarth = false;
        MyRings.Show();
    }

    private void onLectureEnded()
    {
        reactingOnEarth = true;
        MyRings.Hide();
    }

    private void onDemoShowStartedPlaying()
    {
        reactingOnEarth = false;
        disable();
    }

    private void onDemoShowEndedPlaying()
    {
        reactingOnEarth = true;
        enable();
    }

    private void onMoreInfoShowed()
    {
        reactingOnEarth = false;
        disable();
    }

    private void onMoreInfoHided()
    {
        reactingOnEarth = true;
        enable();
    }

    //На изменения Земли реагируем только если мы не в демо-режиме и не внутри лекции, ибо в этих режимах часто идет работа с Землей
    private bool reactingOnEarth = true;

    private void onEarthChangeStarted()
    {
        if (reactingOnEarth)
        {
            enabledByFunctionality = false;
            MyTinter.Tint();
        }
    }

    private void onEarthChangesEnded()
    {
        if (reactingOnEarth)
        {
            enabledByFunctionality = true;
            MyTinter.Untint();
        }
    }
}
