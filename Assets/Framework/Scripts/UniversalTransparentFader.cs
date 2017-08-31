using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TransparentFaderInitialVars
{
    public Color InitialColor;
    public float InitialAlpha;
}

public class UniversalTransparentFader : MonoBehaviour
{
    public Renderer[] Renderers;
    TransparentFaderInitialVars[] renderersInitialVars;
    public TextMeshProUGUI[] TextsUGUI;
    TransparentFaderInitialVars[] textsUGUIInitialVars;
    public TextMeshPro[] Texts;
    TransparentFaderInitialVars[] textsInitialVars;
    public Image[] UIImages;
    TransparentFaderInitialVars[] uiImagesInitialVars;

    int wantedState = 0;
    int state = 0;
    public float ShowingTime = 0.89f;

    public bool HidedOnStart = true;

    void Awake()
    {
        renderersInitialVars = new TransparentFaderInitialVars[Renderers.Length];
        textsUGUIInitialVars = new TransparentFaderInitialVars[TextsUGUI.Length];
        textsInitialVars = new TransparentFaderInitialVars[Texts.Length];
        uiImagesInitialVars = new TransparentFaderInitialVars[UIImages.Length];

        for (int i = 0; i < Renderers.Length; i++)
        {
            renderersInitialVars[i] = new TransparentFaderInitialVars();
        }
        for (int i = 0; i < TextsUGUI.Length; i++)
        {
            textsUGUIInitialVars[i] = new TransparentFaderInitialVars();
        }
        for (int i = 0; i < Texts.Length; i++)
        {
            textsInitialVars[i] = new TransparentFaderInitialVars();
        }
        for (int i = 0; i < UIImages.Length; i++)
        {
            uiImagesInitialVars[i] = new TransparentFaderInitialVars();
        }

        UpdateInitialVars();

        if (HidedOnStart)
        {
            HideImmediately();
        }
    }

    public void UpdateInitialVars()
    {
        for (int i = 0; i < Renderers.Length; i++)
        {
            renderersInitialVars[i].InitialColor = Renderers[i].material.color;
            renderersInitialVars[i].InitialAlpha = renderersInitialVars[i].InitialColor.a;
        }
        for (int i = 0; i < TextsUGUI.Length; i++)
        {
            textsUGUIInitialVars[i].InitialColor = TextsUGUI[i].color;
            textsUGUIInitialVars[i].InitialAlpha = textsUGUIInitialVars[i].InitialColor.a;
        }
        for (int i = 0; i < Texts.Length; i++)
        {
            textsInitialVars[i].InitialColor = Texts[i].color;
            textsInitialVars[i].InitialAlpha = textsInitialVars[i].InitialColor.a;
        }
        for (int i = 0; i < UIImages.Length; i++)
        {
            uiImagesInitialVars[i].InitialColor = UIImages[i].color;
            uiImagesInitialVars[i].InitialAlpha = uiImagesInitialVars[i].InitialColor.a;
        }
    }

