using UnityEngine;
using TMPro;
using System;


public class HitechLabelController : MonoBehaviour
{
    private enum HitechLabelControllerState { Hided, Opening, Opened, Hiding }

    public float timeOfShowingAndHiding = 1.0f;

    private float counter = 0.0f;

    private Material labelMat;
    private TextMeshProUGUI text;

    private HitechLabelControllerState state;

    private void Awake()
    {
        try
        {
            labelMat = transform.FindChild("Label").gameObject.GetComponent<Renderer>().material;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        text = GetComponentInChildren<TextMeshProUGUI>();

        HideImmediately();
        state = HitechLabelControllerState.Hided;
    }

    private void goToState(HitechLabelControllerState newState)
    {
        state = newState;

        switch (state)
        {
            case HitechLabelControllerState.Opening:
                LeanTween.cancel(gameObject);
                Color c1 = Color.white;
                c1.a = 0;
                text.color = c1;
                LeanTween.value(gameObject, counter, 1f, timeOfShowingAndHiding * (1 - counter)).setOnUpdate((float val) =>
                {
                    counter = val;
                    if (labelMat != null) labelMat.SetFloat("_PercentOfAppearing", counter);
                    c1.a = counter;
                    text.color = c1;
                }).setOnComplete(() =>
                {
                    goToState(HitechLabelControllerState.Opened);
                });
                break;
            case HitechLabelControllerState.Hiding:
                LeanTween.cancel(gameObject);
                Color c2 = Color.white;
                c2.a = 0;
                text.color = c2;
                LeanTween.value(gameObject, counter, 0f, timeOfShowingAndHiding * counter).setOnUpdate((float val) =>
                {
                    counter = val;
                    if (labelMat != null) labelMat.SetFloat("_PercentOfAppearing", counter);
                    c2.a = counter;
                    text.color = c2;
                }).setOnComplete(() =>
                {
                    goToState(HitechLabelControllerState.Hided);
                });
                break;
        }
    }

    public void Show()
    {
        if ((state == HitechLabelControllerState.Hided) || (state == HitechLabelControllerState.Hiding))
        {
            goToState(HitechLabelControllerState.Opening);
        }
    }

    public void Hide()
    {
        if ((state == HitechLabelControllerState.Opened) || (state == HitechLabelControllerState.Opening))
        {
            goToState(HitechLabelControllerState.Hiding);
        }
    }

    public void HideImmediately()
    {
        counter = 0;
        if (labelMat != null) labelMat.SetFloat("_PercentOfAppearing", 0f);
        Color c = Color.white;
        c.a = 0f;
        text.color = c;
        goToState(HitechLabelControllerState.Hided);
    }
}