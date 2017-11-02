using System;
using UnityEngine;
using HoloToolkit.Unity;

/*
 * Менеджер нативной навигации отвечает за более сложный тип навигации чем простые перетаскивания, тапы и прочее. Он пытается выполнять то, что хочет в данный момент пользователь, при том, 
 * что ему прямо об этом не указали. Главное и единственное преимущество этого подхода - избавление от необходимости прямо выбирать действие, например, из контекстного меню.
 * В данный момент нам кажется, что пользователю было бы удобно:
 * 1.) По движанию руки, зажавшей тап 1 раз, в оси х вращать объект по оси у
 * 2.) По движанию руки, зажавшей тап 1 раз, в оси у вращать объект по оси z
 * 3.) По движанию руки, зажавшей тап 1 раз, в оси х и у скейлить объект
 * 4.) По движению руки, зажавшей двойной тап - перемещать объект
 * 5.) Когда поднята рука и взгляд наведен на предмет - по прошествии какого-то времени должно появляться контекстное меню этого предмета
 */

/// <summary>
/// Класс предоставляет набор кажущихся нам нативными триггеров, которые могут быть использованы и истолкованы агентами манипуляции как стадии манипуляции.
/// </summary>
public class NativeManipulationManager : Singleton<NativeManipulationManager>
{
    [Tooltip("How much to scale each axis of hand movement (camera relative) when manipulating the object")]
    public Vector3 handPositionScale = new Vector3(2.0f, 2.0f, 4.0f);  // Default tuning values, expected to be modified per application
    Vector3 initialHandPosition;

    public Action<GameObject> NativeXManipulationStarted;
    public Action<float> NativeXManipulationUpdated;
    public Action NativeXManipulationCompleted;

    public Action<GameObject> NativeYManipulationStarted;
    public Action<float> NativeYManipulationUpdated;
    public Action NativeYManipulationCompleted;

    public Action<GameObject> NativeZManipulationStarted;
    public Action<float> NativeZManipulationUpdated;
    public Action NativeZManipulationCompleted;

    public Action<GameObject> NativeXYManipulationStarted;
    public Action<float> NativeXYManipulationUpdated;
    public Action NativeXYManipulationCompleted;

    public Action<GameObject> NativeDoubleTapManipulationStarted;
    public Action<Vector3> NativeDoubleTapManipulationUpdated;
    public Action NativeDoubleTapManipulationCompleted;

    public Action<GameObject> NativeSingleTapManipulationStarted;
    public Action<Vector3> NativeSingleTapManipulationUpdated;
    public Action NativeSingleTapManipulationCompleted;

    /// <summary>
    /// Срабатывает только 1 раз, когда поднята рука и по прошествии TriggerDelay. Для того чтобы заствить его сработать еще раз, нужно либо вызвать ResetDelayTrigger - тогда можно не 
    /// опускать руку и скрипт снова отсчитает TriggerDelay и вызовет Action, либо нужно опустить руку и снова ее поднять.
    /// </summary>
    public Action DelayTriggered;

    void Awake()
    {
        /*GestureManager.Instance.ManipulationStarted += manipulationStarted;
        GestureManager.Instance.ManipulationCanceled += manipulationCompleted;
        GestureManager.Instance.ManipulationCompleted += manipulationCompleted;

        GestureManager.Instance.SourceDetected += onSourceDetected;
        GestureManager.Instance.SourceLost += onSourceLost;*/

        SourceOfGestures.Instance.ManipulationStarted += onManipulationStarted;
        SourceOfGestures.Instance.ManipulationCanceled += onManipulationCompleted;
        SourceOfGestures.Instance.ManipulationCompleted += onManipulationCompleted;
        SourceOfGestures.Instance.SourceDetected += onSourceDetected;
        SourceOfGestures.Instance.SourceLost += onSourceLost;

        TapsListener.Instance.UserSingleTapPressed += onUserSingleTapped;
        TapsListener.Instance.UserDoubleTapPressed += onUserDoubleTapped;
    }

    #region Delay Trigger

    /// <summary>
    /// После ожидания, длящегося данный промежуток времени, и наведения взгляда на объект, появляется контекстное меню.
    /// </summary>
    public float TriggerDelay = 1.23f;
    private float triggerCounter = 0;
    private bool sourceDetected = false;
    private bool delayTriggerTriggered = false;

