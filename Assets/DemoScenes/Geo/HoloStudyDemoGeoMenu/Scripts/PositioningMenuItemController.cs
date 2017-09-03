using UnityEngine;

public class PositioningMenuItemController : MonoBehaviour {

	public void GoToPositioning()
    {
        SceneStateMachine.Instance.GoToBeginningVoidState(() =>
        {
            StartPositioning.Instance.ShowPositioningHolos();
        });
    }
}
