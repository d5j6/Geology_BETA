using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogWindowScript : MonoBehaviour
{

    private void ApplicationOnLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (condition[0] != '!')
            return;

        //Increase Text height when add new message
        float x = logText.preferredHeight;
        logText.rectTransform.sizeDelta = new Vector2(logText.minWidth, x + 10);
        //Add debug text into Text object
        logText.text += (condition + "\n");
        //Move scrollRect down
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }


    private Text logText;
    private ScrollRect scrollRect;

    // Use this for initialization
    void Start()
    {
        //Find and save Text and ScrollRect objects
        logText = GameObject.Find("ContentText").GetComponent<Text>();
        scrollRect = GameObject.Find("ScrollRect").GetComponent<ScrollRect>();
        //Intercepting all debug messages
        Application.logMessageReceived += ApplicationOnLogMessageReceived;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
