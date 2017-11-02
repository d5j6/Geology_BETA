using UnityEngine;

public class StandardSimpleButton : MonoBehaviour
{
    protected bool enabledByFunctionality = true;

    protected virtual void Awake()
    {
        TapsListener.Instance.UserSingleTapped += onUserSingleTapped;
        TapsListener.Instance.UserDoubleTapped += onUserDoubleTapped;
    }

    protected virtual void OnDestroy()
    {
        if (TapsListener.Instance != null)
        {
            TapsListener.Instance.UserSingleTapped -= onUserSingleTapped;
            TapsListener.Instance.UserDoubleTapped -= onUserDoubleTapped;
        }
    }

    protected void onUserSingleTapped(GameObject go)
    {
        if (enabledByFunctionality && go == gameObject)
        {
            singleTapAction();
        }
    }

    protected void onUserDoubleTapped(GameObject go)
    {
        if (enabledByFunctionality && go == gameObject)
        {
            doubleTapAction();
        }
    }

    protected virtual void singleTapAction()
    {

    }

    protected virtual void doubleTapAction()
    {

    }
}
