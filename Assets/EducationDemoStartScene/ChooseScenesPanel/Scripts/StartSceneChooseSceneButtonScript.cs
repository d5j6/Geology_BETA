using UnityEngine;
using System;
using HoloToolkit.Unity;

public class StartSceneChooseSceneButtonScript : StandardSimpleButton, IButton3D {

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

    void Update()
    {
        isNeed = CubeButton.isNeed;
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

    protected override void singleTapAction()
    {
        base.singleTapAction();

        if (isNeed)
        {
          //  
        }

        // Andrew Milko
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

        ChooseScenePanelScript.Instance.Hide(() =>
        {
            LoadSceneByTap();
        });
    }

    

    private void LoadSceneByTap()
    {
        if (animationMayChange)
        {
            if (SceneToLoad != "")
            {
                Loader.Instance.LoadScene(SceneToLoad, SceneLoadingMode.Single);  

                if (SceneToLoad == "ChemistryScene")
                {
                    Loader.Instance.TurnOffManagers();

                    if (PlaySpaceManager.Instance.numberOfTimes == 1)
                    {
                        PlaySpaceManager.Instance.CreatePlanes();
                        PlaySpaceManager.Instance.numberOfTimes = 0;
                    }

                    Debug.Log("Starting naking planes...");
                }
            }
        }
    }

    public void OnTap(RaycastHit hitInfo)
    {

    }
}