using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OwnGazeManager : Singleton<OwnGazeManager>
{
    #region Strategy implementation
    private interface IGazeStrategy
    {
        void Alghorithm();
    }

    private class GazeNoneStrategy : IGazeStrategy
    {
        public void Alghorithm() { }
    }

    private class GazeDefaultStrategy : IGazeStrategy
    {
        private OwnGazeManager _ownGaze;
        private int _spartialMeshLayer = LayerMask.NameToLayer("SpartialMesh");
        private int _demonstrationLayer = LayerMask.NameToLayer("Demonstration");

        public GazeDefaultStrategy(OwnGazeManager gaze)
        {
            _ownGaze = gaze;
        }

        public void Alghorithm()
        {
            //Выпускаем луч из головы пользователя
            RaycastHit hitInfo = _ownGaze.hitInfo;
            bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);

            //Additional for Debug
            _ownGaze.IsGazingAtObject = isHit;

            if (isHit)
            {
                _ownGaze.hitPoint = hitInfo.point;
                _ownGaze.pointNormal = hitInfo.normal;

                //ADDITIONAL BY HSE
                _ownGaze.HitObject = hitInfo.collider.gameObject;

                //Пытаемся привести объект к типу IInteractive, что означает то, что с ним можно взаимодействовать
                IInteractive newFocused = hitInfo.transform.GetComponent<IInteractive>();

                //Если удалось распознать интерактивный объект, определяем, что именно мы видим
                if (newFocused != null)
                {
                    _ownGaze.hitObjectType = HitObjectTypes.Interactive;

                    //Определим слой, к которому принадлежит найденный объект
                    //Так как их всего два, то посмотрим: является этот объект конпкой меню или нет?
                    bool isObjectOnDemostrationlayer = hitInfo.transform.gameObject.layer == _demonstrationLayer;

                    if (isObjectOnDemostrationlayer)
                    {
                        //Работаем с кнопкой меню

                        if (_ownGaze._currentFocusedChapter == newFocused)
                            return;

                        if (_ownGaze._currentFocusedChapter != null)
                            if (_ownGaze.OnGazeLeaveFromInteractiveEvent != null)
                                _ownGaze.OnGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);

                        _ownGaze._currentFocusedChapter = newFocused;

                        if (_ownGaze.OnGazeEnterToInteractiveEvent != null)
                            _ownGaze.OnGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);
                    }
                    else
                    {
                        //Работаем с элементом таблицы

                        if (_ownGaze._currentFocused == newFocused)
                            return;

                        if (_ownGaze._currentFocused != null)
                            if (_ownGaze.OnGazeLeaveFromInteractiveEvent != null)
                                _ownGaze.OnGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocused);

                        _ownGaze._currentFocused = newFocused;

                        if (_ownGaze.OnGazeEnterToInteractiveEvent != null)
                            _ownGaze.OnGazeEnterToInteractiveEvent.Invoke(_ownGaze._currentFocused);
                    }
                }
                else

                //Пользователь уперся взглядом в полигональную сетку комнаты или любой другой объект
                    if (hitInfo.transform.gameObject.layer == _spartialMeshLayer)
                    _ownGaze.hitObjectType = HitObjectTypes.Spatial;
                else
                    _ownGaze.hitObjectType = HitObjectTypes.Default;
            }
            else
                //Пользователь палит в пустоту
                _ownGaze.hitObjectType = HitObjectTypes.None;

            if (_ownGaze.HitObjectType != HitObjectTypes.Interactive)
            {
                //TODO: В теории, пользователь может так быстро и точно перевести взгляд 
                //с элемента таблицы на кнопку меню,
                //что программа не попадет в эту условную ветку.
                //Такое маловероятно, но всё-таки педусмотреть можно (но потом)

                //Отвели взгляд с элемента таблицы
                if (_ownGaze._currentFocused != null)
                {
                    if (_ownGaze.OnGazeLeaveFromInteractiveEvent != null)
                        _ownGaze.OnGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocused);

                    _ownGaze._currentFocused = null;
                }

                //Отвели взгляд с кнопки меню
                if (_ownGaze._currentFocusedChapter != null)
                {
                    if (_ownGaze.OnGazeLeaveFromInteractiveEvent != null)
                        _ownGaze.OnGazeLeaveFromInteractiveEvent.Invoke(_ownGaze._currentFocusedChapter);

                    _ownGaze._currentFocusedChapter = null;
                }
            }
        }
    }

    private class GazeDragAndDropStrategy : IGazeStrategy
    {
        private OwnGazeManager _ownGaze;

        private int _spartialMeshLayer = LayerMask.NameToLayer("SpartialMesh");

        RaycastHit hitInfo;

        public GazeDragAndDropStrategy(OwnGazeManager gaze)
        {
            _ownGaze = gaze;
        }

        public void Alghorithm()
        {
            bool isHit = Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, 32f);

            if (isHit)
            {
                _ownGaze.hitPoint = hitInfo.point;
                _ownGaze.pointNormal = hitInfo.normal;

                _ownGaze.hitObjectType = HitObjectTypes.Default;

                if (hitInfo.transform.gameObject.layer == _spartialMeshLayer)
                {
                    _ownGaze.hitObjectType = HitObjectTypes.Spatial;
                }
                else
                {
                    if (hitInfo.transform.GetComponent<IInteractive>() != null)
                    {
                        _ownGaze.hitObjectType = HitObjectTypes.Interactive;
                    }
                }
            }
            else
            {
                _ownGaze.hitObjectType = HitObjectTypes.None;
            }
        }
    }
    #endregion

    #region Properties
    private bool isInitialized;

    public enum HitObjectTypes
    {
        None,
        Default,
        Interactive,
        Spatial
    }

    //ADDITIONAL BY HSE
    public GameObject HitObject { get; set; }
    private IGazeStrategy strategy;

    //Additional by HSE
    //Neccessary attributes for correct work of UI ray casting
    RaycastHit hitInfo;
    bool IsGazingAtObject;

    private IInteractive _currentFocused;
    private IInteractive _currentFocusedChapter;
    private SkipGidButton _currentFocusedReset;

    public IInteractive CurrentFocused
    {
        get
        {
            return _currentFocused;
        }
        protected set
        {
            _currentFocused = value;
        }
    }
    public IInteractive CurrentFocusedChapter
    {
        get
        {
            return _currentFocusedChapter;
        }
        protected set
        {
            _currentFocusedChapter = value;
        }
    }
    public SkipGidButton CurrentFocusedReset
    {
        get
        {
            return _currentFocusedReset;
        }
        protected set
        {
            _currentFocusedReset = value;
        }
    }

    private HitObjectTypes hitObjectType;
    public HitObjectTypes HitObjectType { get { return hitObjectType; } }

    private Vector3 hitPoint;
    public Vector3 HitPoint { get { return hitPoint; } }

    private Vector3 pointNormal;
    public Vector3 PointNormal { get { return pointNormal; } }

    public event Action<IInteractive> OnGazeEnterToInteractiveEvent;
    public event Action<IInteractive> OnGazeLeaveFromInteractiveEvent;

    private string strategyName;
    #endregion

    public void ChangeStrategyToNone()
    {
        Reset();
        strategy = new GazeNoneStrategy();
        strategyName = strategy.GetType().ToString();
    }

    public void ChangeStrategyToDefault()
    {
        Reset();
        strategy = new GazeDefaultStrategy(this);
        strategyName = strategy.GetType().ToString();
    }

    public void ChangeStrategyToDragAndDrop()
    {
        if (_currentFocused != null)
        {
            OnGazeLeaveFromInteractiveEvent.Invoke(_currentFocused);
        }
        if (_currentFocusedChapter != null)
        {
            OnGazeLeaveFromInteractiveEvent.Invoke(_currentFocusedChapter);
        }

        Reset();
        strategy = new GazeDragAndDropStrategy(this);
        strategyName = strategy.GetType().ToString();
    }

    private void Reset()
    {
        _currentFocusedChapter = null;
        hitObjectType = HitObjectTypes.None;
    }

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        ChangeStrategyToDefault();

        isInitialized = true;
    }

    void Update()
    {
        if (strategy != null)
            strategy.Alghorithm();
    }
}