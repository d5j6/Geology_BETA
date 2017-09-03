using UnityEngine;

public class PositioningButton : MonoBehaviour
{
    public TransparentFader ButtonFader;
    public TransparentFader SignFader;

    public void DeactivateImmediately()
    {
        ButtonFader.HideImmediately();
        SignFader.HideImmediately();
        GetComponent<Collider>().enabled = false;
    }

    public void ActivateImmediately()
    {
        ButtonFader.ShowImmediately();
        SignFader.ShowImmediately();
        GetComponent<Collider>().enabled = true;
    }

    public void Activate(System.Action callback = null)
    {
        GetComponent<Collider>().enabled = true;
        ButtonFader.Show();
        SignFader.Show(callback);
    }

    public void Deactivate(System.Action callback = null)
    {
        ButtonFader.Hide(() =>
        {
            GetComponent<Collider>().enabled = false;
        });
        SignFader.Hide(callback);
    }

    public void DisableByFunctionality()
    {
        GetComponent<Button3DVoid>().EnabledByFunctionality = false;
    }

    public void EnableByFunctionality()
    {
        GetComponent<Button3DVoid>().EnabledByFunctionality = true;
    }
}
