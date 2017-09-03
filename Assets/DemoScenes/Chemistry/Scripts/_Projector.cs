using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Projector : MonoBehaviour {

    public GameObject Table;

	// Use this for initialization
	void Start () {

        Vector3 factor = new Vector3(1.5f, 1.0f, 0f);
        this.gameObject.transform.position = Table.transform.TransformPoint(factor);
    }
}