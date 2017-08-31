using UnityEngine;
using TMPro;
using HoloToolkit.Unity;
using System;

/// <summary>
/// Просто окошко загрузки. Все что оно умеет - появляться, исчезать, добавлять точки к строке "Loading" и иногда следовать за взглядом 
/// </summary>
public class LoadingWindowScript : LoadingWindow {

    public GameObject BackgroundObject;
    public GameObject AllParent;
    public TextMeshPro LoadingText;

    Material backgroundMat;
    Color c = Color.white;
    float counter = 0;

    Vector3 initialScale;

    public bool TagalongBehavior = true;
    [HideInInspector]
    public bool Showed = false;

    protected override void Awake ()
    {
        base.Awake();

        initialScale = transform.localScale;
        backgroundMat = BackgroundObject.GetComponent<MeshRenderer>().material;
    }

    public override void Show(Action callback = null)
    {
        base.Show();

        Showed = true;
        AllParent.SetActive(true);
        LeanTween.value(gameObject, 0, 1000, 500f).setOnUpdate((float val) =>
        {
            LoadingText.text = "Loading\n";
            for (int i = 0; i < (val/6 - Mathf.Floor(val / 6))*6; i++)
            {
                LoadingText.text += ".";
            }
        });

        LeanTween.value(gameObject, counter, 1, 1.18f* (1 - counter)).setOnStart(() =>
        {
            transform.position = Camera.main.transform.forward * 1.9f;
            transform.localScale = initialScale;
            if (TagalongBehavior)
            {
                if (GetComponent<FixedAngularSize>() != null) GetComponent<FixedAngularSize>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<SimpleTagalong>().enabled = true;
            }
        }).setOnUpdate((float val) =>
        {
            counter = val;
            c.a = val;
            backgroundMat.color = c;
            LoadingText.color = c;
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public override void Hide(Action callback = null)
    {
        base.Hide();

        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, counter, 0, 1.18f * counter).setOnUpdate((float val) =>
        {
            counter = val;
            c.a = val;
            backgroundMat.color = c;
            LoadingText.color = c;
        }).setOnComplete(() =>
        {
            if (TagalongBehavior)
            {
                GetComponent<SimpleTagalong>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                if (GetComponent<FixedAngularSize>() != null) GetComponent<FixedAngularSize>().enabled = false;
            }
            transform.localScale = Vector3.zero;
            AllParent.SetActive(false);
            LeanTween.cancel(gameObject);
            Showed = false;

            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }
}
