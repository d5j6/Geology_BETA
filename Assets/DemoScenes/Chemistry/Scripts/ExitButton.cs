using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitButton : MonoBehaviour, IInteractive
{
    [SerializeField]
    private UnityEvent singleTapActions;

    public List<ActionType> GetAllowedActions()
    {
        throw new NotImplementedException();
    }

    public void OnGazeEnter()
    {
        throw new NotImplementedException();
    }

    public void OnGazeLeave()
    {
        throw new NotImplementedException();
    }

    public void OnGestureTap()
    {
        if (singleTapActions != null)
            singleTapActions.Invoke();
    }

    public void StopDrag()
    {
        throw new NotImplementedException();
    }

    public bool TryToDrag()
    {
        throw new NotImplementedException();
    }

    public bool TryToResize()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
