using UnityEngine;
using TMPro;

public class PieDividerController : MonoBehaviour {

    public TextMeshPro Oceanic;
    public TextMeshPro Continental;
    public MeshRenderer FrameGrid;
    public MeshRenderer FrameSolid;

    bool showed = false;

    public void Show(float t = 0.96f, System.Action callback = null)
    {
        if (!showed)
        {
            showed = true;
            Color c1 = Oceanic.color;
            Color c2 = Continental.color;
            Color c3 = FrameGrid.material.color;
            Color c4 = FrameSolid.material.color;

            LeanTween.value(gameObject, 0, 1, t).setOnUpdate((float val) =>
            {
                c1.a = val;
                c2.a = val;
                c3.a = val;
                c4.a = Mathf.Lerp(0f, 0.796f, val);
                Oceanic.color = c1;
                Continental.color = c2;
                FrameGrid.material.color = c3;
                FrameSolid.material.color = c4;
            }).setOnComplete(() =>
            {
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

    public void Hide(float t = 0.96f, System.Action callback = null)
    {
        if (showed)
        {
            showed = false;
            Color c1 = Oceanic.color;
            Color c2 = Continental.color;
            Color c3 = FrameGrid.material.color;
            Color c4 = FrameSolid.material.color;

            LeanTween.value(gameObject, 1, 0, t).setOnUpdate((float val) =>
            {
                c1.a = val;
                c2.a = val;
                c3.a = val;
                c4.a = Mathf.Lerp(0f, 0.796f, val);
                Oceanic.color = c1;
                Continental.color = c2;
                FrameGrid.material.color = c3;
                FrameSolid.material.color = c4;
            }).setOnComplete(() =>
            {
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
}
