using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _angles = new Vector3(45f, 45f, 45f);

    void Update()
    {
        transform.Rotate(_angles * Time.deltaTime);
    }
}
