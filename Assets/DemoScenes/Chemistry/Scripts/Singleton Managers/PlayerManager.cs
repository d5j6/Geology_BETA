using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerManager : Singleton<PlayerManager>
{
    #region State implementation
    private interface IPlayerState
    {
        void OnGazeEnterHandler(IInteractive interactive);
        void OnGazeLeaveHandler(IInteractive interactive);
        void OnGestureTapHandler(IInteractive interactive);
        void TryToDragInteractive(IInteractive interactive);
        void StopDraggingInteractive(IInteractive draggingInteractive);
        void ChangeStateToDefault();
    }

    private interface IResizable : IPlayerState
    {
        
    }

    private class PlayerDefaultState : IPlayerState
    {
        public void OnGazeEnterHandler(IInteractive interactive)
        {
            interactive.OnGazeEnter();
        }

        public void OnGazeLeaveHandler(IInteractive interactive)
        {
            interactive.OnGazeLeave();
        }

        public void OnGestureTapHandler(IInteractive interactive)
        {
            //Debug.Log(interactive);
            List<ActionType> allowedActionTypes = interactive.GetAllowedActions();

            //TODO: заменить работу исключительно с одним типом на работу со множеством в дальнейшем
            ActionType allowedActionType = allowedActionTypes[0];

            //foreach(ActionType allowedActionType in allowedActionTypes)
            switch (allowedActionType)
            {
                case ActionType.TapOnly:

                    new TapCommand(interactive).Execute();
                    break;
                case ActionType.DragAndDrop:
                    TryToDragInteractive(interactive);
                    break;
                case ActionType.Resize:
                    TryToResizeInteractive(interactive);
                    break;
                default:
                    break;
            }
        }

        public void TryToResizeInteractive(IInteractive interactive)
        {
            new ResizeCommand(interactive).Execute();
        }

        public void TryToDragInteractive(IInteractive interactive)
        {
            new DragCommand(interactive).Execute();
        }

        public void StopDraggingInteractive(IInteractive draggingInteractive) { }

        public void ChangeStateToDefault() { }
    }

    private class PlayerDragAndDropState : IPlayerState
    {
        private IInteractive _draggingInteractive;

        public PlayerDragAndDropState(IInteractive interactive)
        {
            _draggingInteractive = interactive;
        }

        public void ChangeStateToDemonstration() { }

        public void ChangeStateToDefault() { }

        public void OnGazeEnterHandler(IInteractive interactive) { }

        public void OnGazeLeaveHandler(IInteractive interactive) { }

        public void OnGestureTapHandler(IInteractive interactive)
        {
            StopDraggingInteractive(_draggingInteractive);
        }

        public void StopDraggingInteractive(IInteractive draggingInteractive)
        {
            if(draggingInteractive == null)
            {
                return;
            }

            draggingInteractive.StopDrag();

            Instance.state = new PlayerDefaultState();
            Instance._stateName = Instance.state.GetType().ToString();

            Instance._inputFacade.ChangeStrategyToDefault();
        }

        public void TryToDragInteractive(IInteractive interactive) { }
    }
    #endregion

    #region Command implementation
    private interface ICommand
    {
        void Execute();
    }

    private class TapCommand : ICommand
    {
        private IInteractive _interactive;

        public TapCommand(IInteractive interactive)
        {
            _interactive = interactive;
        }

        public void Execute()
        {
            _interactive.OnGestureTap();
        }
    }

    private class DragCommand : ICommand
    {
        private IInteractive _interactive;

        public DragCommand(IInteractive interactive)
        {
            _interactive = interactive;
        }

        public void Execute()
        {
            if(_interactive.TryToDrag())
            {
                Instance._inputFacade.ChangeStrategyToDragAndDrop();

                Instance.state = new PlayerDragAndDropState(_interactive);
                Instance._stateName = Instance.state.GetType().ToString();
            }
        }
    }

    private class ResizeCommand : ICommand
    {
        private IInteractive _interactive;

        public ResizeCommand(IInteractive interactive)
        {
            _interactive = interactive;
        }

        public void Execute()
        {
            if (_interactive.TryToResize())
            {
                Instance._inputFacade.ChangeStrategyToResize();

                Instance.state = new PlayerDefaultState();
                Instance._stateName = Instance.state.GetType().ToString();
            }
        }
    }

    #endregion

    #region Fields
    private bool _isInitialized;

    private IPlayerState state;

    private InputStrategyFacade _inputFacade;

    private string _stateName;

    private PeriodicTable periodicTable;

    private ProjectorController projector;

    private GameObject professor;

    private GameObject chaptersMenu;

    private SkipGidButton skipGidButton;
    #endregion

    #region Properties
    public InputStrategyFacade.Strategies Strategy { get { return _inputFacade.Strategy; } }

    public PeriodicTable PeriodicTable { get { return periodicTable; } }

    public ProjectorController Projector { get { return projector; } }

    public SkipGidButton SkipGidButton { get { return skipGidButton; } }

    public GameObject Professor { get { return professor; } }

    public GameObject ChaptersMenu { get { return chaptersMenu; } }

    public float StartTime { get; private set; }

    public bool IsScanned { get; set; }
    #endregion

    #region Initialize and Start methods
    public void Initialize()
    {
        if(_isInitialized)
        {
            return;
        }

        state = new PlayerDefaultState();
        _stateName = state.GetType().ToString();

        _inputFacade = new InputStrategyFacade();

        _inputFacade.SetListeners(OnGazeEnterHandler, OnGazeLeaveHandler, OnGestureTapHandler);
        _inputFacade.SetListeneresForNavigation(OnNavigationStart, OnNavigationUpdate);
        _inputFacade.ChangeStrategyToDefault();
        _isInitialized = true;

        periodicTable = GameObject.FindObjectOfType<PeriodicTable>();
        projector = GameObject.FindObjectOfType<ProjectorController>();
        skipGidButton = GameObject.FindObjectOfType<SkipGidButton>();
        professor = GameObject.FindObjectOfType<ProfessorController>().gameObject;
        chaptersMenu = GameObject.FindObjectOfType<menu>().gameObject;

        StartTime = Time.time;
    }
    #endregion

    #region Input events handlers
    void OnGazeEnterHandler(IInteractive interactive)
    {
        state.OnGazeEnterHandler(interactive);
    }

    void OnGazeLeaveHandler(IInteractive interactive)
    {
        state.OnGazeLeaveHandler(interactive);
    }

    void OnGestureTapHandler(IInteractive interactive)
    {
        state.OnGestureTapHandler(interactive);
    }

    void OnNavigationStart(IInteractive interactive)
    {
        state.OnGestureTapHandler(interactive);
    }

    void OnNavigationUpdate(IInteractive interactive)
    {
        state.OnGestureTapHandler(interactive);
    }
    #endregion

    public void TapOnInteractive(IInteractive interactive)
    {
        state.OnGestureTapHandler(interactive);
    }

    public void TryToDragInteractive(IInteractive interactive)
    {
        state.TryToDragInteractive(interactive);
    }

    public void StopDraggingInteractive(IInteractive draggingInteractive)
    {
        state.StopDraggingInteractive(draggingInteractive);
    }

    public void ChangeStateToDefault()
    {
        state.ChangeStateToDefault();
    }

    //public void ChangeStateToDemonstration()
    //{
    //    CutsceneManager.Instance.ActivateButton();
    //    state.ChangeStateToDemonstration();
    //}

    //public void ChangeStateToStandart()
    //{
    //    if (!CutsceneManager.Instance.IsStop)
    //    {
    //        CutsceneManager.Instance.StopCutscene();
    //        CutsceneManager.Instance.DeactivateButton();
    //    }

    //    state.ChangeStateToDefault();
    //}
}
