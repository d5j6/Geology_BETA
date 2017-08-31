using System;
using UnityEngine;

public class Button3DFloat : MonoBehaviour, IButton3D
{
    public float PassedParameter = 0;
    bool onCooldown = false;
    public float Cooldown = 0.0f;

    [SerializeField]
    public Button3DFloatEvent OnTapEvent;
    [SerializeField]
    public Button3DFloatEvent OnGazeOverEvent;
    [SerializeField]
    public Button3DFloatEvent OnGazeLeaveEvent;

    [SerializeField]
    public Button3DFloatEvent OnEnabledEvent;
    [SerializeField]
    public Button3DFloatEvent OnDisabledEvent;

    public bool EnabledByCollider
    {
        get
        {
            return false;
        }
        set
        {
            GetComponent<Collider>().enabled = value;

            if (value)
            {
                if (OnEnabledEvent != null)
                {
                    OnEnabledEvent.Invoke(PassedParameter);
                }
            }
            else
            {
                if (OnDisabledEvent != null)
                {
                    OnDisabledEvent.Invoke(PassedParameter);
                }
            }
        }
    }

    bool _enabledByFunctionality = true;
    public bool EnabledByFunctionality
    {
        get
        {
            return _enabledByFunctionality;
        }
        set
        {
            _enabledByFunctionality = value;
        }
    }

    public void OnGazeLeave()
    {
        if (OnGazeLeaveEvent != null)
        {
            OnGazeLeaveEvent.Invoke(PassedParameter);
        }
    }

    public void OnGazeOver(RaycastHit hitInfo)
    {
        if (OnGazeOverEvent != null)
        {
            OnGazeOverEvent.Invoke(PassedParameter);
        }
    }

    public void OnTap(RaycastHit hitInfo)
    {
        if (OnTapEvent != null)
        {
            if (EnabledByFunctionality)
            {
                if (!onCooldown)
                {
                    OnTapEvent.Invoke(PassedParameter);
                    if (Cooldown > 0.0f)
                    {
                        onCooldown = true;
                        LeanTween.delayedCall(gameObject, Cooldown, () => { onCooldown = false; });
                    }
                }
            }
        }
    }

    protected virtual void onGazeOverAction()
    {

    }

    protected virtual void onGazeLeaveAction()
    {

    }

    protected virtual void onTapAction()
    {

    }
}
