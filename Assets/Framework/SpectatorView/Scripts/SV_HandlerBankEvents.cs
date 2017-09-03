using HoloToolkit.Sharing;
using System;
using UnityEngine;
using Andy.IdGenerator;
using SV_Data = SV_Sharing.SharingData;

public class SV_HandlerBankEvents : MonoBehaviour
{
    #region Event Methods

    public void OnInt(SV_Data data)
    {
        switch (data.tag)
        {
            // 4
            case "NativeSingleTapManipulationStarted":
                var obj = GetObjById.Instance.GetObject(data.intValue);

                if (obj)
                {
                    NativeManipulationManager.Instance.NativeSingleTapManipulationStarted.Invoke(obj);
                }

                Debug.Log("NativeSingleTapManipulationStarted: " + data.intValue);

                break;
            // 5
            case "NativeDoubleTapManipulationStarted":
                var obj1 = GetObjById.Instance.GetObject(data.intValue);

                if (obj1)
                {
                    NativeManipulationManager.Instance.NativeDoubleTapManipulationStarted.Invoke(obj1);
                }

                Debug.Log("NativeDoubleTapManipulationStarted: " + data.intValue);

                break;
            // 7
            case "NativeXYManipulationStarted":
                var obj2 = GetObjById.Instance.GetObject(data.intValue);

                if (obj2)
                {
                    NativeManipulationManager.Instance.NativeXYManipulationStarted.Invoke(obj2);
                }

                Debug.Log("NativeXYManipulationStarted: " + data.intValue);

                break;

            // 8
            case "NativeXManipulationStarted":
                var obj3 = GetObjById.Instance.GetObject(data.intValue);

                if (obj3)
                {
                    NativeManipulationManager.Instance.NativeXManipulationStarted.Invoke(obj3);
                }

                Debug.Log("NativeXManipulationStarted: " + data.intValue);

                break;

            // 9
            case "NativeYManipulationStarted":
                var obj4 = GetObjById.Instance.GetObject(data.intValue);

                if (obj4)
                {
                    NativeManipulationManager.Instance.NativeYManipulationStarted.Invoke(obj4);
                }

                Debug.Log("NativeYManipulationStarted: " + data.intValue);

                break;

            // 10
            case "NativeZManipulationStarted":
                var obj5 = GetObjById.Instance.GetObject(data.intValue);

                if (obj5)
                {
                    NativeManipulationManager.Instance.NativeZManipulationStarted.Invoke(obj5);
                }

                Debug.Log("NativeZManipulationStarted: " + data.intValue);

                break;

            // 21
            case "UserTapped":
                var obj6 = GetObjById.Instance.GetObject(data.intValue);

                if (obj6)
                {
                    TapsListener.Instance.UserTapped.Invoke(obj6);
                }

                Debug.Log("UserTapped: " + data.intValue);

                break;

            // 22
            case "UserDoubleTapped":
                var obj7 = GetObjById.Instance.GetObject(data.intValue);

                if (obj7)
                {
                    TapsListener.Instance.UserDoubleTapped.Invoke(obj7);
                }

                Debug.Log("UserDoubleTapped: " + data.intValue);

                break;

            // 23
            case "UserSingleTapPressed":
                var obj8 = GetObjById.Instance.GetObject(data.intValue);

                if (obj8)
                {
                    TapsListener.Instance.UserSingleTapPressed.Invoke(obj8);
                }

                Debug.Log("UserSingleTapPressed: " + data.intValue);

                break;

            // 24
            case "UserDoubleTapPressed":
                var obj9 = GetObjById.Instance.GetObject(data.intValue);

                if (obj9)
                {
                    TapsListener.Instance.UserDoubleTapPressed.Invoke(obj9);
                }

                Debug.Log("UserDoubleTapPressed: " + data.intValue);

                break;

            // 25
            case "UserSingleTapped":
                var obj10 = GetObjById.Instance.GetObject(data.intValue);

                if (obj10)
                {
                    TapsListener.Instance.UserSingleTapped.Invoke(obj10);
                }

                Debug.Log("UserSingleTapped: " + data.intValue);

                break;

            case "change_lang":

                LanguageController.Instance.ChooseLang(data.intValue, true);

                Debug.Log("change_lang: " + data.intValue);

                break;

            case "test":
                Debug.Log("Value: " + data.intValue);
                break;
        }
    }

