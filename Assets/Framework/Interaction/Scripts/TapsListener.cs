﻿using UnityEngine;
using System;
using HoloToolkit.Unity;
using System.Collections;

public class TapsListener : Singleton<TapsListener>
{
    private float doubleClickTimer = 0;
    private bool fistClick = false;
    /// <summary>
    /// Максимальный интервал времени между двумя законченными тапами, при котором засчитывается двойной тап
    /// </summary>
    public float DoubleClickSpeed = 0.58f;
    public Action<GameObject> UserSingleTapped;
    public Action<GameObject> UserDoubleTapped;
    public Action<GameObject> UserTapped;
    
    /// <summary>
    /// Сколько ждать после нажатия, чтобы сработало собитые Pressed
    /// </summary>
    public float TapPressedDelay = 0.33f;
    public Action<GameObject> UserSingleTapPressed;
    public Action<GameObject> UserDoubleTapPressed;

    /*
     * Переменная нужна потому что есть один момент, когда мы не хотим слушать клики - это сразу после отжатия клика после события перетаскивания. Мы хотим чтобы перетаскивание заканчивалось
     * и клик в момент конца перетаскивания не детектился
     */

    private bool listeningForTaps = true;

    // Use this for initialization
    private void Start()
    {
        SourceOfGestures.Instance.UserTapped += TapEventHandler;

        SourceOfGestures.Instance.SourcePressed += PressedEventHandler;
        SourceOfGestures.Instance.SourceReleased += ReleasedEventHandler;
    }

    /*
     * Событие тапа и удерживания
     */

    private enum TapState { Pressed, Released }
    private TapState tapState = TapState.Released;
    private float tapCounter;
    private TapState doubleTapState = TapState.Released;
    private float doubleTapCounter;

    private void PressedEventHandler()
    {
        if (fistClick)
        {
            doubleTapState = TapState.Pressed;
            doubleTapCounter = 0f;
            tapPressedInvoked = false;
            doubleTapPressedInvoked = false;
            tapState = TapState.Released;
        }
        else
        {
            tapState = TapState.Pressed;
            tapCounter = 0f;
            tapPressedInvoked = false;
            doubleTapPressedInvoked = false;
            doubleTapState = TapState.Released;

            gameObjectFocusedWhenPressStarted = GazeManager.Instance.FocusedObject;
        }
    }

    private void ReleasedEventHandler()
    {
        tapState = TapState.Released;
        doubleTapState = TapState.Released;
        doubleTapCounter = 0f;
        tapCounter = 0f;
        tapPressedInvoked = false;
        doubleTapPressedInvoked = false;

        //Ждем 1 кадр, ибо мы хз, в какой очередности происходят события Тапа и SourceReleased'а.
        StartCoroutine(waitForFramesAndListenTaps(1));
    }

    private IEnumerator waitForFramesAndListenTaps(float framesToWait)
    {
        for (int i = 0; i < framesToWait; i++)
        {
            yield return null;
        }
        listeningForTaps = true;
    }


    private void TapEventHandler()
    {
        if (listeningForTaps)
        {
            if (UserTapped != null)
            {
                UserTapped.Invoke(GazeManager.Instance.FocusedObject);
            }

            if (fistClick)
            {
                if (doubleClickTimer <= DoubleClickSpeed)
                {
                    Debug.Log("Double Tap Invoked!");
                    if (UserDoubleTapped != null)
                    {
                        UserDoubleTapped.Invoke(gameObjectFocusedWhenPressStarted);
                    }
                }

                fistClick = false;
            }
            else
            {
                doubleClickTimer = 0;
                fistClick = true;
            }
        }
    }
    private bool tapPressedInvoked = false;
    private bool doubleTapPressedInvoked = false;

    private GameObject gameObjectFocusedWhenPressStarted;

    private void Update()
    {
        if (tapState == TapState.Pressed)
        {
            tapCounter += Time.deltaTime;
            //Debug.Log("tapCounter = " + tapCounter + " | TapPressedDelay = " + TapPressedDelay);
            if (!tapPressedInvoked && tapCounter >= TapPressedDelay)
            {
                tapPressedInvoked = true;
                Debug.Log("Single Tap Pressed Invoked!");
                if (UserSingleTapPressed != null)
                {
                    UserSingleTapPressed.Invoke(gameObjectFocusedWhenPressStarted);
                }

                //Временно выключаем слушание кликов - пока не закончили драг
                listeningForTaps = false;
            }
        }
        else if (doubleTapState == TapState.Pressed)
        {
            doubleTapCounter += Time.deltaTime;
            if (!doubleTapPressedInvoked && doubleTapCounter >= TapPressedDelay)
            {
                fistClick = false;
                doubleTapPressedInvoked = true;
                Debug.Log("User Double Tap Pressed Invoked!");
                if (UserDoubleTapPressed != null)
                {
                    UserDoubleTapPressed.Invoke(gameObjectFocusedWhenPressStarted);
                }

                //Временно выключаем слушание кликов - пока не закончили драг
                listeningForTaps = false;
            }
        }

        if (fistClick)
        {
            doubleClickTimer += Time.deltaTime;

            if (doubleClickTimer > DoubleClickSpeed)
            {
                doubleClickTimer = 0;
                Debug.Log("Single Tap Invoked!");
                if (UserSingleTapped != null)
                {
                    UserSingleTapped.Invoke(gameObjectFocusedWhenPressStarted);
                }

                fistClick = false;
            }
        }
    }
}
