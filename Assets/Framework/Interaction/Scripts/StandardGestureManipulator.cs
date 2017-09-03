using System;
using UnityEngine;
using Zenject;
using HoloToolkit.Unity;
using SV_Events = SV_HandlerBankEvents;
using Andy.IdGenerator;

/*[AttributeUsage(AttributeTargets.Property)]
public class ExposePropertyAttribute : Attribute
{

}*/

public class StandardGestureManipulator : MonoBehaviour, IGestureManipulator
{
    public enum AxisOfRotation { X, Y, Z, None }

    [Tooltip("Включает/отключает функционал перемещения")]
    public bool EnableMove = true;
    [Tooltip("Включает/отключает функционал поворота")]
    public bool EnableRotation = true;
    [Tooltip("Поворот в режиме добавления каждый кадр текущего значения оффсета. Если выключить, будет добавляться только текущий оффсет (при таком варианте нужно значителньно увеличить чувствительноть вращения)")]
    public bool RotationInAdditiveMode = true;
    [Tooltip("Инвертирует направление вращения по оси X")]
    public bool InverseRotationX = false;
    [Tooltip("Инвертирует направление вращения по оси X")]
    public bool InverseRotationY = false;
    [Tooltip("Инвертирует направление вращения по оси X")]
    public bool InverseRotationZ = false;
    [Tooltip("Контролирует ось по которой происходит вращение когда рука перемещается вдоль оси Y")]
    public AxisOfRotation YRotationAxis = AxisOfRotation.Z;
    [Tooltip("Включает/отключает функционал масштабирования")]
    public bool EnableScale = true;

    public Action Moving { get; set; }

    public Action Rotating { get; set; }

    public Action Scaling { get; set; }


    public Action MoveStarted { get; set; }

    public Action MoveEnded { get; set; }

    public Action RotationStarted { get; set; }

    public Action RotationEnded { get; set; }

    public Action ScaleStarted { get; set; }

    public Action ScaleEnded { get; set; }

    /*
     * Эта переменная нужна для того, чтобы определять что юзер смотрел на нас когда начал манипуляцию. Если такую проверку не делать, то манипуляции подвергнутся все объекты на сцене с данным 
     * MonoBehaviour'ом без разбора.
     */
    bool whenManipulationStartedUserWatchedOnMe = false;

    public GameObject TargetObject = null;

    [Inject]
    IGestureManipulationAgent manipulationAgent;
    public IGestureManipulationAgent ManipulationAgent
    {
        get
        {
            return manipulationAgent;
        }
    }

    //[ExposeProperty]
    public float MovingSensitivity
    {
        get
        {
            return 1f;
        }
        set
        {

        }
    }

    //[ExposeProperty]
    public float RotationSensitivity
    {
        get
        {
            return 200f;
        }
        set
        {

        }
    }

    //[ExposeProperty]
    public float ScanlingSensitivity
    {
        get
        {
            return 3f;
        }
        set
        {

        }
    }

    Interpolator targetInterpolator;

    void Awake()
    {
        NativeManipulationAgent.Instance.Move += onMove;
        NativeManipulationAgent.Instance.Rotate += onRotate;
        NativeManipulationAgent.Instance.Scale += onScale;

        NativeManipulationAgent.Instance.MoveStarted += onMoveStarted;
        NativeManipulationAgent.Instance.ScaleStarted += onScaleStarted;
        NativeManipulationAgent.Instance.RotationStarted += onRotationStarted;

        NativeManipulationAgent.Instance.MoveEnded += onMoveEnded;
        NativeManipulationAgent.Instance.ScaleEnded += onScaleEnded;
        NativeManipulationAgent.Instance.RotationEnded += onRotationEnded;
    }

    void Start()
    {
        targetInterpolator = TargetObject.GetComponent<Interpolator>();
    }

    void onMoveEnded()
    {
        if (EnableMove && whenManipulationStartedUserWatchedOnMe)
        {
            whenManipulationStartedUserWatchedOnMe = false;
            if (MoveEnded != null)
            {
                MoveEnded.Invoke();
            }
        }
    }

    void onScaleEnded()
    {
        if (EnableScale && whenManipulationStartedUserWatchedOnMe)
        {
            whenManipulationStartedUserWatchedOnMe = false;
            if (ScaleEnded != null)
            {
                ScaleEnded.Invoke();
            }
        }
    }

    void onRotationEnded()
    {
        if (EnableRotation && whenManipulationStartedUserWatchedOnMe)
        {
            whenManipulationStartedUserWatchedOnMe = false;
            if (RotationEnded != null)
            {
                RotationEnded.Invoke();
            }
        }
    }

