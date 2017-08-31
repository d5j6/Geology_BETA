using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Menu : MonoBehaviour {

    public GameObject Table;

    // Use this for initialization
    void Start () {

        Vector3 factor = new Vector3(1, 0, 0f);
        this.gameObject.transform.position = Table.transform.TransformPoint(-factor);
    }
}
