public class SedimentaryButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        PieController.Instance.SedimentaryShowed += onShowed;
        PieController.Instance.SedimentaryHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (PieController.Instance != null)
        {
            PieController.Instance.SedimentaryShowed -= onShowed;
            PieController.Instance.SedimentaryHided -= onHided;
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

        PieController.Instance.ShowSedimentaryLayer();
    }
}