    void onRotationStarted(GameObject targetGO)
    {
        if (EnableRotation && targetGO == gameObject)
        {
            whenManipulationStartedUserWatchedOnMe = true;

            if (RotationStarted != null)
            {
                RotationStarted.Invoke();
            }

            if (!RotationInAdditiveMode)
            {
                rotationOffset = TargetObject.transform.rotation.eulerAngles;
            }
        }
    }

    Vector3 initialObjectPosition;
    void onMoveStarted(GameObject targetGO)
    {
        if (EnableMove && targetGO == gameObject)
        {
            whenManipulationStartedUserWatchedOnMe = true;
            initialObjectPosition = Camera.main.transform.InverseTransformPoint(TargetObject.transform.position);

            if (MoveStarted != null)
            {
                MoveStarted.Invoke();
            }
        }
    }

    void onMove(Vector3 offset)
    {
        if (EnableMove && whenManipulationStartedUserWatchedOnMe)
        {
            // When performing a manipulation gesture, the hand generally only translates a relatively small amount.
            // If we move the object only as much as the hand itself moves, users can only make small adjustments before
            // the hand is lost and the gesture completes.  To improve the usability of the gesture we scale each
            // axis of hand movement by some amount (camera relative).  This value can be changed in the editor or
            // at runtime based on the needs of individual movement scenarios.
            Vector3 scaledLocalHandPositionDelta = Vector3.Scale(offset, NativeManipulationManager.Instance.handPositionScale);

            // Once we've figured out how much the object should move relative to the camera we apply that to the initial
            // camera relative position.  This ensures that the object remains in the appropriate location relative to the camera
            // and the hand as the camera moves.  The allows users to use both gaze and gesture to move objects.  Once they
            // begin manipulating an object they can rotate their head or walk around and the object will move with them
            // as long as they maintain the gesture, while still allowing adjustment via hand movement.
            Vector3 localObjectPosition = initialObjectPosition + scaledLocalHandPositionDelta * MovingSensitivity;
            Vector3 worldObjectPosition = Camera.main.transform.TransformPoint(localObjectPosition);

            // If the object has an interpolator we should use it, otherwise just move the transform directly
            if (targetInterpolator != null)
            {
                targetInterpolator.SetTargetPosition(worldObjectPosition);
            }
            else
            {
                TargetObject.transform.position = worldObjectPosition;
            }

            if (Moving != null)
            {
                Moving.Invoke();
            }
            /*
            if (TargetObject.GetComponent<IDHolder>())
            {
                var id = TargetObject.GetComponent<IDHolder>().ID;
                SV_Sharing.Instance.SendJson(new SV_Events.Case2(id, TargetObject.transform), "MoveModel"); // 26
            }
            */
        }
    }

    Vector3 rotationOffset;

