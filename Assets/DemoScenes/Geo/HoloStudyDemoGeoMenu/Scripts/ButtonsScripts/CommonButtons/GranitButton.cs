public class GranitButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        PieController.Instance.GranitShowed += onShowed;
        PieController.Instance.GranitHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (PieController.Instance != null)
        {
            PieController.Instance.GranitShowed -= onShowed;
            PieController.Instance.GranitHided -= onHided;
        }
    }

    private void onShowed()
    {
        MyRings.Show();
    }

    private void onHided()
    {
        MyRings.Hide();
    }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        PieController.Instance.ShowGranitLayer();
    }
}
