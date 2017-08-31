using UnityEngine;
using System.Collections;

public class CameraLookerAt : MonoBehaviour {

    public Transform lookAtTransform;

	void Update () {
        transform.LookAt(lookAtTransform);
	}
}