    void onRotate(Vector3 angles)
    {
        if (EnableRotation 
            && whenManipulationStartedUserWatchedOnMe)
        {
            float magni = angles.x;
            //По умолчанию вращаем по Y. По идее это должно происходить, если манипуляция идет происходит дволь оси х
            AxisOfRotation targetAxis = AxisOfRotation.Y;
            if (Mathf.Abs(angles.y) > Mathf.Abs(angles.x))
            {
                //Если у нас манипуляция идет вдоль оси Y то мы хотим вращать объект по оси, указанной в YRotationAxis
                targetAxis = YRotationAxis;
				magni = angles.y;
            }
			
			float inverse = 1f;
            Vector3 finalRotation = Vector3.zero;

            if (RotationInAdditiveMode)
            {
                switch (targetAxis)
                {
                    case AxisOfRotation.X:
                        finalRotation = new Vector3(magni, 0f, 0f);
                        if (InverseRotationX)
                        {
                            inverse = -1f;
                        }
                        break;
                    case AxisOfRotation.Y:
                        finalRotation = new Vector3(0f, magni, 0f);
                        if (InverseRotationY)
                        {
                            inverse = -1f;
                        }
                        break;
                    case AxisOfRotation.Z:
                        finalRotation = new Vector3(0f, 0f, magni);
                        if (InverseRotationZ)
                        {
                            inverse = -1f;
                        }
                        break;
                }
            }
            else
            {
                switch (targetAxis)
                {
                    case AxisOfRotation.X:
                        finalRotation = rotationOffset + new Vector3(magni * RotationSensitivity, 0f, 0f);
                        if (InverseRotationX)
                        {
                            inverse = -1f;
                        }
                        break;
                    case AxisOfRotation.Y:
                        finalRotation = rotationOffset + new Vector3(0f, magni * RotationSensitivity, 0f);
                        if (InverseRotationY)
                        {
                            inverse = -1f;
                        }
                        break;
                    case AxisOfRotation.Z:
                        finalRotation = rotationOffset + new Vector3(0f, 0f, magni * RotationSensitivity);
                        if (InverseRotationZ)
                        {
                            inverse = -1f;
                        }
                        break;
                }
            }

            if (RotationInAdditiveMode)
            {
                if (targetInterpolator != null)
                {
                    /*rotateAdding += -1 * GestureManager.Instance.ManipulationHandPosition.y * RotationSensitivity;
                    Vector3 eulRot = initialObjectRotation.eulerAngles;
                    eulRot.x += rotateAdding;*/
                    targetInterpolator.SetTargetRotation(Quaternion.Euler(finalRotation * RotationSensitivity * -1f * Time.deltaTime));
                    //targetInterpolator.SetTargetRotation(Quaternion.Euler(eulRot));
                }
                else
                {
                    //transform.Rotate(new Vector3(-1 * GestureManager.Instance.ManipulationOffset.y * RotationSensitivity * Time.deltaTime, 0, 0));
                    TargetObject.transform.Rotate(finalRotation * RotationSensitivity * -1f * inverse * Time.deltaTime);
                }
            }
            else
            {
                if (targetInterpolator != null)
                {
                    /*rotateAdding += -1 * GestureManager.Instance.ManipulationHandPosition.y * RotationSensitivity;
                    Vector3 eulRot = initialObjectRotation.eulerAngles;
                    eulRot.x += rotateAdding;*/
                    targetInterpolator.SetTargetRotation(Quaternion.Euler(finalRotation * RotationSensitivity * -1f * Time.deltaTime));
                    //targetInterpolator.SetTargetRotation(Quaternion.Euler(eulRot));
                }
                else
                {
                    //transform.Rotate(new Vector3(-1 * GestureManager.Instance.ManipulationOffset.y * RotationSensitivity * Time.deltaTime, 0, 0));
                    TargetObject.transform.rotation = Quaternion.Euler(finalRotation * inverse );
                }
            }

            if (Rotating != null)
            {
                Rotating.Invoke();
            }

            /*
            if (TargetObject.GetComponent<IDHolder>())
            {
                var id = TargetObject.GetComponent<IDHolder>().ID;
                SV_Sharing.Instance.SendJson(new SV_Events.Case2(id, TargetObject.transform), "RotateModel"); // 27
            }
            */
        }
    }

    public float MaxScale = 4f;
    public float MinScale = 0.1f;
    float scaleAdding;
    Vector3 initialObjectScale;
    void onScaleStarted(GameObject targetGO)
    {
        if (EnableScale && targetGO == gameObject)
        {
            scaleAdding = 0;
            initialObjectScale = TargetObject.transform.localScale;
            whenManipulationStartedUserWatchedOnMe = true;

            if (ScaleStarted != null)
            {
                ScaleStarted.Invoke();
            }
        }
    }

    void onScale(Vector3 scales)
    {
        if (EnableScale && whenManipulationStartedUserWatchedOnMe)
        {
            scaleAdding += scales.y * ScanlingSensitivity * Time.deltaTime;

            if (scaleAdding > MaxScale - 1)
            {
                scaleAdding = MaxScale - 1;
            }
            else if (scaleAdding < MinScale - 1)
            {
                scaleAdding = MinScale - 1;
            }

            if (targetInterpolator != null)
            {
                targetInterpolator.SetTargetLocalScale(initialObjectScale * (1 + scaleAdding));
            }
            else
            {
                TargetObject.transform.localScale = initialObjectScale * (1 + scaleAdding);
            }

            if (Scaling != null)
            {
                Scaling.Invoke();
            }

            /*
            if (TargetObject.GetComponent<IDHolder>())
            {
                var id = TargetObject.GetComponent<IDHolder>().ID;
                SV_Sharing.Instance.SendJson(new SV_Events.Case2(id, TargetObject.transform), "ScaleModel"); // 28
            }
            */
        }
    }

    void OnDestroy()
    {
        if (NativeManipulationAgent.Instance != null)
        {
            NativeManipulationAgent.Instance.Move -= onMove;
            NativeManipulationAgent.Instance.Rotate -= onRotate;
            NativeManipulationAgent.Instance.Scale -= onScale;

            NativeManipulationAgent.Instance.MoveStarted -= onMoveStarted;
            NativeManipulationAgent.Instance.ScaleStarted -= onScaleStarted;
            NativeManipulationAgent.Instance.RotationStarted -= onRotationStarted;

            NativeManipulationAgent.Instance.MoveEnded -= onMoveEnded;
            NativeManipulationAgent.Instance.ScaleEnded -= onScaleEnded;
            NativeManipulationAgent.Instance.RotationEnded -= onRotationEnded;
        }
    }
}
