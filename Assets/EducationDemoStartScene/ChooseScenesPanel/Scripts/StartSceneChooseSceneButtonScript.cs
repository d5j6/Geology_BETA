using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartSceneChooseSceneButtonScript : StandardSimpleButton, IButton3D
{
    public bool MultiLanguage;

    private bool isNeed = false;
    
    bool _animationMayChange = false;
    public bool animationMayChange
    {
        get
        {
            return _animationMayChange;
        }
        set
        {
            _animationMayChange = value;
            if (_animationMayChange)
            {
                initialColor = buttonMat.color;
            }
        }
    }
    float counter = 0f;
    Material buttonMat;
    Color c = Color.white;
    Color initialColor;
    public string SceneToLoad = "";

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

    public void Show()
    {

    }

    protected override void Awake()
    {
        base.Awake();

        buttonMat = GetComponent<MeshRenderer>().material;
        initialColor = buttonMat.color;
        initialColor.a = 1f;
    }

    public void OnGazeLeave()
    {
        if (animationMayChange)
        {
            LeanTween.value(gameObject, counter, 0, counter).setOnUpdate((float val) =>
            {
                counter = val;
                buttonMat.color = Color.Lerp(initialColor, c, val);
            });
        }
    }

    public void OnGazeOver(RaycastHit hitInfo)
    {
        if (animationMayChange)
        {
            if (counter < 1)
            {
                LeanTween.cancel(gameObject);

                counter += Time.deltaTime;
                if (counter > 1)
                {
                    counter = 1;
                }

                buttonMat.color = Color.Lerp(initialColor, c, counter);
            }
        }
    }

    private void Update()
    {
        isNeed = CubeButton.isNeed;
    }

    protected override void singleTapAction()
    {
        base.singleTapAction();

        if (isNeed)
        {
            Debug.Log("Work!");

            Destroy(FindObjectOfType<DemoShowStateMachine>());
            Destroy(FindObjectOfType<MoreInfoController>());
            Destroy(FindObjectOfType<EarthController>());
            Destroy(FindObjectOfType<PiePolygon>());
            Destroy(FindObjectOfType<SceneStateMachine>());
            Destroy(FindObjectOfType<PieController>());
            Destroy(FindObjectOfType<BigSimpleInfoPanelController>());
            Destroy(FindObjectOfType<SlicedEarthPolygon>());
            Destroy(FindObjectOfType<HoloStudyDemoGeoMenuController>());
            Destroy(FindObjectOfType<ProfessorOnPlatform>());
            Destroy(FindObjectOfType<AudioSourceController>());
        }

        ChooseScenePanelScript.Instance.Hide();

        if (MultiLanguage)
            LanguageController.Instance.ShowWindow(null, LoadSceneByTap);
        else
            LoadSceneByTap();
    }


    private void LoadSceneByTap()
    {
        if (animationMayChange)
        {
            if (SceneToLoad != "")
            {

                if (SceneToLoad == "ChemistryScene")
                {
                    
                    Loader.Instance.LoadScene(SceneToLoad, SceneLoadingMode.Single);
                    ManagersActivationScript.Instance.DeactivateInteractionManagers();
                    // Initializator.Instance.Awake();
                    // isNeed = false;
                }
                else
                {
                    Loader.Instance.LoadScene(SceneToLoad, SceneLoadingMode.Single);
                }
            }
        }
    }

    

    public void OnTap(RaycastHit hitInfo)
    {

    }
}
