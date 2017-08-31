public class BasaltButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        PieController.Instance.BasaltShowed += onShowed;
        PieController.Instance.BasaltHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (PieController.Instance != null)
        {
            PieController.Instance.BasaltShowed -= onShowed;
            PieController.Instance.BasaltHided -= onHided;
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

        PieController.Instance.ShowBasaltLayer();
    }
}
