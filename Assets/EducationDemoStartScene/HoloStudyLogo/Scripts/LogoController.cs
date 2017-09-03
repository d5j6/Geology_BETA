using UnityEngine;
using TMPro;

public class LogoController : MonoBehaviour {

    public float showTime = 0.5f;
    public float stableTime = 2.5f;
    public float hideTime = 0.7f;

    public TextMeshProUGUI text;

    public float GetDuration()
    {
        return showTime + stableTime + hideTime;
    }

    public void HideImmediately()
    {
        Color c = new Color(1f, 1f, 1f, 0f);
        text.color = c;
    }

    public void StartLogo()
    {
        text.enabled = true;
        LeanTween.value(gameObject, new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), showTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((Color c) =>
        {
            text.color = c;
        });

        LeanTween.value(gameObject, new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f), hideTime).setDelay(showTime + stableTime).setEase(LeanTweenType.easeInOutSine).setOnUpdate((Color c) =>
        {
            text.color = c;
        }).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}