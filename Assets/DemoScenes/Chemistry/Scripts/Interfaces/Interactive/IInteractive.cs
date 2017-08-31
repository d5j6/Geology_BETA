using UnityEngine;
using System.Collections.Generic;
using System;

public interface IInteractive : ILookable
{
    void OnGestureTap();
    List<ActionType> GetAllowedActions();
    bool TryToDrag();
    void StopDrag();
    bool TryToResize();
}

//public interface IInteractiveResize : IInteractive
//{
//    bool TryToResize();
//}

public enum ActionType
{
    TapOnly,
    DragAndDrop,
    Resize
}