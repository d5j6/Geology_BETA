using UnityEngine;

public class ProfessorInroductionSpeechController : MonoBehaviour {

    /*public GameObject Door;
    DoorController doorController;
    public GameObject Professor;
    public Transform ProfessorStartinTransformation;
    CharacterAnimController professorController;

    System.Action onSpeechComplete;

    void Awake()
    {
        Professor.SetActive(false);
        Door.SetActive(false);

        LeanTween.delayedCall(2f, StartIntroduction);
    }

	public void StartIntroduction(System.Action callback = null)
    {
        //Debug.Log("Intro");
        onSpeechComplete = callback;

        Door.SetActive(true);
        doorController = Door.GetComponent<DoorController>();
        Vector3 doorsTargetPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
        doorsTargetPosition.y = Camera.main.transform.position.y;
        doorController.ShowUp(doorsTargetPosition, () =>
        {
            Professor.SetActive(true);
            Professor.transform.parent = ProfessorStartinTransformation;
            Professor.transform.localPosition = Vector3.zero;
            Professor.transform.localRotation = Quaternion.identity;
            Professor.transform.localScale = Vector3.one;
            professorController = Professor.GetComponent<CharacterAnimController>();
            doorController.Open(onDoorOpened, 2.31f);
        });
    }

    void onDoorOpened()
    {
        LeanTween.delayedCall(2.82f, () =>
        {
            doorController.FadeOut(3f);
            LeanTween.delayedCall(0.54f, () =>
            {
                doorController.Close(3f);
            });
        });
        professorController.WS = 1f;
        LeanTween.delayedCall(4f, () =>
        {
            //Debug.Log("Professor stop");
            professorController.WS = 0f;
            professorController.Hello = true;
            LeanTween.delayedCall(5f, startTalking);
        });
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            professorController.Hello = true;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            professorController.Hello = false;
        }

    }

    void startTalking()
    {
        professorController.Hello = false;
        professorController.Talking = true;
        LeanTween.delayedCall(10f, () =>
        {
            professorController.Talking = false;
            professorController.Hello = true;
            LeanTween.delayedCall(5f, goingAway);
        });
    }

    void goingAway()
    {
        professorController.Hello = false;
        professorController.AD = 1f;

        LeanTween.delayedCall(4.7f, () =>
        {
            doorController.Open(null, 2.31f);
            doorController.FadeIn(2.4f);
            professorController.AD = 0;
            professorController.WS = 1;
            LeanTween.delayedCall(6f, Disappear);
        });
    }

    void Disappear()
    {
        doorController.Close(2.31f, () =>
        {
            Professor.SetActive(false);
            doorController.Hide(() =>
            {
                if (onSpeechComplete != null)
                {
                    onSpeechComplete.Invoke();
                    onSpeechComplete = null;
                }
            });
        });
        
    }*/
}
