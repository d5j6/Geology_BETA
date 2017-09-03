using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LangChooseStateMachine : MonoBehaviour {

    //float counter = 0f;
    public bool active = false;
    public LanguageController LanguageController;
    public UniversalTransparentFader UniversalFader;
    private int lastGazedIndex = 0;
    private int currentGazedIndex = 0;
    public GameObject[] buttons;
    public float FrameSpeed = 0.5f;
    public GameObject FrameImage;
    private Vector3 lastFramePos;
    private Vector3 targetFramePos;
    private float lastScale;
    private float targetScale;

    private int currentSelected = 0;
    private bool canSelect = true;

    public void LookAtButton(int index)
    {
        if ((active) && (currentGazedIndex != index) && (canSelect))
        {
            currentGazedIndex = index;
            lastFramePos = FrameImage.transform.localPosition;
            targetFramePos = buttons[index].transform.localPosition;
            targetFramePos.y += 0.02f;
            //targetFramePos.y += 0.044f * LanguageController.Instance.transform.localPosition.y;
            //targetFramePos.z += -0.0302f * LanguageController.Instance.transform.localPosition.z;
            lastScale = buttons[lastGazedIndex].transform.GetChild(0).localScale.x;
            targetScale = buttons[currentGazedIndex].transform.GetChild(0).localScale.x;
            lastGazedIndex = currentGazedIndex;
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, 0, 1, FrameSpeed).setEase(LeanTweenType.easeInOutCirc).setOnUpdate((float val) =>
            {
                FrameImage.transform.localPosition = Vector3.Lerp(lastFramePos, targetFramePos, val);
                FrameImage.transform.localScale = new Vector3(Mathf.Lerp(lastScale, targetScale, val), 0.1f, 0.2f);
            });
        }
    }

    private void Start()
    {
        ChooseLang((int)LanguageManager.Instance.CurrentLanguage);
    }

    public void ChooseLang(int index)
    {
        if ((active) && (index != currentSelected))
        {
            LanguageController.ChooseLang(index);
            canSelect = false;
            Color c = Color.white;
            LeanTween.value(gameObject, 0, 1, 0.89f).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, 0.5f, val);
                buttons[index].transform.GetChild(1).GetComponent<Renderer>().material.color = c;
                c.a = 0.5f - c.a;
                buttons[currentSelected].transform.GetChild(1).GetComponent<Renderer>().material.color = c;
            }).setOnComplete(() =>
            {
                currentSelected = index;
                canSelect = true;
                UniversalFader.UpdateInitialVars();
            });
        }
    }

    public void OK(int index)
    {
        if ((active) && (index != currentSelected) && (canSelect))
        {
            LanguageController.OK();
            /*canSelect = false;
            Color c = Color.white;
            LeanTween.value(gameObject, 0, 1, 0.89f).setOnUpdate((float val) =>
            {
                c.a = Mathf.Lerp(0, 0.5f, val);
                buttons[index].transform.GetChild(0).GetComponent<Image>().color = c;
                c.a = 0.5f - c.a;
                buttons[currentSelected].transform.GetChild(0).GetComponent<Image>().color = c;
            }).setOnComplete(() =>
            {
                currentSelected = index;
                canSelect = true;
                UniversalFader.UpdateInitialVars();
            });*/
        }
    }
}
