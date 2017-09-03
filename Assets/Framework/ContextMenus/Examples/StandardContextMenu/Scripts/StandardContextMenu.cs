using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using HoloToolkit.Unity;

public enum ContextMenuAttachingMode { ToUser, ToObject }
public class StandardContextMenu : Singleton<StandardContextMenu>, IContextMenu
{
    public GameObject ButtonPrefab;
    void Start()
    {
        InitPool();
        TapsListener.Instance.UserDoubleTapped += OnUserDoubleTapped;
        TapsListener.Instance.UserTapped += OnUserTapped;
        transform.localScale = Vector3.one * (1f / (distanceOfButtonsFromOrigin * 9f)) * MenuSize;
        Debug.Log("transform.localScale = " + transform.localScale);
    }

    #region View

    public ContextMenuAttachingMode AttachTarget = ContextMenuAttachingMode.ToUser;
    public float DistanceAttachTarget = 1f;
    public float MenuSize = 1;
    float distanceOfButtonsFromOrigin = 1.458f;
    void Update()
    {
        if (state != 0)
        {
            if (AttachTarget == ContextMenuAttachingMode.ToUser)
            {
                transform.position = Camera.main.transform.position + ((currentItems as StandardContextMenuItems).gameObject.transform.position - Camera.main.transform.position).normalized * DistanceAttachTarget;
            }
            else
            {
                transform.position = (currentItems as StandardContextMenuItems).gameObject.transform.position - ((currentItems as StandardContextMenuItems).gameObject.transform.position - Camera.main.transform.position).normalized * DistanceAttachTarget;
            }
        }
    }

    #endregion

    #region Behaviour
    
    int state = 0;

    void OnUserDoubleTapped(GameObject obj)
    {
        if (state == 0)
        {
            if (obj != null)
            {
                StandardContextMenuItems menuItems = obj.GetComponent<StandardContextMenuItems>();
                if (menuItems != null)
                {
                    state = 1;
                    Show(menuItems, () =>
                    {
                        state = 2;
                    });
                }
            }
        }
    }

    /// <summary>
    /// When clicking on menu button we waiting some delay, while animation of pressing is playing
    /// </summary>
    public float DelayBeforeMenuClosing = 0.8f;
    void OnUserTapped(GameObject obj)
    {
        if (state == 2)
        {
            bool pressingOnMenuButton = false;
            StandardContextMenuButton button = null;
            if (obj != null)
            {
                button = obj.GetComponent<StandardContextMenuButton>();
                if (button != null)
                {
                    pressingOnMenuButton = true;
                }
            }

            if (!pressingOnMenuButton)
            {
                state = 3;
                Hide(() =>
                {
                    state = 0;
                });
            }
            else
            {
                if ((button.itemInfo.SubmenuItems == null) && (button.itemInfo.Type != ContextMenuItemType.BackButton))
                {
                    //Если это обычная кнопка - закрываем меню, чуть-чуть подождав
                    StartCoroutine(waitBeforeAction(DelayBeforeMenuClosing, () =>
                    {
                        state = 3;
                        Hide(() =>
                        {
                            state = 0;
                        });
                    }));
                }
                
            }
        }
    }

    IEnumerator waitBeforeAction(float delay, Action action)
    {
        float counter = 0f;
        while (counter < delay)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        action.Invoke();
    }

    #endregion

    #region Interface

    /*
     * Меню должно открываться-закрываться по первому требованию, без задержек, поэтому
     */

    public float TimeOfGazeLeaveBeforeAutoClosing = 2f;

    IContextMenuItems temp;
    public void Show(IContextMenuItems items, Action callback = null)
    {
        if (!poolReady)
        {
            temp = items;
            tempActionOnShow = callback;
            onPoolReady += invokeShow;
        }
        else
        {
            show(items, callback);
        }
    }

    Action tempActionOnShow;

    void invokeShow()
    {
        Action callback = tempActionOnShow;
        tempActionOnShow = null;
        show(temp, callback);
    }

    public void Hide(Action callback = null)
    {
        if (!poolReady)
        {
            onPoolReady -= invokeShow;
        }
        else
        {
            hideButtons(callback);
        }
    }

    #endregion

    #region Pool
    public int PoolCapacity = 30;
    public GameObject[] buttonsPool;
    public bool[] objectInUse;
    int lastGOGiven = 0;

