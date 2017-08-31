using UnityEngine;

public class Button3DFloatColorTint : Button3DFloat
{
    ColorTintButtonState state = ColorTintButtonState.StaticUnlooked;

    public float TintingTime = 0.68f;
    public Color UnlookedColor = Color.yellow;
    public Color LookedColor = Color.white;
    public Color TappedColor = Color.cyan;

    Material mat;

    float counter = 0f;
    bool looked = false;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
        mat.color = UnlookedColor;
    }

    protected override void onGazeLeaveAction()
    {
        looked = false;

        if ((state != ColorTintButtonState.StaticUnlooked) && (state != ColorTintButtonState.Tapping) && (state != ColorTintButtonState.Unlooked))
        {
            state = ColorTintButtonState.Unlooked;
            Color initialColor = mat.color;
            Color c;

            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, counter, 0, TintingTime).setOnUpdate((float val) =>
            {
                counter = val;
                c = Color.Lerp(UnlookedColor, initialColor, val);
                mat.color = c;
            }).setOnComplete(() =>
            {
                state = ColorTintButtonState.StaticUnlooked;

                if (looked)
                {
                    onGazeOverAction();
                }
            });
        }
    }

    protected override void onGazeOverAction()
    {
        looked = true;

        if ((state != ColorTintButtonState.StaticLooked) && (state != ColorTintButtonState.Tapping) && (state != ColorTintButtonState.Looked))
        {
            state = ColorTintButtonState.Looked;
            Color initialColor = mat.color;
            Color c;

            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, counter, 1, TintingTime).setOnUpdate((float val) =>
            {
                counter = val;
                c = Color.Lerp(initialColor, LookedColor, val);
                mat.color = c;
            }).setOnComplete(() =>
            {
                state = ColorTintButtonState.StaticLooked;

                if (!looked)
                {
                    onGazeLeaveAction();
                }
            });
        }
    }

    protected override void onTapAction()
    {

        if (state != ColorTintButtonState.Tapping)
        {
            state = ColorTintButtonState.Tapping;
            Color initialColor = mat.color;
            Color c;

            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, 0, 1, TintingTime).setOnUpdate((float val) =>
            {
                c = Color.Lerp(initialColor, TappedColor, Mathf.Sin(val * Mathf.PI));
                mat.color = c;
            }).setOnComplete(() =>
            {
                if (looked)
                {
                    state = ColorTintButtonState.Unlooked;
                    onGazeOverAction();
                }
                else
                {
                    state = ColorTintButtonState.Looked;
                    onGazeLeaveAction();
                }
            });
        }
    }
}
