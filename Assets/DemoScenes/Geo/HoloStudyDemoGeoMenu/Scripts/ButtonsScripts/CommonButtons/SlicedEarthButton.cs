public class SlicedEarthButton : CommonRingedAndTintedButton
{
    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.EarthWideOpened += onShowed;
        EarthController.Instance.EarthWideClosed += onHided;
        PiePolygon.Instance.Showed += onHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.EarthWideOpened -= onShowed;
            EarthController.Instance.EarthWideClosed -= onHided;
        }

        if (PiePolygon.Instance != null)
        {
            PiePolygon.Instance.Showed -= onHided;
        }
    }

    private void onShowed()
    {
        SubmenuOpener[] submenus = GetComponents<SubmenuOpener>();
        for (int i = 0; i < submenus.Length; i++)
        {
            submenus[i].Show();
        }
        MyRings.Show();
    }

    private void onHided()
    {
        SubmenuOpener[] submenus = GetComponents<SubmenuOpener>();
        for (int i = 0; i < submenus.Length; i++)
        {
            submenus[i].Hide();
        }
        MyRings.Hide();
    }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        SceneStateMachine.Instance.GoToSlicedEarthState();
        //EarthController.Instance.OpenEarthWidely();
    }
}
