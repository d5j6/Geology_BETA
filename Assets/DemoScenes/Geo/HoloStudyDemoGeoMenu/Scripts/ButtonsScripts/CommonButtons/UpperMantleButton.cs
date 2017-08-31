public class UpperMantleButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.UpperMantleShowed += onShowed;
        EarthController.Instance.UpperMantleHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.UpperMantleShowed -= onShowed;
            EarthController.Instance.UpperMantleHided -= onHided;
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

        EarthController.Instance.UpperMantle().Whole().Go();
    }
}
