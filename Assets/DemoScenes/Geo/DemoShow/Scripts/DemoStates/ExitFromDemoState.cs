using System;

public class ExitFromDemoState : IDemoShowState
{
    public DemoShowStuff demoShowStuff;

    Action onStageEnd;

    public IDemoShowState NextState
    {
        get;
        set;
    }

    public void enter(Action callback = null)
    {
        exitFromDemo();
    }

    void exitFromDemo()
    {
        DemoShowStateMachine.Instance.ResetStateMachine();
    }

    public void exit(Action callback = null)
    {

    }

    public void interrupt(Action callback = null)
    {

    }
}
