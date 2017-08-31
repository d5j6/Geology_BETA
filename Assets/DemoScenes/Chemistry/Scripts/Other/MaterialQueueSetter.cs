using UnityEngine;
using System.Collections;

public class MaterialQueueSetter : MonoBehaviour
{
    [SerializeField]
    private int overrideQueue;

    void Awake()
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        if(meshRend != null)
        {
            meshRend.material.renderQueue = overrideQueue;
        }
    }
}
