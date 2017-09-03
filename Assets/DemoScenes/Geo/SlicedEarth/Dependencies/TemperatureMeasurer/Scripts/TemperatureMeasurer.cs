using UnityEngine;
//using HoloWorldExtensions;
using TMPro;
using HoloToolkit.Unity;
using System.Collections.Generic;

public class TemperatureMeasurer : Singleton<TemperatureMeasurer>
{
    public bool CurrentlyMeasuring = false;

    public GameObject Planet;
    public GameObject PlanetSurface;
    public GameObject GradientBar;
    public TextMeshProUGUI LowerText;
    public TextMeshProUGUI HigherText;

    Color lowerColor;
    Color higherColor;
    Color barColor;
    Material gradientBarMaterial;
    Material planetSurfaceMaterial;

    System.Action callbackFunc;

    public System.Action onStopMeasuring;

    Dictionary<Language, Dictionary<string, float>> timings;

    void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("TemperatureMeasurer_1", 48f);

        timings[Language.English].Add("TemperatureMeasurer_1", 40f);
    }

    void Start()
    {
        gradientBarMaterial = GradientBar.GetComponent<Renderer>().material;
        planetSurfaceMaterial = PlanetSurface.GetComponent<Renderer>().material;
        lowerColor = LowerText.color;
        higherColor = HigherText.color;

        lowerColor.a = 0f;
        higherColor.a = 0f;
        barColor = new Color(1, 1, 1, 0);

        gradientBarMaterial.color = barColor;
        LowerText.color = lowerColor;
        HigherText.color = higherColor;

        //gameObject.setRenderers(false);
        //gameObject.setActivated(false);
    }

    public void Measure(System.Action callback = null)
    {
        CurrentlyMeasuring = true;
        callbackFunc = callback;

        //gameObject.setActivated(true);
        //gameObject.setRenderers(true);
        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.x -= 0.76f;
        pos.y -= 0.4f;
        pos.z -= 0.45f;
        transform.localPosition = pos;

        LeanTween.value(gameObject, 0, 1, 3).setDelay(0.1f).setOnStart(() =>
        {
            if (planetSurfaceMaterial == null)
            {
                planetSurfaceMaterial = PlanetSurface.GetComponent<Renderer>().material;
            }
        }).setOnUpdate((float val) =>
        {
            planetSurfaceMaterial.SetFloat("_AlternativeTextureVisibilityCoeff", val);
        }).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
            {
                lowerColor.a = val;
                higherColor.a = val;
                barColor.a = val;
                gradientBarMaterial.color = barColor;
                LowerText.color = lowerColor;
                HigherText.color = higherColor;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1, 0, 2).setDelay(10).setOnUpdate((float val) =>
                {
                    planetSurfaceMaterial.SetFloat("_AlternativeTextureVisibilityCoeff", val);
                }).setOnComplete(() =>
                {
                    LeanTween.value(gameObject, 1, 0, 0.5f).setOnUpdate((float val) =>
                    {
                        lowerColor.a = val;
                        higherColor.a = val;
                        barColor.a = val;
                        gradientBarMaterial.color = barColor;
                        LowerText.color = lowerColor;
                        HigherText.color = higherColor;
                    }).setOnComplete(() =>
                    {
                        //gameObject.setRenderers(false);
                        //gameObject.setActivated(false);

                        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
                        {
                            CurrentlyMeasuring = false;
                            if (callbackFunc != null)
                            {
                                callbackFunc.Invoke();
                                callbackFunc = null;
                            }

                            if (onStopMeasuring != null)
                            {
                                onStopMeasuring.Invoke();
                                onStopMeasuring = null;
                            }
                        });
                    });
                });
            });
        });
    }

    public void MeasureInLectureMode(System.Action callback = null)
    {
        if (timings == null)
        {
            InitializeTimings();
        }
        CurrentlyMeasuring = true;
        callbackFunc = callback;

        //gameObject.setActivated(true);
        //gameObject.setRenderers(true);
        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.x -= 0.86f;
        pos.y -= 0.5f;
        pos.z -= 0.5f;
        transform.localPosition = pos;

        LeanTween.value(gameObject, 0, 1, 3).setDelay(0.1f).setOnStart(() =>
        {
            if (planetSurfaceMaterial == null)
            {
                planetSurfaceMaterial = PlanetSurface.GetComponent<Renderer>().material;
            }
        }).setOnUpdate((float val) =>
        {
            planetSurfaceMaterial.SetFloat("_AlternativeTextureVisibilityCoeff", val);
        }).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
            {
                lowerColor.a = val;
                higherColor.a = val;
                barColor.a = val;
                gradientBarMaterial.color = barColor;
                LowerText.color = lowerColor;
                HigherText.color = higherColor;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1, 0, 2).setDelay(timings[LanguageManager.Instance.CurrentLanguage]["TemperatureMeasurer_1"]).setOnUpdate((float val) =>
                {
                    planetSurfaceMaterial.SetFloat("_AlternativeTextureVisibilityCoeff", val);
                }).setOnComplete(() =>
                {
                    LeanTween.value(gameObject, 1, 0, 0.5f).setOnUpdate((float val) =>
                    {
                        lowerColor.a = val;
                        higherColor.a = val;
                        barColor.a = val;
                        gradientBarMaterial.color = barColor;
                        LowerText.color = lowerColor;
                        HigherText.color = higherColor;
                    }).setOnComplete(() =>
                    {
                        //gameObject.setRenderers(false);
                        //gameObject.setActivated(false);

                        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
                        {
                            CurrentlyMeasuring = false;
                            if (callbackFunc != null)
                            {
                                callbackFunc.Invoke();
                                callbackFunc = null;
                            }

                            if (onStopMeasuring != null)
                            {
                                onStopMeasuring.Invoke();
                                onStopMeasuring = null;
                            }
                        });
                    });
                });
            });
        });
    }

    public void MeasureInDemoMode(System.Action callback = null)
    {
        CurrentlyMeasuring = true;
        callbackFunc = callback;

        //gameObject.setActivated(true);
        //gameObject.setRenderers(true);
        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.x -= 0.86f;
        pos.y -= 0.5f;
        pos.z -= 0.5f;
        transform.localPosition = pos;

        LeanTween.value(gameObject, 0, 1, 3).setDelay(0.1f).setOnStart(() =>
        {
            if (planetSurfaceMaterial == null)
            {
                planetSurfaceMaterial = PlanetSurface.GetComponent<Renderer>().material;
            }
        }).setOnUpdate((float val) =>
        {
            planetSurfaceMaterial.SetFloat("_AlternativeTextureVisibilityCoeff", val);
        }).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
            {
                lowerColor.a = val;
                higherColor.a = val;
                barColor.a = val;
                gradientBarMaterial.color = barColor;
                LowerText.color = lowerColor;
                HigherText.color = higherColor;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1, 0, 2).setDelay(24f).setOnUpdate((float val) =>
                {
                    planetSurfaceMaterial.SetFloat("_AlternativeTextureVisibilityCoeff", val);
                }).setOnComplete(() =>
                {
                    LeanTween.value(gameObject, 1, 0, 0.5f).setOnUpdate((float val) =>
                    {
                        lowerColor.a = val;
                        higherColor.a = val;
                        barColor.a = val;
                        gradientBarMaterial.color = barColor;
                        LowerText.color = lowerColor;
                        HigherText.color = higherColor;
                    }).setOnComplete(() =>
                    {
                        //gameObject.setRenderers(false);
                        //gameObject.setActivated(false);

                        LeanTween.value(gameObject, 0, 1, 0.2f).setOnComplete(() =>
                        {
                            CurrentlyMeasuring = false;
                            if (callbackFunc != null)
                            {
                                callbackFunc.Invoke();
                                callbackFunc = null;
                            }

                            if (onStopMeasuring != null)
                            {
                                onStopMeasuring.Invoke();
                                onStopMeasuring = null;
                            }
                        });
                    });
                });
            });
        });
    }
}