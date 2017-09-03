using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using System.Collections.Generic;

public class MassMeasurer : Singleton<MassMeasurer>
{
    public bool CurrentlyMeasuring = false;

    private CurvedSpaceController _curvedSpaceController;

    public GameObject Planet;
    [Range(0f, 1f)]
    public float RadiusOfPlanet = 1f;

    public long Mass = 5974;

    public System.Action onStopMeasuring;

    Dictionary<Language, Dictionary<string, float>> timings;

    void InitializeTimings()
    {
        timings = new Dictionary<Language, Dictionary<string, float>>();
        timings.Add(Language.Russian, new Dictionary<string, float>());
        timings.Add(Language.English, new Dictionary<string, float>());

        timings[Language.Russian].Add("MassMeasurer_1", 33f);

        timings[Language.English].Add("MassMeasurer_1", 25f);
    }

    void Awake()
    {
        _curvedSpaceController = GetComponent<CurvedSpaceController>();
    }

    public void Measure(System.Action callback = null)
    {
        CurrentlyMeasuring = true;
        Vector3 pos = Vector3.zero;
        //Vector3 pos = Planet.transform.position;
        pos.y -= 1f;
        transform.localPosition = pos;
        LeanTween.value(gameObject, 1f, 0.35f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            pos.x = val;
            pos.y = val;
            pos.z = val;
            Planet.transform.localScale = pos;
        }).setOnComplete(() =>
        {
            _curvedSpaceController.ShowText();
        });
        //LeanTween.scale(Planet, new Vector3(0.35f, 0.35f, 0.35f), 2f);
        LeanTween.moveLocalY(Planet, 0f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            //_curvedSpaceController.gameObject.setActivated(true);
            _curvedSpaceController.LabelMass = 0;
            _curvedSpaceController.Mass = 0f;
            _curvedSpaceController.Spreading = 0f;
            _curvedSpaceController.Thickness = 0f;
            _curvedSpaceController.Centering = 0f;
            // _curvedSpaceController.Spreading = Mathf.PI * 2.0f;
            //_curvedSpaceController.Centering = 0.215f;
            _curvedSpaceController.Mass = 0f;
            LeanTween.value(gameObject, 0f, 1f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                _curvedSpaceController.Thickness = val * 0.002f;
            }).setOnComplete(() =>
            {
                LeanTween.moveLocalY(Planet, -0.25f, 5f).setEase(LeanTweenType.easeInOutCubic);
                LeanTween.value(gameObject, 0f, 1f, 5f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    _curvedSpaceController.Mass = 0.209f * val;
                    _curvedSpaceController.LabelMass = (long)Mathf.Round(Mass * val);
                    //_curvedSpaceController.Spreading = Mathf.PI * 2.0f * val;
                    //_curvedSpaceController.Centering = 0.215f * val;
                }).setOnComplete(() =>
                {
                    LeanTween.value(gameObject, 1f, 0f, 1f).setEase(LeanTweenType.easeInOutCubic).setDelay(3f).setOnUpdate((float val) =>
                    {
                        _curvedSpaceController.Thickness = val * 0.002f;
                    }).setOnStart(() =>
                    {
                        _curvedSpaceController.HideText();
                        LeanTween.scale(Planet, Vector3.one * RadiusOfPlanet, 2f).setEase(LeanTweenType.easeInOutCubic);
                        LeanTween.moveLocalY(Planet, 0f, 2f).setEase(LeanTweenType.easeInOutCubic);
                    }).setOnComplete(() =>
                    {
                        CurrentlyMeasuring = false;
                        if (callback != null)
                        {
                            callback.Invoke();
                            callback = null;
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
    }

    public void MeasureInLectureMode(System.Action callback = null)
    {
        if (timings == null)
        {
            InitializeTimings();
        }
        CurrentlyMeasuring = true;
        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.y -= 1f;
        transform.localPosition = pos;
        LeanTween.value(gameObject, 1f, 0.35f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            pos.x = val;
            pos.y = val;
            pos.z = val;
            Planet.transform.localScale = pos;
        }).setOnComplete(() =>
        {
            _curvedSpaceController.ShowText();
        });
        //LeanTween.scale(Planet, new Vector3(0.35f, 0.35f, 0.35f), 2f);
        LeanTween.moveLocalY(Planet, 0f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            //_curvedSpaceController.gameObject.setActivated(true);
            _curvedSpaceController.LabelMass = 0;
            _curvedSpaceController.Mass = 0f;
            _curvedSpaceController.Spreading = 0f;
            _curvedSpaceController.Thickness = 0f;
            _curvedSpaceController.Centering = 0f;
            // _curvedSpaceController.Spreading = Mathf.PI * 2.0f;
            //_curvedSpaceController.Centering = 0.215f;
            _curvedSpaceController.Mass = 0f;
            LeanTween.value(gameObject, 0f, 1f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                _curvedSpaceController.Thickness = val * 0.002f;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 5f).setEase(LeanTweenType.easeInOutCubic).setOnStart(() =>
                {
                    LeanTween.moveLocalY(Planet, -0.25f, 5f).setEase(LeanTweenType.easeInOutCubic);
                }).setOnUpdate((float val) =>
                {
                    _curvedSpaceController.Mass = 0.209f * val;
                    _curvedSpaceController.LabelMass = (long)Mathf.Round(Mass * val);
                    //_curvedSpaceController.Spreading = Mathf.PI * 2.0f * val;
                    //_curvedSpaceController.Centering = 0.215f * val;
                }).setOnComplete(() =>
                {
                    LeanTween.delayedCall(6f, () =>
                    {
                        LeanTween.value(gameObject, 1f, 0f, 1f).setDelay(timings[LanguageManager.Instance.CurrentLanguage]["MassMeasurer_1"]).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                        {
                            _curvedSpaceController.Thickness = val * 0.002f;
                        }).setOnStart(() =>
                        {
                            _curvedSpaceController.HideText();
                            LeanTween.scale(Planet, Vector3.one * RadiusOfPlanet, 2f).setEase(LeanTweenType.easeInOutCubic);
                            LeanTween.moveLocalY(Planet, 0f, 2f).setEase(LeanTweenType.easeInOutCubic);
                        }).setOnComplete(() =>
                        {
                            CurrentlyMeasuring = false;
                            if (callback != null)
                            {
                                callback.Invoke();
                                callback = null;
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
        //Vector3 pos = Planet.transform.position;
        Vector3 pos = Vector3.zero;
        pos.y -= 1f;
        transform.localPosition = pos;
        LeanTween.value(gameObject, 1f, 0.35f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
        {
            pos.x = val;
            pos.y = val;
            pos.z = val;
            Planet.transform.localScale = pos;
        }).setOnComplete(() =>
        {
            _curvedSpaceController.ShowText();
        });
        //LeanTween.scale(Planet, new Vector3(0.35f, 0.35f, 0.35f), 2f);
        LeanTween.moveLocalY(Planet, 0f, 2f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() =>
        {
            //_curvedSpaceController.gameObject.setActivated(true);
            _curvedSpaceController.LabelMass = 0;
            _curvedSpaceController.Mass = 0f;
            _curvedSpaceController.Spreading = 0f;
            _curvedSpaceController.Thickness = 0f;
            _curvedSpaceController.Centering = 0f;
            // _curvedSpaceController.Spreading = Mathf.PI * 2.0f;
            //_curvedSpaceController.Centering = 0.215f;
            _curvedSpaceController.Mass = 0f;
            LeanTween.value(gameObject, 0f, 1f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
            {
                _curvedSpaceController.Thickness = val * 0.002f;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 5f).setEase(LeanTweenType.easeInOutCubic).setOnStart(() =>
                {
                    LeanTween.moveLocalY(Planet, -0.25f, 5f).setEase(LeanTweenType.easeInOutCubic);
                }).setOnUpdate((float val) =>
                {
                    _curvedSpaceController.Mass = 0.209f * val;
                    _curvedSpaceController.LabelMass = (long)Mathf.Round(Mass * val);
                    //_curvedSpaceController.Spreading = Mathf.PI * 2.0f * val;
                    //_curvedSpaceController.Centering = 0.215f * val;
                }).setOnComplete(() =>
                {
                    LeanTween.delayedCall(6f, () =>
                    {
                        LeanTween.value(gameObject, 1f, 0f, 1f).setDelay(6f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                        {
                            _curvedSpaceController.Thickness = val * 0.002f;
                        }).setOnStart(() =>
                        {
                            _curvedSpaceController.HideText();
                            LeanTween.scale(Planet, Vector3.one * RadiusOfPlanet, 2f).setEase(LeanTweenType.easeInOutCubic);
                            LeanTween.moveLocalY(Planet, 0f, 2f).setEase(LeanTweenType.easeInOutCubic);
                        }).setOnComplete(() =>
                        {
                            CurrentlyMeasuring = false;
                            if (callback != null)
                            {
                                callback.Invoke();
                                callback = null;
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

    public void HideImmediately()
    {
        //_curvedSpaceController.gameObject.setActivated(false);
    }
}