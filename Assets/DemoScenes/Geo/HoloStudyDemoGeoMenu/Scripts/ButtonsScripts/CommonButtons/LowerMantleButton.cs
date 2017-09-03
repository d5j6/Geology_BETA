using UnityEngine;

public class LowerMantleButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.LowerMantleShowed += onShowed;
        EarthController.Instance.LowerMantleHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.LowerMantleShowed -= onShowed;
            EarthController.Instance.LowerMantleHided -= onHided;
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

        EarthController.Instance.LowerMantle().Whole().Go();
    }
}
