using HoloToolkit.Unity;
using UnityEngine;
using System;

public class EarthController : Singleton<EarthController>
{
    public static float RotationSpeed = 5f;
    public static float ChangeSettingTime = 7.0f;
    public static float ExtendDistance = 1f;
    public static float ChangeWholenessTime = 3.5f;
    public static LeanTweenType DefaultTweenType = LeanTweenType.easeInOutCubic;

    public static float SurfaceLevel = 1f;
    public static float CrustLevel = 0.995f;
    public static float UpperMantleLevel = 0.95986f;
    public static float LowerMantleLevel = 0.828f;
    public static float OuterCoreLevel = 0.52619f;
    public static float InnerCoreLevel = 0.22639f;

    public enum LayerState { InnerCore, OuterCore, LowerMantle, UpperMantle, Crust, Surface, None }
    public enum SettingState { Joined, Extended, None }
    public enum WholenessState { Whole, Sliced, None }

    private LayerState currentLayer = LayerState.None;
    private SettingState currentSetting = SettingState.None;
    private WholenessState currentWholeness = WholenessState.None;

    public EarthLayerController SurfaceController;
    public EarthLayerController CrustController;
    public EarthLayerController UpperMantleController;
    public EarthLayerController LowerMantleController;
    public EarthLayerController OuterCoreController;
    public EarthLayerController InnerCoreController;

    private EarthLayerController[] layers = new EarthLayerController[6];

    public GameObject GetSurfaceGO()
    {
        return SurfaceController.gameObject.transform.GetChild(0).gameObject;
    }

    public Material GetSurfaceMaterial()
    {
        return SurfaceController.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        Surface().Whole().Join().Immediately().Go();
    }

    private void Awake()
    {
        initialize();
    }

    private new SphereCollider collider;

    private void initialize()
    {
        layers[0] = InnerCoreController;
        layers[0].Initialize(LayerState.InnerCore);
        layers[1] = OuterCoreController;
        layers[1].Initialize(LayerState.OuterCore);
        layers[2] = LowerMantleController;
        layers[2].Initialize(LayerState.LowerMantle);
        layers[3] = UpperMantleController;
        layers[3].Initialize(LayerState.UpperMantle);
        layers[4] = CrustController;
        layers[4].Initialize(LayerState.Crust);
        layers[5] = SurfaceController;
        layers[5].Initialize(LayerState.Surface);

        collider = GetComponent<SphereCollider>();
    }

    private void OnDestroy()
    {

    }
    
