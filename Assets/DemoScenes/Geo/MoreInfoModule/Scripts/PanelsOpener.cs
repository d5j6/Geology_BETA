using UnityEngine;
using System.Collections.Generic;
using TMPro;

public enum PanelOpenableState { Opened, Closed, Opening, Closing }

public class PanelsOpener : MonoBehaviour
{
    public System.Action OnClose;

    public PanelOpenableState state = PanelOpenableState.Opened;

    public GameObject HidedGameObject;

    public GameObject LeftQuad;
    public GameObject RightQuad;
    public GameObject TopQuad;
    public GameObject BottomQuad;

    private Vector3 _leftQuadLocalScale = new Vector3(0.5f, 1f, 1f);
    private Vector3 _rightQuadLocalScale = new Vector3(0.5f, 1f, 1f);
    private Vector3 _topQuadLocalScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 _bottomQuadLocalScale = new Vector3(1f, 0.5f, 1f);

    private Vector3 _leftQuadLocalPosition = new Vector3(-0.25f, 0f, 0f);
    private Vector3 _rightQuadLocalPosition = new Vector3(0.25f, 0f, 0f);
    private Vector3 _topQuadLocalPosition = new Vector3(0f, 0.25f, 0f);
    private Vector3 _bottomQuadLocalPosition = new Vector3(0f, -0.25f, 0f);

    private Vector3 _leftQuadTargetLocalScale = new Vector3(0f, 1f, 1f);
    private Vector3 _rightQuadTargetLocalScale = new Vector3(0f, 1f, 1f);
    private Vector3 _topQuadTargetLocalScale = new Vector3(1f, 0f, 1f);
    private Vector3 _bottomQuadTargetLocalScale = new Vector3(1f, 0f, 1f);

    private Vector3 _leftQuadTargetLocalPosition = new Vector3(-0.5f, 0f, 0f);
    private Vector3 _rightQuadTargetLocalPosition = new Vector3(0.5f, 0f, 0f);
    private Vector3 _topQuadTargetLocalPosition = new Vector3(0f, 0.5f, 0f);
    private Vector3 _bottomQuadTargetLocalPosition = new Vector3(0f, -0.5f, 0f);

    private Renderer LeftQuadRenderer;
    private Renderer RightQuadRenderer;
    private Renderer TopQuadRenderer;
    private Renderer BottomQuadRenderer;

    public float ShowAndHideDuration = 1f;

    public GameObject[] fadeds;

    public HologramController hologramController;

    //public IFadebleContainer[] fadeds;

    void Awake()
    {
        initRenderers();

        HideImmediately();
    }

    void initRenderers()
    {
        LeftQuadRenderer = LeftQuad.GetComponent<Renderer>();
        RightQuadRenderer = RightQuad.GetComponent<Renderer>();
        TopQuadRenderer = TopQuad.GetComponent<Renderer>();
        BottomQuadRenderer = BottomQuad.GetComponent<Renderer>();
    }

    public float GetShowAndHideDuration()
    {
        return ShowAndHideDuration;
    }

    public void HideImmediately()
    {
        if (state != PanelOpenableState.Closed)
        {
            state = PanelOpenableState.Closed;
            if (LeftQuadRenderer == null) initRenderers();
            setPercentOpened(0f);
            SetRenderers(HidedGameObject, false);
            //HidedGameObject.setRenderers(false);
            LeftQuadRenderer.enabled = false;
            RightQuadRenderer.enabled = false;
            TopQuadRenderer.enabled = false;
            BottomQuadRenderer.enabled = false;
        }
    }

    private void setPercentOpened(float percent)
    {
        LeftQuad.transform.localScale = Vector3.Lerp(_leftQuadLocalScale, _leftQuadTargetLocalScale, percent);
        LeftQuad.transform.localPosition = Vector3.Lerp(_leftQuadLocalPosition, _leftQuadTargetLocalPosition, percent);

        RightQuad.transform.localScale = Vector3.Lerp(_rightQuadLocalScale, _rightQuadTargetLocalScale, percent);
        RightQuad.transform.localPosition = Vector3.Lerp(_rightQuadLocalPosition, _rightQuadTargetLocalPosition, percent);

        TopQuad.transform.localScale = Vector3.Lerp(_topQuadLocalScale, _topQuadTargetLocalScale, percent);
        TopQuad.transform.localPosition = Vector3.Lerp(_topQuadLocalPosition, _topQuadTargetLocalPosition, percent);

        BottomQuad.transform.localScale = Vector3.Lerp(_bottomQuadLocalScale, _bottomQuadTargetLocalScale, percent);
        BottomQuad.transform.localPosition = Vector3.Lerp(_bottomQuadLocalPosition, _bottomQuadTargetLocalPosition, percent);
    }

    public void Show()
    {
        if (state == PanelOpenableState.Closed)
        {
            state = PanelOpenableState.Opening;
            LeftQuadRenderer.enabled = true;
            RightQuadRenderer.enabled = true;
            TopQuadRenderer.enabled = true;
            BottomQuadRenderer.enabled = true;
            SetRenderers(HidedGameObject, true);
            //HidedGameObject.setRenderers(true);

            LeanTween.value(gameObject, 0f, 1f, ShowAndHideDuration).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                setPercentOpened(val);
            }).setOnComplete(() =>
            {
                state = PanelOpenableState.Opened;

                LeftQuadRenderer.enabled = false;
                RightQuadRenderer.enabled = false;
                TopQuadRenderer.enabled = false;
                BottomQuadRenderer.enabled = false;

                foreach (GameObject faded in fadeds)
                {
                    SpriteController sc = faded.GetComponent<SpriteController>();
                    if (sc != null)
                    {
                        //sc.FadeIn();
                    }
                    else
                    {
                        //faded.GetComponent<ChartSliceController>().FadeIn();
                    }
                }

                //hologramController.Activate();
            });
            
        }
    }

    public void Hide()
    {
        if (state == PanelOpenableState.Opened)
        {
            state = PanelOpenableState.Closing;

            LeftQuadRenderer.enabled = true;
            RightQuadRenderer.enabled = true;
            TopQuadRenderer.enabled = true;
            BottomQuadRenderer.enabled = true;

            LeanTween.value(gameObject, 1f, 0f, ShowAndHideDuration).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                setPercentOpened(val);

                foreach (GameObject faded in fadeds)
                {
                    SpriteController sc = faded.GetComponent<SpriteController>();
                    if (sc != null)
                    {
                        sc.FadeOut();
                    }
                    else
                    {
                        faded.GetComponent<ChartSliceController>().FadeOut();
                    }
                }
            }).setOnComplete(() =>
            {
                state = PanelOpenableState.Closed;
                SetRenderers(HidedGameObject, false);
                //HidedGameObject.setRenderers(false);
                LeftQuadRenderer.enabled = false;
                RightQuadRenderer.enabled = false;
                TopQuadRenderer.enabled = false;
                BottomQuadRenderer.enabled = false;

                if (OnClose != null)
                {
                    OnClose();
                }
            });
        }
        else if (state == PanelOpenableState.Closed)
        {
            if (OnClose != null)
            {
                OnClose();
            }
        }
    }

    public void SetRenderers(GameObject target, bool enabled)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = enabled;
        }

        TextMeshProUGUI[] textRenderers = target.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < textRenderers.Length; i++)
        {
            textRenderers[i].enabled = enabled;
        }
    }
}