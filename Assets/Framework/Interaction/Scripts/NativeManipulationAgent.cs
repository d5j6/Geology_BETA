using System;
using HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// Класс берет триггеры, кажущиеся нам нативными и истолковывает их как определенные манипуляции. В общем-то в большинстве мест это просто прямой посредник от NativeManipulationManager'а, 
/// но кое-где он вносит и свои коррективы, например, приводит выход осевых вращений NativeManipulationManager'а в формату ротации IGestureManipulationAgent'а. Вообще-то скорее всего это 
/// лишняя прослойка. Надо убрать, когда время будет.
/// </summary>
public class NativeManipulationAgent : Singleton<NativeManipulationAgent>, IGestureManipulationAgent
{
    public Action<Vector3> Move { get; set; }

    public Action<Vector3> Rotate { get; set; }

    public Action<Vector3> Scale { get; set; }


    public Action<GameObject> MoveStarted { get; set; }

    public Action MoveEnded { get; set; }

    public Action<GameObject> RotationStarted { get; set; }

    public Action RotationEnded { get; set; }

    public Action<GameObject> ScaleStarted { get; set; }

    public Action ScaleEnded { get; set; }

    private void Start()
    {
        NativeManipulationManager.Instance.NativeXManipulationUpdated += onNativeXManipulationUpdated;
        NativeManipulationManager.Instance.NativeYManipulationUpdated += onNativeYManipulationUpdated;
        NativeManipulationManager.Instance.NativeZManipulationUpdated += onNativeZManipulationUpdated;
        NativeManipulationManager.Instance.NativeXYManipulationUpdated += onNativeXYManipulationUpdated;
        NativeManipulationManager.Instance.NativeDoubleTapManipulationUpdated += onNativeDoubleTapManipulationUpdated;

        NativeManipulationManager.Instance.NativeDoubleTapManipulationStarted += onNativeDoubleTapManipulationStarted;
        NativeManipulationManager.Instance.NativeDoubleTapManipulationCompleted += onNativeDoubleTapManipulationCompleted;

        NativeManipulationManager.Instance.NativeXManipulationStarted += onNativeXManipulationStarted;
        NativeManipulationManager.Instance.NativeXManipulationCompleted += onNativeXManipulationCompleted;

        NativeManipulationManager.Instance.NativeYManipulationStarted += onNativeYManipulationStarted;
        NativeManipulationManager.Instance.NativeYManipulationCompleted += onNativeYManipulationCompleted;

        NativeManipulationManager.Instance.NativeXYManipulationStarted += onNativeXYManipulationStarted;
        NativeManipulationManager.Instance.NativeXYManipulationCompleted += onNativeXYManipulationCompleted;
    }

    void onNativeXManipulationUpdated(float offset)
    {
        if (Rotate != null)
        {
            Rotate(new Vector3(offset, 0f, 0f));
        }
    }

    void onNativeYManipulationUpdated(float offset)
    {
        if (Rotate != null)
        {
            Rotate(new Vector3(0f, offset, 0f));
        }
    }

    void onNativeZManipulationUpdated(float offset)
    {

    }

    void onNativeXYManipulationUpdated(float offset)
    {
        if (Scale != null)
        {
            Scale(new Vector3(offset, offset, offset));
        }
    }

    void onNativeDoubleTapManipulationUpdated(Vector3 offset)
    {
        if (Move != null)
        {
            Move(offset);
        }
    }


    void onNativeDoubleTapManipulationStarted(GameObject targetGO)
    {
        if (MoveStarted != null)
        {
            MoveStarted.Invoke(targetGO);
        }
    }

    void onNativeDoubleTapManipulationCompleted()
    {
        if (MoveEnded != null)
        {
            MoveEnded.Invoke();
        }
    }

    void onNativeXManipulationStarted(GameObject targetGO)
    {
        if (RotationStarted != null)
        {
            RotationStarted.Invoke(targetGO);
        }
    }

    void onNativeXManipulationCompleted()
    {
        if (RotationEnded != null)
        {
            RotationEnded.Invoke();
        }
    }

    void onNativeYManipulationStarted(GameObject targetGO)
    {
        if (RotationStarted != null)
        {
            RotationStarted.Invoke(targetGO);
        }
    }

    void onNativeYManipulationCompleted()
    {
        if (RotationEnded != null)
        {
            RotationEnded.Invoke();
        }
    }

    void onNativeXYManipulationStarted(GameObject targetGO)
    {
        if (ScaleStarted != null)
        {
            ScaleStarted.Invoke(targetGO);
        }
    }

    void onNativeXYManipulationCompleted()
    {
        if (ScaleEnded != null)
        {
            ScaleEnded.Invoke();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (NativeManipulationManager.Instance != null)
        {
            NativeManipulationManager.Instance.NativeXManipulationUpdated -= onNativeXManipulationUpdated;
            NativeManipulationManager.Instance.NativeYManipulationUpdated -= onNativeYManipulationUpdated;
            NativeManipulationManager.Instance.NativeZManipulationUpdated -= onNativeZManipulationUpdated;
            NativeManipulationManager.Instance.NativeXYManipulationUpdated -= onNativeXYManipulationUpdated;
            NativeManipulationManager.Instance.NativeDoubleTapManipulationUpdated -= onNativeDoubleTapManipulationUpdated;

            NativeManipulationManager.Instance.NativeDoubleTapManipulationStarted -= onNativeDoubleTapManipulationStarted;
            NativeManipulationManager.Instance.NativeDoubleTapManipulationCompleted -= onNativeDoubleTapManipulationCompleted;

            NativeManipulationManager.Instance.NativeXManipulationStarted -= onNativeXManipulationStarted;
            NativeManipulationManager.Instance.NativeXManipulationCompleted -= onNativeXManipulationCompleted;

            NativeManipulationManager.Instance.NativeYManipulationStarted -= onNativeYManipulationStarted;
            NativeManipulationManager.Instance.NativeYManipulationCompleted -= onNativeYManipulationCompleted;

            NativeManipulationManager.Instance.NativeXYManipulationStarted -= onNativeXYManipulationStarted;
            NativeManipulationManager.Instance.NativeXYManipulationCompleted -= onNativeXYManipulationCompleted;
        }
    }
}
