using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using System;
using TMPro;

public class OwnGestureManager : Singleton<OwnGestureManager>
{
    #region Strategy implementation
    private interface IGestureStrategy
    {
        void Alghoritm();
    }

    private class GestureNoneStrategy : IGestureStrategy
    {
        public void Alghoritm() {  }
    }

    private class GestureResizeStrategy : IGestureStrategy
    {
        public void Alghoritm()
        {
            if (OwnGazeManager.Instance.HitObjectType != OwnGazeManager.HitObjectTypes.Interactive)
            {
                return;
            }

            if (Instance.OnTapEvent != null)
            {
                Instance.OnTapEvent.Invoke(OwnGazeManager.Instance.CurrentFocused);
            }

            if (Instance.NavStart != null)
            {
                Debug.Log("Nav Start");
                Instance.NavStart.Invoke(OwnGazeManager.Instance.CurrentFocused);
            }

            if (Instance.NavUpdate != null)
            {
                Debug.Log("Nav Update");
                Instance.NavUpdate.Invoke(OwnGazeManager.Instance.CurrentFocused);
            }
            else
            {
                Debug.Log("NavUpdate empty");
            }
        }
    }

    private class GestureDragAndDropStrategy : IGestureStrategy
    {
        public void Alghoritm()
        {
            if(OwnGazeManager.Instance.HitObjectType != OwnGazeManager.HitObjectTypes.Spatial)
            {
                return;
            }

            if(Instance.OnTapEvent != null)
            {
                Instance.OnTapEvent.Invoke(null);
            }
        }
    }

    private class GestureDefaultStrategy : IGestureStrategy
    {
        public void Alghoritm()
        {
            if(Instance.OnTapEvent != null)
            {
                //Определяем, в какой обект упирается взгляд пользователя
                //Если пользователь смотрит на кнопку "Free mode", то вызываем нажатие на эту кнопку и смену режимов
                if (OwnGazeManager.Instance.CurrentFocusedReset != null)
                {
                    //Однако от данной конпки мы отказываемся, так что здесь не нужно какое-либо действие

                    //Instance.onTapEvent.Invoke(OwnGazeManager.Instance.currentFocusedReset);
                    //return;
                }

                //Если пользователь смотрит на кнопку меню, реализуем нажатие на выбранную кнопку
                if(OwnGazeManager.Instance.CurrentFocusedChapter != null)
                {
                    Instance.OnTapEvent.Invoke(OwnGazeManager.Instance.CurrentFocusedChapter);
                    return;
                }

                //Еслиже пользователь палит в элемент таблицы, реализуем нажатие на выбранный элемент
                if (OwnGazeManager.Instance.CurrentFocused != null)
                {
                    Instance.OnTapEvent.Invoke(OwnGazeManager.Instance.CurrentFocused);
                    return;
                }
            }
        }
    }

    #endregion

    #region Properties
    private bool isInitialized;

    private IGestureStrategy strategy;

    public KeyCode editorTapKey = KeyCode.F;

    private GestureRecognizer tapGestureRecognizer;

    public event Action<IInteractive> OnTapEvent;
    public event Action<IInteractive> NavStart;
    public event Action<IInteractive> NavUpdate;
    public event Action<IInteractive> NavComplete;
    public event Action<IInteractive> NavCancel;

    private string strategyName;
    #endregion

    public void ChangeStrategyToNone()
    {
        if (tapGestureRecognizer.IsCapturingGestures())
            tapGestureRecognizer.StopCapturingGestures();

        strategy = new GestureNoneStrategy();
        strategyName = strategy.GetType().ToString();

        tapGestureRecognizer.StartCapturingGestures();
    }

    public void ChangeStrategyToDefault()
    {
        if (tapGestureRecognizer.IsCapturingGestures())
            tapGestureRecognizer.StopCapturingGestures();

        strategy = new GestureDefaultStrategy();
        strategyName = strategy.GetType().ToString();

        tapGestureRecognizer.StartCapturingGestures();
    }

    public void ChangeStrategyToResize()
    {
        if (tapGestureRecognizer.IsCapturingGestures())
            tapGestureRecognizer.StopCapturingGestures();

        strategy = new GestureResizeStrategy();
        strategyName = strategy.GetType().ToString();

        tapGestureRecognizer.StartCapturingGestures();

        
    }

    public void ChangeStrategyToDragAndDrop()
    {
        if(tapGestureRecognizer.IsCapturingGestures())
            tapGestureRecognizer.StopCapturingGestures();

        strategy = new GestureDragAndDropStrategy();
        strategyName = strategy.GetType().ToString();

        tapGestureRecognizer.StartCapturingGestures();

        
    }

    public void Initialize()
    {
        if(isInitialized)
        {
            return;
        }

        tapGestureRecognizer = new GestureRecognizer();

        tapGestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.NavigationX);
        tapGestureRecognizer.TappedEvent += OnTap;
        tapGestureRecognizer.NavigationStartedEvent += NavigationStart;
        tapGestureRecognizer.NavigationUpdatedEvent += NavigationUpdate;
        tapGestureRecognizer.NavigationCompletedEvent += NavigationComplete;
        tapGestureRecognizer.NavigationCanceledEvent += NavigationCansel;

        //Debug.Log("Events initialized");

        ChangeStrategyToNone();

        isInitialized = true;
    }

    void OnTap(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        
        
        strategy.Alghoritm();
    }

    void NavigationStart(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        Debug.Log("Works navStart");
        strategy.Alghoritm();
    }

    void NavigationUpdate(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        strategy.Alghoritm();
    }

    void NavigationComplete(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        strategy.Alghoritm();
    }

    void NavigationCansel(InteractionSourceKind source, Vector3 normalOffset, Ray headRay)
    {
        strategy.Alghoritm();
    }

#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(editorTapKey))
        {
            Debug.Log("!" + strategyName);
            strategy.Alghoritm();
        }
    }
#endif
}
