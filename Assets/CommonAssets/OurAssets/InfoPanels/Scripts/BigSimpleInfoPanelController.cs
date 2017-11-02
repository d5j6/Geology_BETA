using UnityEngine;
using TMPro;
using HoloToolkit.Unity;

public class BigSimpleInfoPanelController : Singleton<BigSimpleInfoPanelController>
{
    public TextMeshProUGUI Label;
    public TextMeshProUGUI Text;

    public TMProScroller TMProScroller;

    private new BoxCollider collider;
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();

        transform.GetChild(0).localScale = Vector3.zero;
    }

    private void Start()
    {
        NativeManipulationManager.Instance.NativeYManipulationStarted += onYManipulationStarted;
        NativeManipulationManager.Instance.NativeYManipulationUpdated += onYManipulationUpdated;
        NativeManipulationManager.Instance.NativeYManipulationCompleted += onYManipulationCompleted;
    }

    private void OnDestroy()
    {
        if (NativeManipulationManager.Instance != null)
        {
            NativeManipulationManager.Instance.NativeYManipulationStarted -= onYManipulationStarted;
            NativeManipulationManager.Instance.NativeYManipulationUpdated -= onYManipulationUpdated;
            NativeManipulationManager.Instance.NativeYManipulationCompleted -= onYManipulationCompleted;
        }
    }

    private bool whenManipulationStartedUserWatchedOnMe = false;
    private void onYManipulationStarted(GameObject obj)
    {
        if (obj == gameObject)
        {
            whenManipulationStartedUserWatchedOnMe = true;
        }
    }

    private float ScrollSensitivity = 0.015f;
    private void onYManipulationUpdated(float val)
    {
        if (whenManipulationStartedUserWatchedOnMe)
        {
            TMProScroller.Scroll(val * ScrollSensitivity);
        }
    }

    private void onYManipulationCompleted()
    {
        whenManipulationStartedUserWatchedOnMe = false;
    }

    private bool brought = false;
    public void BringToScene(System.Action callback = null)
    {
        if (!brought)
        {
            brought = true;
            LeanTween.scale(transform.GetChild(0).gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeInCubic).setOnComplete(() =>
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

    public void RemoveFromSceneImmediately()
    {
        transform.GetChild(0).localScale = Vector3.zero;
    }

    public void RemoveFromScene(System.Action callback = null)
    {
        if (brought)
        {
            brought = false;
            collider.enabled = false;
            GetComponent<InfoPanelFader>().Hide();
            LeanTween.scale(transform.GetChild(0).gameObject, Vector3.one * 0.01f, 1f).setEase(LeanTweenType.easeInCubic).setOnComplete(() =>
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

    public void Show(float scrollMax, string label, string text)
    {
        collider.enabled = true;
        GetComponent<InfoPanelFader>().Show();
        if (text != "")
        {
            Text.GetComponent<TMProScroller>().ScrollPos = 0;
            Text.text = text;
            Label.text = label;
            TMProScroller.MaxScrollPos = scrollMax;
        }
    }

    public void Show(InformationsMockup query)
    {
        collider.enabled = true;
        string label = DataController.Instance.GetLabelFor(query);
        string text = DataController.Instance.GetDescriptionFor(query);
        float scrollMax = DataController.Instance.GetMaxScrollPosFor(query);
        
        GetComponent<InfoPanelFader>().Show();
        if (text != "")
        {
            Text.GetComponent<TMProScroller>().ScrollPos = 0;
            Text.text = text;
            Label.text = label;
            TMProScroller.MaxScrollPos = scrollMax;
        }
    }

    public void Hide(System.Action callback = null)
    {
        collider.enabled = false;
        GetComponent<InfoPanelFader>().Hide(callback);
    }

    public void HideImmediately()
    {
        collider.enabled = false;
        GetComponent<InfoPanelFader>().HideImmediately();
    }
}
