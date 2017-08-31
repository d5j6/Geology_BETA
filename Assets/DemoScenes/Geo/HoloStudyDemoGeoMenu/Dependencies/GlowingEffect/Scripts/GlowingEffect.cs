using UnityEngine;
using System.Collections;

public class GlowingEffect : MonoBehaviour {

    public float rotatingSpeed = 3.6f;
    public float scalingSpeed = 1f;
    Vector3 initialScale;
    float scaleDeviation = 0.1f;
    float counter = 0;

    void Start()
    {
        initialScale = transform.GetChild(0).localScale;
    }

	// Update is called once per frame
	void Update () {
        transform.GetChild(0).Rotate(Vector3.forward, rotatingSpeed * Time.deltaTime);
        counter += Time.deltaTime;
        transform.GetChild(0).localScale = initialScale * (1 + scaleDeviation * Mathf.Sin(counter * scalingSpeed));
    }
}
