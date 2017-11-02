using UnityEngine;
using HoloToolkit.Unity;

public class WorldCursor : Singleton<WorldCursor> {

    private MeshRenderer meshRenderer;

    public static GameObject FocusedObject;
    private GameObject oldFocusedObject;

    public System.Action AnyTapCallback;
    public System.Action OnUserTapped;

    public KeyCode EditorTapKey = KeyCode.F;

    void LateUpdate()
    {
        if (GazeManager.Instance == null)
        {
            return;
        }
        
        gameObject.transform.position = GazeManager.Instance.Position + GazeManager.Instance.Normal * 0.01f;
        
        transform.rotation = Quaternion.FromToRotation(Vector3.back, GazeManager.Instance.Normal);

        IButton3D[] button3d;
        if (GazeManager.Instance.FocusedObject != null)
        {
            FocusedObject = GazeManager.Instance.FocusedObject;

            if ((FocusedObject != oldFocusedObject) && (oldFocusedObject != null))
            {
                button3d = oldFocusedObject.GetComponents<IButton3D>();
                if (button3d != null)
                {
                    for (int i = 0; i < button3d.Length; i++)
                    {
                        button3d[i].OnGazeLeave();
                    }
                }
            }

            oldFocusedObject = FocusedObject;

            button3d = FocusedObject.GetComponents<IButton3D>();
            if (button3d != null)
            {
                for (int i = 0; i < button3d.Length; i++)
                {
                    button3d[i].OnGazeOver(GazeManager.Instance.HitInfo);
                }
            }

            meshRenderer.enabled = true;
        }
        else
        {
            if (FocusedObject != null)
            {
                button3d = FocusedObject.GetComponents<IButton3D>();
                if (button3d != null)
                {
                    for (int i = 0; i < button3d.Length; i++)
                    {
                        button3d[i].OnGazeLeave();
                    }
                }
            }

            FocusedObject = null;

            meshRenderer.enabled = false;
        }

#if UNITY_EDITOR
        if (Input.GetKeyUp(EditorTapKey))
        {
            StandartOnTapEvent();
        }
#endif
    }

    void Start()
    {
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
    }

    void StandartOnTapEvent()
    {
        if (AnyTapCallback != null)
        {
            AnyTapCallback.Invoke();
            AnyTapCallback = null;
            return;
        }

        if (OnUserTapped != null)
        {
            OnUserTapped.Invoke();
        }

        if (GazeManager.Instance.FocusedObject != null)
        {
            IButton3D[] button3d = GazeManager.Instance.FocusedObject.GetComponents<IButton3D>();
            if (button3d != null)
            {
                for (int i = 0; i < button3d.Length; i++)
                {
                    button3d[i].OnTap(GazeManager.Instance.HitInfo);
                }
            }
        }
    }
}