using UnityEngine;

public class DemoShowStuff
{
    public GameObject Professor;
    public Button3DVoid TeBeginningOfDemoButton;
    public RingsSelectingController TeBeginningOfDemoButtonRings;
}

public interface IDemoShowState
{
    IDemoShowState NextState { get; set; }
    void enter(System.Action callback = null);
    void exit(System.Action callback = null);
    void interrupt(System.Action callback = null);
}
