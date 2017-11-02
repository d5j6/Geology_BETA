using UnityEngine;
using HoloToolkit.Unity;
using System.Collections.Generic;
using TMPro;

public class ChooseScenePanelScript : Singleton<ChooseScenePanelScript>
{
    /*public GameObject Background;
    public GameObject Button1;
    public GameObject Button2;*/

    public List<GameObject> Buttons = new List<GameObject>();
    public List<TextMeshProUGUI> Texts = new List<TextMeshProUGUI>();
    List<Material> Materials = new List<Material>();
    List<Color> Colors = new List<Color>();

    /*Material mat1;
    Material mat2;
    Material mat3;
    Color c1;
    Color c2;
    Color c3;*/

    float counter = 0f;

    AudioSource currentAudio;

    public bool TagAlongBehaviour = true;

    // Use this for initialization
    void Awake ()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Materials.Add(Buttons[i].GetComponent<MeshRenderer>().material);
            Colors.Add(Buttons[i].GetComponent<MeshRenderer>().material.color);
        }

        /*mat1 = Background.GetComponent<MeshRenderer>().material;
        mat2 = Button1.GetComponent<MeshRenderer>().material;
        mat3 = Button2.GetComponent<MeshRenderer>().material;

        c1 = mat1.color;
        c2 = mat2.color;
        c3 = mat3.color;*/
    }

    public void Show(System.Action callback = null)
    {
        switch(LanguageManager.Instance.CurrentLanguage)
        {
            case Language.English:
                Texts[0].text = "ENG";
                break;
            case Language.Russian:
                Texts[0].text = "RUS";
                break;
        }

        LeanTween.value(gameObject, counter, 1, 0.78f * (1 - counter)).setOnStart(() =>
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].SetActive(true);
            }
            transform.position = Camera.main.transform.forward * Camera.main.transform.position.z;
            transform.localScale = Vector3.one*0.66f;
            if (TagAlongBehaviour)
            {
                GetComponent<SimpleTagalong>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
                transform.localScale = Vector3.one;
            }
        }).setOnUpdate((float val) =>
        {
            counter = val;
            Color c;
            for (int i = 0; i < Colors.Count; i++)
            {
                c = Colors[i];
                c.a = val;
                Materials[i].color = c;
            }

            c = Color.white;
            for (int i = 0; i < Texts.Count; i++)
            {
                c.a = val;
                Texts[i].color = c;
            }
        }).setOnComplete(() =>
        {
            StartSceneChooseSceneButtonScript sscsbs;
            DemoFromMainMenuButton dfmmb;
            for (int i = 0; i < Buttons.Count; i++)
            {
                sscsbs = Buttons[i].GetComponent<StartSceneChooseSceneButtonScript>();
                if (sscsbs != null)
                {
                    sscsbs.animationMayChange = true;
                }
                else
                {
                    dfmmb = Buttons[i].GetComponent<DemoFromMainMenuButton>();
                    if (dfmmb != null)
                    {
                        dfmmb.animationMayChange = true;
                    }
                }
            }

            switch(LanguageManager.Instance.CurrentLanguage)
            {
                case Language.Russian:
                    currentAudio = GetComponents<AudioSource>()[0];
                    break;
                case Language.English:
                    currentAudio = GetComponents<AudioSource>()[1];
                    break;
            }
            currentAudio.enabled = true;
            currentAudio.Play();
            
            LeanTween.value(gameObject, 0, 1, 0.2f).setOnUpdate((float val) =>
            {
                currentAudio.volume = val;
            });

            if (callback != null)
            {
                callback.Invoke();
                callback = null;
            }
        });
    }

    public void MakeAllButtonsIndifferent()
    {
        StartSceneChooseSceneButtonScript sscsbs;
        DemoFromMainMenuButton dfmmb;
        for (int i = 0; i < Buttons.Count; i++)
        {
            sscsbs = Buttons[i].GetComponent<StartSceneChooseSceneButtonScript>();
            if (sscsbs != null)
            {
                sscsbs.animationMayChange = false;
            }
            else
            {
                dfmmb = Buttons[i].GetComponent<DemoFromMainMenuButton>();
                if (dfmmb != null)
                {
                    dfmmb.animationMayChange = false;
                }
            }
        }

        //transform.localScale = Vector3.one*0.66f;
    }

    public void Hide(System.Action callback = null)
    {
        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, 0, 1, 0.2f).setOnUpdate((float val) =>
        {
            currentAudio.volume = val;
        }).setOnComplete(() =>
        {
            currentAudio.Stop();
            currentAudio.enabled = false;
        });
        LeanTween.value(gameObject, counter, 0, 0.78f * counter).setOnUpdate((float val) =>
        {
            counter = val;
            Color c;
            for (int i = 0; i < Colors.Count; i++)
            {
                c = Colors[i];
                c.a = val;
                Materials[i].color = c;
            }

            c = Color.white;
            for (int i = 0; i < Texts.Count; i++)
            {
                c.a = val;
                Texts[i].color = c;
            }
        }).setOnComplete(() =>
        {
            if (TagAlongBehaviour)
            {
                GetComponent<SimpleTagalong>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
            }
            transform.localScale = Vector3.zero;
            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].SetActive(false);
            }
            if (callback != null)
            {
                callback.Invoke();
                callback = null;
            }
        });
    }

    public void ToLangChoosing()
    {
        MakeAllButtonsIndifferent();
        Hide(() =>
        {
            LanguageController.Instance.ShowWindow();
        });
    }
}
