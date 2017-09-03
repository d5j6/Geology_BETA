using System;

public class InterruptExitFromDemoState : IDemoShowState
{
    public DemoShowStuff demoShowStuff;

    private Action onStageEnd;

    public IDemoShowState NextState
    {
        get;
        set;
    }

    public void enter(Action callback = null)
    {
        exitFromDemo();
    }

    private void exitFromDemo()
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
