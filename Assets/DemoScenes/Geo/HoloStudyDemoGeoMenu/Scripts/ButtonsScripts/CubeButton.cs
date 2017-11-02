public class CubeButton : StandardSimpleButton {

    public static bool isNeed { get; set; }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        isNeed = true;

        SceneStateMachine.Instance.GoBackToMainMenu();
    }
}
