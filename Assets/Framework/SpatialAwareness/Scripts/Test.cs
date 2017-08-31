using UnityEngine;
public class Test : MonoBehaviour
{
    CubeMatrixCoordinatesEnumerable positions = new CubeMatrixCoordinatesEnumerable();
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.K))
        {
            Debug.Log("Next Vector: " + positions.GetNext());
        }
	}
}
