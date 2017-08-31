using UnityEngine;
using System;
using TMPro;
using HoloToolkit.Unity;

public class MoreInfoController : Singleton<MoreInfoController>
{
    public GameObject currentPlanet;
    
    private Material mainScaleMaterial;

    public GameObject ring3D1;
    public GameObject ring1;
    public GameObject ring2;

    public GameObject mainScale;

    public GameObject label1;
    public GameObject label2;
    public GameObject label3;
    public TextMeshProUGUI label1Text;
    public TextMeshProUGUI label2Text;
    public TextMeshProUGUI label3Text;

    public GameObject moreInfoPanel;
    
    public GameObject wireframePlanet;

    public int diameter1Value = 12714;
    public int diameter2Value = 12742;

    public float temperature1 = 56.7f;
    public float temperature2 = 15f;
    public float temperature3 = -89.2f;
    public float temperature1Value = 1f;
    public float temperature2Value = 0.805f;
    public float temperature3Value = 0.15f;


    private Material _currentPlanetMaterial;
    
    private Material _ring1Material1;
    private Material _ring2Material1;
    private Material _ring3D1Material;
    private Color _ring1InitialColor;
    private Color _ring2InitialColor;
    private Color _ring3D1InitialColor;

    private Material _label1Material;
    private Material _label2Material;
    private Material _label3Material;
    
    private bool ringsRotating = false;
    public float ring1RotationSpeed = 12f;
    public float ring2RotationSpeed = -1.6f;
    public float ring3D1RotationSpeed = 5.786512f;

    private bool _showed = false;

    public float ringsAppearingTime = 5.6f;

    private bool _canToggle = true;

    public HologramController hologramController;
    public GameObject Labels;
    public GameObject Rings;
    public GameObject MainScale;

    public Action onHide;

    int wantedState = 0;
    int state = 0;

    protected override void Awake()
    {
        base.Awake();

        mainScaleMaterial = mainScale.GetComponent<Renderer>().material;
    }

    public bool GetShowed()
    {
        return _showed;
    }

    public void ToggleMoreInfo()
    {
        if (SceneStateMachine.Instance != null && SceneStateMachine.Instance.IsEarthState())
        {
            if (_canToggle)
            {
                _canToggle = false;
                if (!_showed)
                {
                    ShowMoreInfo(() =>
                    {
                        _canToggle = true;
                    });
                }
                else
                {
                    HideMoreInfo(() =>
                    {
                        _canToggle = true;
                    });
                }
            }
        }
    }

    public Action MoreInfoShowed;

