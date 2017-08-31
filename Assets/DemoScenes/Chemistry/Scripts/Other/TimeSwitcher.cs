using UnityEngine;
using System.Collections;

public class TimeSwitcher : MonoBehaviour
{
    int currentIndex = 0;
    float[] speeds = new float[] { 1f, 96f };

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Time.timeScale = speeds[++currentIndex % 2];
        }
    }
}