    public void OnFloat(SV_Data data)
    {
        switch (data.tag)
        {
            // 11
            case "NativeXYManipulationUpdated":
                NativeManipulationManager.Instance.NativeXYManipulationUpdated.Invoke(data.floatValue);
                Debug.Log("NativeXYManipulationUpdated: " + data.floatValue);
                break;

            // 12
            case "NativeXManipulationUpdated":
                NativeManipulationManager.Instance.NativeXManipulationUpdated.Invoke(data.floatValue);
                Debug.Log("NativeXManipulationUpdated: " + data.floatValue);
                break;

            // 13
            case "NativeYManipulationUpdated":
                NativeManipulationManager.Instance.NativeYManipulationUpdated.Invoke(data.floatValue);
                Debug.Log("NativeYManipulationUpdated: " + data.floatValue);
                break;

            // 14
            case "NativeZManipulationUpdated":
                NativeManipulationManager.Instance.NativeZManipulationUpdated.Invoke(data.floatValue);
                Debug.Log("NativeZManipulationUpdated: " + data.floatValue);
                break;
        }
    }

    public void OnJson(SV_Data data)
    {
        switch (data.tag)
        {
            // 2, 6
            case "NativeSingleTapManipulationUpdated":
                var obj = JsonUtility.FromJson<Case1>(data.stringValue);
                NativeManipulationManager.Instance.NativeSingleTapManipulationUpdated.Invoke(obj.v3);
                Debug.Log("NativeSingleTapManipulationUpdated: " + obj.v3);
                break;

            // 3
            case "NativeDoubleTapManipulationUpdated":
                var obj1 = JsonUtility.FromJson<Case1>(data.stringValue);
                NativeManipulationManager.Instance.NativeDoubleTapManipulationUpdated.Invoke(obj1.v3);
                Debug.Log("NativeDoubleTapManipulationUpdated: " + obj1.v3);
                break;

            // 26
            case "MoveModel":
                var obj2 = JsonUtility.FromJson<Case2>(data.stringValue);

                var model = GetObjById.Instance.GetObject(data.intValue);

                ApplyTransform(model, obj2);
                Debug.Log("MoveModel: " + obj2.transform.position);
                break;

            // 27
            case "RotateModel":
                var obj3 = JsonUtility.FromJson<Case2>(data.stringValue);

                var model1 = GetObjById.Instance.GetObject(data.intValue);

                ApplyTransform(model1, obj3);
                Debug.Log("RotateModal: " + obj3.transform.rotation);
                break;

            // 28
            case "ScaleModel":
                var obj4 = JsonUtility.FromJson<Case2>(data.stringValue);

                var model2 = GetObjById.Instance.GetObject(data.intValue);

                ApplyTransform(model2, obj4);
                Debug.Log("ScaleModal: " + obj4.transform.localScale);
                break;
        }
    }

    public void OnBool(SV_Data data)
    {
        switch (data.tag)
        {
            // 1
            case "DelayTriggered":
                NativeManipulationManager.Instance.DelayTriggered.Invoke();
                Debug.Log("DelayTriggered");
                break;
            // 15
            case "NativeSingleTapManipulationCompleted":
                NativeManipulationManager.Instance.NativeSingleTapManipulationCompleted.Invoke();
                Debug.Log("NativeSingleTapManipulationCompleted");
                break;
            // 16
            case "NativeDoubleTapManipulationCompleted":
                NativeManipulationManager.Instance.NativeDoubleTapManipulationCompleted.Invoke();
                Debug.Log("NativeDoubleTapManipulationCompleted");
                break;
            // 17
            case "NativeXManipulationCompleted":
                NativeManipulationManager.Instance.NativeXManipulationCompleted.Invoke();
                Debug.Log("NativeXManipulationCompleted");
                break;
            // 18
            case "NativeYManipulationCompleted":
                NativeManipulationManager.Instance.NativeYManipulationCompleted.Invoke();
                Debug.Log("NativeYManipulationCompleted");
                break;
            // 19
            case "NativeZManipulationCompleted":
                NativeManipulationManager.Instance.NativeZManipulationCompleted.Invoke();
                Debug.Log("NativeZManipulationCompleted");
                break;
            // 20
            case "NativeXYManipulationCompleted":
                NativeManipulationManager.Instance.NativeXYManipulationCompleted.Invoke();
                Debug.Log("NativeXYManipulationCompleted");
                break;
        }
    }

    #endregion

    #region Utility Methods

    private void ApplyTransform(GameObject model, Case2 obj)
    {
        if (model)
        {
            model.transform.position = obj.transform.position;
            model.transform.rotation = obj.transform.rotation;
            model.transform.localScale = obj.transform.localScale;
        }
    }

    #endregion

    #region Nested Classes

    [Serializable]
    public class Case1
    {
        public Vector3 v3;

        public Case1(Vector3 v3)
        {
            this.v3 = v3;
        }
    }

    [Serializable]
    public class Case2
    {
        public int ID;
        public Transform transform;

        public Case2(int ID, Transform transform)
        {
            this.ID = ID;
            this.transform = transform;
        }
    }

    #endregion
}
