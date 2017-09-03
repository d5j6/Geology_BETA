public class EarthButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.EarthWideClosed += onShowed;
        EarthController.Instance.EarthIntermediateState += onHided;
        EarthController.Instance.EarthWideOpened += onHided;
        PiePolygon.Instance.Showed += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.EarthWideClosed -= onShowed;
            EarthController.Instance.EarthIntermediateState -= onHided;
            EarthController.Instance.EarthWideOpened -= onHided;
        }

        if (PiePolygon.Instance != null)
        {
            PiePolygon.Instance.Showed -= onHided;
        }
    }

    private void onShowed()
    {
        if (gameObject.activeInHierarchy)
        {
            GetComponent<SubmenuOpener>().Show();
            MyRings.Show();
        }
        else
        {
            GetComponent<SubmenuOpener>().Activated += onObjectActivated;
        }
    }

    private void onObjectActivated()
    {
        GetComponent<SubmenuOpener>().Activated -= onObjectActivated;
        GetComponent<SubmenuOpener>().Show();
        MyRings.Show();
    }

    private void onHided()
    {
        GetComponent<SubmenuOpener>().Hide();
        MyRings.Hide();
    }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        SceneStateMachine.Instance.GoToEarthState();
        //EarthController.Instance.Surface().Whole().Join().Go();
    }
}
