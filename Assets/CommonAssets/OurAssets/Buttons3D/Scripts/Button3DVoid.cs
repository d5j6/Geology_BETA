using UnityEngine;


public class Button3DVoid : MonoBehaviour, IButton3D
{
    bool onCooldown = false;
    public float Cooldown = 0.0f;

    [SerializeField]
    public Button3DVoidEvent OnTapEvent;
    [SerializeField]
    public Button3DVoidEvent OnGazeOverEvent;
    [SerializeField]
    public Button3DVoidEvent OnGazeLeaveEvent;

    [SerializeField]
    public Button3DVoidEvent OnEnabledEvent;
    [SerializeField]
    public Button3DVoidEvent OnDisabledEvent;

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
                    OnEnabledEvent.Invoke();
                }
            }
            else
            {
                if (OnDisabledEvent != null)
                {
                    OnDisabledEvent.Invoke();
                }
            }
        }
    }

    public bool _enabledByFunctionality = true;
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
            OnGazeLeaveEvent.Invoke();
            onGazeLeaveAction();
        }
    }

    public void OnGazeOver(RaycastHit hitInfo)
    {
        if (OnGazeOverEvent != null)
        {
            OnGazeOverEvent.Invoke();
            onGazeOverAction();
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
                    OnTapEvent.Invoke();
                    onTapAction();
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
