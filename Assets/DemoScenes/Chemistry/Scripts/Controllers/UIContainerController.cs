using UnityEngine;
using System.Collections;

public class UIContainerController : MonoBehaviour
{
    [SerializeField]
    private float _interpolationValue = 1f;
    void Update()
    {
        Vector3 eulers = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;
        eulers.z = 0f;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(eulers), _interpolationValue * Time.deltaTime);
    }
}
