using UnityEngine;
using System;
using HoloToolkit.Unity;

public class PieController : Singleton<PieController>
{
    #region Labels Control

    private enum PieSideActive { Left, Front, Right, Back, Default }
    private PieSideActive currentSide = PieSideActive.Default;

    private bool showingLabels = false;

    [SerializeField]
    private GameObject[] LeftSideLabels;
    [SerializeField]
    private GameObject[] RightSideLabels;
    [SerializeField]
    private GameObject[] BackSideLabels;
    [SerializeField]
    private GameObject[] FrontSideLabels;

    private void Update () {
        if (showingLabels)
        {
            Vector3 localRotation = (Quaternion.Inverse(/*Camera.main.transform.rotation*/Quaternion.LookRotation((Camera.main.transform.position - transform.position).normalized, Vector3.up)) * transform.rotation).eulerAngles;

            //Debug.Log("localRotation.y: " + localRotation.y);

            if (localRotation.y >= 225f && localRotation.y < 315.0f)
            {
                goToSide(PieSideActive.Front);
            }
            else if (localRotation.y >= 135f && localRotation.y < 225.0f)
            {
                goToSide(PieSideActive.Left);
            }
            else if (localRotation.y >= 45f && localRotation.y < 135.0f)
            {
                goToSide(PieSideActive.Back);
            }
            else
            {
                goToSide(PieSideActive.Right);
            }
        }
    }

    private void goToSide(PieSideActive newSide)
    {
        if (newSide != currentSide)
        {
            hideLabelsOnSide(currentSide);
            currentSide = newSide;
            showLabelsOnSide(currentSide);
        }
    }

    private void showLabelsOnSide(PieSideActive side)
    {
        int i;
        switch (side)
        {
            case PieSideActive.Left:
                for (i = 0; i < LeftSideLabels.Length; i++)
                {
                    LeftSideLabels[i].GetComponent<HitechLabelController>().Show();
                }
                break;
            case PieSideActive.Front:
                for (i = 0; i < FrontSideLabels.Length; i++)
                {
                    FrontSideLabels[i].GetComponent<HitechLabelController>().Show();
                }
                break;
            case PieSideActive.Right:
                for (i = 0; i < RightSideLabels.Length; i++)
                {
                    RightSideLabels[i].GetComponent<HitechLabelController>().Show();
                }
                break;
            case PieSideActive.Back:
                for (i = 0; i < BackSideLabels.Length; i++)
                {
                    BackSideLabels[i].GetComponent<HitechLabelController>().Show();
                }
                break;
        }
    }

