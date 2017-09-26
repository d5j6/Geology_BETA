

public class CubeButton : StandardSimpleButton {

    public static bool isNeed { get; private set; }

    private void Start()
    {
        isNeed = false;
    }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        // SceneStateMachine.Instance.GoBackToMainMenu();

        Loader.Instance.GoToPreviousScene(SceneLoadingMode.Single, true, true);

        isNeed = true;
    }
}
