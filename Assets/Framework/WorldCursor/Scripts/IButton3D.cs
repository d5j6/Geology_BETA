using UnityEngine.Events;
using UnityEngine;

public enum Button3dState { StaticUnlooked, StaticLooked, Unlooked, Looked, Taped }

[System.Serializable]
public class Button3DVoidEvent : UnityEvent { }
[System.Serializable]
public class Button3DIntEvent : UnityEvent<int> { }
[System.Serializable]
public class Button3DFloatEvent : UnityEvent<float> { }

public interface IButton3D
{
    void OnGazeOver(RaycastHit hitInfo);
    void OnTap(RaycastHit hitInfo);
    void OnGazeLeave();

    bool EnabledByCollider { get; set; }
    bool EnabledByFunctionality { get; set; }
}