    void onSourceDetected()
    {
        sourceDetected = true;
    }

    void onSourceLost()
    {
        sourceDetected = false;
        ResetDelayTrigger();
    }

    public void ResetDelayTrigger()
    {
        triggerCounter = 0;
        delayTriggerTriggered = false;
    }

    #endregion

    void Update()
    {
        #region Delay Trigger Update

        if (sourceDetected && !delayTriggerTriggered)
        {
            triggerCounter += Time.deltaTime;

            if (triggerCounter >= TriggerDelay)
            {
                if (DelayTriggered != null)
                {
                    DelayTriggered.Invoke();
                }
                delayTriggerTriggered = true;
            }
        }

        #endregion

        #region Native Manipulations Update

        switch (currentFirstAction)
        {
            case UsersFirstAction.SingleTap:
                if (NativeSingleTapManipulationUpdated != null)
                {
                    Vector3 localHandPosition = Camera.main.transform.InverseTransformPoint(SourceOfGestures.Instance.ManipulationHandPosition);
#if UNITY_EDITOR
                    Vector3 initialHandToCurrentHand = localHandPosition - initialHandPosition;
#else
                            Vector3 initialHandToCurrentHand = localHandPosition - initialHandPosition;
#endif

                    NativeSingleTapManipulationUpdated.Invoke(initialHandToCurrentHand);
                }
                break;
            case UsersFirstAction.DoubleTap:
                if (NativeDoubleTapManipulationUpdated != null)
                {
                    Vector3 localHandPosition = Camera.main.transform.InverseTransformPoint(SourceOfGestures.Instance.ManipulationHandPosition);
#if UNITY_EDITOR
                    Vector3 initialHandToCurrentHand = localHandPosition - initialHandPosition;
#else
                            Vector3 initialHandToCurrentHand = localHandPosition - initialHandPosition;
#endif

                    NativeDoubleTapManipulationUpdated.Invoke(initialHandToCurrentHand);
                }
                break;
        }

        switch (currentStage)
        {
            case NativeManipulationActionStage.FirstActionProcessing:
                if (userDoubleTaped || userSingleTaped)
                {
                    if (userSingleTaped)
                    {
                        currentFirstAction = UsersFirstAction.SingleTap;
                        currentStage = NativeManipulationActionStage.SecondActionProcessing;

                        if (NativeSingleTapManipulationStarted != null)
                        {
                            NativeSingleTapManipulationStarted.Invoke(gameObjectFocusedWhenPressStarted);
                        }
                    }
                    else
                    {
                        currentFirstAction = UsersFirstAction.DoubleTap;
                        //После дабл тапа мы не слушаем второй экшн, а сразу знаем что делать
                        currentStage = NativeManipulationActionStage.Executing;

                        initialHandPosition = Camera.main.transform.InverseTransformPoint(SourceOfGestures.Instance.ManipulationHandPosition);
                        if (NativeDoubleTapManipulationStarted != null)
                        {
                            NativeDoubleTapManipulationStarted.Invoke(gameObjectFocusedWhenPressStarted);
                        }
                    }

                    userSingleTaped = false;
                    userDoubleTaped = false;
                }
                break;
            case NativeManipulationActionStage.SecondActionProcessing:
                Vector3 offset = SourceOfGestures.Instance.CameraRelativeOffset;

                //Debug.Log("offset: (" + offset.x + ", " + offset.y + ", " + offset.z + ")");

                if (NativeSingleTapManipulationUpdated != null)
                {
                    NativeSingleTapManipulationUpdated.Invoke(offset);
                }
                
                if (offset.magnitude >= TriggerOffset)
                {
                    Vector3 abs = (new Vector3(Mathf.Abs(offset.x), Mathf.Abs(offset.y), Mathf.Abs(offset.z))).normalized;
                    //float absX = Mathf.Abs(offset.x);
                    //float absY = Mathf.Abs(offset.y);

                    /*float ratio = 0f;
                    if (absX < absY)
                    {
                        ratio = absX / absY;
                    }
                    else
                    {
                        ratio = absY / absX;
                    }*/

                    if (Mathf.Abs(abs.x - abs.y) < 0.66f)
                    {
                        currentSecondAction = UsersSecondAction.XYManipulation;
                        
                        if (NativeXYManipulationStarted != null)
                        {
                            NativeXYManipulationStarted.Invoke(gameObjectFocusedWhenPressStarted);
                        }
                    }
                    else
                    {
                        if (abs.x > abs.y && abs.x > abs.z)
                        {
                            currentSecondAction = UsersSecondAction.XManipulation;
                            
                            if (NativeXManipulationStarted != null)
                            {
                                NativeXManipulationStarted.Invoke(gameObjectFocusedWhenPressStarted);
                            }
                        }
                        else if (abs.y > abs.z)
                        {
                            currentSecondAction = UsersSecondAction.YManipulation;
                            
                            if (NativeYManipulationStarted != null)
                            {
                                NativeYManipulationStarted.Invoke(gameObjectFocusedWhenPressStarted);
                            }
                        }
                        else
                        {
                            currentSecondAction = UsersSecondAction.ZManipulation;
                            
                            if (NativeZManipulationStarted != null)
                            {
                                NativeZManipulationStarted.Invoke(gameObjectFocusedWhenPressStarted);
                            }
                        }
                    }

                    OffsetWhenTriggered = SourceOfGestures.Instance.CameraRelativeOffset;

                    currentStage = NativeManipulationActionStage.Executing;
                }
                break;
            case NativeManipulationActionStage.Executing:
                switch (currentFirstAction)
                {
                    case UsersFirstAction.DoubleTap:
                        /*if (NativeDoubleTapManipulationUpdated != null)
                        {
                            Vector3 localHandPosition = Camera.main.transform.InverseTransformPoint(SourceOfGestures.Instance.ManipulationHandPosition);
#if UNITY_EDITOR
                            Vector3 initialHandToCurrentHand = localHandPosition - initialHandPosition;
#else
                            Vector3 initialHandToCurrentHand = localHandPosition - initialHandPosition;
#endif

                            NativeDoubleTapManipulationUpdated.Invoke(initialHandToCurrentHand);
                        }*/
                        break;
                    case UsersFirstAction.SingleTap:
                        switch (currentSecondAction)
                        {
                            case UsersSecondAction.XYManipulation:
                                if (NativeXYManipulationUpdated != null)
                                {
                                    float x = SourceOfGestures.Instance.CameraRelativeOffset.x;
                                    float y = SourceOfGestures.Instance.CameraRelativeOffset.y;
                                    NativeXYManipulationUpdated.Invoke(Mathf.Sqrt(x*x + y*y)*(x/Mathf.Abs(x)) - OffsetWhenTriggered.magnitude*(OffsetWhenTriggered.x/Mathf.Abs(OffsetWhenTriggered.x)));
                                }
                                break;
                            case UsersSecondAction.XManipulation:
                                if (NativeXManipulationUpdated != null)
                                {
                                    NativeXManipulationUpdated.Invoke(SourceOfGestures.Instance.CameraRelativeOffset.x - OffsetWhenTriggered.x);
                                }
                                break;
                            case UsersSecondAction.YManipulation:
                                if (NativeYManipulationUpdated != null)
                                {
                                    NativeYManipulationUpdated.Invoke(SourceOfGestures.Instance.CameraRelativeOffset.y - OffsetWhenTriggered.y);
                                }
                                break;
                            case UsersSecondAction.ZManipulation:
                                if (NativeZManipulationUpdated != null)
                                {
                                    NativeZManipulationUpdated.Invoke(SourceOfGestures.Instance.CameraRelativeOffset.z - OffsetWhenTriggered.z);
                                }
                                break;
                        }
                        break;
                }
                break;
        }

        #endregion
    }

