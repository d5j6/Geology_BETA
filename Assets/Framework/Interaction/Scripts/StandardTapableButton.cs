using System;
using UnityEngine;
using UnityEngine.Events;

public class StandardTapableButton : StandardSimpleButton, IButton3D
{
    [SerializeField]
    private UnityEvent singleTapActions;
    [SerializeField]
    private UnityEvent doubleTapActions;

    [SerializeField]
    private UnityEvent gazeOverActions;
    [SerializeField]
    private UnityEvent gazeLeaveActions;

    protected override void singleTapAction()
    {
        base.singleTapAction();

        if (singleTapActions != null)
        {
            singleTapActions.Invoke();
        }
    }

    protected override void doubleTapAction()
    {
        base.doubleTapAction();

        if (doubleTapActions != null)
        {
            doubleTapActions.Invoke();
        }
    }

    public void OnGazeOver(RaycastHit hitInfo)
    {
        if (gazeOverActions != null)
        {
            gazeOverActions.Invoke();
        }
    }

    public void OnGazeLeave()
    {
        if (gazeLeaveActions != null)
        {
            gazeLeaveActions.Invoke();
        }
    }

    public void OnTap(RaycastHit hitInfo)
    {

    }

    public bool EnabledByCollider
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public bool EnabledByFunctionality
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
}
