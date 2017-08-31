using UnityEngine;
using System;
using TMPro;

public class StandardContextMenuButton : MonoBehaviour, IStandardMenuButton, IButton3D
{
    private int state = 0;
    private int wantedState = 0;
    public StandardContextMenuItem itemInfo;
    private Action callbackOnShow;
    private Action callbackOnHide;

    private float initialScale = 0.5f;
    private Vector3 targetPos;

    public Action ButtonAction;

    private GameObject backgroungImage;
    private Material backgroundMat;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        standardRimThickness = rend.material.GetFloat("_RimThickness");
        currentRimThickness = standardRimThickness;
        rend.enabled = false;
        backgroungImage = transform.GetChild(0).gameObject;
        rend2 = backgroungImage.GetComponent<MeshRenderer>();
        backgroundMat = rend2.material;
        rend2.enabled = false;
        text = transform.GetChild(1).GetComponent<TextMeshPro>();
        text.enabled = false;
    }

    public void Show(StandardContextMenuItem itemInfo, float angle, float distance,  Action callbackOnShow = null)
    {
        this.itemInfo = itemInfo;
        this.callbackOnShow = callbackOnShow;
        ButtonAction = null;
        if (itemInfo.SubmenuItems != null)
        {
            ButtonAction += () =>
            {
                StandardContextMenu.Instance.GoToSubmenu(itemInfo.SubmenuItems);
            };
        }
        else if (itemInfo.Type == ContextMenuItemType.BackButton)
        {
            ButtonAction += () =>
            {
                StandardContextMenu.Instance.GoToPreviousSubmenu();
            };
        }
        else
        {
            ButtonAction += itemInfo.Action;
        }

        transform.localPosition = Vector3.zero;
        targetPos = transform.localPosition;
        targetPos.x += -Mathf.Cos(angle) * distance;
        targetPos.y += Mathf.Sin(angle) * distance;
        Color c = rend.material.color;
        c.a = 0;
        rend.material.color = c;
        Color c1 = itemInfo.NameColor;
        c1.a = c.a;
        text.text = itemInfo.Name;
        text.color = c1;
        Color c2 = Color.white;
        c2.a = c.a;
        backgroundMat.color = c2;
        if (itemInfo.Icon != null)
        {
            backgroundMat.SetTexture("_MainTex", itemInfo.Icon);
        }
        else
        {
            backgroundMat.SetTexture("_MainTex", null);
        }

        showButton();
    }

    public void Hide(Action callbackOnHide = null)
    {
        this.callbackOnHide = callbackOnHide;
        hideButton();
    }

    MeshRenderer rend;
    MeshRenderer rend2;
    TextMeshPro text;

    public bool EnabledByCollider
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public bool EnabledByFunctionality
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    private int indexInPool;
    public int IndexInPool
    {
        get
        {
            return indexInPool;
        }
        set
        {
            indexInPool = value;
        }
    }

    float mainScaleMultiplier;

    void updateScale()
    {
        //Debug.Log("Vector3.one * currentOnGazeScaleAdded = " + Vector3.one * currentOnGazeScaleAdded);
        //transform.localScale = Vector3.one * mainScaleMultiplier + Vector3.one * currentOnGazeScaleAdded;
        transform.localScale = Vector3.one * mainScaleMultiplier;
    }

    void showButton()
    {
        wantedState = 1;
        if (state == 0)
        {
            state = 1;

            Color c = rend.material.color;
            Color c1 = itemInfo.NameColor;
            Color c2 = Color.white;
            LeanTween.value(gameObject, 0, 1, 0.635f).setEase(LeanTweenType.easeOutCubic).setOnStart(() =>
            {
                mainScaleMultiplier = initialScale;
                updateScale();
                //transform.localScale = Vector3.one * initialScale + Vector3.one * currentOnGazeScaleAdded;
                rend.enabled = true;
                text.enabled = true;
                if (itemInfo.Icon != null)
                {
                    rend2.enabled = true;
                }

            }).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, 1, val);
                rend.material.color = c;
                c1.a = c.a;
                text.color = c1;
                if (itemInfo.Icon != null)
                {
                    c2.a = c.a;
                    backgroundMat.color = c2;
                }
                transform.localPosition = Vector3.Lerp(Vector3.zero, targetPos, val);
                mainScaleMultiplier = Mathf.Lerp(initialScale, 1, val);
                updateScale();
                //transform.localScale = Vector3.Lerp(Vector3.one * initialScale + Vector3.one * currentOnGazeScaleAdded, Vector3.one + Vector3.one * currentOnGazeScaleAdded, val);
            }).setOnComplete(() =>
            {
                if (callbackOnShow != null)
                {
                    callbackOnShow.Invoke();
                    callbackOnShow = null;
                }

                state = 2;
                if (wantedState == 0)
                {
                    hideButton();
                }
            });
        }
    }

    void hideButton()
    {
        wantedState = 0;
        if (state == 2)
        {
            state = 3;

            Color c = rend.material.color;
            Color c1 = itemInfo.NameColor;
            Color c2 = Color.white;
            LeanTween.value(gameObject, 1, 0, 0.4275f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, 1, val);
                rend.material.color = c;
                c1.a = c.a;
                text.color = c1;
                if (itemInfo.Icon != null)
                {
                    c2.a = c.a;
                    backgroundMat.color = c2;
                }
                transform.localPosition = Vector3.Lerp(Vector3.zero, targetPos, val);
                mainScaleMultiplier = Mathf.Lerp(initialScale, 1, val);
                updateScale();
                //transform.localScale = Vector3.Lerp(Vector3.one * initialScale + Vector3.one * currentOnGazeScaleAdded, Vector3.one + Vector3.one * currentOnGazeScaleAdded, val);
            }).setOnComplete(() =>
            {
                rend.enabled = false;
                text.enabled = false;
                rend2.enabled = false;
                if (callbackOnHide != null)
                {
                    callbackOnHide.Invoke();
                    callbackOnHide = null;
                }

                state = 0;
                if (wantedState == 1)
                {
                    showButton();
                }
            });
        }
    }

    float currentRimThickness;
    float standardRimThickness;

    int stateOfRim = 0;
    int wantedStateOfRim = 0;
    public void OnGazeOver(RaycastHit hitInfo)
    {
        GazeOverAction();
    }

    void GazeOverAction()
    {
        wantedStateOfRim = 1;
        if (stateOfRim == 0)
        {
            stateOfRim = 1;
            LeanTween.value(transform.GetChild(0).gameObject, 1, 0, 0.2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                currentRimThickness = Mathf.Lerp(0, standardRimThickness, val);
                rend.material.SetFloat("_RimThickness", currentRimThickness);
            }).setOnComplete(() =>
            {
                stateOfRim = 2;
                if (wantedStateOfRim == 0)
                {
                    OnGazeLeave();
                }
            });
        }
        
    }

    public void OnTap(RaycastHit hitInfo)
    {
        stateOfRim = 4;
        LeanTween.value(transform.GetChild(0).gameObject, 0, 1, StandardContextMenu.Instance.DelayBeforeMenuClosing).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            currentRimThickness = Mathf.Lerp(0, standardRimThickness*2, Mathf.Sin(Mathf.PI * val));
            rend.material.SetFloat("_RimThickness", currentRimThickness);
        }).setOnComplete(() =>
        {
            stateOfRim = 2;
        });

        if (ButtonAction != null)
        {
            ButtonAction.Invoke();
        }
    }

    public void OnGazeLeave()
    {
        wantedStateOfRim = 0;
        if (stateOfRim == 2)
        {
            stateOfRim = 3;
            LeanTween.value(transform.GetChild(0).gameObject, 0, 1, 0.2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                currentRimThickness = Mathf.Lerp(0, standardRimThickness, val);
                rend.material.SetFloat("_RimThickness", currentRimThickness);
            }).setOnComplete(() =>
            {
                stateOfRim = 0;
                if (wantedStateOfRim == 1)
                {
                    GazeOverAction();
                }
            });
        }
        
    }
}