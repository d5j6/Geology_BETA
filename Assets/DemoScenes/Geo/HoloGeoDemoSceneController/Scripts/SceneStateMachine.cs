using System;
using UnityEngine;
using HoloToolkit.Unity;

public class SceneStateMachine : Singleton<SceneStateMachine>, ISceneManager
{
    private GameObject Earth;
    private GameObject BigInfoPanel;
    private GameObject Menu;
    private GameObject Pie;

    /*
     * States
     */

    private IDemoSceneState state;
    private StatesStuffContainer statesStuffObject = new StatesStuffContainer();
    private BeginningVoidState beginningVoidState;
    private DefaultViewState defaultViewState;
    private SlicedEarthState slicedEarthState;
    private EarthState earthState;
    private PieState pieState;
    private MassMeasuringState massMeasuringState;
    private MagneticFieldMeasuringState magneticFieldMeasuringState;
    private TemperatureState temperatureState;

    public Action StateChangeStarted;
    public Action StateChangeEnded;

    private IDemoSceneState firstPryorityState;
    private bool stateProcessing = false;
    private Action firstPryorityStateCallback;

    public void GoToDefaultState(Action callback = null)
    {
        gotoState(defaultViewState, callback);
    }

    public void GoToEarthState(Action callback = null)
    {
        gotoState(earthState, callback);
    }

    public void GoToSlicedEarthState(Action callback = null)
    {
        gotoState(slicedEarthState, callback);
    }

    public void GoToPieState(Action callback = null)
    {
        gotoState(pieState, callback);
    }

    public void GoToMassMeasuringState(Action callback = null)
    {
        gotoState(massMeasuringState, callback);
    }

    public void GoToMagneticFieldMeasuringState(Action callback = null)
    {
        gotoState(magneticFieldMeasuringState, callback);
    }

    public void GoToTemperatureState(Action callback = null)
    {
        gotoState(temperatureState, callback);
    }

    public void GoToBeginningVoidState(Action callback = null)
    {
        gotoState(beginningVoidState, callback);
    }

    public bool IsEarthContainingState()
    {
        return state.EarthState;
    }

    public bool IsEarthState()
    {
        return state is EarthState || state is DefaultViewState;
    }

    public bool IsPieState()
    {
        return state is PieState;
    }

    public bool StartsAutomatically
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public void PrepareToUnload(Action callback)
    {
        if (AudioSourceController.Instance != null)
        {
            AudioSourceController.Instance.StopPlaying(() =>
            {
                gotoState(beginningVoidState, () =>
                {
                    Destroy(AudioSourceController.Instance.gameObject);
                    DemoShowStateMachine.playing = false;

                    callback.Invoke();
                });
            });
        }
    }

    public void GoBackToMainMenu()
    {
        Loader.Instance.GoToPreviousScene(SceneLoadingMode.Single, true);
    }

    public void goToStartState(Action callback = null)
    {
        MoreInfoController.Instance.HideMoreInfo();

        if ((state is EarthState) || (state is DefaultViewState))
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
        else
        {
            if (!stateProcessing)
            {
                if (state is BeginningVoidState)
                {
                    gotoState(defaultViewState, callback);
                }
                else
                {
                    gotoState(earthState, callback);
                }
            }
            else
            {
                if (state is BeginningVoidState)
                {
                    firstPryorityState = defaultViewState;
                }
                else
                {
                    firstPryorityState = earthState;
                }
                firstPryorityStateCallback = callback;
            }
        }
    }

    private void Start()
    {
        gotoState(beginningVoidState);
        Loader.Instance.IThinkIWasLoadedCompletelyAndCanStart(this);
    }

    private void InitializeStuff()
    {
        BigInfoPanel = BigSimpleInfoPanelController.Instance.gameObject;
        Earth = SlicedEarthPolygon.Instance.gameObject;
        Pie = PiePolygon.Instance.gameObject;
        Menu = HoloStudyDemoGeoMenuController.Instance.gameObject;

        statesStuffObject.Earth = Earth;
        statesStuffObject.BigInfoPanel = BigInfoPanel;
        statesStuffObject.Menu = Menu;
        statesStuffObject.Pie = Pie;
    }

    protected override void Awake()
    {
        base.Awake();

        InitializeStates();
    }

    private void InitializeStates()
    {
        statesStuffObject.CurrentState = state;
        statesStuffObject.AllParent = null;
        statesStuffObject.Earth = Earth;
        statesStuffObject.BigInfoPanel = BigInfoPanel;
        statesStuffObject.Menu = Menu;
        statesStuffObject.Pie = Pie;

        beginningVoidState = new BeginningVoidState(statesStuffObject);
        defaultViewState = new DefaultViewState(statesStuffObject);
        slicedEarthState = new SlicedEarthState(statesStuffObject);
        earthState = new EarthState(statesStuffObject);
        pieState = new PieState(statesStuffObject);
        massMeasuringState = new MassMeasuringState(statesStuffObject);
        magneticFieldMeasuringState = new MagneticFieldMeasuringState(statesStuffObject);
        temperatureState = new TemperatureState(statesStuffObject);
    }

    public void gotoState(IDemoSceneState newState, System.Action callback = null)
    {
        if (Earth == null)
        {
            InitializeStuff();
        }

        if (state != newState)
        {
            //MoreInfoController.Instance.HideMoreInfo();

            if (StateChangeStarted != null)
            {
                StateChangeStarted.Invoke();
            }

            stateProcessing = true;
            if (state == null)
            {
                state = newState;
                statesStuffObject.CurrentState = newState;
                state.enter(() =>
                {
                    if (StateChangeEnded != null)
                    {
                        StateChangeEnded.Invoke();
                    }
                    stateProcessing = false;
                    if (firstPryorityStateCallback != null)
                    {
                        firstPryorityStateCallback.Invoke();
                        firstPryorityStateCallback = null;
                    }
                });
            }
            else
            {
                state.exit(() =>
                {
                    statesStuffObject.LastState = state;
                    statesStuffObject.CurrentState = newState;
                    state = newState;
                    state.enter(() =>
                    {
                        if (StateChangeEnded != null)
                        {
                            StateChangeEnded.Invoke();
                        }
                        stateProcessing = false;
                        if (state.RecursiveState)
                        {
                            if (state.ButtonRings != null)
                            {
                                state.ButtonRings.HideVoid();
                            }
                            IDemoSceneState temp = state;
                            state = statesStuffObject.LastState;
                            statesStuffObject.LastState = temp;
                        }
                        if (firstPryorityState != null)
                        {
                            gotoState(firstPryorityState, firstPryorityStateCallback);
                            firstPryorityStateCallback = null;
                            firstPryorityState = null;
                        }
                        if (callback != null)
                        {
                            callback.Invoke();
                            callback = null;
                        }
                    });
                });
            }
        }
    }

    public void StartScene()
    {
        if (!DemoShowStateMachine.playing && (ProfessorsLecturesController.Instance == null || !ProfessorsLecturesController.Instance.LecturePlaying))
        {
            gotoState(defaultViewState);
        }
    }
}
