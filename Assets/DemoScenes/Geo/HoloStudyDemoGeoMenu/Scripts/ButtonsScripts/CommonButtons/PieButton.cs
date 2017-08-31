public class PieButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        PiePolygon.Instance.Showed += onShowed;
        PiePolygon.Instance.Hided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (PiePolygon.Instance != null)
        {
            PiePolygon.Instance.Showed -= onShowed;
            PiePolygon.Instance.Hided -= onHided;
        }
    }

    private void onShowed()
    {
        GetComponent<SubmenuOpener>().Show();
        MyRings.Show();
    }

    private void onHided()
    {
        GetComponent<SubmenuOpener>().Hide();
        MyRings.Hide();
    }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        SceneStateMachine.Instance.GoToPieState();
    }
}
