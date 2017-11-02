using UnityEngine;
using HoloToolkit.Unity;
using System;

public class DemoShowStateMachine : Singleton<DemoShowStateMachine> {

    public Action DemoShowStartedPlaying;
    public Action DemoShowEndedPlaying;

    public static bool playing = false;

    public GameObject Professor;
    public Button3DVoid TeBeginningOfDemoButton;
    public RingsSelectingController TeBeginningOfDemoButtonRings;

    private DemoShowStuff demoShowStuff;
    private GreetingsAndProfessorState greetingsAndProfessorState;
    private ClickNextForContinuingState clickNextForContinuingState1;
    private EarthInfoState earthInfoState;
    private ClickNextForContinuingState clickNextForContinuingState2;
    private SlicesState slicesState;
    private ClickNextForContinuingState clickNextForContinuingState3;
    private PieDemoState pieDemoState;
    private ExitFromDemoState exitFromDemoState;
    private InterruptExitFromDemoState interruptExitFromDemoState;
    private IDemoShowState state;
    private Action<IDemoShowState, Action> onExit;

    private void Awake()
    {
        demoShowStuff = new DemoShowStuff();
        demoShowStuff.Professor = Professor;
        demoShowStuff.TeBeginningOfDemoButton = TeBeginningOfDemoButton;
        demoShowStuff.TeBeginningOfDemoButtonRings = TeBeginningOfDemoButtonRings;

        greetingsAndProfessorState = new GreetingsAndProfessorState();
        clickNextForContinuingState1 = new ClickNextForContinuingState();
        greetingsAndProfessorState.NextState = clickNextForContinuingState1;
        greetingsAndProfessorState.demoShowStuff = demoShowStuff;
        clickNextForContinuingState1.demoShowStuff = demoShowStuff;
        earthInfoState = new EarthInfoState();
        clickNextForContinuingState1.NextState = earthInfoState;
        earthInfoState.demoShowStuff = demoShowStuff;
        clickNextForContinuingState2 = new ClickNextForContinuingState();
        earthInfoState.NextState = clickNextForContinuingState2;
        clickNextForContinuingState2.demoShowStuff = demoShowStuff;
        slicesState = new SlicesState();
        clickNextForContinuingState2.NextState = slicesState;
        slicesState.demoShowStuff = demoShowStuff;
        clickNextForContinuingState3 = new ClickNextForContinuingState();
        slicesState.NextState = clickNextForContinuingState3;
        clickNextForContinuingState3.demoShowStuff = demoShowStuff;
        pieDemoState = new PieDemoState();
        clickNextForContinuingState3.NextState = pieDemoState;
        pieDemoState.demoShowStuff = demoShowStuff;
        exitFromDemoState = new ExitFromDemoState();
        pieDemoState.NextState = exitFromDemoState;
        exitFromDemoState.demoShowStuff = demoShowStuff;
        exitFromDemoState.NextState = null;

        interruptExitFromDemoState = new InterruptExitFromDemoState();
        interruptExitFromDemoState.demoShowStuff = demoShowStuff;

        //clickNextForContinuingState1.NextState = pieDemoState;
    }

    public void ResetStateMachine()
    {
        state = null;
        playing = false;
        
        if (DemoShowEndedPlaying != null)
        {
            DemoShowEndedPlaying.Invoke();
        }
    }

    private void Start()
    {
        if (playing)
        {
            StartDemoShow();
        }
    }

    public void StartDemoShow()
    {
        playing = true;
        if (DemoShowStartedPlaying != null)
        {
            DemoShowStartedPlaying.Invoke();
        }
        GotoState(greetingsAndProfessorState);
    }

    public void GotoState(IDemoShowState newState, Action callback = null)
    {
        if ((newState != null) && (newState != state))
        {
            if (state == null)
            {
                state = newState;

                state.enter(callback);
            }
            else
            {
                onExit = (IDemoShowState s, Action cb) =>
                {
                    state = s;
                    state.enter(cb);
                };
                state.exit(() =>
                {
                    if (onExit != null)
                    {
                        onExit(newState, callback);
                    }
                });
            }
        }
    }

    public void Interrupt(Action callback = null)
    {
        if (state != null)
        {
            onExit = null;
            state.interrupt(() =>
            {
                ResetStateMachine();
            });
        }
        else
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }
}
