using UnityEngine;

public class StatesStuffContainer
{
    public IDemoSceneState CurrentState;
    public IDemoSceneState LastState;

    public bool LoadPrefabsDynamically = false;

    public Transform AllParent;
    public GameObject Earth;
    public GameObject BigInfoPanel;
    public GameObject Menu;
    public GameObject Pie;
}

public interface IDemoSceneState
{
    Fader ButtonFader { set; get; }
    RingsSelectingController ButtonRings { set; get; }
    bool EarthState { get; }
    bool RecursiveState { get; }

    void enter(System.Action callback = null);
    void exit(System.Action callback);
}
