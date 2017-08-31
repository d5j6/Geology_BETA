using UnityEngine;
using System.Collections.Generic;

public class AllTinter : MonoBehaviour {

    private bool tinted = false;
    private bool wantedTint = false;
    public Color TintColor = Color.gray;
    private List<Material> mats;
    private Color[] initialColors;
    private Color[] targetColors;
    private float counter = 0f;
    public bool TintByShader = false;
    
    private void Awake () {
        mats = new List<Material>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                mats.Add(renderers[i].materials[j]);
            }
        }
        
        initialColors = new Color[mats.Count];
        for (int i = 0; i < initialColors.Length; i++)
        {
            initialColors[i] = mats[i].color;
        }

        targetColors = new Color[mats.Count];
        float f;
        for (int i = 0; i < targetColors.Length; i++)
        {
            f = (initialColors[i].r * 0.3f + initialColors[i].g * 0.59f + initialColors[i].b * 0.11f)/3f;
            targetColors[i] = new Color(f, f, f, 1);
        }

    }

    public void Tint(float t = 0.98917f)
    {
        //Debug.Log("Trying to tint");
        wantedTint = true;
        if ((!tinted) && (mats != null))
        {
            tinted = true;
            if (LeanTween.isTweening(gameObject))
            {
                LeanTween.cancel(gameObject);
            }
            LTDescr query;
            if (TintByShader)
            {
                query = LeanTween.value(gameObject, counter, 1, t).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    counter = val;
                    for (int i = 0; i < mats.Count; i++)
                    {
                        mats[i].SetFloat("_GrayAmount", counter);
                    }
                });
            }
            else
            {
                //Debug.Log("TintByShader != null");
                //Debug.Log("mats.Count = " + mats.Count);
                query = LeanTween.value(gameObject, counter, 1, t).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    counter = val;
                    for (int i = 0; i < mats.Count; i++)
                    {
                        mats[i].color = Color.Lerp(initialColors[i], targetColors[i], val);
                    }
                });
            }
            query.setOnComplete(() =>
            {
                if (wantedTint != tinted)
                {
                    if (!wantedTint)
                    {
                        Untint();
                    }
                }
            });
        }
        
    }

    public void Untint(float t = 0.98917f)
    {
        wantedTint = false;
        if ((tinted) && (mats != null))
        {
            tinted = false;
            if (LeanTween.isTweening(gameObject))
            {
                LeanTween.cancel(gameObject);
            }
            LTDescr query;
            if (TintByShader)
            {
                query = LeanTween.value(gameObject, counter, 0, t).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    counter = val;
                    for (int i = 0; i < mats.Count; i++)
                    {
                        mats[i].SetFloat("_GrayAmount", counter);
                    }
                });
            }
            else
            {
                query = LeanTween.value(gameObject, counter, 0, t).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    counter = val;
                    for (int i = 0; i < mats.Count; i++)
                    {
                        mats[i].color = Color.Lerp(initialColors[i], targetColors[i], val);
                    }
                });
            }
            query.setOnComplete(() =>
            {
                if (wantedTint != tinted)
                {
                    if (wantedTint)
                    {
                        Tint();
                    }
                }
            });
        }
    }
}