    #region Native Manipulations

    /// <summary>
    /// После перемещения руки на это расстояние скрипт должен понять что за действие совершает пользователь
    /// </summary>
    public float TriggerOffset = 0.05f;
    //Запоминаем оффсет, который был во время триггера и вычитаем его из передающегося значения для избежания резких скачков
    private Vector3 OffsetWhenTriggered = Vector3.zero;

    private bool userSingleTaped = false;
    private bool userDoubleTaped = false;

    /// <summary>
    /// Всего есть 3 стадии у контролирующих жестов - стадия ожидания изначального дейтсивя - сингл или дабл тап,
    /// стадия собственно когда мы пытаемся понять, какого именно взаимодействия хочет пользователь,
    /// и собственно выполнение действия навигации
    /// </summary>
    private enum NativeManipulationActionStage { FirstActionProcessing, SecondActionProcessing, Executing }
    private NativeManipulationActionStage currentStage = NativeManipulationActionStage.FirstActionProcessing;

    private enum UsersFirstAction { None, SingleTap, DoubleTap }
    private UsersFirstAction currentFirstAction = UsersFirstAction.None;

    private enum UsersSecondAction { None, XManipulation, YManipulation, ZManipulation, XYManipulation }
    private UsersSecondAction currentSecondAction = UsersSecondAction.None;

