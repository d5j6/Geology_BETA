using UnityEngine;
using HoloToolkit.Unity;

public enum Modificateness { Freeze, Free }
public enum PlanetWholeness { Hole = 1, Sliced = 2 }
public enum PlanetSetting { Solo = 1, Jointly = 2, Extended = 3 }
public enum PlanetLayer { Surface = 1, Crust = 2, UpperMantle = 3, LowerMantle = 4, OuterCore = 5, InnerCore = 6, AllOpened = 7 }

public struct SphereDiameter
{
    public float OuterDiameter;
    public float InnerDiameter;
}

public enum SertainPlane { Outer, Inner, LeftAndRight }

public class OptimizedPlanetController1 : Singleton<OptimizedPlanetController1> {
    [Range(0f, 360f)]
    public float RotationSpeed = 15f;

    public GameObject surfaceGO;
    GameObject crustGO;
    GameObject upperMantleGO;
    GameObject lowerMantleGO;
    GameObject outerCoreGO;
    GameObject innerCoreGO;

    OptimizedSphereController1 _surfaceController;
    OptimizedSphereController1 surface
    {
        get
        {
            if (_surfaceController == null)
            {
                CreateLayer(PlanetLayer.Surface);
            }

            return _surfaceController;
        }
        set
        {
            _surfaceController = value;
        }
    }
    OptimizedSphereController1 _crustController;
    OptimizedSphereController1 crust
    {
        get
        {
            if (_crustController == null)
            {
                CreateLayer(PlanetLayer.Crust);
            }

            return _crustController;
        }
        set
        {
            _crustController = value;
        }
    }
    OptimizedSphereController1 _upperMantleController;
    OptimizedSphereController1 upperMantle
    {
        get
        {
            if (_upperMantleController == null)
            {
                CreateLayer(PlanetLayer.UpperMantle);
            }

            return _upperMantleController;
        }
        set
        {
            _upperMantleController = value;
        }
    }
    OptimizedSphereController1 _lowerMantleController;
    OptimizedSphereController1 lowerMantle
    {
        get
        {
            if (_lowerMantleController == null)
            {
                CreateLayer(PlanetLayer.LowerMantle);
            }

            return _lowerMantleController;
        }
        set
        {
            _lowerMantleController = value;
        }
    }
    OptimizedSphereController1 _outerCoreController;
    OptimizedSphereController1 outerCore
    {
        get
        {
            if (_outerCoreController == null)
            {
                CreateLayer(PlanetLayer.OuterCore);
            }

            return _outerCoreController;
        }
        set
        {
            _outerCoreController = value;
        }
    }
    OptimizedSphereController1 _innerCoreController;
    OptimizedSphereController1 innerCore
    {
        get
        {
            if (_innerCoreController == null)
            {
                CreateLayer(PlanetLayer.InnerCore);
            }

            return _innerCoreController;
        }
        set
        {
            _innerCoreController = value;
        }
    }

    SphereDiameter[] sphereDiameters = new SphereDiameter[6];
    PlanetLayer oldLayer;

    public Modificateness modificateness = Modificateness.Free;
    public PlanetWholeness whoness = PlanetWholeness.Hole;
    public PlanetSetting setting = PlanetSetting.Jointly;
    public PlanetLayer layer = PlanetLayer.Surface;

    public float extendedOffset = 0.3f;
    public float extendTime = 2f;
    public float openOnAngleTime = 3.3f;

    public float animationTimeMultiplyer = 1f;
    
    bool massMeasuringLaunched = false;
    bool magneticFieldMeasuringLaunched = false;
    bool temperatureMeasuringLaunched = false;

    GameObject massMeasurer;
    GameObject magneticFieldMeasurer;
    GameObject temperatureMeasurer;
    GameObject einsteinLesson1Holder;

    public void MeasureMass(int qwe = 0)
    {
        if (!massMeasuringLaunched)
        {
            massMeasuringLaunched = true;

            Loader.Instance.LoadAndIntsntiateGOPrefab("CurvedSpace", onCurvedDynamicSpaceInstantiated);
        }
    }

