using HoloToolkit.Unity;
using UnityEngine;

public class LanguageController : Singleton<LanguageController>
{
    public bool TagAlongBehaviour = true;

    private void Awake()
    {
        /*
        if (IsAlreadyExists)
        {
            Destroy(gameObject);
        }
        */
    }

    public void ChooseLang(int langIndex)
    {
        LanguageManager.Instance.CurrentLanguage = (Language)langIndex;
    }

    public void ShowWindow(System.Action callback = null)
    {
        if (TagAlongBehaviour)
        {
            GetComponent<Collider>().enabled = true;
            if (GetComponent<FixedAngularSize>() != null) GetComponent<FixedAngularSize>().enabled = true;
            GetComponent<SimpleTagalong>().enabled = true;
        }
        GetComponent<YSprite>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<LangChooseStateMachine>().active = true;
        transform.GetChild(0).GetComponent<UniversalTransparentFader>().Show(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void HideWindow(System.Action callback = null)
    {
        transform.GetChild(0).GetChild(0).GetComponent<LangChooseStateMachine>().active = false;
        transform.GetChild(0).GetComponent<UniversalTransparentFader>().Hide(() =>
        {
            transform.GetChild(0).gameObject.SetActive(false);
            if (TagAlongBehaviour)
            {
                GetComponent<SimpleTagalong>().enabled = false;
                if (GetComponent<FixedAngularSize>() != null) GetComponent<FixedAngularSize>().enabled = false;
                GetComponent<Collider>().enabled = false;
            }
            GetComponent<YSprite>().enabled = false;
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void OK()
    {
        HideWindow(() =>
        {
            ChooseScenePanelScript.Instance.Show();
        });
    }
}