    private void hideLabelsOnSide(PieSideActive side)
    {
        int i;
        switch (side)
        {
            case PieSideActive.Left:
                for (i = 0; i < LeftSideLabels.Length; i++)
                {
                    LeftSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
            case PieSideActive.Front:
                for (i = 0; i < FrontSideLabels.Length; i++)
                {
                    FrontSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
            case PieSideActive.Right:
                for (i = 0; i < RightSideLabels.Length; i++)
                {
                    RightSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
            case PieSideActive.Back:
                for (i = 0; i < BackSideLabels.Length; i++)
                {
                    BackSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
        }
    }

    #endregion

    #region Layers Control

    /*
     * У нас сценка с пирожком работает по следующему сценарию. При открытии дефолтное состояние - ничего не выделено, показан разделитель.
     * При выборе любого камня убирается разделитель, если он есть, убирается слой, если он есть и показывается выбранный слой.
     * При выборе пирожка, убирается слой, если он есть, и показывается разделитель.
     */

    #region Intreface
    
    public static float LayersFadeTime = 0.96f;
    public static float LayersTintCoeff = 0.47f;
    public static float DividerFadeTime = 0.96f;

    /// <summary>
    /// Вызывается, когда начинается переход между состояниями
    /// </summary>
    public Action ChangesStarted;
    /// <summary>
    /// Вызывается, когда переход между состояниями закончен
    /// </summary>
    public Action ChangesEnded;
	
    public Action PieDividerShowed;
    public Action PieDividerHided;
    public Action MagmaShowed;
    public Action MagmaHided;
    public Action BasaltShowed;
    public Action BasaltHided;
    public Action GranitShowed;
    public Action GranitHided;
    public Action SedimentaryShowed;
    public Action SedimentaryHided;

    public void StartShowingLabels()
    {
        showingLabels = true;
    }

    public void StopShowingLabels()
    {
        showingLabels = false;
    }

    public void ShowPieDivider(Action callback = null)
    {
        if (ChangesStarted != null)
        {
            ChangesStarted.Invoke();
        }

        hideCurrentLayer(() =>
        {
            showPieDivider(callback);
        });
    }

    public void HidePieDivider(Action callback = null)
    {
        hidePieDivider(callback);
    }

    public void ShowSedimentaryLayer(Action callback = null)
    {
        hidePieDivider(() =>
        {
            hideCurrentLayer(() =>
            {
                goToLayer(PieLayerSelected.Sedimentary, callback);
            });
        });
    }

    public void HideSedimentaryLayer(Action callback = null)
    {
        hideCurrentLayer(callback);
    }

    public void ShowGranitLayer(Action callback = null)
    {
        hidePieDivider(() =>
        {
            hideCurrentLayer(() =>
            {
                goToLayer(PieLayerSelected.Granit, callback);
            });
        });
    }

    public void HideGranitLayer(Action callback = null)
    {
        hideCurrentLayer(callback);
    }

    public void ShowBasaltLayer(Action callback = null)
    {
        hidePieDivider(() =>
        {
            hideCurrentLayer(() =>
            {
                goToLayer(PieLayerSelected.Basalt, callback);
            });
        });
    }

    public void HideBasaltLayer(Action callback = null)
    {
        hideCurrentLayer(callback);
    }

    public void ShowMagmaLayer(Action callback = null)
    {
        hidePieDivider(() =>
        {
            hideCurrentLayer(() =>
            {
                goToLayer(PieLayerSelected.Magma, callback);
            });
        });
    }

    public void HideMagmaLayer(Action callback = null)
    {
        if (MagmaHided != null)
        {
            MagmaHided.Invoke();
        }

        hideCurrentLayer(callback);
    }

    #endregion Interface
    
    public enum PieLayerSelected { Divider, Sedimentary, Granit, Basalt, Magma }
    private PieLayerSelected currentLayerSelected = PieLayerSelected.Divider;

    [SerializeField]
    private PieDividerController pieDivider;

    [SerializeField]
    private MeshRenderer magmaRend;
    [SerializeField]
    private MeshRenderer basaltRend;
    [SerializeField]
    private MeshRenderer granitRend;
    [SerializeField]
    private MeshRenderer sedimentaryRend;

    private Material magmaMaterial;
    private Material basaltMaterial;
    private Material granitMaterial;
    private Material sedimentaryMaterial;

    private void Awake()
    {
        magmaMaterial = magmaRend.material;
        basaltMaterial = basaltRend.material;
        granitMaterial = granitRend.material;
        sedimentaryMaterial = sedimentaryRend.material;
    }

    private void hidePieDivider(Action callback = null)
    {
        if (PieDividerHided != null)
        {
            PieDividerHided.Invoke();
        }

        pieDivider.Hide(DividerFadeTime, callback);
    }

    private void showPieDivider(Action callback = null)
    {
        if (PieDividerShowed != null)
        {
            PieDividerShowed.Invoke();
        }

        pieDivider.Show(DividerFadeTime, () =>
		{
			if (callback != null)
			{
				callback.Invoke();
			}
			
			if (ChangesEnded != null)
			{
				ChangesEnded.Invoke();
			}
		});
    }

    private void hideCurrentLayer(Action callback = null)
    {
        /*
         * Закрытие текущего слоя представляет собой фейдаутинг материала и все.
         */

        Material materialToFadeOut = null;
        switch (currentLayerSelected)
        {
            case PieLayerSelected.Magma:
                materialToFadeOut = magmaMaterial;
                if (MagmaHided != null)
                {
                    MagmaHided.Invoke();
                }
                break;
            case PieLayerSelected.Basalt:
                materialToFadeOut = basaltMaterial;
                if (BasaltHided != null)
                {
                    BasaltHided.Invoke();
                }
                break;
            case PieLayerSelected.Granit:
                materialToFadeOut = granitMaterial;
                if (GranitHided != null)
                {
                    GranitHided.Invoke();
                }
                break;
            case PieLayerSelected.Sedimentary:
                materialToFadeOut = sedimentaryMaterial;
                if (SedimentaryHided != null)
                {
                    SedimentaryHided.Invoke();
                }
                break;
        }
        
        if (materialToFadeOut != null)
        {
            LeanTween.value(gameObject, LayersTintCoeff, 0f, LayersFadeTime).setOnUpdate((float val) =>
            {
                materialToFadeOut.SetFloat("_ColorMultiplier", 1f + val);
            }).setOnComplete(() =>
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        }
        else
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    private void goToLayer(PieLayerSelected newLayer, Action callback = null)
    {
        /*
         * Переход к другому слою заключается в том, что мы сначала фейдиним выбранный слой, а затем меняем инфу на плашках
         */

        Material materialToFadeIn = null;
        if (newLayer != currentLayerSelected)
        {
            if (ChangesStarted != null)
            {
                ChangesStarted.Invoke();
            }

            currentLayerSelected = newLayer;

            switch (newLayer)
            {
                case PieLayerSelected.Magma:
                    materialToFadeIn = magmaMaterial;
                    if (MagmaShowed != null)
                    {
                        MagmaShowed.Invoke();
                    }
                    break;
                case PieLayerSelected.Basalt:
                    materialToFadeIn = basaltMaterial;
                    if (BasaltShowed != null)
                    {
                        BasaltShowed.Invoke();
                    }
                    break;
                case PieLayerSelected.Granit:
                    materialToFadeIn = granitMaterial;
                    if (GranitShowed != null)
                    {
                        GranitShowed.Invoke();
                    }
                    break;
                case PieLayerSelected.Sedimentary:
                    materialToFadeIn = sedimentaryMaterial;
                    if (SedimentaryShowed != null)
                    {
                        SedimentaryShowed.Invoke();
                    }
                    break;
            }

            if (materialToFadeIn != null)
            {
                LeanTween.value(gameObject, 0f, LayersTintCoeff, LayersFadeTime).setOnUpdate((float val) =>
                {
                    materialToFadeIn.SetFloat("_ColorMultiplier", 1f + val);
                }).setOnComplete(() =>
                {
                    showInfoPanels(newLayer, () =>
					{
						if (callback != null)
						{
							callback.Invoke();
						}
						
						if (ChangesEnded != null)
						{
							ChangesEnded.Invoke();
						}
					});
                });
            }
            else
            {
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

    private void showInfoPanels(PieLayerSelected layer, Action callback = null)
    {
        string textureName = "";
        InformationsMockup data = InformationsMockup.Crust;
        switch (layer)
        {
            case PieLayerSelected.Sedimentary:
                textureName = "Sandstone5";
                data = InformationsMockup.Sandstone;
                break;
            case PieLayerSelected.Granit:
                textureName = "Granit1";
                data = InformationsMockup.Granit;
                break;
            case PieLayerSelected.Basalt:
                textureName = "basalt2";
                data = InformationsMockup.Basalt;
                break;
            case PieLayerSelected.Magma:
                textureName = "lava2";
                data = InformationsMockup.Mantle;
                break;
        }
        Stone3DViewController.Instance.Show(textureName);

        SmallInfoViewController.Instance.Show(DataController.Instance.GetDescriptionFor(data), DataController.Instance.GetMaxScrollPosFor(data));

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    #endregion
}