    void onCurvedDynamicSpaceInstantiated(GameObject go)
    {
        massMeasurer = go;
        massMeasurer.GetComponent<MassMeasurer>().Planet = gameObject;

        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
        {
            massMeasurer.GetComponent<MassMeasurer>().Measure(onMassMeasuringComplete);
        });
    }

    void onMassMeasuringComplete()
    {
        Destroy(massMeasurer);
        massMeasuringLaunched = false;
    }

    public void MeasureMagneticField(int qwe = 0)
    {
        if (!magneticFieldMeasuringLaunched)
        {
            magneticFieldMeasuringLaunched = true;

            Loader.Instance.LoadAndIntsntiateGOPrefab("MagneticMeasurer", onMagneticMeasurerInstantiated);
        }
    }

    void onMagneticMeasurerInstantiated(GameObject go)
    {
        magneticFieldMeasurer = go;
        magneticFieldMeasurer.GetComponent<MagneticFieldMeasurer>().Planet = gameObject;
        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
        {
            magneticFieldMeasurer.GetComponent<MagneticFieldMeasurer>().Measure(onMagneticFieldMeasuringComplete);
        });
    }

    void onMagneticFieldMeasuringComplete()
    {
        Destroy(magneticFieldMeasurer);
        magneticFieldMeasuringLaunched = false;
    }

    public void MeasureTemperature(int qwe = 0)
    {
        if (!temperatureMeasuringLaunched)
        {
            temperatureMeasuringLaunched = true;

            Loader.Instance.LoadAndIntsntiateGOPrefab("TemperatureMeasurer", onTemperatureMeasurerInstantiated);
        }
    }

    void onTemperatureMeasurerInstantiated(GameObject go)
    {
        temperatureMeasurer = go;
        temperatureMeasurer.GetComponent<TemperatureMeasurer>().Planet = transform.parent.gameObject;
        temperatureMeasurer.GetComponent<TemperatureMeasurer>().PlanetSurface = surfaceGO.transform.GetChild(0).gameObject;
        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
        {
            temperatureMeasurer.GetComponent<TemperatureMeasurer>().Measure(onTemperatureMeasuringComplete);
        });
    }

    void onTemperatureMeasuringComplete()
    {
        Destroy(temperatureMeasurer);
        temperatureMeasuringLaunched = false;
    }

    public void SetBrightness(float newBrightness)
    {
        surface.LayerBrightness = newBrightness;
        crust.LayerBrightness = newBrightness;
        upperMantle.LayerBrightness = newBrightness;
        lowerMantle.LayerBrightness = newBrightness;
        outerCore.LayerBrightness = newBrightness;
        innerCore.LayerBrightness = newBrightness;
    }

    public void SetDarkSideBritghness(float newBrightness)
    {
        surface.DarkSideBrightness = newBrightness;
    }

    public void goToWholeness(int stateID, float timeMultiplyer = 1f)
    {
        if (timeMultiplyer != 1f)
        {
            animationTimeMultiplyer = timeMultiplyer;
        }
        if (modificateness == Modificateness.Free) goToWholenessState((PlanetWholeness)stateID);
    }

    public void goToWholeness1(int stateID)
    {
        animationTimeMultiplyer = 1;
        if (modificateness == Modificateness.Free) goToWholenessState((PlanetWholeness)stateID);
    }

    public void goToSetting(int stateID, float timeMultiplyer = 1f)
    {
        if (timeMultiplyer != 1f)
        {
            animationTimeMultiplyer = timeMultiplyer;
        }
        if (modificateness == Modificateness.Free) goToSettingState((PlanetSetting)stateID);
    }

    public void goToSetting1(int stateID)
    {
        animationTimeMultiplyer = 1;
        if (modificateness == Modificateness.Free) goToSettingState((PlanetSetting)stateID);
    }

    public void goToLayer(int stateID, float timeMultiplyer = 1f)
    {
        if (timeMultiplyer != 1f)
        {
            animationTimeMultiplyer = timeMultiplyer;
        }
        if (modificateness == Modificateness.Free) goToLayerState((PlanetLayer)stateID);
    }

    public void goToLayer1(int stateID)
    {
        animationTimeMultiplyer = 1;
        if (modificateness == Modificateness.Free) goToLayerState((PlanetLayer)stateID);
    }

    public void goToWholenessState(PlanetWholeness newState)
    {
        Debug.Log("goToWholenessState: oldState = " + newState + ", newState = " + newState);
        float finalOpenTime = openOnAngleTime * animationTimeMultiplyer;

        if (newState != whoness)
        {
            switch (newState)
            {
                case PlanetWholeness.Hole:
                    switch (layer)
                    {
                        case PlanetLayer.Surface:
                            surface.close(finalOpenTime);
                            break;
                        case PlanetLayer.Crust:
                            crust.close(finalOpenTime);
                            break;
                        case PlanetLayer.UpperMantle:
                            upperMantle.close(finalOpenTime);
                            break;
                        case PlanetLayer.LowerMantle:
                            lowerMantle.close(finalOpenTime);
                            break;
                        case PlanetLayer.OuterCore:
                            outerCore.close(finalOpenTime);
                            break;
                        case PlanetLayer.InnerCore:
                            innerCore.close(finalOpenTime);
                            break;
                    }
                    break;
                case PlanetWholeness.Sliced:
                    /*
                     * Когда мы разрезаем слой, мы хотим выдеть его разрезанным полностью, так что разрезаем и все нижележащие слои
                     * А хотя нет, там возникает еще куча моментов, так что разрезаем только этот слой и показываем тот, что под ним
                     */
                    switch (layer)
                    {
                        case PlanetLayer.Surface:
                            crust.underlyingState = UnderlyingState.OnTop;
                            crust.closedOnStart = true;
                            crust.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    surface.openOnAngle(180f, finalOpenTime);
                                });
                            });
                            break;
                        case PlanetLayer.Crust:
                            upperMantle.underlyingState = UnderlyingState.OnTop;
                            upperMantle.closedOnStart = true;
                            upperMantle.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                crust.underlyingState = UnderlyingState.OnTop;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.openOnAngle(180f, finalOpenTime);
                                });
                            });
                            break;
                        case PlanetLayer.UpperMantle:
                            lowerMantle.underlyingState = UnderlyingState.OnTop;
                            lowerMantle.closedOnStart = true;
                            lowerMantle.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                upperMantle.underlyingState = UnderlyingState.OnTop;
                                upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.openOnAngle(180f, finalOpenTime);
                                });
                            });
                            break;
                        case PlanetLayer.LowerMantle:
                            outerCore.underlyingState = UnderlyingState.OnTop;
                            outerCore.closedOnStart = true;
                            outerCore.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                lowerMantle.underlyingState = UnderlyingState.OnTop;
                                lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    lowerMantle.openOnAngle(180f, finalOpenTime);
                                });
                            });
                            break;
                        case PlanetLayer.OuterCore:
                            innerCore.underlyingState = UnderlyingState.OnTop;
                            innerCore.closedOnStart = true;
                            innerCore.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                outerCore.underlyingState = UnderlyingState.OnTop;
                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    outerCore.openOnAngle(180f, finalOpenTime);
                                });
                            });
                            break;
                        case PlanetLayer.InnerCore:
                            innerCore.underlyingState = UnderlyingState.OnTop;
                            innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                innerCore.openOnAngle(180f, finalOpenTime);
                            });
                            break;
                    }
                    break;
            }
            
            whoness = newState;
        }
    }

    public void goToSettingState(PlanetSetting newState)
    {
        Debug.Log("goToSettingState: oldState = " + setting + ", newState = " + newState);

        float finalExtendTime = extendTime * animationTimeMultiplyer;
        float finalOpenTime = openOnAngleTime * animationTimeMultiplyer;

        if (newState != setting)
        {
            switch (newState)
            {
                case PlanetSetting.Solo:
                    switch (layer)
                    {
                        case PlanetLayer.Surface:
                            /*crust.disappear360();
                            upperMantle.disappear360();
                            lowerMantle.disappear360();
                            outerCore.disappear360();
                            innerCore.disappear360();*/
                            break;
                        case PlanetLayer.Crust:
                            /*surface.disappear360();
                            upperMantle.disappear360();
                            lowerMantle.disappear360();
                            outerCore.disappear360();
                            innerCore.disappear360();*/

                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                surface.disappear360();
                            });
                            break;
                        case PlanetLayer.UpperMantle:
                            /*surface.disappear360();
                            crust.disappear360();
                            lowerMantle.disappear360();
                            outerCore.disappear360();
                            innerCore.disappear360();*/

                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    surface.disappear360();
                                    crust.disappear360();
                                });
                            });
                            break;
                        case PlanetLayer.LowerMantle:
                            /*surface.disappear360();
                            crust.disappear360();
                            upperMantle.disappear360();
                            outerCore.disappear360();
                            innerCore.disappear360();*/

                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        surface.disappear360();
                                        crust.disappear360();
                                        upperMantle.disappear360();
                                    });
                                });
                            });
                            break;
                        case PlanetLayer.OuterCore:
                            /*surface.disappear360();
                            crust.disappear360();
                            upperMantle.disappear360();
                            lowerMantle.disappear360();
                            innerCore.disappear360();*/

                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        lowerMantle.underlyingState = UnderlyingState.Underlying;
                                        lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            surface.disappear360();
                                            crust.disappear360();
                                            upperMantle.disappear360();
                                            lowerMantle.disappear360();
                                        });
                                    });
                                });
                            });
                            break;
                        case PlanetLayer.InnerCore:
                            /*surface.disappear360();
                            crust.disappear360();
                            upperMantle.disappear360();
                            lowerMantle.disappear360();
                            outerCore.disappear360();*/

                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        lowerMantle.underlyingState = UnderlyingState.Underlying;
                                        lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            outerCore.underlyingState = UnderlyingState.Underlying;
                                            outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                surface.disappear360();
                                                crust.disappear360();
                                                upperMantle.disappear360();
                                                lowerMantle.disappear360();
                                                outerCore.disappear360();
                                            });
                                        });
                                    });
                                });
                            });
                            break;
                    }
                    break;
                case PlanetSetting.Jointly:
                    /*if (setting == PlanetSetting.Extended)
                    {*/
                        LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
                        {
                            surface.DestroyInnerPlane();
                            crust.DestroyInnerPlane();
                            upperMantle.DestroyInnerPlane();
                            lowerMantle.DestroyInnerPlane();
                            outerCore.DestroyInnerPlane();
                            innerCore.DestroyInnerPlane();
                        });
                        LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
                        {
                            surface.DestroyInnerPlane();
                            crust.DestroyInnerPlane();
                            upperMantle.DestroyInnerPlane();
                            lowerMantle.DestroyInnerPlane();
                            outerCore.DestroyInnerPlane();
                            innerCore.DestroyInnerPlane();
                        });
                    LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
                        {
                            surface.DestroyInnerPlane();
                            crust.DestroyInnerPlane();
                            upperMantle.DestroyInnerPlane();
                            lowerMantle.DestroyInnerPlane();
                            outerCore.DestroyInnerPlane();
                            innerCore.DestroyInnerPlane();
                        });
                    LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
                        {
                            surface.DestroyInnerPlane();
                            crust.DestroyInnerPlane();
                            upperMantle.DestroyInnerPlane();
                            lowerMantle.DestroyInnerPlane();
                            outerCore.DestroyInnerPlane();
                            innerCore.DestroyInnerPlane();
                        });
                    LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
                        {
                            surface.DestroyInnerPlane();
                            crust.DestroyInnerPlane();
                            upperMantle.DestroyInnerPlane();
                            lowerMantle.DestroyInnerPlane();
                            outerCore.DestroyInnerPlane();
                            innerCore.DestroyInnerPlane();
                        });
                    LeanTween.moveLocal(innerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
                        {
                            surface.DestroyInnerPlane();
                            crust.DestroyInnerPlane();
                            upperMantle.DestroyInnerPlane();
                            lowerMantle.DestroyInnerPlane();
                            outerCore.DestroyInnerPlane();
                            innerCore.DestroyInnerPlane();
                        });
                    /*}
                    else if (setting == PlanetSetting.Solo)
                    {*/
                    switch (layer)
                        {
                            case PlanetLayer.Surface:

                                /*crust.openOnAngle(180f, finalOpenTime);
                                upperMantle.openOnAngle(180f, finalOpenTime);
                                lowerMantle.openOnAngle(180f, finalOpenTime);
                                outerCore.openOnAngle(180f, finalOpenTime);
                                innerCore.openOnAngle(180f, finalOpenTime);*/
                                break;
                            case PlanetLayer.Crust:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.closedOnStart = false;

                                surface.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        surface.openOnAngle(180f);
                                    });
                                });

                                /*upperMantle.openOnAngle(180f, finalOpenTime);
                                lowerMantle.openOnAngle(180f, finalOpenTime);
                                outerCore.openOnAngle(180f, finalOpenTime);
                                innerCore.openOnAngle(180f, finalOpenTime);*/
                                break;
                            case PlanetLayer.UpperMantle:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.closedOnStart = false;
                                surface.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.underlyingState = UnderlyingState.Underlying;
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            crust.openOnAngle(180f, finalOpenTime);
                                            surface.openOnAngle(180f, finalOpenTime);
                                        });
                                    });
                                });
                                

                                /*lowerMantle.openOnAngle(180f, finalOpenTime);
                                outerCore.openOnAngle(180f, finalOpenTime);
                                innerCore.openOnAngle(180f, finalOpenTime);*/
                                break;
                            case PlanetLayer.LowerMantle:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.closedOnStart = false;
                                surface.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.underlyingState = UnderlyingState.Underlying;
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            upperMantle.underlyingState = UnderlyingState.Underlying;
                                            upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                crust.openOnAngle(180f, finalOpenTime);
                                                upperMantle.openOnAngle(180f, finalOpenTime);
                                                surface.openOnAngle(180f, finalOpenTime);
                                            });
                                        });
                                    });
                                });
                            

                                /*outerCore.openOnAngle(180f, finalOpenTime);
                                innerCore.openOnAngle(180f, finalOpenTime);*/
                                break;
                            case PlanetLayer.OuterCore:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.closedOnStart = false;
                                surface.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.underlyingState = UnderlyingState.Underlying;
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            upperMantle.underlyingState = UnderlyingState.Underlying;
                                            upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    crust.openOnAngle(180f, finalOpenTime);
                                                    upperMantle.openOnAngle(180f, finalOpenTime);
                                                    lowerMantle.openOnAngle(180f, finalOpenTime);
                                                    surface.openOnAngle(180f, finalOpenTime);
                                                });
                                            });
                                        });
                                    });
                                });
                            
                            

                                /*innerCore.openOnAngle(180f, finalOpenTime);*/
                                break;
                            case PlanetLayer.InnerCore:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.closedOnStart = false;
                                surface.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.underlyingState = UnderlyingState.Underlying;
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            upperMantle.underlyingState = UnderlyingState.Underlying;
                                            upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    outerCore.underlyingState = UnderlyingState.Underlying;
                                                    outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                    {
                                                        crust.openOnAngle(180f, finalOpenTime);
                                                        upperMantle.openOnAngle(180f, finalOpenTime);
                                                        lowerMantle.openOnAngle(180f, finalOpenTime);
                                                        outerCore.openOnAngle(180f, finalOpenTime);
                                                        surface.openOnAngle(180f, finalOpenTime);
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            
                            
                                break;
                        }
                    /*}*/
                    break;
                case PlanetSetting.Extended:
                    switch(layer)
                    {
                        case PlanetLayer.Crust:
                            surface.CreatePlanes(LoadQueries.Inner, () =>
                            {
                                LeanTween.moveLocal(surface.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                            });
                            break;
                        case PlanetLayer.UpperMantle:
                            crust.CreatePlanes(LoadQueries.Inner, () =>
                            {
                                LeanTween.moveLocal(surface.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(crust.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                            });
                            break;
                        case PlanetLayer.LowerMantle:
                            upperMantle.CreatePlanes(LoadQueries.Inner, () =>
                            {
                                LeanTween.moveLocal(surface.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(crust.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(upperMantle.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                            });
                            break;
                        case PlanetLayer.OuterCore:
                            lowerMantle.CreatePlanes(LoadQueries.Inner, () =>
                            {
                                LeanTween.moveLocal(surface.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(crust.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(upperMantle.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(lowerMantle.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeInOutCubic);
                            });
                            break;
                        case PlanetLayer.InnerCore:
                            /*LeanTween.moveLocal(surface.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeOutCubic);
                            LeanTween.moveLocal(crust.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeOutCubic);
                            LeanTween.moveLocal(upperMantle.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeOutCubic);
                            LeanTween.moveLocal(lowerMantle.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeOutCubic);
                            LeanTween.moveLocal(outerCore.gameObject, new Vector3(0f, 0f, extendedOffset), extendTime).setEase(LeanTweenType.easeOutCubic);*/

                            outerCore.CreatePlanes(LoadQueries.Inner, () =>
                            {
                                LeanTween.moveLocal(surface.gameObject, surface.transform.forward * extendedOffset, extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(crust.gameObject, crust.transform.forward * extendedOffset, extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(upperMantle.gameObject, upperMantle.transform.forward * extendedOffset, extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(lowerMantle.gameObject, lowerMantle.transform.forward * extendedOffset, extendTime).setEase(LeanTweenType.easeInOutCubic);
                                LeanTween.moveLocal(outerCore.gameObject, outerCore.transform.forward * extendedOffset, extendTime).setEase(LeanTweenType.easeInOutCubic);
                            });

                            break;
                    }
                    break;
            }

            setting = newState;
        }
    }

    public void goToLayerState(PlanetLayer newState)
    {
        /*
         * Когда мы идем на какой-то слой, мы можем находиться в двух состояниях - либо слой который мы хотим находится под слоями, открытыми сейчас, либо над.
         * Если под - мы убираем слои над ним - по умолчанию открываем на 180. Если над - то, если экстендед - придвигаем слои и закрываем, если нет - просто закрываем
         */

        Debug.Log("1111goToLayerState: oldState = " + layer + ", newState = " + newState);

        float finalExtendTime = extendTime * animationTimeMultiplyer;
        float finalOpenTime = openOnAngleTime * animationTimeMultiplyer;

        if (newState != layer)
        {
            oldLayer = layer;

            switch (newState)
            {
                case PlanetLayer.Surface:
                    //BigInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.Earth), DataController.Instance.GetDescriptionFor(InformationsMockup.Earth));
                    //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.Earth), DataController.Instance.GetDescriptionFor(InformationsMockup.Earth));
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Earth);

                    switch (layer)
                    {
                        case PlanetLayer.Crust:
                            crust.underlyingState = UnderlyingState.Underlying;
                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                        surface.close(finalOpenTime);
                                        crust.underlyingState = UnderlyingState.Underlying;
                                        LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                        crust.close(finalOpenTime);
                                    });
                                });
                            });
                            break;
                        case PlanetLayer.UpperMantle:
                            upperMantle.underlyingState = UnderlyingState.Underlying;
                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                            surface.close(finalOpenTime);
                                            crust.underlyingState = UnderlyingState.Underlying;
                                            LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                            crust.close(finalOpenTime);
                                            upperMantle.underlyingState = UnderlyingState.Underlying;
                                            LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                            upperMantle.close(finalOpenTime);
                                        });
                                    });
                                });
                            });
                            
                            break;
                        case PlanetLayer.LowerMantle:
                            lowerMantle.underlyingState = UnderlyingState.Underlying;
                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                surface.close(finalOpenTime);
                                                crust.underlyingState = UnderlyingState.Underlying;
                                                LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                crust.close(finalOpenTime);
                                                upperMantle.underlyingState = UnderlyingState.Underlying;
                                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                upperMantle.close(finalOpenTime);
                                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                lowerMantle.close(finalOpenTime);
                                            });
                                        });
                                    });
                                });
                            });
                            
                            break;
                        case PlanetLayer.OuterCore:
                            outerCore.underlyingState = UnderlyingState.Underlying;
                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    surface.close(finalOpenTime);
                                                    crust.underlyingState = UnderlyingState.Underlying;
                                                    LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    crust.close(finalOpenTime);
                                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                                    LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    upperMantle.close(finalOpenTime);
                                                    lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                    LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    lowerMantle.close(finalOpenTime);
                                                    outerCore.underlyingState = UnderlyingState.Underlying;
                                                    LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    outerCore.close(finalOpenTime);
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                            
                            break;
                        case PlanetLayer.AllOpened:
                        case PlanetLayer.InnerCore:
                            innerCore.underlyingState = UnderlyingState.Underlying;
                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.Outer, () =>
                            {
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                    {
                                                        LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        surface.close(finalOpenTime);
                                                        crust.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        crust.close(finalOpenTime);
                                                        upperMantle.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        upperMantle.close(finalOpenTime);
                                                        lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        lowerMantle.close(finalOpenTime);
                                                        outerCore.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        outerCore.close(finalOpenTime);
                                                        innerCore.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(innerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        innerCore.close(finalOpenTime);
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });

                            break;
                    }
                    break;
                case PlanetLayer.Crust:
                    if ((int)newState < (int)oldLayer)
                    {
                        crust.closedOnStart = false;
                    }
                    else
                    {
                        crust.closedOnStart = true;
                    }
                    crust.CreatePlane(SertainPlane.Outer, () =>
                    {
                        //BigInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.Crust), DataController.Instance.GetDescriptionFor(InformationsMockup.Crust));
                        //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.Crust), DataController.Instance.GetDescriptionFor(InformationsMockup.Crust));
                        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Crust);

                        switch (oldLayer)
                        {
                            case PlanetLayer.Surface:
                                Debug.Log("We are on Surface layer and wanted crust");
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    Debug.Log("123");
                                    surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        Debug.Log("Opening surface on 180 angle");
                                        surface.openOnAngle(180f, finalOpenTime);
                                    });
                                });
                                break;
                            case PlanetLayer.UpperMantle:
                                upperMantle.underlyingState = UnderlyingState.Underlying;
                                crust.underlyingState = UnderlyingState.OnTop;
                                crust.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            crust.underlyingState = UnderlyingState.OnTop;
                                            LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                            crust.close(finalOpenTime);
                                            upperMantle.underlyingState = UnderlyingState.Underlying;
                                            LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                            upperMantle.close(finalOpenTime);
                                        });
                                    });
                                });
                                
                                break;
                            case PlanetLayer.LowerMantle:
                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                crust.underlyingState = UnderlyingState.OnTop;
                                crust.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                crust.underlyingState = UnderlyingState.OnTop;
                                                LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                crust.close(finalOpenTime);
                                                upperMantle.underlyingState = UnderlyingState.Underlying;
                                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                upperMantle.close(finalOpenTime);
                                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                lowerMantle.close(finalOpenTime);
                                            });
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.OuterCore:
                                outerCore.underlyingState = UnderlyingState.Underlying;
                                crust.underlyingState = UnderlyingState.OnTop;
                                crust.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    crust.underlyingState = UnderlyingState.OnTop;
                                                    LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    crust.close(finalOpenTime);
                                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                                    LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    upperMantle.close(finalOpenTime);
                                                    lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                    LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    lowerMantle.close(finalOpenTime);
                                                    outerCore.underlyingState = UnderlyingState.Underlying;
                                                    LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                    outerCore.close(finalOpenTime);
                                                });
                                            });
                                        });
                                    });
                                });
                                
                                break;
                            case PlanetLayer.InnerCore:
                            case PlanetLayer.AllOpened:
                                innerCore.underlyingState = UnderlyingState.Underlying;
                                crust.underlyingState = UnderlyingState.OnTop;
                                crust.CreatePlanes(LoadQueries.Outer, () =>
                                {
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                    {
                                                        crust.underlyingState = UnderlyingState.OnTop;
                                                        LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        crust.close(finalOpenTime);
                                                        upperMantle.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        upperMantle.close(finalOpenTime);
                                                        lowerMantle.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        lowerMantle.close(finalOpenTime);
                                                        outerCore.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        outerCore.close(finalOpenTime);
                                                        innerCore.underlyingState = UnderlyingState.Underlying;
                                                        LeanTween.moveLocal(innerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                                        innerCore.close(finalOpenTime);
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                                
                                break;
                        }
                    });
                    break;
                case PlanetLayer.UpperMantle:
                    if ((int)newState < (int)oldLayer)
                    {
                        upperMantle.closedOnStart = false;
                    }
                    else
                    {
                        upperMantle.closedOnStart = true;
                    }
                    upperMantle.CreatePlane(SertainPlane.Outer, () =>
                    {
                        //BigInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.UpperMantle), DataController.Instance.GetDescriptionFor(InformationsMockup.UpperMantle));
                        //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.UpperMantle), DataController.Instance.GetDescriptionFor(InformationsMockup.UpperMantle));
                        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.UpperMantle);

                        switch (oldLayer)
                        {
                            case PlanetLayer.Surface:
                                Debug.Log("Wanted UpperMatle when it's Sufrase");
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    surface.CreatePlanes(LoadQueries.Outer, () =>
                                    {
                                        crust.underlyingState = UnderlyingState.Underlying;
                                        crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            surface.openOnAngle(180f, finalOpenTime);
                                            crust.openOnAngle(180f, finalOpenTime);
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.Crust:
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.openOnAngle(180f, finalOpenTime);
                                });
                                break;
                            case PlanetLayer.LowerMantle:
                                upperMantle.underlyingState = UnderlyingState.OnTop;
                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                upperMantle.close(finalOpenTime);
                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                lowerMantle.close(finalOpenTime);
                                break;
                            case PlanetLayer.OuterCore:
                                upperMantle.underlyingState = UnderlyingState.OnTop;
                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                upperMantle.close(finalOpenTime);
                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                lowerMantle.close(finalOpenTime);
                                outerCore.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                outerCore.close(finalOpenTime);
                                break;
                            case PlanetLayer.AllOpened:
                            case PlanetLayer.InnerCore:
                                upperMantle.underlyingState = UnderlyingState.OnTop;
                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                upperMantle.close(finalOpenTime);
                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                lowerMantle.close(finalOpenTime);
                                outerCore.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                outerCore.close(finalOpenTime);
                                innerCore.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(innerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                innerCore.close(finalOpenTime);
                                break;
                        }
                    });
                    break;
                case PlanetLayer.LowerMantle:
                    if ((int)newState < (int)oldLayer)
                    {
                        lowerMantle.closedOnStart = false;
                    }
                    else
                    {
                        lowerMantle.closedOnStart = true;
                    }
                    lowerMantle.CreatePlane(SertainPlane.Outer, () =>
                    {
                        //BigInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.LowerMantle), DataController.Instance.GetDescriptionFor(InformationsMockup.LowerMantle));
                        //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.LowerMantle), DataController.Instance.GetDescriptionFor(InformationsMockup.LowerMantle));
                        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.LowerMantle);

                        switch (oldLayer)
                        {
                            case PlanetLayer.Surface:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.underlyingState = UnderlyingState.Underlying;
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.underlyingState = UnderlyingState.Underlying;
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            surface.openOnAngle(180f, finalOpenTime);
                                            crust.openOnAngle(180f, finalOpenTime);
                                            upperMantle.openOnAngle(180f, finalOpenTime);
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.Crust:
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        crust.openOnAngle(180f, finalOpenTime);
                                        upperMantle.openOnAngle(180f, finalOpenTime);
                                    });
                                });
                                break;
                            case PlanetLayer.UpperMantle:
                                upperMantle.underlyingState = UnderlyingState.Underlying;
                                upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.openOnAngle(180f, finalOpenTime);
                                });
                                break;
                            case PlanetLayer.OuterCore:
                                /*LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                surface.close(finalOpenTime);
                                LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                crust.close(finalOpenTime);
                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                upperMantle.close(finalOpenTime);*/
                                lowerMantle.underlyingState = UnderlyingState.OnTop;
                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                lowerMantle.close(finalOpenTime);
                                outerCore.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                outerCore.close(finalOpenTime);
                                break;
                            case PlanetLayer.AllOpened:
                            case PlanetLayer.InnerCore:
                                /*LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                surface.close(finalOpenTime);
                                LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                crust.close(finalOpenTime);
                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                upperMantle.close(finalOpenTime);*/
                                lowerMantle.underlyingState = UnderlyingState.OnTop;
                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                lowerMantle.close(finalOpenTime);
                                outerCore.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                outerCore.close(finalOpenTime);
                                innerCore.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(innerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                innerCore.close(finalOpenTime);
                                break;
                        }
                    });
                    break;
                case PlanetLayer.OuterCore:
                    if ((int)newState < (int)oldLayer)
                    {
                        outerCore.closedOnStart = false;
                    }
                    else
                    {
                        outerCore.closedOnStart = true;
                    }
                    outerCore.CreatePlane(SertainPlane.Outer, () =>
                    {
                        //BigInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.OuterCore), DataController.Instance.GetDescriptionFor(InformationsMockup.OuterCore));
                        //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.OuterCore), DataController.Instance.GetDescriptionFor(InformationsMockup.OuterCore));
                        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.OuterCore);

                        switch (oldLayer)
                        {
                            case PlanetLayer.Surface:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.underlyingState = UnderlyingState.Underlying;
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.underlyingState = UnderlyingState.Underlying;
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.underlyingState = UnderlyingState.Underlying;
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                surface.openOnAngle(180f, finalOpenTime);
                                                crust.openOnAngle(180f, finalOpenTime);
                                                upperMantle.openOnAngle(180f, finalOpenTime);
                                                lowerMantle.openOnAngle(180f, finalOpenTime);
                                            });
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.Crust:
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        lowerMantle.underlyingState = UnderlyingState.Underlying;
                                        lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            crust.openOnAngle(180f, finalOpenTime);
                                            upperMantle.openOnAngle(180f, finalOpenTime);
                                            lowerMantle.openOnAngle(180f, finalOpenTime);
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.UpperMantle:
                                upperMantle.underlyingState = UnderlyingState.Underlying;
                                upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    lowerMantle.underlyingState = UnderlyingState.Underlying;
                                    lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.openOnAngle(180f, finalOpenTime);
                                        lowerMantle.openOnAngle(180f, finalOpenTime);
                                    });
                                });
                                break;
                            case PlanetLayer.LowerMantle:
                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    lowerMantle.openOnAngle(180f, finalOpenTime);
                                });
                                break;
                            case PlanetLayer.AllOpened:
                            case PlanetLayer.InnerCore:
                                /*LeanTween.moveLocal(surface.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                surface.close(finalOpenTime);
                                LeanTween.moveLocal(crust.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                crust.close(finalOpenTime);
                                LeanTween.moveLocal(upperMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                upperMantle.close(finalOpenTime);
                                LeanTween.moveLocal(lowerMantle.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                lowerMantle.close(finalOpenTime);*/
                                outerCore.underlyingState = UnderlyingState.OnTop;
                                LeanTween.moveLocal(outerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                outerCore.close(finalOpenTime);
                                innerCore.underlyingState = UnderlyingState.Underlying;
                                LeanTween.moveLocal(innerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                innerCore.close(finalOpenTime);
                                break;
                        }
                    });
                    break;
                case PlanetLayer.InnerCore:
                    if ((int)newState < (int)oldLayer)
                    {
                        innerCore.closedOnStart = false;
                    }
                    else
                    {
                        innerCore.closedOnStart = true;
                    }
                    innerCore.CreatePlane(SertainPlane.Outer, () =>
                    {
                        //BigInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.InnerCore), DataController.Instance.GetDescriptionFor(InformationsMockup.InnerCore));
                        //BigSimpleInfoPanelController.Instance.Show(DataController.Instance.GetLabelFor(InformationsMockup.InnerCore), DataController.Instance.GetDescriptionFor(InformationsMockup.InnerCore));
                        BigSimpleInfoPanelController.Instance.Show(InformationsMockup.InnerCore);

                        switch (oldLayer)
                        {
                            case PlanetLayer.Surface:
                                surface.underlyingState = UnderlyingState.OnTop;
                                surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    crust.underlyingState = UnderlyingState.Underlying;
                                    crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        upperMantle.underlyingState = UnderlyingState.Underlying;
                                        upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            lowerMantle.underlyingState = UnderlyingState.Underlying;
                                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                outerCore.underlyingState = UnderlyingState.Underlying;
                                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    surface.openOnAngle(180f, finalOpenTime);
                                                    crust.openOnAngle(180f, finalOpenTime);
                                                    upperMantle.openOnAngle(180f, finalOpenTime);
                                                    lowerMantle.openOnAngle(180f, finalOpenTime);
                                                    outerCore.openOnAngle(180f, finalOpenTime);
                                                });
                                            });
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.Crust:
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        lowerMantle.underlyingState = UnderlyingState.Underlying;
                                        lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            outerCore.underlyingState = UnderlyingState.Underlying;
                                            outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                crust.openOnAngle(180f, finalOpenTime);
                                                upperMantle.openOnAngle(180f, finalOpenTime);
                                                lowerMantle.openOnAngle(180f, finalOpenTime);
                                                outerCore.openOnAngle(180f, finalOpenTime);
                                            });
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.UpperMantle:
                                upperMantle.underlyingState = UnderlyingState.Underlying;
                                upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    lowerMantle.underlyingState = UnderlyingState.Underlying;
                                    lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        outerCore.underlyingState = UnderlyingState.Underlying;
                                        outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            upperMantle.openOnAngle(180f, finalOpenTime);
                                            lowerMantle.openOnAngle(180f, finalOpenTime);
                                            outerCore.openOnAngle(180f, finalOpenTime);
                                        });
                                    });
                                });
                                break;
                            case PlanetLayer.LowerMantle:
                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    outerCore.underlyingState = UnderlyingState.Underlying;
                                    outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        lowerMantle.openOnAngle(180f, finalOpenTime);
                                        outerCore.openOnAngle(180f, finalOpenTime);
                                    });
                                });
                                break;
                            case PlanetLayer.OuterCore:
                                outerCore.underlyingState = UnderlyingState.Underlying;
                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    outerCore.openOnAngle(180f, finalOpenTime);
                                });
                                break;
                            case PlanetLayer.AllOpened:
                                Debug.Log("AllOpened -> InnerCore");
                                innerCore.underlyingState = UnderlyingState.OnTop;
                                LeanTween.moveLocal(innerCore.gameObject, Vector3.zero, finalExtendTime).setEase(LeanTweenType.easeOutCubic);
                                innerCore.close(finalOpenTime);
                                break;
                        }
                    });
                    break;
                case PlanetLayer.AllOpened:
                    BigSimpleInfoPanelController.Instance.Show(InformationsMockup.Earth);

                    if ((int)newState < (int)oldLayer)
                    {
                        innerCore.closedOnStart = false;
                    }
                    else
                    {
                        innerCore.closedOnStart = true;
                    }


                    switch (oldLayer)
                    {
                        case PlanetLayer.Surface:
                            surface.underlyingState = UnderlyingState.OnTop;
                            surface.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                crust.underlyingState = UnderlyingState.Underlying;
                                crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    upperMantle.underlyingState = UnderlyingState.Underlying;
                                    upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        lowerMantle.underlyingState = UnderlyingState.Underlying;
                                        lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            outerCore.underlyingState = UnderlyingState.Underlying;
                                            outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                innerCore.underlyingState = UnderlyingState.Underlying;
                                                innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                                {
                                                    surface.openOnAngle(180f, finalOpenTime);
                                                    crust.openOnAngle(180f, finalOpenTime);
                                                    upperMantle.openOnAngle(180f, finalOpenTime);
                                                    lowerMantle.openOnAngle(180f, finalOpenTime);
                                                    outerCore.openOnAngle(180f, finalOpenTime);
                                                    innerCore.openOnAngle(180f, finalOpenTime);
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                            break;
                        case PlanetLayer.Crust:
                            crust.underlyingState = UnderlyingState.Underlying;
                            crust.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                upperMantle.underlyingState = UnderlyingState.Underlying;
                                upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    lowerMantle.underlyingState = UnderlyingState.Underlying;
                                    lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        outerCore.underlyingState = UnderlyingState.Underlying;
                                        outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            innerCore.underlyingState = UnderlyingState.Underlying;
                                            innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                            {
                                                crust.openOnAngle(180f, finalOpenTime);
                                                upperMantle.openOnAngle(180f, finalOpenTime);
                                                lowerMantle.openOnAngle(180f, finalOpenTime);
                                                outerCore.openOnAngle(180f, finalOpenTime);
                                                innerCore.openOnAngle(180f, finalOpenTime);
                                            });
                                        });
                                    });
                                });
                            });
                            break;
                        case PlanetLayer.UpperMantle:
                            upperMantle.underlyingState = UnderlyingState.Underlying;
                            upperMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                lowerMantle.underlyingState = UnderlyingState.Underlying;
                                lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    outerCore.underlyingState = UnderlyingState.Underlying;
                                    outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        innerCore.underlyingState = UnderlyingState.Underlying;
                                        innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                        {
                                            upperMantle.openOnAngle(180f, finalOpenTime);
                                            lowerMantle.openOnAngle(180f, finalOpenTime);
                                            outerCore.openOnAngle(180f, finalOpenTime);
                                            innerCore.openOnAngle(180f, finalOpenTime);
                                        });
                                    });
                                });
                            });
                            break;
                        case PlanetLayer.LowerMantle:
                            lowerMantle.underlyingState = UnderlyingState.Underlying;
                            lowerMantle.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                outerCore.underlyingState = UnderlyingState.Underlying;
                                outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    innerCore.underlyingState = UnderlyingState.Underlying;
                                    innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                    {
                                        lowerMantle.openOnAngle(180f, finalOpenTime);
                                        outerCore.openOnAngle(180f, finalOpenTime);
                                        innerCore.openOnAngle(180f, finalOpenTime);
                                    });
                                });
                            });
                            break;
                        case PlanetLayer.OuterCore:
                            outerCore.underlyingState = UnderlyingState.Underlying;
                            outerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                innerCore.underlyingState = UnderlyingState.Underlying;
                                innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                                {
                                    outerCore.openOnAngle(180f, finalOpenTime);
                                    innerCore.openOnAngle(180f, finalOpenTime);
                                });
                            });
                            break;
                        case PlanetLayer.InnerCore:
                            innerCore.underlyingState = UnderlyingState.Underlying;
                            innerCore.CreatePlanes(LoadQueries.RightAndLeft, () =>
                            {
                                innerCore.openOnAngle(180f, finalOpenTime);
                            });
                            break;
                    }
                    break;
            }

            layer = newState;
        }
    }

    public float MySize
    {
        get { return 1f; }
        set { }
    }

    public Vector3 MyWorldPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    void CreateLayer(PlanetLayer layer)
    {
        switch (layer)
        {
            case PlanetLayer.Surface:
                if (surfaceGO != null)
                {
                    return;
                }
                break;
            case PlanetLayer.Crust:
                if (crustGO != null)
                {
                    return;
                }
                break;
            case PlanetLayer.UpperMantle:
                if (upperMantleGO != null)
                {
                    return;
                }
                break;
            case PlanetLayer.LowerMantle:
                if (lowerMantleGO != null)
                {
                    return;
                }
                break;
            case PlanetLayer.OuterCore:
                if (outerCoreGO != null)
                {
                    return;
                }
                break;
            case PlanetLayer.InnerCore:
                if (innerCoreGO != null)
                {
                    return;
                }
                break;
        }

        GameObject targetGO = new GameObject();
        targetGO.transform.parent = transform;
        targetGO.transform.localPosition = Vector3.zero;
        targetGO.transform.localRotation = Quaternion.identity;
        targetGO.transform.localScale = Vector3.one;

        switch (layer)
        {
            case PlanetLayer.Surface:
                targetGO.name = "SurfaceSphere";
                surfaceGO = targetGO;
                surfaceGO.AddComponent<OptimizedSphereController1>();
                surface = surfaceGO.GetComponent<OptimizedSphereController1>();
                surface.Prefix = "Surface";
                surface.OuterDiameter = sphereDiameters[0].OuterDiameter;
                surface.InnerDiameter = sphereDiameters[0].InnerDiameter;
                surface.MySize = surface.OuterDiameter;
                surface.underlyingState = UnderlyingState.OnTop;
                break;
            case PlanetLayer.Crust:
                targetGO.name = "CrustSphere";
                crustGO = targetGO;
                crustGO.AddComponent<OptimizedSphereController1>();
                crust = crustGO.GetComponent<OptimizedSphereController1>();
                crust.Prefix = "Crust";
                crust.OuterDiameter = sphereDiameters[1].OuterDiameter;
                crust.InnerDiameter = sphereDiameters[1].InnerDiameter;
                crust.MySize = crust.OuterDiameter;
                break;
            case PlanetLayer.UpperMantle:
                targetGO.name = "UpperMantleSphere";
                upperMantleGO = targetGO;
                upperMantleGO.AddComponent<OptimizedSphereController1>();
                upperMantle = upperMantleGO.GetComponent<OptimizedSphereController1>();
                upperMantle.Prefix = "UpperMantle";
                upperMantle.OuterDiameter = sphereDiameters[2].OuterDiameter;
                upperMantle.InnerDiameter = sphereDiameters[2].InnerDiameter;
                upperMantle.MySize = upperMantle.OuterDiameter;
                break;
            case PlanetLayer.LowerMantle:
                targetGO.name = "LowerMantleSphere";
                lowerMantleGO = targetGO;
                lowerMantleGO.AddComponent<OptimizedSphereController1>();
                lowerMantle = lowerMantleGO.GetComponent<OptimizedSphereController1>();
                lowerMantle.Prefix = "LowerMantle";
                lowerMantle.OuterDiameter = sphereDiameters[3].OuterDiameter;
                lowerMantle.InnerDiameter = sphereDiameters[3].InnerDiameter;
                lowerMantle.MySize = lowerMantle.OuterDiameter;
                break;
            case PlanetLayer.OuterCore:
                targetGO.name = "OuterCoreSphere";
                outerCoreGO = targetGO;
                outerCoreGO.AddComponent<OptimizedSphereController1>();
                outerCore = outerCoreGO.GetComponent<OptimizedSphereController1>();
                outerCore.Prefix = "OuterCore";
                outerCore.OuterDiameter = sphereDiameters[4].OuterDiameter;
                outerCore.InnerDiameter = sphereDiameters[4].InnerDiameter;
                outerCore.MySize = outerCore.OuterDiameter;
                break;
            case PlanetLayer.InnerCore:
                targetGO.name = "InnerCoreSphere";
                innerCoreGO = targetGO;
                innerCoreGO.AddComponent<OptimizedSphereController1>();
                innerCore = innerCoreGO.GetComponent<OptimizedSphereController1>();
                innerCore.Prefix = "InnerCore";
                innerCore.OuterDiameter = sphereDiameters[5].OuterDiameter;
                innerCore.InnerDiameter = sphereDiameters[5].InnerDiameter;
                innerCore.MySize = innerCore.OuterDiameter;
                break;
        }
    }

    // Use this for initialization
    void Start ()
    {
        layer = (PlanetLayer)1;
        setting = (PlanetSetting)2;
        whoness = (PlanetWholeness)1;

        sphereDiameters[0].OuterDiameter = 1.0f;
        sphereDiameters[0].InnerDiameter = 0.995f;
        sphereDiameters[1].OuterDiameter = 0.995f;
        sphereDiameters[1].InnerDiameter = 0.95986f;
        sphereDiameters[2].OuterDiameter = 0.95986f;
        sphereDiameters[2].InnerDiameter = 0.8127f;
        sphereDiameters[3].OuterDiameter = 0.8127f;
        sphereDiameters[3].InnerDiameter = 0.52619f;
        sphereDiameters[4].OuterDiameter = 0.52619f;
        sphereDiameters[4].InnerDiameter = 0.22639f;
        sphereDiameters[5].OuterDiameter = 0.22639f;
        sphereDiameters[5].InnerDiameter = 0.001f;


        surface.underlyingState = UnderlyingState.OnTop;
        surface.closedOnStart = true;
        surface.CreatePlane(SertainPlane.Outer);

        LookAt(GameObject.Find("Observer").transform);

        /*Timer.setTask(8f, () =>
        {
            surface.openOnAngle(180f, finalOpenTime);
        });

        Timer.setTask(8f, () =>
        {
            crust.openOnAngle(180f, finalOpenTime);
        });

        Timer.setTask(8f, () =>
        {
            upperMantle.openOnAngle(180f, finalOpenTime);
        });

        Timer.setTask(8f, () =>
        {
            lowerMantle.openOnAngle(180f, finalOpenTime);
        });

        Timer.setTask(8f, () =>
        {
            outerCore.openOnAngle(180f, finalOpenTime);
        });

        Timer.setTask(8f, () =>
        {
            innerCore.openOnAngle(180f, finalOpenTime);
        });*/

        /*Timer.setTask(12f, () =>
        {
            disassemble();
        });*/

        /*LeanTween.value(gameObject, 0f, angleToOpenOn, 4.14f).setDelay(0.5f).setOnUpdate((float val) =>
        {
            surface.OpenedOnAngle = val;
        });

        LeanTween.value(gameObject, 0f, angleToOpenOn, 4.14f).setDelay(5f).setOnUpdate((float val) =>
        {
            crust.OpenedOnAngle = val;
        });

        LeanTween.value(gameObject, 0f, angleToOpenOn, 4.14f).setDelay(10f).setOnUpdate((float val) =>
        {
            upperMantle.OpenedOnAngle = val;
        });

        LeanTween.value(gameObject, 0f, angleToOpenOn, 4.14f).setDelay(15f).setOnUpdate((float val) =>
        {
            lowerMantle.OpenedOnAngle = val;
        });

        LeanTween.value(gameObject, 0f, angleToOpenOn, 4.14f).setDelay(20f).setOnUpdate((float val) =>
        {
            outerCore.OpenedOnAngle = val;
        });*/
    }

    public void disassemble()
    {
        Vector3 direction = surface.transform.forward;
        LeanTween.move(crust.gameObject, direction * -1.0f, 1.5f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(upperMantle.gameObject, direction * -2.0f, 1.5f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(lowerMantle.gameObject, direction * -3.0f, 1.5f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(outerCore.gameObject, direction * -4.0f, 1.5f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.move(innerCore.gameObject, direction * -5.0f, 1.5f).setEase(LeanTweenType.easeInOutQuad);
    }

    // Update is called once per frame
    void Update ()
    {
        if (surface != null) surface.rotate(RotationSpeed);
        if (crust != null) crust.rotate(RotationSpeed);
        if (upperMantle != null) upperMantle.rotate(RotationSpeed);
        if (lowerMantle != null) lowerMantle.rotate(RotationSpeed);
        if (outerCore != null) outerCore.rotate(RotationSpeed);
        if (innerCore != null) innerCore.rotate(RotationSpeed);
    }

    public void LookAt(Transform target)
    {
        if (surface != null) surface.LookAt(target);
        if (crust != null) crust.LookAt(target);
        if (upperMantle != null) upperMantle.LookAt(target);
        if (lowerMantle != null) lowerMantle.LookAt(target);
        if (outerCore != null) outerCore.LookAt(target);
        if (innerCore != null) innerCore.LookAt(target);
    }
}