    bool poolReady = false;
    Action onPoolReady;

    void InitPool()
    {
        buttonsPool = new GameObject[PoolCapacity];
        objectInUse = new bool[PoolCapacity];
        StartCoroutine(fullfillPool());
    }

    IEnumerator fullfillPool()
    {
        int loadingSpeed = 2; //Как много инстанциировать за кадр
        for (int i = 0; i < PoolCapacity;)
        {
            for (int j = 0; j < loadingSpeed; j++)
            {
                buttonsPool[i] = Instantiate(ButtonPrefab);
                buttonsPool[i].transform.parent = transform;
                buttonsPool[i].transform.localPosition = Vector3.zero;
                buttonsPool[i].transform.localRotation = Quaternion.identity;
                buttonsPool[i].transform.localScale = Vector3.one;
                buttonsPool[i].GetComponent<IStandardMenuButton>().IndexInPool = i;
                objectInUse[i] = false;
                i++;
                if (i + j >= PoolCapacity)
                {
                    break;
                }
            }
            yield return null;
        }
        poolReady = true;
        if (onPoolReady != null)
        {
            onPoolReady.Invoke();
        }
    }

    GameObject getGOFromPool()
    {
        /*
         * Ищем свободный объект перебором по всему массиву objectInUse. В качестве оптимизации начинаем с индекса последнего выданного объекта.
         */

        GameObject result = null;
        bool vacantObjectFounded = false;
        int localIndex = lastGOGiven;
        for (int i = 0; i < PoolCapacity; i++)
        {
            if (!objectInUse[localIndex])
            {
                lastGOGiven = localIndex + 1;
                objectInUse[localIndex] = true;
                result = buttonsPool[localIndex];
                vacantObjectFounded = true;
                break;
            }
            else
            {
                localIndex++;
                if (localIndex >= PoolCapacity)
                {
                    localIndex = 0;
                }
            }
        }

        if (!vacantObjectFounded)
        {
            throw new Exception("StandardContextMenu: gamobjects pool limit (" + PoolCapacity + ") exceeded");
        }
        else
        {
            return result;
        }
    }

    public void ReturnGOToPool(GameObject go)
    {
        if (go == null)
        {
            Debug.Log("go == null");
        }
        if (go.GetComponent<IStandardMenuButton>() == null)
        {
            Debug.Log("go.GetComponent<IStandardMenuButton>() == null");
        }
        int index = go.GetComponent<IStandardMenuButton>().IndexInPool;
        objectInUse[index] = false;
        lastGOGiven = index;
    }

    #endregion

    #region Logic

    IContextMenuItems currentItems;
    List<ContextMenuItem> previousSubmenuItems;
    List<ContextMenuItem> currentSubmenuItems;
    List<IStandardMenuButton> currentSubmenuButtons;
    int currentNumOfShowedButtons = 0;
    int currentNumOfLaunchedButtons = 0;

    void show(IContextMenuItems items, Action callback = null)
    {
        /*
         * Прежде, чем показывать новое меню, мы должны уничтожить старое, если оно есть
         */
        if ((currentItems == items) || (currentItems != null))
        {
            //Нужно показать то же меню, что уже загружено
            ShowMenu(items, callback);
        }
        else if (currentItems != null)
        {
            //Нужно показать другое новое меню
            ClearMenu(() =>
            {
                ShowMenu(items, callback);
            });
        }
        else
        {
            //Нужно показать новое меню
            ShowMenu(items, callback);
        }
    }

    void signUpForUpdatesOnGazeLeaved()
    {

    }

    void ShowMenu(IContextMenuItems items, Action callback = null)
    {
        if (currentItems != items)
        {
            allocGOsRecursively(items.MenuItems);
        }
        currentItems = items;
        signUpForUpdatesOnGazeLeaved();
        GoToSubmenu(currentItems.MenuItems, callback);
    }

