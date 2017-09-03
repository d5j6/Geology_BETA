using UnityEngine;
using System;

public class EarthLayerController : MonoBehaviour
{
    private new string name;
    private float outerLevel;
    private float innerLevel;

    #region Interface

    public void Initialize(EarthController.LayerState layer)
    {
        switch (layer)
        {
            case EarthController.LayerState.Surface:
                name = "Surface";
                outerLevel = EarthController.SurfaceLevel;
                innerLevel = EarthController.CrustLevel;
                break;
            case EarthController.LayerState.Crust:
                name = "Crust";
                outerLevel = EarthController.CrustLevel;
                innerLevel = EarthController.UpperMantleLevel;
                break;
            case EarthController.LayerState.UpperMantle:
                name = "UpperMantle";
                outerLevel = EarthController.UpperMantleLevel;
                innerLevel = EarthController.LowerMantleLevel;
                break;
            case EarthController.LayerState.LowerMantle:
                name = "LowerMantle";
                outerLevel = EarthController.LowerMantleLevel;
                innerLevel = EarthController.OuterCoreLevel;
                break;
            case EarthController.LayerState.OuterCore:
                outerLevel = EarthController.OuterCoreLevel;
                innerLevel = EarthController.InnerCoreLevel;
                name = "OuterCore";
                break;
            case EarthController.LayerState.InnerCore:
                outerLevel = EarthController.InnerCoreLevel;
                innerLevel = 0f;
                name = "InnerCore";
                break;
        }
    }

    private float currentRotation = 0f;
    private enum RotationMode { ByTransform, ByTexture }
    private RotationMode rotationMode = RotationMode.ByTransform;
    public void Rotate(float angle)
    {
        currentRotation += angle * Time.deltaTime;
        if (rotationMode == RotationMode.ByTransform)
        {
            transform.localRotation = Quaternion.Euler(0f, currentRotation, 0f);
        }
        else
        {
            if (outerPlaneMaterial != null) outerPlaneMaterial.SetFloat("_RotationAngle", currentRotation);
            if (innerPlaneMaterial != null) innerPlaneMaterial.SetFloat("_RotationAngle", currentRotation);
        }
    }

    public void ClearRequiredPlanes()
    {
        requierdPlanes = Planes.None;
    }

    public void RequireOuter()
    {
        FlagsHelper.Set(ref requierdPlanes, Planes.Outer);
    }

    public void RequireInner()
    {
        FlagsHelper.Set(ref requierdPlanes, Planes.Inner);
    }

    public void RequireSides()
    {
        FlagsHelper.Set(ref requierdPlanes, Planes.Sides);
    }

    public void Join()
    {
        requiredSetting = EarthController.SettingState.Joined;
    }

    public void Extend()
    {
        requiredSetting = EarthController.SettingState.Extended;
    }

    public void Slice()
    {
        requiredWholeness = EarthController.WholenessState.Sliced;
    }

    public void Whole()
    {
        requiredWholeness = EarthController.WholenessState.Whole;
    }

    public void ChangeStatesImmediately()
    {
        showPlanes();
        sliceLayerImmediately();
        extendLayerImmediately();
        joinLayerImmediately();
        wholeLayerImmediately();
        hidePlanes();
    }

    public Action AllStructuralChangesDone;

    private Action onStateChanged;
    public void ChangeStates(Action callback = null)
    {
        /*
         * При изменении состояний важна последовательность
         * Если требуется показать какой-то плейн, он открывается в первую очередь. Если требуется скрыть какой-то плейн, он убирается после всех остальных действий.
         * Нам никогда не придется отдалять закрытый слой, потому что закрытыми могут быть только выбранный слой или слои, лежащие ниже него, а они всегда придвинуты.
         * Но когда нам нужно придвинуть слой, его иногда нужно еще и закрыть, поэтому сначала придвигаем, затем закрываем
         * Таким образом получаем универсальную последовательность действий: Показываем плейны, открываем слои, выдвигаем слои, задвигаем слои, закрываем слои, скрываем плейны
         */

        onStateChanged = callback;

        showPlanes(() =>
        {
            sliceLayer(() =>
            {
                extendLayer(() =>
                {
                    joinLayer(() =>
                    {
                        wholeLayer(() =>
                        {
                            if (AllStructuralChangesDone != null)
                            {
                                /*
                                 * Так как каждый слой закончит в разное время, а исчезать должны одновременно, контролируем это через основной скрипт отдельно.
                                 */
                                AllStructuralChangesDone.Invoke();
                            }
                        });
                    });
                });
            });
        });
    }

