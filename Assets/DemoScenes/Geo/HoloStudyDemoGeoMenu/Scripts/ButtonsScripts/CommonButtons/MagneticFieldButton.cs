public class MagneticFieldButton : CommonRingedAndTintedButton
{
    private void Start()
    {
        EarthController.Instance.MagneticFieldMeasuringStarted += onShowed;
        EarthController.Instance.MagneticFieldMeasuringEnded += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.MagneticFieldMeasuringStarted -= onShowed;
            EarthController.Instance.MagneticFieldMeasuringEnded -= onHided;
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

        EarthController.Instance.MeasureMagneticField();
    }
}
