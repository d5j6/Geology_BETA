using UnityEngine;
using System.Collections;

public class Initializator : Singleton<Initializator>
{
    [SerializeField]
    private SequenceInitializator seqInitilizator;

    private void Awake()
    {
        DataManager.Instance.Initialize();
        OwnGazeManager.Instance.Initialize();
        OwnGestureManager.Instance.Initialize();
        PlayerManager.Instance.Initialize();
        Debug.Log("Initializator works");
    }
}
