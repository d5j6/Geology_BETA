using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class ProfessorInroductionSpeechWithBubblesController : Singleton<ProfessorInroductionSpeechWithBubblesController> {

    public GameObject Professor;
    public ParticleSystem CloudsPS;
    public ParticleSystem BubblesPS;

    System.Action onIntroductionOver;
    CharacterAnimController professorController;

    public float speed = 2;

    public bool ProfessorVisible()
    {
        return Professor.activeSelf;
    }

    void Awake()
    {
        Professor.GetComponent<Animator>().speed = 1.5f;
        professorController = Professor.GetComponent<CharacterAnimController>();
        Professor.SetActive(false);
    }

    public void ShowByScalingAndStartTalking(System.Action callbackOnShowed = null)
    {
        CloudsPS.Play();
        LeanTween.delayedCall(2.4f / speed, () =>
        {
            Professor.SetActive(true);
            Professor.transform.localPosition = new Vector3(0, 0, -0.2f);
            Professor.transform.localScale = Vector3.one * 0.02f;
            Professor.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            LeanTween.value(Professor, 0, 1, 1f / speed).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                Professor.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
            }).setOnComplete(() =>
            {
                Debug.Log("Talkint = true! 0 !");

                professorController.Talking = true;

                if (callbackOnShowed != null)
                {
                    callbackOnShowed.Invoke();
                }
            });
        });
    }

    public void StopTalkingAndScaleToZero(System.Action callback = null)
    {
        professorController.Talking = false;
        LeanTween.value(Professor, 1, 0, 1f / speed).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            Professor.transform.localScale = Vector3.one * Mathf.Lerp(0.02f, 1f, val);
        }).setOnComplete(() =>
        {
            LeanTween.delayedCall(0.1f, () =>
            {
                Professor.SetActive(false);

                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        });
        CloudsPS.Stop();
    }

    public void ShowIntroWithBubbles (Vector3 place, System.Action callback = null) {
        onIntroductionOver = callback;
        showOnPlaceWithBubbles(place);
    }

    void showOnPlaceWithBubbles(Vector3 place)
    {
        transform.localPosition = place;
        BubblesPS.Play();
        LeanTween.delayedCall(0.7f / speed, () =>
        {
            CloudsPS.Play();
        });
        LeanTween.delayedCall(1.4f / speed, () =>
        {
            Professor.SetActive(true);
            Professor.transform.localPosition = new Vector3(0, 0, -0.2f);
            Professor.transform.localScale = Vector3.one * 0.02f;
            Professor.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            transform.localPosition = place;
            LeanTween.rotateAroundLocal(Professor, Vector3.up, -360f, 2.4f / speed).setEase(LeanTweenType.easeInOutCubic);
            LeanTween.scale(Professor, Vector3.one, 1f / speed).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
            {
                StartCoroutine(TryToWave(2f / speed));
                BubblesPS.Stop();
                LeanTween.delayedCall(BubblesPS.duration, () =>
                {
                    startTalking();
                });
            });
        });
    }

    IEnumerator TryToWave(float tryingTime = 2f)
    {
        float timeIntervals = 0.2f;
        for (int i = 0; i < tryingTime/ timeIntervals; i++)
        {
            professorController.Hello = true;
            yield return new WaitForSeconds(timeIntervals);
        }
    }

    void startTalking()
    {
        Professor.GetComponent<Animator>().speed = 1.0f;
        professorController.Hello = false;
        professorController.Talking = true;
        LeanTween.delayedCall(12f / speed, () =>
        {
            professorController.Talking = false;
            Professor.GetComponent<Animator>().speed = 1.5f;
            StartCoroutine(TryToWave(3f / speed));
            LeanTween.delayedCall(5f / speed, goingAway);
        });
    }

    void goingAway()
    {
        professorController.Hello = false;
        BubblesPS.Play();
        LeanTween.delayedCall(4f / speed, () =>
        {
            BubblesPS.Stop();
            LeanTween.scale(Professor, Vector3.one * 0.02f, 1.3f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.rotateAroundLocal(Professor, Vector3.up, 360f, 1.3f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
            {
                LeanTween.delayedCall(0.1f, () =>
                {
                    Professor.SetActive(false);
                });
            });
            CloudsPS.Stop();
            LeanTween.delayedCall(CloudsPS.duration, () =>
            {
                if (onIntroductionOver != null)
                {
                    onIntroductionOver.Invoke();
                    onIntroductionOver = null;
                }
            });
        });
    }
}
