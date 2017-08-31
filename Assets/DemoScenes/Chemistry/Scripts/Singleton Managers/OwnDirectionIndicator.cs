using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnDirectionIndicator : MonoBehaviour {

    public GameObject Cursor;
    public GameObject DirectionIndicator;
    private GameObject directionIndicatorClone;

    private Quaternion DirecionIndicatorDefaultRotation = Quaternion.identity;

    private bool isIndicatorVisible;
    private float visibleFactor = 0.2f;
    private float distanceFromCursor = 0.06f;

	// Use this for initialization
	void Start () {
		
        if (DirectionIndicator == null || Cursor == null)
        {
            Debug.Log("Check an assignment of prefabs in " + gameObject.name + ".");
        }

        DirecionIndicatorDefaultRotation = DirectionIndicator.transform.rotation;

        directionIndicatorClone = Instantiate(DirectionIndicator);

        foreach (Rigidbody rigidbody in directionIndicatorClone.GetComponents<Rigidbody>())
        {
            Destroy(rigidbody);
        }

        foreach (Collider collider in directionIndicatorClone.GetComponents<Collider>())
        {
            Destroy(collider);
        }

        directionIndicatorClone.transform.SetParent(this.gameObject.transform);
        directionIndicatorClone.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (DirectionIndicator == null || Cursor == null) { return; }

        if (StartScenario.Instance.IsPeriodicTableActive == true)
        {
            Vector3 camToObjectDirection = this.gameObject.transform.position - Camera.main.transform.position;
            camToObjectDirection.Normalize();

            isIndicatorVisible = !IsTargetVisible();
            directionIndicatorClone.SetActive(isIndicatorVisible);

            if (isIndicatorVisible)
            {
                // Домножить поле "rotation" на переменную "DirectionIndicatorDefaultRotation".

                Vector3 resultedVector = Vector3.ProjectOnPlane(camToObjectDirection, -1 * Camera.main.transform.forward);
                resultedVector.Normalize();

                Vector3 CursorPosition = Cursor.transform.position;

                if (resultedVector == Vector3.zero)
                {
                    resultedVector = Camera.main.transform.right;
                }

                directionIndicatorClone.transform.position = CursorPosition + resultedVector * distanceFromCursor;
                directionIndicatorClone.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, resultedVector) * DirecionIndicatorDefaultRotation;
            }
        }
    }

    private bool IsTargetVisible()
    {
        Vector3 cameraWorldToViewPortVector = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        return (cameraWorldToViewPortVector.x > (visibleFactor - 0.5f) && cameraWorldToViewPortVector.x < ((1 - visibleFactor) + 1.5f) && cameraWorldToViewPortVector.y > (visibleFactor - 0.5f) &&
                cameraWorldToViewPortVector.y < 1 - (visibleFactor - 0.2f) && cameraWorldToViewPortVector.z > 0);
    }
}