    public void ShowMoreInfo(Action callback = null)
    {
        /*
         * Итак, когда мы щелкаем More Info, мы хотим чтобы сначала возникали кольца и вращались, вместе с этим оцифровывалась поверхность,
         * затем 3д-кольцо затухает, а 2 2д-кольца синхронизируются по z-оси, и начинают вращаться вокруг нее. Вместе с этим начинают появляться:
         * сначала шкала; затем панель с информацией; после всего 3 Label'а с общей инфой.
         */
         
        wantedState = 1;
        if (state == 0)
        {
            if (MoreInfoShowed != null)
            {
                MoreInfoShowed.Invoke();
            }

            state = 1;

            hologramController.gameObject.SetActive(true);
            Labels.SetActive(true);
            Rings.SetActive(true);
            MainScale.SetActive(true);

            _currentPlanetMaterial = EarthController.Instance.GetSurfaceMaterial();

            transform.position = SlicedEarthPolygon.Instance.transform.position;
            transform.localScale = SlicedEarthPolygon.Instance.transform.localScale;
            Vector3 directionToTarget = Camera.main.transform.position - transform.position;
            if (directionToTarget.sqrMagnitude < 0.001f)
            {
                return;
            }
            directionToTarget.y = 0.0f;
            transform.rotation = Quaternion.LookRotation(-directionToTarget);


            _showed = true;
            ringsRotating = true;
            LeanTween.value(gameObject, 0f, 1f, 1.5f).setOnUpdate((float val) =>
            {
                Color c = _ring1InitialColor;
                c.a = val;
                _ring1Material1.color = c;
                _ring2Material1.color = c;

                c = _ring3D1InitialColor;
                c.a = val;
                _ring3D1Material.color = c;

                _currentPlanetMaterial.SetFloat("_GridBrightness", val);
            });

            LeanTween.rotateAround(ring1.transform.FindChild("Ring").gameObject, Vector3.up, 1440f, ringsAppearingTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAroundLocal(ring1.transform.FindChild("Ring").FindChild("SubRing1").gameObject, Vector3.right, 1440f, ringsAppearingTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAround(ring2, Vector3.forward, ring1RotationSpeed * ringsAppearingTime, ringsAppearingTime);

            LeanTween.rotateAround(ring2.transform.FindChild("Ring").gameObject, Vector3.up, 1080f, ringsAppearingTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAroundLocal(ring2.transform.FindChild("Ring").FindChild("SubRing1").gameObject, Vector3.right, -1080f, ringsAppearingTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAround(ring2, Vector3.forward, ring2RotationSpeed * ringsAppearingTime, ringsAppearingTime);

            LeanTween.rotateAround(ring3D1, Vector3.up, -865f, ringsAppearingTime).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAround(ring3D1.transform.FindChild("Ring3D").gameObject, Vector3.right, -556f, ringsAppearingTime).setEase(LeanTweenType.easeInOutQuad);

            LeanTween.value(gameObject, 1f, 0f, 1.0f).setDelay(ringsAppearingTime - 1f).setOnUpdate((float val) =>
            {
                Color c = _ring3D1InitialColor;
                c.a = val;
                _ring3D1Material.color = c;

                _currentPlanetMaterial.SetFloat("_GridBrightness", val);
            });


            LeanTween.value(gameObject, 0f, 3f, ringsAppearingTime).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) =>
            {
                float tmp = val - Mathf.Floor(val);

                _currentPlanetMaterial.SetFloat("_XCoordOfTheCenter", tmp);
            });


            LeanTween.value(gameObject, 0.163f, 0f, 1.0f).setDelay(3.6f).setOnUpdate((float val) =>
            {
                mainScaleMaterial.SetFloat("_HorizontalStart", val);
            });
            LeanTween.value(gameObject, 0.163f, 1f, 1.0f).setDelay(3.6f).setOnUpdate((float val) =>
            {
                mainScaleMaterial.SetFloat("_HorizontalEnd", val);
            });
            LeanTween.value(gameObject, 0f, 1f, 1.0f).setDelay(3.6f).setOnUpdate((float val) =>
            {
                mainScaleMaterial.SetFloat("_VerticalEnd", val);
            });

            Invoke("ShowHologram", 4.6f);

            LeanTween.value(gameObject, 0f, 1f, 0.78f).setDelay(5.2f).setOnUpdate((float val) =>
            {
                _label1Material.SetFloat("_PercentOfAppearing", val);
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 0.78f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    label1Text.color = new Color(1f, 1f, 1f, val);
                });
            });

            LeanTween.value(gameObject, 0f, 1f, 0.78f).setDelay(5.5f).setOnUpdate((float val) =>
            {
                _label2Material.SetFloat("_PercentOfAppearing", val);
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 0.78f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    label2Text.color = new Color(1f, 1f, 1f, val);
                });
            });

