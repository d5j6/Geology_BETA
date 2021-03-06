﻿public class MassButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.MassMeasuringStarted += onShowed;
        EarthController.Instance.MassMeasuringEnded += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.MassMeasuringStarted -= onShowed;
            EarthController.Instance.MassMeasuringEnded -= onHided;
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

        EarthController.Instance.MeasureMass();
    }
}
