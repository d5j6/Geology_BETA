using UnityEngine;

public class RenderQueueSetter : MonoBehaviour {

    public int Queue = 3000;

	void Start () {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = rend.material;
            if (mat != null)
            {
                mat.renderQueue = Queue;
            }
            else
            {
                Debug.Log("There is no material on renderer to set up render queue!");
            }
        }
        else
        {
            Debug.Log("There is no renderer to set up render queue!");
        }
	}
}
