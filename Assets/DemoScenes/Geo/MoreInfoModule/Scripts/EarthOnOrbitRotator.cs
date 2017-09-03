using UnityEngine;
using System.Collections;

public class EarthOnOrbitRotator : MonoBehaviour
{
    public float angularSpeed = -45f;

    void Update()
    {
        transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime, Space.Self);
    }
}
