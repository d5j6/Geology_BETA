using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SubmenuOpener : MonoBehaviour {

    private float OpeningTime = 2f;
    public float TargetScale = 0.4f;
    public float Padding = 1.1f;

    public List<GameObject> submenuItems = new List<GameObject>();

    private int state = 0;

    /*void Awake()
    {
        for (int i = 0; i < submenuItems.Count; i++)
        {
            submenuItems[i].transform.GetChild(0).localPosition = new Vector3(0, 0, 0.03f);
            submenuItems[i].transform.GetChild(0).localScale = Vector3.one * 0.01f;
        }
    }*/

    public Action Activated; 
    private void Start()
    {
        for (int i = 0; i < submenuItems.Count; i++)
        {
            submenuItems[i].transform.GetChild(0).localPosition = new Vector3(0, 0, 0.03f);
            submenuItems[i].transform.GetChild(0).localScale = Vector3.one * 0.01f;
        }

        if (Activated != null)
        {
            Activated.Invoke();
        }
    }

    public void Toggle()
    {
        switch (state)
        {
            case 0:
                Show();
                break;
            case 2:
                Hide();
                break;
        }
    }

    public void Show()
    {
        if (gameObject.activeInHierarchy)
        {
            if (state == 0)
            {
                state = 1;
                StartCoroutine(ShowSubmenuItems(0.19f));
            }
        }
    }

    private IEnumerator ShowSubmenuItems(float interval, System.Action callback = null)
    {
        for (int i = 0; i < submenuItems.Count; i++)
        {
            System.Action<int> a = (int index) =>
            {
                LeanTween.rotateAroundLocal(submenuItems[index], Vector3.up, 90f, OpeningTime).setEase(LeanTweenType.easeInOutCubic);
                LeanTween.moveLocalZ(submenuItems[index].transform.GetChild(0).gameObject, 1.25f + index * Padding, OpeningTime).setEase(LeanTweenType.easeInOutCubic);
                LeanTween.scale(submenuItems[index].transform.GetChild(0).gameObject, Vector3.one*TargetScale, OpeningTime).setEase(LeanTweenType.easeInOutCubic);
            };
            a(i);
            yield return new WaitForSeconds(interval);
        }

        if (callback != null)
        {
            callback.Invoke();
        }

        state = 2;
    }

    public void Hide()
    {
        if (state == 2)
        {
            state = 3;
            StartCoroutine(HideSubmenuItems(0.19f));
        }
    }

    private IEnumerator HideSubmenuItems(float interval, System.Action callback = null)
    {
        for (int i = 0; i < submenuItems.Count; i++)
        {
            System.Action<int> a = (int index) =>
            {
                LeanTween.rotateAroundLocal(submenuItems[index], Vector3.up, -90f, OpeningTime).setEase(LeanTweenType.easeInOutCubic);
                LeanTween.moveLocalZ(submenuItems[index].transform.GetChild(0).gameObject, 0.03f, OpeningTime).setEase(LeanTweenType.easeInOutCubic);
                LeanTween.scale(submenuItems[index].transform.GetChild(0).gameObject, Vector3.one * 0.01f, OpeningTime).setEase(LeanTweenType.easeInOutCubic);
            };
            a(i);
            yield return new WaitForSeconds(interval);
        }

        if (callback != null)
        {
            callback.Invoke();
        }

        state = 0;
    }
}
