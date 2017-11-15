using HoloToolkit.Unity;

public class CubeButton : StandardSimpleButton {

    public static bool isNeed { get; set; }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        isNeed = true;

        // SpatialMappingObserver.TimeBetweenUpdates = 3.5f;

        SceneStateMachine.Instance.GoBackToMainMenu();
    }
}
