using UnityEngine;

public class TransparentFader : MonoBehaviour {

    int wantedState = 0;
    int state = 0;
    public float ShowingTime = 0.89f;
    Material mat;
    Color initialColor;
    float initialAlpha;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
        initialColor = mat.color;
        initialAlpha = initialColor.a;
        HideImmediately();
    }

    public void Show(System.Action callback = null)
    {
        wantedState = 1;
        if (state == 0)
        {
            state = 1;
            Color c = initialColor;
            c.a = 0;
            LeanTween.value(gameObject, 0, 1, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, initialAlpha, val);
                mat.color = c;
            }).setOnComplete(() =>
            {
                state = 2;

                if (wantedState == 0)
                {
                    Hide();
                }

                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        }
        else
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    public void ShowVoid()
    {
        wantedState = 1;
        if (state == 0)
        {
            state = 1;
            Color c = initialColor;
            c.a = 0;
            LeanTween.value(gameObject, 0, 1, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, initialAlpha, val);
                mat.color = c;
            }).setOnComplete(() =>
            {
                state = 2;

                if (wantedState == 0)
                {
                    Hide();
                }
            });
        }
    }

    public void ShowImmediately()
    {
        wantedState = 1;
        state = 2;
        Color c = initialColor;
        c.a = initialAlpha;
        mat.color = c;
    }

    public void Hide(System.Action callback = null)
    {
        wantedState = 0;
        if (state == 2)
        {
            state = 3;
            Color c = initialColor;
            c.a = 0;
            LeanTween.value(gameObject, 1, 0, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, initialAlpha, val);
                mat.color = c;
            }).setOnComplete(() =>
            {
                state = 0;

                if (wantedState == 1)
                {
                    Show();
                }

                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        }
    }

    public void HideVoid()
    {
        wantedState = 0;
        if (state == 2)
        {
            state = 3;
            Color c = initialColor;
            c.a = 0;
            LeanTween.value(gameObject, 1, 0, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, initialAlpha, val);
                mat.color = c;
            }).setOnComplete(() =>
            {
                if (wantedState == 1)
                {
                    Show();
                }
                state = 0;
            });
        }
    }

    public void HideImmediately()
    {
        wantedState = 0;
        state = 0;
        Color c = initialColor;
        c.a = 0;
        mat.color = c;
    }
}
