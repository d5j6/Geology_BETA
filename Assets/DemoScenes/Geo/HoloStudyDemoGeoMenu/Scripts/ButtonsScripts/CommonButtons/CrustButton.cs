public class CrustButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.CrustShowed += onShowed;
        EarthController.Instance.CrustHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.CrustShowed -= onShowed;
            EarthController.Instance.CrustHided -= onHided;
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

        EarthController.Instance.Crust().Whole().Go();
    }
}