            LeanTween.value(gameObject, 0f, 1f, 0.78f).setDelay(5.8f).setOnUpdate((float val) =>
            {
                _label3Material.SetFloat("_PercentOfAppearing", val);
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 0f, 1f, 0.78f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    label3Text.color = new Color(1f, 1f, 1f, val);
                }).setOnComplete(() =>
                {
                    state = 2;

                    if (wantedState != 1)
                    {
                        HideMoreInfo();
                    }

                    if (callback != null)
                    {
                        callback.Invoke();
                    }
                });
            });
        }
        else if (state == 2)
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    void ShowHologram()
    {
        hologramController.Activate();
    }

    void HideHologram()
    {
        hologramController.Deactivate();
    }

    public Action MoreInfoHided;

    public void HideMoreInfo(Action callback = null)
    {
        /*
         * Когда мы убираем подробную инфу - сначала сворачиваются подробная панель, 3 Label'а, потом общая шкала. Одновременно с этим постепенно 
         * начинают вращаться кольца и после исчезновения Label'ов - исчезают.
         */

        wantedState = 0;
        if (state == 2)
        {
            if (MoreInfoHided != null)
            {
                MoreInfoHided.Invoke();
            }

            state = 3;

            _showed = false;
            LeanTween.value(gameObject, 0f, 1f, 0.75f).setOnUpdate((float val) =>
            {
                Color c = _ring1InitialColor;

                c = _ring3D1InitialColor;
                c.a = val;
                _ring3D1Material.color = c;
            });

            LeanTween.rotateAround(ring1.transform.FindChild("Ring").gameObject, Vector3.up, 720f, ringsAppearingTime / 2f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAroundLocal(ring1.transform.FindChild("Ring").FindChild("SubRing1").gameObject, Vector3.right, 720f, ringsAppearingTime / 2f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAround(ring2, Vector3.forward, ring1RotationSpeed * ringsAppearingTime / 2f, ringsAppearingTime / 2f);

            LeanTween.rotateAround(ring2.transform.FindChild("Ring").gameObject, Vector3.up, 540f, ringsAppearingTime / 2f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAroundLocal(ring2.transform.FindChild("Ring").FindChild("SubRing1").gameObject, Vector3.right, -540f, ringsAppearingTime / 2f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAround(ring2, Vector3.forward, ring2RotationSpeed * ringsAppearingTime / 2f, ringsAppearingTime / 2f);

            LeanTween.rotateAround(ring3D1, Vector3.up, -483f, ringsAppearingTime / 2f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.rotateAround(ring3D1.transform.FindChild("Ring3D").gameObject, Vector3.right, -283f, ringsAppearingTime / 2f).setEase(LeanTweenType.easeInOutQuad);

            LeanTween.value(gameObject, 1f, 0f, 0.5f).setDelay((ringsAppearingTime - 1f) / 2f).setOnUpdate((float val) =>
            {
                Color c = _ring3D1InitialColor;
                c.a = val;
                _ring3D1Material.color = c;

                c = _ring1InitialColor;
                c.a = val;
                _ring1Material1.color = c;
                _ring2Material1.color = c;
            });


            LeanTween.value(gameObject, 0, 0.163f, 0.5f).setDelay((ringsAppearingTime - 2f) / 2f).setOnUpdate((float val) =>
            {
                mainScaleMaterial.SetFloat("_HorizontalStart", val);
            });
            LeanTween.value(gameObject, 1, 0.163f, 0.5f).setDelay((ringsAppearingTime - 2f) / 2f).setOnUpdate((float val) =>
            {
                mainScaleMaterial.SetFloat("_HorizontalEnd", val);
            });
            LeanTween.value(gameObject, 1f, 0f, 0.5f).setDelay((ringsAppearingTime - 2f) / 2f).setOnUpdate((float val) =>
            {
                mainScaleMaterial.SetFloat("_VerticalEnd", val);
            });

            Invoke("HideHologram", 0.39f);

            LeanTween.value(gameObject, 1f, 0f, 0.39f).setDelay((ringsAppearingTime - 0.4f) / 2f).setOnUpdate((float val) =>
            {
                _label1Material.SetFloat("_PercentOfAppearing", val);
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1f, 0f, 0.39f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    label1Text.color = new Color(1f, 1f, 1f, val);
                });
            });

            LeanTween.value(gameObject, 1f, 0f, 0.39f).setDelay((ringsAppearingTime - 0.1f) / 2f).setOnUpdate((float val) =>
            {
                _label2Material.SetFloat("_PercentOfAppearing", val);
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1f, 0f, 0.39f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    label2Text.color = new Color(1f, 1f, 1f, val);
                });
            });

            LeanTween.value(gameObject, 1f, 0f, 0.39f).setDelay((ringsAppearingTime + 0.2f) / 2f).setOnUpdate((float val) =>
            {
                _label3Material.SetFloat("_PercentOfAppearing", val);
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1f, 0f, 0.39f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate((float val) =>
                {
                    label3Text.color = new Color(1f, 1f, 1f, val);
                }).setOnComplete(() =>
                {
                    hideImmediately();

                    hologramController.gameObject.SetActive(false);
                    Labels.SetActive(false);
                    Rings.SetActive(false);
                    MainScale.SetActive(false);

                    state = 0;

                    if (wantedState != 0)
                    {
                        ShowMoreInfo();
                    }

                    if (callback != null)
                    {
                        callback.Invoke();
                    }

                    if (onHide != null)
                    {
                        onHide.Invoke();
                    }
                });
            });
        }
        else if (state == 0)
        {
            if (callback != null)
            {
                callback.Invoke();
            }

            if (onHide != null)
            {
                onHide.Invoke();
            }
        }
    }

    public float GetHidingDuration()
    {
        return (ringsAppearingTime + 0.2f) / 2f + 0.39f;
    }

    private void hideImmediately()
    {
        /*
         * Изначальное положение - кольца в дефолтном положении, полностью прозрачны, шкала, панель с инфой и Label'ы полностью закрыты.
         */

        if (_currentPlanetMaterial != null)
        {
            _currentPlanetMaterial.SetFloat("_GridBrightness", 0f);
        }

        ring3D1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        ring1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        ring2.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Color c = _ring1InitialColor;
        c.a = 0;
        _ring1Material1.color = c;
        c = _ring2InitialColor;
        c.a = 0;
        _ring2Material1.color = c;
        c = _ring3D1InitialColor;
        c.a = 0;
        _ring3D1Material.color = c;
        
        _label1Material.SetFloat("_PercentOfAppearing", 0.0f);
        _label2Material.SetFloat("_PercentOfAppearing", 0.0f);
        _label3Material.SetFloat("_PercentOfAppearing", 0.0f);
        label1Text.color = new Color(1f, 1f, 1f, 0f);
        label2Text.color = new Color(1f, 1f, 1f, 0f);
        label3Text.color = new Color(1f, 1f, 1f, 0f);
        
        mainScaleMaterial.SetFloat("_VerticalStart", 0f);
        mainScaleMaterial.SetFloat("_VerticalEnd", 0f);
        mainScaleMaterial.SetFloat("_HorizontalStart", 0f);
        mainScaleMaterial.SetFloat("_HorizontalEnd", 0f);

        ringsRotating = false;
    }

    // Use this for initialization
    void Start()
    {
        initiateVars();

        hideImmediately();

        hologramController.gameObject.SetActive(false);
        Labels.SetActive(false);
        Rings.SetActive(false);
        MainScale.SetActive(false);
    }

    private void initiateVars()
    {
        _ring1Material1 = ring1.transform.FindChild("Ring").FindChild("SubRing1").gameObject.GetComponentInChildren<Renderer>().material;
        _ring2Material1 = ring2.transform.FindChild("Ring").FindChild("SubRing1").gameObject.GetComponentInChildren<Renderer>().material;
        _ring3D1Material = ring3D1.transform.FindChild("Ring3D").GetComponent<Renderer>().material;
        _ring3D1InitialColor = _ring3D1Material.GetColor("_Color");
        _ring1InitialColor = _ring1Material1.GetColor("_Color");
        _ring2InitialColor = _ring2Material1.GetColor("_Color");

        _label1Material = label1.GetComponent<Renderer>().material;
        _label2Material = label2.GetComponent<Renderer>().material;
        _label3Material = label3.GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ringsRotating)
        {
            ring1.transform.Rotate(0f, 0f, ring1RotationSpeed * Time.deltaTime);
            ring2.transform.Rotate(0f, 0f, ring2RotationSpeed * Time.deltaTime);
            ring3D1.transform.Rotate(0f, ring3D1RotationSpeed * Time.deltaTime, 0f);
        }
	}
}