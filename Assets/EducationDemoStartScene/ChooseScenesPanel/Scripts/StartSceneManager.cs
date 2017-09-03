using UnityEngine;
using HoloToolkit.Unity;
using System;

public class StartSceneManager : Singleton<StartSceneManager>, ISceneManager
{
    private GameObject logo;
    
    private void Start()
    { 
        Loader.Instance.IThinkIWasLoadedCompletelyAndCanStart(this);
    }

    void ISceneManager.StartScene()
    {
        startLogoEarthPolygonShowing();
    }

    private void startLogoEarthPolygonShowing()
    {
        Loader.Instance.LoadAndIntsntiateGOPrefab("Logo", onLogoInstantiated);
    }

    private void onLogoInstantiated(GameObject go)
    {
        logo = go;
        logo.GetComponent<LogoController>().HideImmediately();

        logo.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.8f;

        LeanTween.value(gameObject, 0, 1, logo.GetComponent<LogoController>().GetDuration()).setDelay(1f).setOnStart(() =>
        {
            logo.GetComponent<LogoController>().StartLogo();
        }).setOnComplete(() =>
        {
            showLangChooserScreen();
        });
    }

    private void showLangChooserScreen()
    {
        LanguageController.Instance.ShowWindow();
    }

    void ISceneManager.PrepareToUnload(Action callback)
    {
        ChooseScenePanelScript.Instance.MakeAllButtonsIndifferent();
        BoundaryVolume.Instance.transform.position = ChooseScenePanelScript.Instance.transform.position;
        ChooseScenePanelScript.Instance.Hide(() =>
        {
            callback.Invoke();
        });
    }

    bool ISceneManager.StartsAutomatically
    {
        get
        {
            return false;
        }
    }
}
