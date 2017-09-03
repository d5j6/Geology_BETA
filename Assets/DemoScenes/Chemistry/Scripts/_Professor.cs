using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Professor : MonoBehaviour {

    public GameObject Table;

    // Use this for initialization
    void Start () {

        Vector3 factor = new Vector3(-0.85f, 0.2f, 0.0f);
        this.gameObject.transform.position = Table.transform.TransformPoint(-factor);

	}
}