    public void PerformHideOperation()
    {
        hidePlanes(() =>
        {
            if (onStateChanged != null)
            {
                onStateChanged.Invoke();
                onStateChanged = null;
            }
        });
    }

    public Action Extended;
    public Action Joined;

    #endregion

    #region Planes

    [Flags]
    private enum Planes
    {
        None = 0,
        Sides = 1,
        Outer = 2,
        Inner = 4
    }

    private Material outerPlaneMaterial;
    private Material leftSidePlaneMaterial;
    private Material rightSidePlaneMaterial;
    private Material innerPlaneMaterial;
    private GameObject outerPlane;
    private GameObject leftSidePlane;
    private GameObject rightSidePlane;
    private GameObject innerPlane;

    private Planes currentPlanes = Planes.None;
    private Planes requierdPlanes = Planes.None;

    private int planesLoading = 0;
    private Action callbackOnPlanesLoaded;
    private void planeLoaded()
    {
        planesLoading--;
        if (planesLoading == 0)
        {
            if (callbackOnPlanesLoaded != null)
            {
                callbackOnPlanesLoaded.Invoke();
                callbackOnPlanesLoaded = null;
            }
        }
    }

    private void bringOuterPlane(Action callback = null)
    {
        planesLoading++;
        callbackOnPlanesLoaded = callback;
        Loader.Instance.LoadAndIntsntiateGOPrefab(name + "OuterPlane", outerPlaneBrought);
    }

    private void outerPlaneBrought(GameObject go)
    {
        outerPlane = go;
        go.transform.parent = transform;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;

        MeshRenderer rend = go.GetComponent<MeshRenderer>();
        outerPlaneMaterial = rend.material;
        outerPlaneMaterial.SetFloat("_Closed", currentCloseLevel);
        outerPlaneMaterial.SetFloat("_Diameter", outerLevel);
        rend.enabled = true;

        planeLoaded();
    }

    private void bringInnerPlane(Action callback = null)
    {
        planesLoading++;
        callbackOnPlanesLoaded = callback;
        Loader.Instance.LoadAndIntsntiateGOPrefab(name + "InnerPlane", innerPlaneBrought);
    }

    private void innerPlaneBrought(GameObject go)
    {
        innerPlane = go;
        go.transform.parent = transform;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;

        MeshRenderer rend = go.GetComponent<MeshRenderer>();
        innerPlaneMaterial = rend.material;
        innerPlaneMaterial.SetFloat("_Closed", currentCloseLevel);
        innerPlaneMaterial.SetFloat("_Diameter", innerLevel);
        rend.enabled = true;

        planeLoaded();
    }

    private void bringLeftSidePlane(Action callback = null)
    {
        planesLoading++;
        callbackOnPlanesLoaded = callback;
        Loader.Instance.LoadAndIntsntiateGOPrefab(name + "LeftSlice", leftSidePlaneBrought);
    }

    private void leftSidePlaneBrought(GameObject go)
    {
        leftSidePlane = go;
        go.transform.parent = transform;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;

        MeshRenderer rend = go.GetComponent<MeshRenderer>();
        leftSidePlaneMaterial = rend.material;
        leftSidePlaneMaterial.SetFloat("_Closed", currentCloseLevel);
        leftSidePlaneMaterial.SetFloat("_DiameterOuter", outerLevel);
        leftSidePlaneMaterial.SetFloat("_DiameterInner", innerLevel);
        rend.enabled = true;

        planeLoaded();
    }

    private void bringRightSidePlane(Action callback = null)
    {
        planesLoading++;
        callbackOnPlanesLoaded = callback;
        Loader.Instance.LoadAndIntsntiateGOPrefab(name + "RightSlice", rightSidePlaneBrought);
    }

    private void rightSidePlaneBrought(GameObject go)
    {
        rightSidePlane = go;
        go.transform.parent = transform;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;

        MeshRenderer rend = go.GetComponent<MeshRenderer>();
        rightSidePlaneMaterial = rend.material;
        rightSidePlaneMaterial.SetFloat("_Closed", currentCloseLevel);
        rightSidePlaneMaterial.SetFloat("_DiameterOuter", outerLevel);
        rightSidePlaneMaterial.SetFloat("_DiameterInner", innerLevel);
        rend.enabled = true;

        planeLoaded();
    }

    private void DestroyOuterPlane()
    {
        Destroy(outerPlane);
        outerPlane = null;
        outerPlaneMaterial = null;
    }

    private void DestroyInnerPlane()
    {
        Destroy(innerPlane);
        innerPlane = null;
        innerPlaneMaterial = null;
    }

