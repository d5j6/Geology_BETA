using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

/// <summary>
/// Класс отвечает за логику взаимодействия с контекстными меню - по какому действию они открываются и по какому закрываются.
/// </summary>

public class DefaultContextMenusController : MonoBehaviour
{
    /*
     * На текущий момент логика взаимодейтсвия с контекстными меню такая:
     * Если мы навелись на объект на котором висят элементы контекстного меню, если наша рука поднята, мы начинаем новый отсчет
     * Как только мы отводим взгляд или опустили руку - отсчет прекращается.
     * Как только счетчик сработал - если это меню еще не отображено - отображаем его
     */

    void Awake()
    {
        SourceOfGestures.Instance.SourceDetected += onSourceDetected;
        SourceOfGestures.Instance.SourceLost += onSourceLost;
    }

    public float TriggerDelay = 1.23f;
    private float triggerCounter = 0;
    private bool sourceDetected = false;

    void onSourceDetected()
    {
        sourceDetected = true;
    }

    void onSourceLost()
    {
        sourceDetected = false;
        ResetDelayTrigger();
    }

    public void ResetDelayTrigger()
    {
        triggerCounter = 0;
    }

    bool gazeOnItemMenus = false;
    bool menuOpened = false;
    private void Update()
    {
        if (!menuOpened)
        {
            gazeOnItemMenus = false;
            IContextMenuItems items = null;
            if (GazeManager.Instance.FocusedObject != null)
            {
                items = GazeManager.Instance.FocusedObject.GetComponent<IContextMenuItems>();
                if (items != null)
                {
                    gazeOnItemMenus = true;
                }
            }

            if (sourceDetected && gazeOnItemMenus)
            {
                triggerCounter += Time.deltaTime;

                if (triggerCounter >= TriggerDelay)
                {
                    /*if (items.DesiredContextMenu.CurrentState == IContextMenuState.Closed)
                    {
                        items.DesiredContextMenu.Show(items);
                    }*/
                    items.DesiredContextMenu.Show(items);
                }
            }
            else
            {
                triggerCounter = 0;
            }
        }
    }
}
