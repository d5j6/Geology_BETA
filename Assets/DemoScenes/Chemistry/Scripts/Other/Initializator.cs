using UnityEngine;
using System.Collections;

public class Initializator : MonoBehaviour
{
    [SerializeField]
    private SequenceInitializator seqInitilizator;

    void Awake()
    {
        DataManager.Instance.Initialize();
        OwnGazeManager.Instance.Initialize();
        OwnGestureManager.Instance.Initialize();
        PlayerManager.Instance.Initialize();
    }
}
