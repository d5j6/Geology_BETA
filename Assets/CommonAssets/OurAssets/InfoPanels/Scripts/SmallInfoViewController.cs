using HoloToolkit.Unity;
using TMPro;
using UnityEngine;

public class SmallInfoViewController : Singleton<SmallInfoViewController> {

    public TextMeshProUGUI Text;

    public TMProScroller TMProScroller;

    private new BoxCollider collider;
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        transform.GetChild(0).gameObject.SetActive(false);
        collider.enabled = false;
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
    public void BringToScene(Vector3 targetSize, System.Action callback = null)
    {
        if (!brought)
        {
            brought = true;
            LeanTween.scale(transform.GetChild(0).gameObject, targetSize, 1f).setOnComplete(() =>
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

    public void RemoveFromScene(System.Action callback = null)
    {
        if (brought)
        {
            brought = false;
            collider.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<InfoPanelFader>().Hide();
            LeanTween.scale(transform.GetChild(0).gameObject, Vector3.one * 0.01f, 1f).setOnComplete(() =>
            {
                transform.GetChild(0).gameObject.SetActive(false);

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

    public void Show(string text, float scrollMax)
    {
        collider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<InfoPanelFader>().Show();
        if (text != "")
        {
            Text.GetComponent<TMProScroller>().ScrollPos = 0;
            Text.text = text;
            TMProScroller.MaxScrollPos = scrollMax;
        }
    }

    public void Show(InformationsMockup query)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        string text = DataController.Instance.GetDescriptionFor(query);
        float scrollMax = DataController.Instance.GetMaxScrollPosFor(query);
        collider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<InfoPanelFader>().Show();
        if (text != "")
        {
            Text.GetComponent<TMProScroller>().ScrollPos = 0;
            Text.text = text;
            TMProScroller.MaxScrollPos = scrollMax;
        }
    }

    public void Hide(System.Action callback = null)
    {
        collider.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<InfoPanelFader>().Hide(() =>
        {
            transform.GetChild(0).gameObject.SetActive(false);

            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }
}
