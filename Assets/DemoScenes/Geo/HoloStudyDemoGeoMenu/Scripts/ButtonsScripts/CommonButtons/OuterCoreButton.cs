public class OuterCoreButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.OuterCoreShowed += onShowed;
        EarthController.Instance.OuterCoreHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.OuterCoreShowed -= onShowed;
            EarthController.Instance.OuterCoreHided -= onHided;
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

        EarthController.Instance.OuterCore().Whole().Go();
    }
}
