public class MoreInfoButton : RingedAndTintedButton
{
    /*
     * Кнопка MoreInfo имеет следующую логику - она становится неактивной, когда профессор читает лекцию, идет демо-режим или показывается пирожок
     * Когда показывается MoreInfo, она становися выделенной, все остальное время - нет
     */

    protected override void singleTapAction()
    {
        base.singleTapAction();

        if (MoreInfoController.Instance != null)
        {
            MoreInfoController.Instance.ToggleMoreInfo();
        }
    }

    private void Start()
    {
        ProfessorsLecturesController.Instance.LectureStarted += onLectureStarted;
        ProfessorsLecturesController.Instance.LectureEnded += onLectureEnded;

        DemoShowStateMachine.Instance.DemoShowStartedPlaying += onDemoShowStartedPlaying;
        DemoShowStateMachine.Instance.DemoShowEndedPlaying += onDemoShowEndedPlaying;

        MoreInfoController.Instance.MoreInfoShowed += onMoreInfoShowed;
        MoreInfoController.Instance.MoreInfoHided += onMoreInfoHided;

        PiePolygon.Instance.Showed += onPieShowed;
        PiePolygon.Instance.Hided += onPieHided;
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

        if (PiePolygon.Instance != null)
        {
            PiePolygon.Instance.Showed -= onPieShowed;
            PiePolygon.Instance.Hided -= onPieHided;
        }
    }

    private void onDemoShowStartedPlaying()
    {
        disable();
    }

    private void onDemoShowEndedPlaying()
    {
        enable();
    }

    private void onLectureStarted()
    {
        disable();
    }

    private void onLectureEnded()
    {
        enable();
    }

    private void onMoreInfoShowed()
    {
        MyRings.Show();
    }

    private void onMoreInfoHided()
    {
        MyRings.Hide();
    }

    private void onPieShowed()
    {
        disable();
    }

    private void onPieHided()
    {
        enable();
    }
}
