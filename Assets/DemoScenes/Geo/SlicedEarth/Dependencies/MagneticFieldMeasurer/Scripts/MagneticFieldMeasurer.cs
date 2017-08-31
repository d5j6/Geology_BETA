using UnityEngine;
using HoloWorldExtensions;
using TMPro;
using HoloToolkit.Unity;
using System.Collections.Generic;

public class MagneticFieldMeasurer : Singleton<MagneticFieldMeasurer>
{
    public bool CurrentlyMeasuring = false;

    public GameObject Planet;
    public GameObject MagneticFieldLayers;
    public GameObject SolarWind;
    private ParticleSystem _particleSystem;
    public GameObject MagneticFieldLayer1;
    public GameObject MagneticFieldLayer2;
    public GameObject MagneticFieldLayer3;
    public GameObject MagneticFieldLayer4;
    public TextMeshProUGUI SolarWindText;
    public TextMeshProUGUI GaussText;

    SolarWindController _solarWindController;
    Material MagneticFieldLayer1Mat;
    Material MagneticFieldLayer2Mat;
    Material MagneticFieldLayer3Mat;
    Material MagneticFieldLayer4Mat;
    Material SolarWindMat;

    Color c1;
    Color c2;
    Color c3;
    Color c4;
    Color p;
    Color t;
    Color gt;

    float defaultEmissionRate;
    float additiveEmissionRate = 3000.0f;
    public float SpeedOfWindAdding = 1.5f;
    public float DefaultWindSpeed = 1f;
    [Range(0, 1)]
    public float InnerLayersDefaultWindSpeedCoeff = 0.3f;

    System.Action callbackFunc;

    public System.Action onStopMeasuring;

    Dictionary<Language, Dictionary<string, float>> timings;

    void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("MagneticFieldMeasurer_1", 20f);
        timings[Language.Russian].Add("MagneticFieldMeasurer_2", 12f);

