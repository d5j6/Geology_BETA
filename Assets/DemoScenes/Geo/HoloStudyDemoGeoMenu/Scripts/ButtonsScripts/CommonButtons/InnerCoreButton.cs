public class InnerCoreButton : CommonRingedAndTintedButton
{
    private void Start()
    {
        EarthController.Instance.InnerCoreShowed += onShowed;
        EarthController.Instance.InnerCoreHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.InnerCoreShowed -= onShowed;
            EarthController.Instance.InnerCoreHided -= onHided;
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

        EarthController.Instance.InnerCore().Whole().Go();
    }
}
