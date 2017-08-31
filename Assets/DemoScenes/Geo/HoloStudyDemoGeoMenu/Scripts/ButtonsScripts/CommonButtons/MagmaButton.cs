public class MagmaButton : CommonRingedAndTintedButton
{
    private void Start()
    {
        PieController.Instance.MagmaShowed += onShowed;
        PieController.Instance.MagmaHided += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (PieController.Instance != null)
        {
            PieController.Instance.MagmaShowed -= onShowed;
            PieController.Instance.MagmaHided -= onHided;
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

        PieController.Instance.ShowMagmaLayer();
    }
}