    private void DestroyLeftSidePlane()
    {
        Destroy(leftSidePlane);
        leftSidePlane = null;
        leftSidePlaneMaterial = null;
    }

    private void DestroyRightSidePlane()
    {
        Destroy(rightSidePlane);
        rightSidePlane = null;
        rightSidePlaneMaterial = null;
    }

    #endregion

    #region Setting

    private EarthController.SettingState currentSetting = EarthController.SettingState.Joined;
    private EarthController.SettingState requiredSetting = EarthController.SettingState.Joined;
    
    private void join(Action callback = null)
    {
        if (Joined != null)
        {
            Joined.Invoke();
        }

        LeanTween.value(gameObject, 1f, 0f, EarthController.ChangeSettingTime).setEase(EarthController.DefaultTweenType).setOnUpdate((float val) =>
        {
            transform.localPosition = Mathf.Lerp(0f, EarthController.ExtendDistance, val) * Vector3.forward;
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    private void extend(Action callback = null)
    {
        if (Extended != null)
        {
            Extended.Invoke();
        }

        LeanTween.value(gameObject, 0f, 1f, EarthController.ChangeSettingTime).setEase(EarthController.DefaultTweenType).setOnUpdate((float val) =>
        {
            transform.localPosition = Mathf.Lerp(0f, EarthController.ExtendDistance, val) * Vector3.forward;
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    private void joinLayerImmediately()
    {
        if (requiredSetting == EarthController.SettingState.Joined && requiredSetting != currentSetting)
        {
            currentSetting = requiredSetting;

            transform.localPosition = Vector3.zero;
        }
    }

    private void extendLayerImmediately()
    {
        if (requiredSetting == EarthController.SettingState.Extended && requiredSetting != currentSetting)
        {
            currentSetting = requiredSetting;

            transform.localPosition = EarthController.ExtendDistance * Vector3.forward;
        }
    }

    #endregion

    #region Wholeness

    private EarthController.WholenessState currentWholeness = EarthController.WholenessState.Whole;
    private EarthController.WholenessState requiredWholeness = EarthController.WholenessState.Whole;

    private float currentCloseLevel = 1f;
    private void slice(Action callback = null)
    {
        LeanTween.value(gameObject, 1f, 0.5f, EarthController.ChangeWholenessTime).setEase(EarthController.DefaultTweenType).setOnStart(() =>
        {
            rotationMode = RotationMode.ByTexture;
            transform.localRotation = Quaternion.identity;
        }).setOnUpdate((float val) =>
        {
            currentCloseLevel = val;
            if (outerPlaneMaterial != null)
            {
                outerPlaneMaterial.SetFloat("_Closed", val);
            }
            if (innerPlaneMaterial != null)
            {
                innerPlaneMaterial.SetFloat("_Closed", val);
            }
            if (leftSidePlaneMaterial != null)
            {
                leftSidePlaneMaterial.SetFloat("_Closed", val);
            }
            if (rightSidePlaneMaterial != null)
            {
                rightSidePlaneMaterial.SetFloat("_Closed", val);
            }
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    private void sliceLayerImmediately()
    {
        if (requiredWholeness == EarthController.WholenessState.Sliced && requiredWholeness != currentWholeness)
        {
            currentWholeness = requiredWholeness;

            rotationMode = RotationMode.ByTexture;
            transform.localRotation = Quaternion.identity;

            currentCloseLevel = 0.5f;
            if (outerPlaneMaterial != null)
            {
                outerPlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
            if (innerPlaneMaterial != null)
            {
                innerPlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
            if (leftSidePlaneMaterial != null)
            {
                leftSidePlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
            if (rightSidePlaneMaterial != null)
            {
                rightSidePlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
        }
    }

    private void wholeLayerImmediately()
    {
        if (requiredWholeness == EarthController.WholenessState.Whole && requiredWholeness != currentWholeness)
        {
            currentWholeness = requiredWholeness;

            rotationMode = RotationMode.ByTransform;
            if (outerPlaneMaterial != null) outerPlaneMaterial.SetFloat("_RotationAngle", 0f);
            if (innerPlaneMaterial != null) innerPlaneMaterial.SetFloat("_RotationAngle", 0f);

            currentCloseLevel = 1f;
            if (outerPlaneMaterial != null)
            {
                outerPlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
            if (innerPlaneMaterial != null)
            {
                innerPlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
            if (leftSidePlaneMaterial != null)
            {
                leftSidePlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
            if (rightSidePlaneMaterial != null)
            {
                rightSidePlaneMaterial.SetFloat("_Closed", currentCloseLevel);
            }
        }
    }

    private void whole(Action callback = null)
    {
        LeanTween.value(gameObject, 0.5f, 1f, EarthController.ChangeWholenessTime).setEase(EarthController.DefaultTweenType).setOnUpdate((float val) =>
        {
            currentCloseLevel = val;
            if (outerPlaneMaterial != null)
            {
                outerPlaneMaterial.SetFloat("_Closed", val);
            }
            if (innerPlaneMaterial != null)
            {
                innerPlaneMaterial.SetFloat("_Closed", val);
            }
            if (leftSidePlaneMaterial != null)
            {
                leftSidePlaneMaterial.SetFloat("_Closed", val);
            }
            if (rightSidePlaneMaterial != null)
            {
                rightSidePlaneMaterial.SetFloat("_Closed", val);
            }
        }).setOnComplete(() =>
        {
            rotationMode = RotationMode.ByTransform;
            if (outerPlaneMaterial != null) outerPlaneMaterial.SetFloat("_RotationAngle", 0f);
            if (innerPlaneMaterial != null) innerPlaneMaterial.SetFloat("_RotationAngle", 0f);

            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    #endregion

    #region Actions Chain

    private void showPlanes(Action callback = null)
    {
        bool planesShowing = false;
        if (FlagsHelper.IsSet(requierdPlanes, Planes.Outer) && !FlagsHelper.IsSet(currentPlanes, Planes.Outer))
        {
            planesShowing = true;
            FlagsHelper.Set(ref currentPlanes, Planes.Outer);

            bringOuterPlane(callback);
        }
        if (FlagsHelper.IsSet(requierdPlanes, Planes.Sides) && !FlagsHelper.IsSet(currentPlanes, Planes.Sides))
        {
            planesShowing = true;
            FlagsHelper.Set(ref currentPlanes, Planes.Sides);

            bringLeftSidePlane(callback);
            bringRightSidePlane(callback);
        }
        if (FlagsHelper.IsSet(requierdPlanes, Planes.Inner) && !FlagsHelper.IsSet(currentPlanes, Planes.Inner))
        {
            planesShowing = true;
            FlagsHelper.Set(ref currentPlanes, Planes.Inner);

            bringInnerPlane(callback);
        }

        if (!planesShowing && callback != null)
        {
            callback.Invoke();
        }
    }

    private void sliceLayer(Action callback = null)
    {
        if (requiredWholeness == EarthController.WholenessState.Sliced && requiredWholeness != currentWholeness)
        {
            currentWholeness = requiredWholeness;
            slice(callback);
        }
        else if (callback != null)
        {
            callback.Invoke();
        }
    }

    private void extendLayer(Action callback = null)
    {
        if (requiredSetting == EarthController.SettingState.Extended && requiredSetting != currentSetting)
        {
            currentSetting = requiredSetting;
            extend(callback);
        }
        else if (callback != null)
        {
            callback.Invoke();
        }
    }

    private void joinLayer(Action callback = null)
    {
        if (requiredSetting == EarthController.SettingState.Joined && requiredSetting != currentSetting)
        {
            currentSetting = requiredSetting;
            join(callback);
        }
        else if (callback != null)
        {
            callback.Invoke();
        }
    }

    private void wholeLayer(Action callback = null)
    {
        if (requiredWholeness == EarthController.WholenessState.Whole && requiredWholeness != currentWholeness)
        {
            currentWholeness = requiredWholeness;
            whole(callback);
        }
        else if (callback != null)
        {
            callback.Invoke();
        }
    }

    private void hidePlanes(Action callback = null)
    {
        if (!FlagsHelper.IsSet(requierdPlanes, Planes.Outer) && FlagsHelper.IsSet(currentPlanes, Planes.Outer))
        {
            FlagsHelper.Unset(ref currentPlanes, Planes.Outer);
            DestroyOuterPlane();
        }
        if (!FlagsHelper.IsSet(requierdPlanes, Planes.Sides) && FlagsHelper.IsSet(currentPlanes, Planes.Sides))
        {
            FlagsHelper.Unset(ref currentPlanes, Planes.Sides);
            DestroyLeftSidePlane();
            DestroyRightSidePlane();
        }
        if (!FlagsHelper.IsSet(requierdPlanes, Planes.Inner) && FlagsHelper.IsSet(currentPlanes, Planes.Inner))
        {
            FlagsHelper.Unset(ref currentPlanes, Planes.Inner);
            DestroyInnerPlane();
        }

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    #endregion
}