    private void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            layers[i].Rotate(RotationSpeed);
        }
    }

    public Action EarthWideOpened;
    public Action EarthWideClosed;
    public Action EarthIntermediateState;

    public void OpenEarthWidely(Action callback = null)
    {
        InnerCore().Slice().Join().Go(callback);
    }

    public EarthController Surface()
    {
        goToLayer(LayerState.Surface);
        return this;
    }

    public EarthController Crust()
    {
        goToLayer(LayerState.Crust);
        return this;
    }

    public EarthController UpperMantle()
    {
        goToLayer(LayerState.UpperMantle);
        return this;
    }

    public EarthController LowerMantle()
    {
        goToLayer(LayerState.LowerMantle);
        return this;
    }

    public EarthController OuterCore()
    {
        goToLayer(LayerState.OuterCore);
        return this;
    }

    public EarthController InnerCore()
    {
        goToLayer(LayerState.InnerCore);
        return this;
    }

    public EarthController Whole()
    {
        goToWholeness(WholenessState.Whole);
        return this;
    }

    public EarthController Slice()
    {
        goToWholeness(WholenessState.Sliced);
        return this;
    }

    public EarthController Join()
    {
        goToSetting(SettingState.Joined);
        return this;
    }

    public EarthController Extend()
    {
        goToSetting(SettingState.Extended);
        return this;
    }

    private bool immediately = false;
    public EarthController Immediately()
    {
        immediately = true;
        return this;
    }

    /// <summary>
    /// Вызывается, когда начинается переход между состояниями
    /// </summary>
    public Action ChangesStarted;
    /// <summary>
    /// Вызывается, когда переход между состояниями закончен
    /// </summary>
    public Action ChangesEnded;
    
    private Action callbackOnGo;
    public void Go(Action callback = null)
    {
        if (changesWhereMade)
        {
            if (currentLayer == LayerState.Surface && currentWholeness == WholenessState.Whole && currentSetting == SettingState.Joined)
            {
                if (EarthWideClosed != null)
                {
                    EarthWideClosed.Invoke();
                }
            }
            else if (currentLayer == LayerState.InnerCore && currentWholeness == WholenessState.Sliced && currentSetting == SettingState.Joined)
            {
                if (EarthWideOpened != null)
                {
                    EarthWideOpened.Invoke();
                }
            }
            else
            {
                if (EarthIntermediateState != null)
                {
                    EarthIntermediateState.Invoke();
                }
            }

            if (ChangesStarted != null)
            {
                ChangesStarted.Invoke();
            }

            changesWhereMade = false;

            refreshStates();
            
            if (!immediately)
            {
                callbackOnGo = callback;

                for (int i = 0; i < 6; i++)
                {
                    layers[i].AllStructuralChangesDone += allStructuralChangesDone;
                    layers[i].ChangeStates(layerFinished);
                }
            }
            else
            {
                immediately = false;
                for (int i = 0; i < 6; i++)
                {
                    layers[i].ChangeStatesImmediately();
                }

                if (callback != null)
                {
                    callback.Invoke();
                }

                if (ChangesEnded != null)
                {
                    ChangesEnded.Invoke();
                }
            }
        }
        else
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    private int layersDone = 0;
    private void allStructuralChangesDone()
    {
        layersDone++;
        if (layersDone == 6)
        {
            layersDone = 0;
            for (int i = 0; i < 6; i++)
            {
                layers[i].AllStructuralChangesDone -= allStructuralChangesDone;
                layers[i].PerformHideOperation();
            }
        }
    }

    private int layersFinished = 0;
    private void layerFinished()
    {
        layersFinished++;
        if (layersFinished == 6)
        {
            layersFinished = 0;
            if (callbackOnGo != null)
            {
                callbackOnGo.Invoke();
                callbackOnGo = null;
            }

            if (ChangesEnded != null)
            {
                ChangesEnded.Invoke();
            }
        }
    }

    private bool changesWhereMade = false;

    private void goToWholeness(WholenessState newWholeness)
    {
        if (newWholeness != currentWholeness)
        {
            changesWhereMade = true;
            currentWholeness = newWholeness;
        }
    }

    private void goToSetting(SettingState newSetting)
    {
        if (newSetting != currentSetting)
        {
            switch (newSetting)
            {
                case SettingState.Extended:
                    if (Extended != null)
                    {
                        Extended.Invoke();
                    }
                    break;
                case SettingState.Joined:
                    if (Joined != null)
                    {
                        Joined.Invoke();
                    }
                    break;
            }
            changesWhereMade = true;
            currentSetting = newSetting;
        }
    }

    public Action SurfaceShowed;
    public Action SurfaceHided;
    public Action CrustShowed;
    public Action CrustHided;
    public Action UpperMantleShowed;
    public Action UpperMantleHided;
    public Action LowerMantleShowed;
    public Action LowerMantleHided;
    public Action OuterCoreShowed;
    public Action OuterCoreHided;
    public Action InnerCoreShowed;
    public Action InnerCoreHided;

    public Action Extended;
    public Action Joined;

    private void goToLayer(LayerState newLayer)
    {
        if (currentLayer != newLayer)
        {
            switch (currentLayer)
            {
                case LayerState.Surface:
                    if (SurfaceHided != null) SurfaceHided.Invoke();
                    break;
                case LayerState.Crust:
                    if (CrustHided != null) CrustHided.Invoke();
                    break;
                case LayerState.UpperMantle:
                    if (UpperMantleHided != null) UpperMantleHided.Invoke();
                    break;
                case LayerState.LowerMantle:
                    if (LowerMantleHided != null) LowerMantleHided.Invoke();
                    break;
                case LayerState.OuterCore:
                    if (OuterCoreHided != null) OuterCoreHided.Invoke();
                    break;
                case LayerState.InnerCore:
                    if (InnerCoreHided != null) InnerCoreHided.Invoke();
                    break;
            }
            switch (newLayer)
            {
                case LayerState.Surface:
                    if (SurfaceShowed != null) SurfaceShowed.Invoke();
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Earth);
                    collider.radius = SurfaceLevel / 2f;
                    break;
                case LayerState.Crust:
                    if (CrustShowed != null) CrustShowed.Invoke();
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Crust);
                    collider.radius = CrustLevel / 2f;
                    break;
                case LayerState.UpperMantle:
                    if (UpperMantleShowed != null) UpperMantleShowed.Invoke();
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.UpperMantle);
                    collider.radius = UpperMantleLevel / 2f;
                    break;
                case LayerState.LowerMantle:
                    if (LowerMantleShowed != null) LowerMantleShowed.Invoke();
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.LowerMantle);
                    collider.radius = LowerMantleLevel / 2f;
                    break;
                case LayerState.OuterCore:
                    if (OuterCoreShowed != null) OuterCoreShowed.Invoke();
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.OuterCore);
                    collider.radius = OuterCoreLevel / 2f;
                    break;
                case LayerState.InnerCore:
                    if (InnerCoreShowed != null) InnerCoreShowed.Invoke();
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.InnerCore);
                    collider.radius = InnerCoreLevel / 2f;
                    break;
            }

            changesWhereMade = true;
            currentLayer = newLayer;
        }
    }

    private void refreshStates()
    {
        for (int i = 0; i < 6; i++)
        {
            #region Setting Required Plains

            layers[i].ClearRequiredPlanes();

            if (i == 5)
            {
                /*
                 * Нам всегда нужен внешний плейн на внешнем слое. Всегда.
                 */
                layers[i].RequireOuter();
            }
                
            if (i < (int)currentLayer)
            {
                if (currentWholeness == WholenessState.Sliced && i == (int)currentLayer - 1)
                {
                    /*
                     * Единственный вариант, когда мы что-то отображаем на слое, лежащем ниже выбранного - это когда у нас разрезанное состояние и слой четко под выбранным
                     */
                    layers[i].RequireOuter();
                }
            }
            else if (i > (int)currentLayer)
            {
                /*
                 * Если слой находится выше выбранного, то он обязательно должен быть разрезан, следовательно он всегда будет иметь боковые плейны
                 */
                 
                layers[i].RequireSides();

                if (currentSetting == SettingState.Extended && i == (int)currentLayer + 1)
                {
                    /*
                     * Если мы в выдвинутом состоянии, то нам нужен внутренний плейн на следующем после текущего слое.
                     */
                    layers[i].RequireInner();
                }
            }
            else
            {
                /*
                 * Выбранный слой зависит и от целиковости и от выдвинутости
                 */
                if (currentWholeness == WholenessState.Whole)
                {
                    /*
                     * Если у нас целиковое состояние, то мы всегда должны рисовать только внешний слой
                     */
                    layers[i].RequireOuter();
                }
                else
                {
                    /*
                     * Если мы в выдвинутом состоянии и разрезанны, то нам нужны бока и внешний слой. Если задвинуты - то только бока.
                     */
                    if (currentSetting == SettingState.Joined)
                    {
                        layers[i].RequireSides();
                    }
                    else
                    {
                        layers[i].RequireOuter();
                        layers[i].RequireSides();
                    }
                }
            }

            #endregion

            #region Setting Settings

            if (currentSetting == SettingState.Joined)
            {
                layers[i].Join();
            }
            else
            {
                if (i > (int)currentLayer)
                {
                    layers[i].Extend();
                }
                else
                {
                    layers[i].Join();
                }
            }

            #endregion

            #region Setting Wholenesses

            /*
             * Разрезанными должны быть все слои выше текущего
             * Целыми должны быть все слои ниже текущего
             * Текущий зависит от настроек
             */

            if (i < (int)currentLayer)
            {
                layers[i].Whole();
            }
            else if (i > (int)currentLayer)
            {
                layers[i].Slice();
            }
            else
            {
                if (currentWholeness == WholenessState.Whole)
                {
                    layers[i].Whole();
                }
                else
                {
                    layers[i].Slice();
                }
            }

            #endregion
        }
    }

    #region Earth Measuring

    public Action MeasuringStarted;
    public Action MeasuringEnded;

    #region Temperature Measuring

    public Action TemperatureMeasuringStarted;
    public Action TemperatureMeasuringEnded;

    private GameObject temperatureMeasurer;
    private bool temperatureMeasuringLaunched = false;
    public void MeasureTemperature()
    {
        if (!temperatureMeasuringLaunched)
        {
            if (MeasuringStarted != null)
            {
                MeasuringStarted.Invoke();
            }
            BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Temperature);

            if (TemperatureMeasuringStarted != null)
            {
                TemperatureMeasuringStarted.Invoke();
            }
            temperatureMeasuringLaunched = true;

            Loader.Instance.LoadAndIntsntiateGOPrefab("TemperatureMeasurer", onTemperatureMeasurerInstantiated);
        }
    }

    void onTemperatureMeasurerInstantiated(GameObject go)
    {
        temperatureMeasurer = go;
        Vector3 initialScale = temperatureMeasurer.transform.localScale;
        temperatureMeasurer.transform.parent = transform.parent;
        temperatureMeasurer.transform.localScale = initialScale;
        temperatureMeasurer.transform.localPosition = Vector3.zero;
        temperatureMeasurer.transform.localRotation = Quaternion.identity;

        temperatureMeasurer.GetComponent<TemperatureMeasurer>().Planet = transform.parent.gameObject;
        temperatureMeasurer.GetComponent<TemperatureMeasurer>().PlanetSurface = SurfaceController.transform.GetChild(0).gameObject;
        temperatureMeasurer.GetComponent<TemperatureMeasurer>().Measure(onTemperatureMeasuringComplete);
    }

    void onTemperatureMeasuringComplete()
    {
        Destroy(temperatureMeasurer);
        temperatureMeasuringLaunched = false;

        if (TemperatureMeasuringEnded != null)
        {
            TemperatureMeasuringEnded.Invoke();
        }

        if (MeasuringEnded != null)
        {
            MeasuringEnded.Invoke();
        }
    }

    #endregion

    #region Mass Measuring

    public Action MassMeasuringStarted;
    public Action MassMeasuringEnded;

    private GameObject massMeasurer;
    private bool massMeasuringLaunched = false;
    public void MeasureMass()
    {
        if (!massMeasuringLaunched)
        {
            if (MeasuringStarted != null)
            {
                MeasuringStarted.Invoke();
            }
            BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Mass);

            if (MassMeasuringStarted != null)
            {
                MassMeasuringStarted.Invoke();
            }

            massMeasuringLaunched = true;

            Loader.Instance.LoadAndIntsntiateGOPrefab("CurvedSpace", onCurvedDynamicSpaceInstantiated);
        }
    }

    private void onCurvedDynamicSpaceInstantiated(GameObject go)
    {
        massMeasurer = go;
        Vector3 initialScale = massMeasurer.transform.localScale;
        massMeasurer.transform.parent = transform.parent;
        massMeasurer.transform.localScale = initialScale;
        massMeasurer.transform.localPosition = Vector3.zero;
        massMeasurer.transform.localRotation = Quaternion.identity;

        massMeasurer.GetComponent<MassMeasurer>().Planet = gameObject;

        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
        {
            massMeasurer.GetComponent<MassMeasurer>().Measure(onMassMeasuringComplete);
        });
    }

    private void onMassMeasuringComplete()
    {
        Destroy(massMeasurer);
        massMeasuringLaunched = false;

        if (MassMeasuringEnded != null)
        {
            MassMeasuringEnded.Invoke();
        }

        if (MeasuringEnded != null)
        {
            MeasuringEnded.Invoke();
        }
    }

    #endregion

    #region Magnetic Field Measuring

    public Action MagneticFieldMeasuringStarted;
    public Action MagneticFieldMeasuringEnded;

    private GameObject magneticFieldMeasurer;
    private bool magneticFieldMeasuringLaunched = false;
    public void MeasureMagneticField()
    {
        if (!magneticFieldMeasuringLaunched)
        {
            if (MeasuringStarted != null)
            {
                MeasuringStarted.Invoke();
            }
            BigSimpleInfoPanelController.Instance.Show(InformationsMockup.MagneticField);

            if (MagneticFieldMeasuringStarted != null)
            {
                MagneticFieldMeasuringStarted.Invoke();
            }

            magneticFieldMeasuringLaunched = true;

            Loader.Instance.LoadAndIntsntiateGOPrefab("MagneticMeasurer", onMagneticMeasurerInstantiated);
        }
    }

    private void onMagneticMeasurerInstantiated(GameObject go)
    {
        magneticFieldMeasurer = go;
        Vector3 initialScale = magneticFieldMeasurer.transform.localScale;
        magneticFieldMeasurer.transform.parent = transform.parent;
        magneticFieldMeasurer.transform.localScale = initialScale;
        magneticFieldMeasurer.transform.localPosition = Vector3.zero;
        magneticFieldMeasurer.transform.localRotation = Quaternion.identity;

        magneticFieldMeasurer.GetComponent<MagneticFieldMeasurer>().Planet = gameObject;
        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
        {
            magneticFieldMeasurer.GetComponent<MagneticFieldMeasurer>().Measure(onMagneticFieldMeasuringComplete);
        });
    }

    private void onMagneticFieldMeasuringComplete()
    {
        Destroy(magneticFieldMeasurer);
        magneticFieldMeasuringLaunched = false;

        if (MagneticFieldMeasuringEnded != null)
        {
            MagneticFieldMeasuringEnded.Invoke();
        }

        if (MeasuringEnded != null)
        {
            MeasuringEnded.Invoke();
        }
    }

    #endregion

    #endregion

}