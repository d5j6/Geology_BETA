using UnityEngine;
using System.Collections;

public class ScreenShotMaker : MonoBehaviour {

    public RenderTexture TargetRenderTexture;
    
    public KeyCode ScreenshootKey = KeyCode.Y;
    public int ResolutionX = 4048;
    public int ResolutionY = 4048;

    int cou = 0;

	// Use this for initialization
	void Start () {
        //Timer.setTask(1.1124f, () => { TakeScreenshot(); });
	}
	
    private void TakeScreenshot()
    {
        RenderTexture.active = TargetRenderTexture;
        Texture2D texture2D = new Texture2D(ResolutionX, ResolutionY, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0, TargetRenderTexture.width, TargetRenderTexture.height), 0, 0);
        texture2D.Apply();

        byte[] bytes = texture2D.EncodeToPNG();
        string filename = "screenshoot_" + cou.ToString() + ".png";
        cou++;
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyUp(ScreenshootKey))
        {
            TakeScreenshot();
        }

    }
}