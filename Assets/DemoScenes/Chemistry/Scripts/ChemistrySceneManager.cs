using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistrySceneManager : MonoBehaviour, ISceneManager
{

    public bool StartsAutomatically
    {
        get
        {
            return false;
        }
    }

    public void PrepareToUnload(Action callback)
    {
        callback();
    }

    public void StartScene()
    {

    }

    // Use this for initialization
    void Start()
    {
        Loader.Instance.IThinkIWasLoadedCompletelyAndCanStart(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
