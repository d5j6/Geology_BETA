using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class SphericalOrbitMoveManipulator : MonoBehaviour
{
    public float MovingSensitivity = 1f;

    float distanceFromCamera;
    Vector3 normalizedRelativeDirection;
    Interpolator targetInterpolator;

    public void UpdateDirectionSoItWasInFrontOfTheCamera()
    {
        normalizedRelativeDirection = Camera.main.transform.forward;
    }

    void Awake()
    {
        normalizedRelativeDirection = (transform.position - Camera.main.transform.position).normalized;
        distanceFromCamera = (transform.position - Camera.main.transform.position).magnitude;

        NativeManipulationManager.Instance.NativeSingleTapManipulationStarted += onNativeSingleTapManipulationStarted;
        NativeManipulationManager.Instance.NativeSingleTapManipulationUpdated += onNativeSingleTapManipulationUpdated;
        NativeManipulationManager.Instance.NativeSingleTapManipulationCompleted += onNativeSingleTapManipulationCompleted;
        targetInterpolator = GetComponent<Interpolator>();
    }

    void Update()
    {
        transform.position = Camera.main.transform.position + normalizedRelativeDirection * distanceFromCamera;
    }

    public bool EnableMove = true;

    private bool whenManipulationStartedUserWatchedOnMe = false;
    private Vector3 initialObjectPosition;

    Vector3 currentMovementOffset = Vector3.zero;

    void onNativeSingleTapManipulationStarted(GameObject targetGO)
    {
        if (EnableMove && targetGO == gameObject)
        {
            whenManipulationStartedUserWatchedOnMe = true;
            initialObjectPosition = Camera.main.transform.InverseTransformPoint(transform.position);
        }
    }

    void onNativeSingleTapManipulationUpdated(Vector3 offset)
    {
        if (EnableMove && whenManipulationStartedUserWatchedOnMe)
        {
            // When performing a manipulation gesture, the hand generally only translates a relatively small amount.
            // If we move the object only as much as the hand itself moves, users can only make small adjustments before
            // the hand is lost and the gesture completes.  To improve the usability of the gesture we scale each
            // axis of hand movement by some amount (camera relative).  This value can be changed in the editor or
            // at runtime based on the needs of individual movement scenarios.
            /*Vector3 scaledLocalHandPositionDelta = Vector3.Scale(offset, NativeManipulationManager.Instance.handPositionScale);

            // Once we've figured out how much the object should move relative to the camera we apply that to the initial
            // camera relative position.  This ensures that the object remains in the appropriate location relative to the camera
            // and the hand as the camera moves.  The allows users to use both gaze and gesture to move objects.  Once they
            // begin manipulating an object they can rotate their head or walk around and the object will move with them
            // as long as they maintain the gesture, while still allowing adjustment via hand movement.
            Vector3 localObjectPosition = initialObjectPosition + scaledLocalHandPositionDelta * MovingSensitivity;
            Vector3 worldObjectPosition = Camera.main.transform.TransformPoint(localObjectPosition);
            
            Vector3 finalObjectPosition = Camera.main.transform.position + (worldObjectPosition - Camera.main.transform.position).normalized * distanceFromCamera;*/

            //normalizedRelativeDirection = (finalObjectPosition - Camera.main.transform.position).normalized;
            normalizedRelativeDirection = Camera.main.transform.forward;
        }
    }

    void onNativeSingleTapManipulationCompleted()
    {
        whenManipulationStartedUserWatchedOnMe = false;
    }
}
