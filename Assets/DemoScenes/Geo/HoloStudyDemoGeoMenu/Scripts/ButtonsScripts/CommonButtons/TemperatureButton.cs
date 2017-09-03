public class TemperatureButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.TemperatureMeasuringStarted += onShowed;
        EarthController.Instance.TemperatureMeasuringEnded += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.TemperatureMeasuringStarted -= onShowed;
            EarthController.Instance.TemperatureMeasuringEnded -= onHided;
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

        EarthController.Instance.MeasureTemperature();
    }
}
