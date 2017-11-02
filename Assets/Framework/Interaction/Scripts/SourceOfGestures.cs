using UnityEngine;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;
using System;
using System.Collections.Generic;

/// <summary>
/// Класс предоставляет доступ к событиям жестов Хололенса. В общем-то он нужен пока исключительно для удобства тестирования, ибо на компе мы можем эмулировать жесты различным способом
/// </summary>
public class SourceOfGestures : Singleton<SourceOfGestures>
{
    public KeyCode EditorManipulatonKey = KeyCode.R;

    // AndrewMilko
    // private GestureRecognizer gestureRecognizer;
    public GestureRecognizer gestureRecognizer { get; private set; }
    private GestureRecognizer manipulationRecognizer;


    private InteractionSourceState currentHandState;
    /// <summary>
    /// The world space position of the hand being used for the current manipulation gesture.  Not valid
    /// if a manipulation gesture is not in progress.
    /// </summary>
    public Vector3 ManipulationHandPosition
    {
        get
        {
#if UNITY_EDITOR
            return Camera.main.transform.forward * 0.8f;
#else
            Vector3 handPosition = Vector3.zero;
            currentHandState.properties.location.TryGetPosition(out handPosition);
            return handPosition;
#endif
        }
    }

    private void Start()
    {
        InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        InteractionManager.SourcePressed += InteractionManager_SourcePressed;
        InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
        InteractionManager.SourceReleased += InteractionManager_SourceReleased;
        InteractionManager.SourceLost += InteractionManager_SourceLost;
        
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
        
        manipulationRecognizer = new GestureRecognizer();
        manipulationRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);

        manipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        manipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        manipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
        manipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;
        
        gestureRecognizer.StartCapturingGestures();
        manipulationRecognizer.StartCapturingGestures();
    }

    public bool ManipulationInProgress { get; private set; }

    public Action ManipulationStarted;

    private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        // Don't start another manipulation gesture if one is already underway
        if (!ManipulationInProgress)
        {
            OnManipulation(true, cumulativeDelta);
            if (ManipulationStarted != null)
            {
                ManipulationStarted();
            }
        }
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        OnManipulation(true, cumulativeDelta);
    }

    public Action ManipulationCompleted;

    private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        OnManipulation(false, cumulativeDelta);
        if (ManipulationCompleted != null)
        {
            ManipulationCompleted();
        }
    }

    public Action ManipulationCanceled;

    private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        OnManipulation(false, cumulativeDelta);
        if (ManipulationCanceled != null)
        {
            ManipulationCanceled();
        }
    }

    /// <summary>
    /// The offset of the hand from its position at the beginning of 
    /// the currently active manipulation gesture, in world space.  Not valid if
    /// a manipulation gesture is not in progress
    /// </summary>
    public Vector3 ManipulationOffset { get; private set; }

    /// <summary>
    /// The offset of the hand from main Camera's point of view
    /// </summary>
    public Vector3 CameraRelativeOffset { get; private set; }

    private void OnManipulation(bool inProgress, Vector3 offset)
    {
        ManipulationInProgress = inProgress;
        ManipulationOffset = offset;

        CameraRelativeOffset = Camera.main.transform.InverseTransformPoint(Camera.main.transform.position + ManipulationOffset);
    }


    public Action SourceDetected;

    private void InteractionManager_SourceDetected(InteractionSourceState state)
    {
        if (SourceDetected != null && state.source.kind == InteractionSourceKind.Hand)
        {
            SourceDetected.Invoke();
        }
    }

    public Action SourcePressed;

    private bool HandPressed { get { return pressedHands.Count > 0; } }
    private HashSet<uint> pressedHands = new HashSet<uint>();

    private void InteractionManager_SourcePressed(InteractionSourceState state)
    {
        if (state.source.kind == InteractionSourceKind.Hand)
        {
            if (!HandPressed)
            {
                currentHandState = state;
            }

            pressedHands.Add(state.source.id);

            if (SourcePressed != null && state.source.kind == InteractionSourceKind.Hand)
            {
                SourcePressed.Invoke();
            }
        }
    }

    private void InteractionManager_SourceUpdated(InteractionSourceState state)
    {
        if (state.source.kind == InteractionSourceKind.Hand)
        {
            if (HandPressed && state.source.id == currentHandState.source.id)
            {
                currentHandState = state;
            }
        }
    }

    public Action SourceLost;
    public Action SourceReleased;

    private void InteractionManager_SourceReleased(InteractionSourceState state)
    {
        if (state.source.kind == InteractionSourceKind.Hand)
        {
            pressedHands.Remove(state.source.id);

            if (SourceLost != null)
            {
                SourceLost.Invoke();
            }

            if (SourceReleased != null)
            {
                SourceReleased.Invoke();
            }
        }
    }

    private void InteractionManager_SourceLost(InteractionSourceState state)
    {
        if (state.source.kind == InteractionSourceKind.Hand)
        {
            pressedHands.Remove(state.source.id);

            if (SourceLost != null)
            {
                SourceLost.Invoke();
            }

            if (SourceReleased != null)
            {
                SourceReleased.Invoke();
            }
        }
    }

    public Action UserTapped;

    private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (UserTapped != null)
        {
            UserTapped.Invoke();
        }
    }


    private Vector3 editorHandPos;
    private Vector3 getEditorCumulativeDelta()
    {
        return Camera.main.transform.forward * 0.8f - editorHandPos;
    }

    private void updateEditorHandPos()
    {
        editorHandPos = Camera.main.transform.forward * 0.8f;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(EditorManipulatonKey))
        {
            // Don't start another manipulation gesture if one is already underway
            if (!ManipulationInProgress)
            {
                updateEditorHandPos();
                OnManipulation(true, getEditorCumulativeDelta());
                if (ManipulationStarted != null)
                {
                    ManipulationStarted();
                }
            }

            if (SourcePressed != null)
            {
                SourcePressed.Invoke();
            }
        }

        if (Input.GetKey(EditorManipulatonKey))
        {
            OnManipulation(true, getEditorCumulativeDelta());
        }

        if (Input.GetKeyUp(EditorManipulatonKey))
        {
            OnManipulation(false, getEditorCumulativeDelta());
            if (ManipulationCanceled != null)
            {
                ManipulationCanceled();
            }
            if (ManipulationCompleted != null)
            {
                ManipulationCompleted();
            }

            if (SourceReleased != null)
            {
                SourceReleased.Invoke();
            }

            if (UserTapped != null)
            {
                UserTapped.Invoke();
            }
        }
    }
#endif
}
