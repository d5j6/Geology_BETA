using UnityEngine;
using System.Collections;
using TMPro;
using Vectrosity;

public class HologramController : MonoBehaviour
{
    [SerializeField]
    private AlphaController _background;

    private bool _isBusy;

    public float busyingTime = 1f;

    void Awake()
    {
        Deactivate(true);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);

        //Deactivate();
    }

    public void Activate()
    {
        if (_isBusy)
        {
            return;
        }

        _isBusy = true;

        _background.FadeIn();

        Invoke("BusyDelay", busyingTime);
    }

    public void Deactivate(bool isInstantly = false)
    {
        _isBusy = true;

        if (isInstantly)
        {
            _background.FadeOut(true);

            _isBusy = false;
        }
        else
        {
            _background.FadeOut(false);

            Invoke("BusyDelay", busyingTime);
        }
    }

    private void BusyDelay()
    {
        _isBusy = false;
    }
}
