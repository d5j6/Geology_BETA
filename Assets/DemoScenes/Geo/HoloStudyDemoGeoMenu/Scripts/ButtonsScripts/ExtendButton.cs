public class ExtendButton : CommonRingedAndTintedButton {

    protected override void Awake()
    {
        base.Awake();

        EarthController.Instance.Extended += onExtended;
        EarthController.Instance.Joined += onJoined;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (EarthController.Instance != null)
        {
            EarthController.Instance.Extended -= onExtended;
            EarthController.Instance.Joined -= onJoined;
        }
    }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        if (MiniExtenderWithArrow.Instance.Extended)
        {
            EarthController.Instance.Join().Go();
        }
        else
        {
            EarthController.Instance.Extend().Go();
        }
    }

    private void onExtended()
    {
        MiniExtenderWithArrow.Instance.Extend();
    }

    private void onJoined()
    {
        MiniExtenderWithArrow.Instance.Join();
    }
}
