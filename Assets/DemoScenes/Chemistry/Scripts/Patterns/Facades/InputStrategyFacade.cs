using UnityEngine;
using System.Collections;
using System;

public class InputStrategyFacade
{
    public enum Strategies { None, Default, Standart, Demonstration, Resize, DragAndDrop}

    public Strategies Strategy { get; private set; }

    public void SetListeners(Action<IInteractive> gazeEnterHandler, Action<IInteractive> gazeLeaveHandler, Action<IInteractive> gestureTapHandler)
    {
        OwnGazeManager.Instance.OnGazeEnterToInteractiveEvent += gazeEnterHandler;
        OwnGazeManager.Instance.OnGazeLeaveFromInteractiveEvent += gazeLeaveHandler;
        OwnGestureManager.Instance.OnTapEvent += gestureTapHandler;
    }

    public void SetListeneresForNavigation(Action<IInteractive> startNavigation, Action<IInteractive> updateNavigation/*, Action<IInteractive> completeNavigation, Action<IInteractive> cancelNavigation*/)
    {
        OwnGestureManager.Instance.NavStart += startNavigation;
        OwnGestureManager.Instance.NavUpdate += updateNavigation;
        //OwnGestureManager.Instance.navComplete += completeNavigation;
        //OwnGestureManager.Instance.navCancel += cancelNavigation;
    }

    public void ChangeStrategyToDefault()
    {
        if (Strategy == Strategies.Default)
            return;

        Strategy = Strategies.Default;
        OwnGazeManager.Instance.ChangeStrategyToDefault();
        OwnGestureManager.Instance.ChangeStrategyToDefault();
    }

    public void ChangeStrategyToDragAndDrop()
    {
        
        Strategy = Strategies.DragAndDrop;
        OwnGazeManager.Instance.ChangeStrategyToDragAndDrop();
        OwnGestureManager.Instance.ChangeStrategyToDragAndDrop();
    }

    public void ChangeStrategyToResize()
    {
        
        Strategy = Strategies.Resize;
        OwnGazeManager.Instance.ChangeStrategyToDefault();
        OwnGestureManager.Instance.ChangeStrategyToResize();
    }
}
