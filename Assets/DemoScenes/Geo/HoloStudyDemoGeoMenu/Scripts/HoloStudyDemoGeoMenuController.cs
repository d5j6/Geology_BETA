using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using System;

public class HoloStudyDemoGeoMenuController : Singleton<HoloStudyDemoGeoMenuController>
{
    public GameObject Cube;
    public GameObject[] GeoMenuItems;
    public GameObject[] SideMenuItems;

    public Action OnMenuShowed;
    public Action OnMenuClosed;

    public float mainMenuScale = 0.9f;
    public float mainMenuPadiing = 0.33f;
    public float sideMenuScale = 0.9f;
    public float sideMenuPadiing = 0.33f;

    void Awake()
    {
        for (int i = 0; i < GeoMenuItems.Length; i++)
        {
            GeoMenuItems[i].transform.localPosition = Vector3.zero;
            GeoMenuItems[i].transform.localScale = Vector3.one*0.01f;
        }
        for (int i = 0; i < SideMenuItems.Length; i++)
        {
            SideMenuItems[i].transform.localPosition = Vector3.zero;
            SideMenuItems[i].transform.localScale = Vector3.one * 0.01f;
        }

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Show(Vector3 place, System.Action callback = null)
    {
        Vector3 startingPlace = place + new Vector3(4f, 1f, 2f);
        transform.position = startingPlace;
        LeanTween.move(gameObject, place, 1.4f).setEase(LeanTweenType.easeInOutCubic);
        transform.GetChild(0).gameObject.SetActive(true);

        ShowCube(() =>
        {
            ShowMainMenu(() =>
            {
                LeanTween.delayedCall(1f, () =>
                {
                    ShowSideMenu(() =>
                    {
                        if (callback != null)
                        {
                            callback.Invoke();
                        }
                        if (OnMenuShowed != null)
                        {
                            OnMenuShowed.Invoke();
                        }
                    });
                });
            });
        });
    }

    public void Hide(Action callback = null)
    {
        if (Cube.transform.localScale.x < 0.1f)
        {
            if (callback != null)
            {
                callback.Invoke();
            }
            if (OnMenuClosed != null)
            {
                OnMenuClosed.Invoke();
            }
        }
        else
        {
            HideSideMenu(() =>
            {
                HideMainMenu(() =>
                {
                    HideCube();
                    Vector3 targetPlace = transform.position + new Vector3(4f, 1f, 2f);
                    LeanTween.move(gameObject, targetPlace, 1.4f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
                    {
                        transform.GetChild(0).gameObject.SetActive(false);

                        if (callback != null)
                        {
                            callback.Invoke();
                        }
                        if (OnMenuClosed != null)
                        {
                            OnMenuClosed.Invoke();
                        }
                    });
                });
            });
        }
    }

    private void ShowCube(System.Action callback = null)
    {
        Cube.transform.localScale = Vector3.one * 0.01f;
        LeanTween.rotateAroundLocal(Cube, Vector3.up, 360f, 1.4f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(Cube, Vector3.one, 1.4f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    private void HideCube(System.Action callback = null)
    {
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, -360f, 1.4f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(gameObject, Vector3.one*0.01f, 1.4f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void ShowMainMenu(System.Action callback = null)
    {
        StartCoroutine(LaunchGeoItems(0.15f, callback));
    }

    private IEnumerator LaunchGeoItems(float interval, System.Action callback = null)
    {
        Action<int> a = (int index) =>
        {
            GeoMenuItems[index].transform.localScale = Vector3.zero;
            LeanTween.moveLocal(GeoMenuItems[index], Vector3.up * 0.2f + Vector3.up * ((index + 1) + mainMenuPadiing * (index + 1)), 1f).setEase(LeanTweenType.easeOutElastic);
            LeanTween.scale(GeoMenuItems[index], Vector3.one * mainMenuScale, 1f);
        };
        for (int i = 0; i < GeoMenuItems.Length; i++)
        {
            a(i);
            yield return new WaitForSeconds(interval);
        }

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    private IEnumerator PushBackGeoItems(float interval, System.Action callback = null)
    {
        Action<int> a = (int index) =>
        {
            LeanTween.moveLocal(GeoMenuItems[index], Vector3.zero, 1f).setEase(LeanTweenType.easeInElastic);
            LeanTween.scale(GeoMenuItems[index], Vector3.zero, 1f);
        };
        for (int i = 0; i < GeoMenuItems.Length; i++)
        {
            a(i);
            yield return new WaitForSeconds(interval);
        }

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    private IEnumerator LaunchSideItems(float interval, System.Action callback = null)
    {
        Action<int> a = (int index) =>
        {
            SideMenuItems[index].transform.localScale = Vector3.zero;
            LeanTween.moveLocal(SideMenuItems[index], Vector3.right * ((index + 1) + sideMenuPadiing * (index + 1)), 1f).setEase(LeanTweenType.easeOutElastic);
            LeanTween.scale(SideMenuItems[index], Vector3.one * sideMenuScale, 1f);
        };
        for (int i = 0; i < SideMenuItems.Length; i++)
        {
            a(i);
            yield return new WaitForSeconds(interval);
        }

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    private IEnumerator PushBackSideItems(float interval, System.Action callback = null)
    {
        Action<int> a = (int index) =>
        {
            LeanTween.moveLocal(SideMenuItems[index], Vector3.zero, 1f).setEase(LeanTweenType.easeInElastic);
            LeanTween.scale(SideMenuItems[index], Vector3.zero, 1f);
        };
        for (int i = 0; i < SideMenuItems.Length; i++)
        {
            a(i);
            yield return new WaitForSeconds(interval);
        }

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public void HideMainMenu(System.Action callback = null)
    {
        StartCoroutine(PushBackGeoItems(0.15f, callback));
    }

    public void ShowSideMenu(System.Action callback = null)
    {
        StartCoroutine(LaunchSideItems(0.15f, callback));
    }

    public void HideSideMenu(System.Action callback = null)
    {
        StartCoroutine(PushBackSideItems(0.15f, callback));
    }
}
