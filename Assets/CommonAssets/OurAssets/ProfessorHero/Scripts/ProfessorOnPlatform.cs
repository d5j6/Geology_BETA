using UnityEngine;
using System;
using HoloToolkit.Unity;

public class ProfessorOnPlatform : Singleton<ProfessorOnPlatform>
{
    public GameObject ProfessorBody;
    public GameObject ProfessorsJoint;
    public ProfessorWithPlatformController ProfOnPlatController;
    public GameObject Platform;
    private CharacterAnimController professorController;

    private Vector3 profPlatformTargetPos;

    public float AnimationSpeedMultiplier = 2f;

    void Awake()
    {
        profPlatformTargetPos = Platform.transform.localPosition;
        ProfessorBody.GetComponent<Animator>().speed = 1.5f;
        professorController = ProfessorBody.GetComponent<CharacterAnimController>();
        ProfessorBody.SetActive(false);
        Platform.SetActive(false);
    }

    public bool ProfessorVisible()
    {
        return ProfessorBody.activeSelf;
    }

    public void ShowByScalingAndStartTalking(System.Action callbackOnShowed = null)
    {
        Platform.SetActive(true);
        ProfessorBody.SetActive(true);
        ProfessorBody.transform.localPosition = new Vector3(0, 0, 0f);
        Platform.transform.localScale = Vector3.one * 0.02f;
        ProfessorBody.transform.localScale = Vector3.one * 0.02f;
        ProfessorBody.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        LeanTween.value(ProfessorBody, 0, 1, 1f / AnimationSpeedMultiplier).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            Platform.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
            ProfessorBody.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
        }).setOnComplete(() =>
        {
            professorController.Talking = true;

            if (callbackOnShowed != null)
            {
                callbackOnShowed.Invoke();
            }
        });
    }

    public void ShowByFlyingInAndStartTalking(Action callbackOnShowed = null)
    {
        if (SceneStateMachine.Instance.IsEarthContainingState())
        {
            transform.localPosition = SlicedEarthPolygon.Instance.transform.localPosition + new Vector3(-0.42315884f, -0.15854688f, -0.500144f);
        }
        else
        {
            transform.localPosition = PiePolygon.Instance.transform.localPosition + new Vector3(-0.42315884f, -0.15854688f, -0.500144f);
        }

        Platform.SetActive(true);
        ProfessorBody.SetActive(true);
        Vector3 randomPlace = new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-1f, 1f));
        randomPlace /= 0.04f;
        ProfessorBody.transform.localPosition = randomPlace;
        Platform.transform.localPosition = randomPlace;
        Platform.transform.localScale = Vector3.one * 0.02f;
        ProfessorBody.transform.localScale = Vector3.one * 0.02f;

        //ProfOnPlatController.SetScale(0.02f);
        ProfessorBody.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

        LeanTween.value(ProfessorBody, 0, 1, 1f / AnimationSpeedMultiplier).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            //ProfOnPlatController.SetScale(Mathf.Lerp(0.02f, 1f, val));
            Platform.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
            ProfessorBody.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
            ProfessorBody.transform.localPosition = Vector3.Lerp(randomPlace, Vector3.zero, val);
            Platform.transform.localPosition = Vector3.Lerp(randomPlace, profPlatformTargetPos, val);
            //Professor.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
        }).setOnComplete(() =>
        {
            professorController.Talking = true;

            if (callbackOnShowed != null)
            {
                callbackOnShowed.Invoke();
            }
        });
    }

    public void StopTalkingAndScaleToZero(System.Action callback = null)
    {
        professorController.Talking = false;
        LeanTween.value(ProfessorBody, 1, 0, 1f / AnimationSpeedMultiplier).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            Platform.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
            ProfessorBody.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
            //ProfOnPlatController.SetScale(Mathf.Lerp(0.02f, 1f, val));
            //Professor.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
        }).setOnComplete(() =>
        {
            LeanTween.delayedCall(0.1f, () =>
            {
                //ProfOnPlatController.StopEngine();
                ProfessorBody.SetActive(false);
                Platform.SetActive(false);

                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        });
    }
}
