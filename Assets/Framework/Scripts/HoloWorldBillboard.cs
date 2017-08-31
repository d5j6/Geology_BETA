// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
    public enum HoloWorldPivotAxis
{
        // Rotate about all axes.
        Free,
        // Rotate about an individual axis.
        X,
    Y,
    Z
}

    /// <summary>
    /// The Billboard class implements the behaviors needed to keep a GameObject
    /// oriented towards the user.
    /// </summary>
    public class HoloWorldBillboard : MonoBehaviour
    {
        /// <summary>
        /// The axis about which the object will rotate.
        /// </summary>
        [Tooltip("Specifies the axis about which the object will rotate (Free rotates about both X and Y).")]
        public HoloWorldPivotAxis PivotAxis = HoloWorldPivotAxis.Free;

        /// <summary>
        /// Overrides the cached value of the GameObject's default rotation.
        /// </summary>
        public Quaternion DefaultRotation { get; private set; }

        private void Awake()
        {

        }

    void Start()
    {
        // Cache the GameObject's default rotation.
        SetDefaultRotation();
    }

    public void SetDefaultRotation()
    {
        //DefaultRotation = gameObject.transform.rotation;
        DefaultRotation = Quaternion.identity;
    }

        /// <summary>
        /// Keeps the object facing the camera.
        /// </summary>
        private void Update()
        {
            // Get a Vector that points from the Camera to the target.
            Vector3 directionToTarget = Camera.main.transform.position - gameObject.transform.position;

            // If we are right next to the camera the rotation is undefined.
            if (directionToTarget.sqrMagnitude < 0.001f)
            {
                return;
            }

            // Adjust for the pivot axis.
            switch (PivotAxis)
            {
                case HoloWorldPivotAxis.X:
                //directionToTarget.x = gameObject.transform.position.x;
                directionToTarget.x = 0.0f;
                break;

            case HoloWorldPivotAxis.Y:
                //directionToTarget.y = gameObject.transform.position.y;
                directionToTarget.y = 0.0f;
                break;

            case HoloWorldPivotAxis.Z:
                //directionToTarget.z = gameObject.transform.position.z;
                directionToTarget.z = 0.0f;
                break;

            case HoloWorldPivotAxis.Free:
                default:
                    // No changes needed.
                    break;
            }

        // Calculate and apply the rotation required to reorient the object and apply the default rotation to the result.
        //Debug.Log("Target Rotation: " + Quaternion.LookRotation(-directionToTarget) * DefaultRotation.eulerAngles);
        Vector3 eulerRot = Quaternion.LookRotation(-directionToTarget) * DefaultRotation.eulerAngles;
        eulerRot.x = 0f;
        eulerRot.z = 0f;
        gameObject.transform.rotation = Quaternion.LookRotation(-directionToTarget) * DefaultRotation;
        //gameObject.transform.localRotation = Quaternion.Euler(eulerRot);
    }
    }