    GameObject gameObjectFocusedWhenPressStarted;

    void onUserSingleTapped(GameObject targetGO)
    {
        gameObjectFocusedWhenPressStarted = targetGO;
        userSingleTaped = true;
    }

    void onUserDoubleTapped(GameObject targetGO)
    {
        gameObjectFocusedWhenPressStarted = targetGO;
        userDoubleTaped = true;
    }

    void onManipulationStarted()
    {

    }

    void onManipulationCompleted()
    {
        switch (currentFirstAction)
        {
            case UsersFirstAction.SingleTap:
                if (NativeSingleTapManipulationCompleted != null)
                {
                    NativeSingleTapManipulationCompleted.Invoke();
                }
                break;
            case UsersFirstAction.DoubleTap:
                if (NativeDoubleTapManipulationCompleted != null)
                {
                    NativeDoubleTapManipulationCompleted.Invoke();
                }
                break;
        }

        if (currentStage != NativeManipulationActionStage.FirstActionProcessing)
        {
            if (currentStage == NativeManipulationActionStage.Executing)
            {
                switch (currentFirstAction)
                {
                    case UsersFirstAction.DoubleTap:
                        /*if (NativeDoubleTapManipulationCompleted != null)
                        {
                            NativeDoubleTapManipulationCompleted.Invoke();
                        }*/
                        break;
                    case UsersFirstAction.SingleTap:
                        switch (currentSecondAction)
                        {
                            case UsersSecondAction.XManipulation:
                                if (NativeXManipulationCompleted != null)
                                {
                                    NativeXManipulationCompleted.Invoke();
                                }
                                break;
                            case UsersSecondAction.YManipulation:
                                if (NativeYManipulationCompleted != null)
                                {
                                    NativeYManipulationCompleted.Invoke();
                                }
                                break;
                            case UsersSecondAction.ZManipulation:
                                if (NativeZManipulationCompleted != null)
                                {
                                    NativeZManipulationCompleted.Invoke();
                                }
                                break;
                            case UsersSecondAction.XYManipulation:
                                if (NativeXYManipulationCompleted != null)
                                {
                                    NativeXYManipulationCompleted.Invoke();
                                }
                                break;
                        }
                        break;
                }
            }

            currentStage = NativeManipulationActionStage.FirstActionProcessing;
            currentFirstAction = UsersFirstAction.None;
            currentSecondAction = UsersSecondAction.None;
            
            userSingleTaped = false;
            userDoubleTaped = false;

            OffsetWhenTriggered = Vector3.zero;
        }
    }

    #endregion
    
    void OnDestroy()
    {
        if (SourceOfGestures.Instance != null)
        {
            SourceOfGestures.Instance.ManipulationStarted -= onManipulationStarted;
            SourceOfGestures.Instance.ManipulationCanceled -= onManipulationCompleted;
            SourceOfGestures.Instance.ManipulationCompleted -= onManipulationCompleted;

            SourceOfGestures.Instance.SourceDetected -= onSourceDetected;
            SourceOfGestures.Instance.SourceLost -= onSourceLost;
        }
        
        if (TapsListener.Instance != null)
        {
            TapsListener.Instance.UserSingleTapPressed -= onUserSingleTapped;
            TapsListener.Instance.UserDoubleTapPressed -= onUserDoubleTapped;
        }
    }

}
