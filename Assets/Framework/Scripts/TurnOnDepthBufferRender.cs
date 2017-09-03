using UnityEngine;

public class TurnOnDepthBufferRender : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}