        timings[Language.English].Add("MagneticFieldMeasurer_1", 17f);
        timings[Language.English].Add("MagneticFieldMeasurer_2", 5f);
    }

    void Awake()
    {
        MagneticFieldLayer1Mat = MagneticFieldLayer1.GetComponent<Renderer>().material;
        MagneticFieldLayer2Mat = MagneticFieldLayer2.GetComponent<Renderer>().material;
        MagneticFieldLayer3Mat = MagneticFieldLayer3.GetComponent<Renderer>().material;
        MagneticFieldLayer4Mat = MagneticFieldLayer4.GetComponent<Renderer>().material;
        c1 = MagneticFieldLayer1Mat.color;
        c2 = MagneticFieldLayer2Mat.color;
        c3 = MagneticFieldLayer3Mat.color;
        c4 = MagneticFieldLayer4Mat.color;
        _particleSystem = SolarWind.GetComponent<ParticleSystem>();
        defaultEmissionRate = _particleSystem.emission.rate.constantMax;
        _solarWindController = SolarWind.GetComponent<SolarWindController>();
        p = _solarWindController.GetComponent<Renderer>().material.GetColor("_TintColor");
        t = SolarWindText.color;
        gt = GaussText.color;

        gameObject.setRenderers(false);
        gameObject.setActivated(false);
    }

    public void Measure(System.Action callback)
    {
        CurrentlyMeasuring = true;
        callbackFunc = callback;

        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.y -= 0.12f;
        transform.localPosition = pos;
        transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);
        LeanTween.value(gameObject, 1f, 0.27f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            pos.x = val;
            pos.y = val;
            pos.z = val;
            Planet.transform.localScale = pos;
        }).setOnComplete(() =>
        {
            gameObject.setActivated(true);
            gameObject.setRenderersAlphaCarefully("_Color", 0f, "_TintColor");
            MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", 0f);
            MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", 0f);
            MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", 0f);
            MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", 0f);
            MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0f);
            MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", 0f);
            MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", 0f);
            MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", 0f);
            t.a = 0f;
            SolarWindText.color = t;
            gt.a = 0f;
            GaussText.color = gt;
            gameObject.setRenderers(true);
            _particleSystem.Play();
            LeanTween.value(gameObject, 0f, 1f, 3f).setOnUpdate((float val) =>
            {
                //c1.a = 0.070588235f * val;0.
                c1.a = 0.16470f * val;
                c2.a = 0.36470588235f * val;
                c3.a = 0.36470588235f * val;
                c4.a = 0.36470588235f * val;
                MagneticFieldLayer1Mat.color = c1;
                MagneticFieldLayer2Mat.color = c2;
                MagneticFieldLayer3Mat.color = c3;
                MagneticFieldLayer4Mat.color = c4;

                gt.a = val;
                GaussText.color = gt;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 1.5f).setOnUpdate((float val) =>
                {
                    p.a = val;
                    _solarWindController.SolarWindMaterial.SetColor("_TintColor", p);

                    MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed);
                    MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", val * 0.107f);
                    MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                    MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                    MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", val * 0.02f);

                    t.a = val;
                    SolarWindText.color = t;
                }).setOnComplete(() =>
                {
                    /*LeanTween.value(gameObject, 0f, 1f, 8f).setOnUpdate((float val) =>
                    {
                        float coeff = (2f - (1 + Mathf.Sin(1.570796326795f + val * 12.56637061436f)))/2f;

                        ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
                        ParticleSystem.MinMaxCurve minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate * coeff;
                        emissionModule.rate = minMaxCurve;
                    });*/
                    ParticleSystem.EmissionModule emissionModule;
                    ParticleSystem.MinMaxCurve minMaxCurve;

                    LeanTween.value(gameObject, 0f, 1f, 0.76f).setDelay(1f).setOnUpdate((float val) =>
                    {
                        emissionModule = _particleSystem.emission;
                        minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                        emissionModule.rate = minMaxCurve;
                    }).setOnStart(() =>
                    {
                        LeanTween.value(gameObject, 0f, 1f, 1.52f).setDelay(0.1f).setOnUpdate((float value) =>
                        {
                            float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                            //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                            MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.107f * coeff);
                            float speedAdding = 1.37f;
                            MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                            MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                        });
                    }).setOnComplete(() =>
                    {
                        emissionModule = _particleSystem.emission;
                        minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate;
                        emissionModule.rate = minMaxCurve;

                        LeanTween.value(gameObject, 0f, 1f, 0.53f).setDelay(1.6f).setOnUpdate((float val) =>
                        {
                            emissionModule = _particleSystem.emission;
                            minMaxCurve = emissionModule.rate;
                            minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                            emissionModule.rate = minMaxCurve;
                        }).setOnStart(() =>
                        {
                            LeanTween.value(gameObject, 0f, 1f, 1.26f).setDelay(0.1f).setOnUpdate((float value) =>
                            {
                                float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                                //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                                MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.107f * coeff);
                                float speedAdding = 1.02f;
                                MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                                MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                                MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                                MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            });
                        }).setOnComplete(() =>
                        {
                            emissionModule = _particleSystem.emission;
                            minMaxCurve = emissionModule.rate;
                            minMaxCurve.constantMax = defaultEmissionRate;
                            emissionModule.rate = minMaxCurve;

                            LeanTween.value(gameObject, 0f, 1f, 1.36f).setDelay(0.96f).setOnUpdate((float val) =>
                            {
                                emissionModule = _particleSystem.emission;
                                minMaxCurve = emissionModule.rate;
                                minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                                emissionModule.rate = minMaxCurve;
                            }).setOnComplete(() =>
                            {
                                emissionModule = _particleSystem.emission;
                                minMaxCurve = emissionModule.rate;
                                minMaxCurve.constantMax = defaultEmissionRate;
                                emissionModule.rate = minMaxCurve;
                            }).setOnStart(() =>
                            {
                                LeanTween.value(gameObject, 0f, 1f, 2.72f).setDelay(0.1f).setOnUpdate((float value) =>
                                {
                                    float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                                    //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                                    MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.223f * coeff);
                                    float speedAdding = 3.78f;
                                    MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                                    MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 2.0f));
                                    MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 1.56f));
                                    MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 1.33f));
                                }).setOnComplete(() =>
                                {
                                    LeanTween.value(gameObject, 1f, 0f, 1.5f).setDelay(1.89f).setOnUpdate((float val) =>
                                    {
                                        p.a = val;
                                        _solarWindController.SolarWindMaterial.SetColor("_TintColor", p);

                                        MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed);
                                        MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", val * 0.107f);
                                        MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                                        MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                                        MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", val * 0.02f);

                                        t.a = val;
                                        SolarWindText.color = t;
                                    }).setOnComplete(() =>
                                    {
                                        _particleSystem.Stop();

                                        LeanTween.value(gameObject, 1f, 0f, 3f).setDelay(2f).setOnUpdate((float val) =>
                                        {
                                            c1.a = 0.070588235f * val;
                                            //c1.a = 0.16470f * val;
                                            c2.a = 0.36470588235f * val;
                                            c3.a = 0.36470588235f * val;
                                            c4.a = 0.36470588235f * val;
                                            MagneticFieldLayer1Mat.color = c1;
                                            MagneticFieldLayer2Mat.color = c2;
                                            MagneticFieldLayer3Mat.color = c3;
                                            MagneticFieldLayer4Mat.color = c4;

                                            gt.a = val;
                                            GaussText.color = gt;
                                        }).setOnComplete(() =>
                                        {
                                            gameObject.setRenderersAlphaCarefully("_Color", 0f, "_TintColor");
                                            MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", 0f);
                                            t.a = 0f;
                                            SolarWindText.color = t;
                                            gt.a = 0f;
                                            GaussText.color = gt;
                                            gameObject.setRenderers(false);
                                            gameObject.setActivated(false);

                                            LeanTween.value(gameObject, 0.27f, 1f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                                            {
                                                pos.x = val;
                                                pos.y = val;
                                                pos.z = val;
                                                Planet.transform.localScale = pos;
                                            }).setOnComplete(() =>
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
                        });
                    });
                });
            });
        });
    }

    public void MeasureInLectureMode(System.Action callback)
    {
        if (timings == null)
        {
            InitializeTimings();
        }
        CurrentlyMeasuring = true;
        Debug.Log("MeasureInLectureMode!");
        callbackFunc = callback;

        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.y -= 0.12f;
        transform.localPosition = pos;
        transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);
        LeanTween.value(gameObject, 1f, 0.27f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            pos.x = val;
            pos.y = val;
            pos.z = val;
            Planet.transform.localScale = pos;
        }).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 0f, 1f, 3f).setOnStart(() =>
            {
                gameObject.setActivated(true);
                gameObject.setRenderersAlphaCarefully("_Color", 0f, "_TintColor");
                MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0f);
                MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", 0f);
                MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", 0f);
                MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", 0f);
                t.a = 0f;
                SolarWindText.color = t;
                gt.a = 0f;
                GaussText.color = gt;
                gameObject.setRenderers(true);
                _particleSystem.Play();
            }).setOnUpdate((float val) =>
            {
                c1.a = 0.070588235f * val;
                //c1.a = 0.16470f * val;
                c2.a = 0.36470588235f * val;
                c3.a = 0.36470588235f * val;
                c4.a = 0.36470588235f * val;
                MagneticFieldLayer1Mat.color = c1;
                MagneticFieldLayer2Mat.color = c2;
                MagneticFieldLayer3Mat.color = c3;
                MagneticFieldLayer4Mat.color = c4;

                gt.a = val;
                GaussText.color = gt;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 1.5f).setDelay(timings[LanguageManager.Instance.CurrentLanguage]["MagneticFieldMeasurer_1"]).setOnUpdate((float val) =>
                {
                    p.a = val;
                    _solarWindController.SolarWindMaterial.SetColor("_TintColor", p);

                    MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed);
                    MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", val * 0.107f);
                    MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                    MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                    MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", val * 0.02f);

                    t.a = val;
                    SolarWindText.color = t;
                }).setOnComplete(() =>
                {
                    /*LeanTween.value(gameObject, 0f, 1f, 8f).setOnUpdate((float val) =>
                    {
                        float coeff = (2f - (1 + Mathf.Sin(1.570796326795f + val * 12.56637061436f)))/2f;

                        ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
                        ParticleSystem.MinMaxCurve minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate * coeff;
                        emissionModule.rate = minMaxCurve;
                    });*/
                    ParticleSystem.EmissionModule emissionModule;
                    ParticleSystem.MinMaxCurve minMaxCurve;

                    LeanTween.value(gameObject, 0f, 1f, 0.76f).setDelay(1f).setOnUpdate((float val) =>
                    {
                        emissionModule = _particleSystem.emission;
                        minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                        emissionModule.rate = minMaxCurve;
                    }).setOnStart(() =>
                    {
                        LeanTween.value(gameObject, 0f, 1f, 1.52f).setDelay(0.1f).setOnUpdate((float value) =>
                        {
                            float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                            //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                            MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.107f * coeff);
                            float speedAdding = 1.37f;
                            MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                            MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                        });
                    }).setOnComplete(() =>
                    {
                        emissionModule = _particleSystem.emission;
                        minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate;
                        emissionModule.rate = minMaxCurve;

                        LeanTween.value(gameObject, 0f, 1f, 0.53f).setDelay(1.6f).setOnUpdate((float val) =>
                        {
                            emissionModule = _particleSystem.emission;
                            minMaxCurve = emissionModule.rate;
                            minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                            emissionModule.rate = minMaxCurve;
                        }).setOnStart(() =>
                        {
                            LeanTween.value(gameObject, 0f, 1f, 1.26f).setDelay(0.1f).setOnUpdate((float value) =>
                            {
                                float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                                //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                                MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.107f * coeff);
                                float speedAdding = 1.02f;
                                MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                                MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                                MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                                MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            });
                        }).setOnComplete(() =>
                        {
                            emissionModule = _particleSystem.emission;
                            minMaxCurve = emissionModule.rate;
                            minMaxCurve.constantMax = defaultEmissionRate;
                            emissionModule.rate = minMaxCurve;

                            LeanTween.value(gameObject, 0f, 1f, 1.36f).setDelay(0.96f).setOnUpdate((float val) =>
                            {
                                emissionModule = _particleSystem.emission;
                                minMaxCurve = emissionModule.rate;
                                minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                                emissionModule.rate = minMaxCurve;
                            }).setOnComplete(() =>
                            {
                                emissionModule = _particleSystem.emission;
                                minMaxCurve = emissionModule.rate;
                                minMaxCurve.constantMax = defaultEmissionRate;
                                emissionModule.rate = minMaxCurve;
                            }).setOnStart(() =>
                            {
                                LeanTween.value(gameObject, 0f, 1f, 2.72f).setDelay(0.1f).setOnUpdate((float value) =>
                                {
                                    float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                                    //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                                    MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.223f * coeff);
                                    float speedAdding = 3.78f;
                                    MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                                    MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 2.0f));
                                    MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 1.56f));
                                    MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 1.33f));
                                }).setOnComplete(() =>
                                {
                                    LeanTween.value(gameObject, 1f, 0f, 1.5f).setDelay(1.89f).setOnUpdate((float val) =>
                                    {
                                        p.a = val;
                                        _solarWindController.SolarWindMaterial.SetColor("_TintColor", p);

                                        MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed);
                                        MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", val * 0.107f);
                                        MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                                        MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                                        MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", val * 0.02f);

                                        t.a = val;
                                        SolarWindText.color = t;
                                    }).setOnComplete(() =>
                                    {
                                        _particleSystem.Stop();

                                        LeanTween.value(gameObject, 1f, 0f, 1.5f).setDelay(timings[LanguageManager.Instance.CurrentLanguage]["MagneticFieldMeasurer_2"]).setOnUpdate((float val) =>     // Fading out
                                        {
                                            c1.a = 0.070588235f * val;
                                            //c1.a = 0.16470f * val;
                                            c2.a = 0.36470588235f * val;
                                            c3.a = 0.36470588235f * val;
                                            c4.a = 0.36470588235f * val;
                                            MagneticFieldLayer1Mat.color = c1;
                                            MagneticFieldLayer2Mat.color = c2;
                                            MagneticFieldLayer3Mat.color = c3;
                                            MagneticFieldLayer4Mat.color = c4;

                                            gt.a = val;
                                            GaussText.color = gt;
                                        }).setOnComplete(() =>
                                        {
                                            gameObject.setRenderersAlphaCarefully("_Color", 0f, "_TintColor");
                                            MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", 0f);
                                            t.a = 0f;
                                            SolarWindText.color = t;
                                            gt.a = 0f;
                                            GaussText.color = gt;
                                            gameObject.setRenderers(false);
                                            gameObject.setActivated(false);

                                            LeanTween.value(gameObject, 0.27f, 1f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                                            {
                                                pos.x = val;
                                                pos.y = val;
                                                pos.z = val;
                                                Planet.transform.localScale = pos;
                                            }).setOnComplete(() =>
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
                        });
                    });
                });
            });
        });
    }

    public void MeasureInDemoMode(System.Action callback)
    {
        CurrentlyMeasuring = true;
        callbackFunc = callback;

        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.y -= 0.12f;
        transform.localPosition = pos;
        transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);
        LeanTween.value(gameObject, 1f, 0.27f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            pos.x = val;
            pos.y = val;
            pos.z = val;
            Planet.transform.localScale = pos;
        }).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 0f, 1f, 3f).setOnStart(() =>
            {
                gameObject.setActivated(true);
                gameObject.setRenderersAlphaCarefully("_Color", 0f, "_TintColor");
                MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", 0f);
                MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0f);
                MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", 0f);
                MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", 0f);
                MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", 0f);
                t.a = 0f;
                SolarWindText.color = t;
                gt.a = 0f;
                GaussText.color = gt;
                gameObject.setRenderers(true);
                _particleSystem.Play();
            }).setOnUpdate((float val) =>
            {
                c1.a = 0.070588235f * val;
                //c1.a = 0.16470f * val;
                c2.a = 0.36470588235f * val;
                c3.a = 0.36470588235f * val;
                c4.a = 0.36470588235f * val;
                MagneticFieldLayer1Mat.color = c1;
                MagneticFieldLayer2Mat.color = c2;
                MagneticFieldLayer3Mat.color = c3;
                MagneticFieldLayer4Mat.color = c4;

                gt.a = val;
                GaussText.color = gt;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 1.5f).setDelay(9f).setOnUpdate((float val) =>
                {
                    p.a = val;
                    _solarWindController.SolarWindMaterial.SetColor("_TintColor", p);

                    MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed);
                    MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                    MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", val * 0.107f);
                    MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                    MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                    MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", val * 0.02f);

                    t.a = val;
                    SolarWindText.color = t;
                }).setOnComplete(() =>
                {
                    /*LeanTween.value(gameObject, 0f, 1f, 8f).setOnUpdate((float val) =>
                    {
                        float coeff = (2f - (1 + Mathf.Sin(1.570796326795f + val * 12.56637061436f)))/2f;

                        ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
                        ParticleSystem.MinMaxCurve minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate * coeff;
                        emissionModule.rate = minMaxCurve;
                    });*/
                    ParticleSystem.EmissionModule emissionModule;
                    ParticleSystem.MinMaxCurve minMaxCurve;

                    LeanTween.value(gameObject, 0f, 1f, 0.76f).setDelay(1f).setOnUpdate((float val) =>
                    {
                        emissionModule = _particleSystem.emission;
                        minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                        emissionModule.rate = minMaxCurve;
                    }).setOnStart(() =>
                    {
                        LeanTween.value(gameObject, 0f, 1f, 1.52f).setDelay(0.1f).setOnUpdate((float value) =>
                        {
                            float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                            //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                            MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.107f * coeff);
                            float speedAdding = 1.37f;
                            MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                            MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                        });
                    }).setOnComplete(() =>
                    {
                        emissionModule = _particleSystem.emission;
                        minMaxCurve = emissionModule.rate;
                        minMaxCurve.constantMax = defaultEmissionRate;
                        emissionModule.rate = minMaxCurve;

                        LeanTween.value(gameObject, 0f, 1f, 0.53f).setDelay(1.6f).setOnUpdate((float val) =>
                        {
                            emissionModule = _particleSystem.emission;
                            minMaxCurve = emissionModule.rate;
                            minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                            emissionModule.rate = minMaxCurve;
                        }).setOnStart(() =>
                        {
                            LeanTween.value(gameObject, 0f, 1f, 1.26f).setDelay(0.1f).setOnUpdate((float value) =>
                            {
                                float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                                //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                                MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.107f * coeff);
                                float speedAdding = 1.02f;
                                MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                                MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                                MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                                MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * InnerLayersDefaultWindSpeedCoeff);
                            });
                        }).setOnComplete(() =>
                        {
                            emissionModule = _particleSystem.emission;
                            minMaxCurve = emissionModule.rate;
                            minMaxCurve.constantMax = defaultEmissionRate;
                            emissionModule.rate = minMaxCurve;

                            LeanTween.value(gameObject, 0f, 1f, 1.36f).setDelay(0.96f).setOnUpdate((float val) =>
                            {
                                emissionModule = _particleSystem.emission;
                                minMaxCurve = emissionModule.rate;
                                minMaxCurve.constantMax = defaultEmissionRate + additiveEmissionRate;
                                emissionModule.rate = minMaxCurve;
                            }).setOnComplete(() =>
                            {
                                emissionModule = _particleSystem.emission;
                                minMaxCurve = emissionModule.rate;
                                minMaxCurve.constantMax = defaultEmissionRate;
                                emissionModule.rate = minMaxCurve;
                            }).setOnStart(() =>
                            {
                                LeanTween.value(gameObject, 0f, 1f, 2.72f).setDelay(0.1f).setOnUpdate((float value) =>
                                {
                                    float coeff = (2f - (1 + Mathf.Cos(value * 6.28318530718f))) / 2f;
                                    //float coeff = Mathf.Lerp(0, 1, Mathf.Sin(value * Mathf.PI) * SpeedOfWindAdding);
                                    MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0.107f + 0.223f * coeff);
                                    float speedAdding = 3.78f;
                                    MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed + coeff * speedAdding);
                                    MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 2.0f));
                                    MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 1.56f));
                                    MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff + coeff * speedAdding * (InnerLayersDefaultWindSpeedCoeff * 1.33f));
                                }).setOnComplete(() =>
                                {
                                    LeanTween.value(gameObject, 1f, 0f, 1.5f).setDelay(1.89f).setOnUpdate((float val) =>
                                    {
                                        p.a = val;
                                        _solarWindController.SolarWindMaterial.SetColor("_TintColor", p);

                                        MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed);
                                        MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", val * DefaultWindSpeed * InnerLayersDefaultWindSpeedCoeff);
                                        MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", val * 0.107f);
                                        MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                                        MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", val * 0.02f);
                                        MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", val * 0.02f);

                                        t.a = val;
                                        SolarWindText.color = t;
                                    }).setOnComplete(() =>
                                    {
                                        _particleSystem.Stop();

                                        LeanTween.value(gameObject, 1f, 0f, 1.5f).setDelay(1f).setOnUpdate((float val) =>
                                        {
                                            c1.a = 0.070588235f * val;
                                            //c1.a = 0.16470f * val;
                                            c2.a = 0.36470588235f * val;
                                            c3.a = 0.36470588235f * val;
                                            c4.a = 0.36470588235f * val;
                                            MagneticFieldLayer1Mat.color = c1;
                                            MagneticFieldLayer2Mat.color = c2;
                                            MagneticFieldLayer3Mat.color = c3;
                                            MagneticFieldLayer4Mat.color = c4;

                                            gt.a = val;
                                            GaussText.color = gt;
                                        }).setOnComplete(() =>
                                        {
                                            gameObject.setRenderersAlphaCarefully("_Color", 0f, "_TintColor");
                                            MagneticFieldLayer1Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer2Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer3Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer4Mat.SetFloat("_SolarWindSpeed", 0f);
                                            MagneticFieldLayer1Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer2Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer3Mat.SetFloat("_DistortionsStrength", 0f);
                                            MagneticFieldLayer4Mat.SetFloat("_DistortionsStrength", 0f);
                                            t.a = 0f;
                                            SolarWindText.color = t;
                                            gt.a = 0f;
                                            GaussText.color = gt;
                                            gameObject.setRenderers(false);
                                            gameObject.setActivated(false);

                                            LeanTween.value(gameObject, 0.27f, 1f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                                            {
                                                pos.x = val;
                                                pos.y = val;
                                                pos.z = val;
                                                Planet.transform.localScale = pos;
                                            }).setOnComplete(() =>
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
                        });
                    });
                });
            });
        });
    }
}