    public void Show(System.Action callback = null)
    {
        wantedState = 1;
        if (state == 0)
        {
            state = 1;
            Color c;
            LeanTween.value(gameObject, 0, 1, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                for (int i = 0; i < Renderers.Length; i++)
                {
                    c = renderersInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, renderersInitialVars[i].InitialAlpha, val);
                    Renderers[i].material.color = c;
                }
                for (int i = 0; i < TextsUGUI.Length; i++)
                {
                    c = textsUGUIInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, textsUGUIInitialVars[i].InitialAlpha, val);
                    TextsUGUI[i].color = c;
                }
                for (int i = 0; i < Texts.Length; i++)
                {
                    c = textsInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, textsInitialVars[i].InitialAlpha, val);
                    Texts[i].color = c;
                }
                for (int i = 0; i < UIImages.Length; i++)
                {
                    c = uiImagesInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, uiImagesInitialVars[i].InitialAlpha, val);
                    UIImages[i].color = c;
                }
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
            Color c;
            LeanTween.value(gameObject, 0, 1, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                for (int i = 0; i < Renderers.Length; i++)
                {
                    c = renderersInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, renderersInitialVars[i].InitialAlpha, val);
                    Renderers[i].material.color = c;
                }
                for (int i = 0; i < TextsUGUI.Length; i++)
                {
                    c = textsUGUIInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, textsUGUIInitialVars[i].InitialAlpha, val);
                    TextsUGUI[i].color = c;
                }
                for (int i = 0; i < Texts.Length; i++)
                {
                    c = textsInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, textsInitialVars[i].InitialAlpha, val);
                    Texts[i].color = c;
                }
                for (int i = 0; i < UIImages.Length; i++)
                {
                    c = uiImagesInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(0f, uiImagesInitialVars[i].InitialAlpha, val);
                    UIImages[i].color = c;
                }
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
        Color c;
        for (int i = 0; i < Renderers.Length; i++)
        {
            c = renderersInitialVars[i].InitialColor;
            c.a = renderersInitialVars[i].InitialAlpha;
            Renderers[i].material.color = c;
        }
        for (int i = 0; i < TextsUGUI.Length; i++)
        {
            c = textsUGUIInitialVars[i].InitialColor;
            c.a = textsUGUIInitialVars[i].InitialAlpha;
            TextsUGUI[i].color = c;
        }
        for (int i = 0; i < Texts.Length; i++)
        {
            c = textsInitialVars[i].InitialColor;
            c.a = textsInitialVars[i].InitialAlpha;
            Texts[i].color = c;
        }
        for (int i = 0; i < UIImages.Length; i++)
        {
            c = uiImagesInitialVars[i].InitialColor;
            c.a = uiImagesInitialVars[i].InitialAlpha;
            UIImages[i].color = c;
        }
    }

    public void Hide(System.Action callback = null)
    {
        wantedState = 0;
        if (state == 2)
        {
            state = 3;
            Color c;
            LeanTween.value(gameObject, 0, 1, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                for (int i = 0; i < Renderers.Length; i++)
                {
                    c = renderersInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(renderersInitialVars[i].InitialAlpha, 0f, val);
                    Renderers[i].material.color = c;
                }
                for (int i = 0; i < TextsUGUI.Length; i++)
                {
                    c = textsUGUIInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(textsUGUIInitialVars[i].InitialAlpha, 0f, val);
                    TextsUGUI[i].color = c;
                }
                for (int i = 0; i < Texts.Length; i++)
                {
                    c = textsInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(textsInitialVars[i].InitialAlpha, 0f, val);
                    Texts[i].color = c;
                }
                for (int i = 0; i < UIImages.Length; i++)
                {
                    c = uiImagesInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(uiImagesInitialVars[i].InitialAlpha, 0f, val);
                    UIImages[i].color = c;
                }
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
            Color c;
            LeanTween.value(gameObject, 0, 1, ShowingTime).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                for (int i = 0; i < Renderers.Length; i++)
                {
                    c = renderersInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(renderersInitialVars[i].InitialAlpha, 0f, val);
                    Renderers[i].material.color = c;
                }
                for (int i = 0; i < TextsUGUI.Length; i++)
                {
                    c = textsUGUIInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(textsUGUIInitialVars[i].InitialAlpha, 0f, val);
                    TextsUGUI[i].color = c;
                }
                for (int i = 0; i < Texts.Length; i++)
                {
                    c = textsInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(textsInitialVars[i].InitialAlpha, 0f, val);
                    Texts[i].color = c;
                }
                for (int i = 0; i < UIImages.Length; i++)
                {
                    c = uiImagesInitialVars[i].InitialColor;
                    c.a = Mathf.Lerp(uiImagesInitialVars[i].InitialAlpha, 0f, val);
                    UIImages[i].color = c;
                }
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
        Color c;
        for (int i = 0; i < Renderers.Length; i++)
        {
            c = renderersInitialVars[i].InitialColor;
            c.a = 0;
            Renderers[i].material.color = c;
        }
        for (int i = 0; i < TextsUGUI.Length; i++)
        {
            c = textsUGUIInitialVars[i].InitialColor;
            c.a = 0;
            TextsUGUI[i].color = c;
        }
        for (int i = 0; i < Texts.Length; i++)
        {
            c = textsInitialVars[i].InitialColor;
            c.a = 0;
            Texts[i].color = c;
        }
        for (int i = 0; i < UIImages.Length; i++)
        {
            c = uiImagesInitialVars[i].InitialColor;
            c.a = 0;
            UIImages[i].color = c;
        }
    }
}