    void allocGOsRecursively(List<ContextMenuItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].SubmenuItems != null)
            {
                allocGOsRecursively(items[i].SubmenuItems);
            }
            (items[i] as StandardContextMenuItem).GOFromPool = getGOFromPool();
        }
    }

    void ClearMenu(Action callback = null)
    {
        StopCoroutine("ShowCurrentSubmenuButtons");
        checkIfThereIsButtonsShowed(callback);
    }

    void hideButtons(Action callback = null)
    {
        StartCoroutine(HideCurrentSubmenuButtons(0.1f, callback));
    }

    void checkIfThereIsButtonsShowed(Action callback)
    {
        if (currentNumOfLaunchedButtons > 0)
        {
            hideButtons(() =>
            {
                StartDeallocingGOsRecursively(currentItems.MenuItems, callback);
            });
        }
        else
        {
            StartDeallocingGOsRecursively(currentItems.MenuItems, callback);
        }
    }

    void StartDeallocingGOsRecursively(List<ContextMenuItem> items, Action callback)
    {
        Debug.Log("deallocing GOs");
        deallocGOsRecursively(items);

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    void deallocGOsRecursively(List<ContextMenuItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].SubmenuItems != null)
            {
                deallocGOsRecursively(items[i].SubmenuItems);
            }
            ReturnGOToPool((items[i] as StandardContextMenuItem).GOFromPool);
            (items[i] as StandardContextMenuItem).GOFromPool = null;
        }
    }

    void decrCurrentNumOfLaunchedButtons(Action callback)
    {
        currentNumOfLaunchedButtons--;
        if (currentNumOfLaunchedButtons <= 0)
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    IEnumerator HideCurrentSubmenuButtons(float interval, Action callback)
    {
        float counter = 0;
        int currentIndex = 0;
        while (counter < interval)
        {
            counter += Time.deltaTime;
            if (counter >= interval)
            {
                counter = 0f;

                currentSubmenuButtons[currentIndex].Hide(() =>
                {
                    decrCurrentNumOfLaunchedButtons(callback);
                });

                currentIndex++;
                if (currentIndex >= currentNumOfLaunchedButtons)
                {
                    break;
                }
            }

            yield return null;
        }
    }

    public void GoToPreviousSubmenu()
    {
        if (previousSubmenuItems != null)
        {
            GoToSubmenu(previousSubmenuItems);
        }
    }

    public void GoToSubmenu(List<ContextMenuItem> subMenu, Action callback = null)
    {
        previousSubmenuItems = currentSubmenuItems;
        currentSubmenuItems = subMenu;

        hideCurrentSubmenu(() =>
        {
            showSubmenu(() =>
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        });
    }

    void hideCurrentSubmenu(Action callback = null)
    {
        if (currentNumOfLaunchedButtons > 0)
        {
            StartCoroutine(HideCurrentSubmenuButtons(0.1f, callback));
        }
        else
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    void showSubmenu(Action callback = null)
    {
        currentSubmenuButtons = new List<IStandardMenuButton>();
        for (int i = 0; i < currentSubmenuItems.Count; i++)
        {
            currentSubmenuButtons.Add((currentSubmenuItems[i] as StandardContextMenuItem).GOFromPool.GetComponent<StandardContextMenuButton>());

            //Debug.Log("Added " + (currentSubmenuItems[i] as StandardContextMenuItem).Name);
        }
        StartCoroutine(ShowCurrentSubmenuButtons(0.17f, callback));
    }

    void incrCurrentNumOfShowedButtons(Action callback)
    {
        currentNumOfShowedButtons++;
        if (currentNumOfShowedButtons >= currentNumOfLaunchedButtons)
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    IEnumerator ShowCurrentSubmenuButtons(float interval, Action callback)
    {
        float timeCounter = 0;
        int currentIndex = 0;
        float angleCounter = 0;
        float angleAdding = Mathf.PI * 2 / currentSubmenuItems.Count;
        while (timeCounter < interval)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= interval)
            {
                timeCounter = 0f;

                currentNumOfLaunchedButtons++;
                currentSubmenuButtons[currentIndex].Show((currentSubmenuItems[currentIndex] as StandardContextMenuItem), angleCounter, distanceOfButtonsFromOrigin, () =>
                {
                    incrCurrentNumOfShowedButtons(callback);
                });
                angleCounter += angleAdding;

                currentIndex++;
                if (currentIndex >= currentSubmenuItems.Count)
                {
                    break;
                }
            }

            yield return null;
        }
    }

    void hide(Action callback = null)
    {
        ClearMenu(callback);
    }

    #endregion
}