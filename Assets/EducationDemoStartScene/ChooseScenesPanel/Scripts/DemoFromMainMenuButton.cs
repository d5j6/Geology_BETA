using UnityEngine;
using System.Collections;
using System;

public class DemoFromMainMenuButton : MonoBehaviour, IButton3D {

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

    void Awake()
    {
        buttonMat = GetComponent<MeshRenderer>().material;
        initialColor = buttonMat.color;
        initialColor.a = 1f;

        TapsListener.Instance.UserSingleTapped += onTapped;
    }

    void OnDestroy()
    {
        if (TapsListener.Instance != null)
        {
            TapsListener.Instance.UserSingleTapped -= onTapped;
        }
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

    public void OnTap(RaycastHit hitInfo)
    {
        if (animationMayChange)
        {
            if (SceneToLoad != "")
            {
                DemoShowStateMachine.playing = true;
                ChooseScenePanelScript.Instance.MakeAllButtonsIndifferent();
                BoundaryVolume.Instance.transform.position = transform.parent.position;
                ChooseScenePanelScript.Instance.Hide(() =>
                {
                    Loader.Instance.LoadScene(SceneToLoad);
                });
            }
        }
    }

    private void onTapped(GameObject obj)
    {
        if (animationMayChange && obj == gameObject)
        {
            if (SceneToLoad != "")
            {
                DemoShowStateMachine.playing = true;
                ChooseScenePanelScript.Instance.MakeAllButtonsIndifferent();
                BoundaryVolume.Instance.transform.position = transform.parent.position;
                ChooseScenePanelScript.Instance.Hide(() =>
                {
                    Loader.Instance.LoadScene(SceneToLoad);
                });
            }
        }
    }
}
