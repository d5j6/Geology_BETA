using UnityEngine;
using System.Collections.Generic;
using HoloToolkit.Unity;

public enum PieLayerSelected { Nothing, Sandstone, Granit, Basalt, Lava }
public enum PieSideActive { Left, Front, Right, Back, Default }

public class PieControllerSimple1 : Singleton<PieControllerSimple1> {

    public PieLayerSelected currentLayerSelected = PieLayerSelected.Nothing;
    private PieSideActive currentSide = PieSideActive.Default;

    private bool infoOpened = false;
    private bool showingLabels = false;

    public List<GameObject> LeftSideLabels = new List<GameObject>();
    public List<GameObject> RightSideLabels = new List<GameObject>();
    public List<GameObject> BackSideLabels = new List<GameObject>();
    public List<GameObject> FrontSideLabels = new List<GameObject>();

    /*public GameObject LeftSide;
    public GameObject RightSide;
    public GameObject FrontSide;
    public GameObject BackSide;*/

    /*private Material LeftSideMat;
    private Material RightSideMat;
    private Material FrontSideMat;
    private Material BackSideMat;*/

    public GameObject Layer1;
    public GameObject Layer2;
    public GameObject Layer3;
    public GameObject Layer4;

    private Material Layer1Mat;
    private Material Layer2Mat;
    private Material Layer3Mat;
    private Material Layer4Mat;

    //InfoPanelFader RockInfoController;
    //InfoPanelFader Rock3DController;

    public PieDividerController PieDivider;

    public void UpdateTint1(int level, float tintCoeff)
    {
        //Debug.Log("tintCoeff = " + tintCoeff);
        Layer1Mat.SetFloat("_ColorMultiplier", 1f + tintCoeff);
    }

    public void UpdateTint2(int level, float tintCoeff)
    {
        //Debug.Log("tintCoeff = " + tintCoeff);
        Layer2Mat.SetFloat("_ColorMultiplier", 1f + tintCoeff);
    }

    public void UpdateTint3(int level, float tintCoeff)
    {
        //Debug.Log("tintCoeff = " + tintCoeff);
        Layer3Mat.SetFloat("_ColorMultiplier", 1f + tintCoeff);
    }

    public void UpdateTint4(int level, float tintCoeff)
    {
        //Debug.Log("tintCoeff = " + tintCoeff);
        Layer4Mat.SetFloat("_ColorMultiplier", 1f + tintCoeff);
    }

    private void Awake()
    {
        Layer1Mat = Layer1.GetComponent<Renderer>().material;
        Layer2Mat = Layer2.GetComponent<Renderer>().material;
        Layer3Mat = Layer3.GetComponent<Renderer>().material;
        Layer4Mat = Layer4.GetComponent<Renderer>().material;

        /*LeftSideMat = LeftSide.GetComponent<Renderer>().material;
        RightSideMat = RightSide.GetComponent<Renderer>().material;
        FrontSideMat = FrontSide.GetComponent<Renderer>().material;
        BackSideMat = BackSide.GetComponent<Renderer>().material;*/
    }

    public void StartShowingLabels()
    {
        showingLabels = true;
        //infoOpened = InterfaceController.Instance.Video3DPanelIsOpened();
        infoOpened = Stone3DViewController.Instance.gameObject.GetComponent<InfoPanelFader>().Opened;
    }

    public void StopShowingLabels()
    {
        showingLabels = false;
    }

    public void ShowSandstoneInfo()
    {
        if (currentLayerSelected != PieLayerSelected.Sandstone)
        {
            PieDivider.Hide();
            currentLayerSelected = PieLayerSelected.Sandstone;
            if (!infoOpened)
            {
                infoOpened = true;
                showInfoPanels(InformationsMockup.Sandstone);
            }
            else
            {
                updateInfoPanels(InformationsMockup.Sandstone);
            }
        }
    }

    public void ShowGranitInfo()
    {
        if (currentLayerSelected != PieLayerSelected.Granit)
        {
            PieDivider.Hide();
            currentLayerSelected = PieLayerSelected.Granit;
            if (!infoOpened)
            {
                infoOpened = true;
                showInfoPanels(InformationsMockup.Granit);
            }
            else
            {
                updateInfoPanels(InformationsMockup.Granit);
            }
        }
    }

    public void ShowBasaltInfo()
    {
        if (currentLayerSelected != PieLayerSelected.Basalt)
        {
            PieDivider.Hide();
            currentLayerSelected = PieLayerSelected.Basalt;
            if (!infoOpened)
            {
                infoOpened = true;
                showInfoPanels(InformationsMockup.Basalt);
            }
            else
            {
                updateInfoPanels(InformationsMockup.Basalt);
            }
        }
    }

    public void ShowMantleInfo()
    {
        if (currentLayerSelected != PieLayerSelected.Lava)
        {
            PieDivider.Hide();
            currentLayerSelected = PieLayerSelected.Lava;
            if (!infoOpened)
            {
                infoOpened = true;
                showInfoPanels(InformationsMockup.UpperMantleLess);
            }
            else
            {
                updateInfoPanels(InformationsMockup.UpperMantleLess);
            }
        }
    }

    private void updateInfoPanels(InformationsMockup query)
    {
        //Texture tex = null;
        switch (query)
        {
            case InformationsMockup.Sandstone:
                Stone3DViewController.Instance.Show("Sandstone5");
                //tex = Resources.Load("Sandstone5") as Texture;
                break;
            case InformationsMockup.Granit:
                Stone3DViewController.Instance.Show("Granit1");
                break;
            case InformationsMockup.Basalt:
                Stone3DViewController.Instance.Show("basalt2");
                break;
            case InformationsMockup.UpperMantleLess:
            case InformationsMockup.Mantle:
                Stone3DViewController.Instance.Show("lava2");
                break;

        }

        //Debug.Log("Updated.");

        //SmallInfoViewController.Instance.Show(DataController.Instance.GetDescriptionFor(query));
        SmallInfoViewController.Instance.Show(query);
        //InterfaceController.Instance.ShowSmallInfoPanel(DataController.Instance.GetDescriptionFor(query));
    }

    private void showInfoPanels(InformationsMockup query)
    {
        switch (query)
        {
            case InformationsMockup.Sandstone:
                Stone3DViewController.Instance.Show("Sandstone5");
                break;
            case InformationsMockup.Granit:
                Stone3DViewController.Instance.Show("Granit1");
                break;
            case InformationsMockup.Basalt:
                Stone3DViewController.Instance.Show("basalt2");
                break;
            case InformationsMockup.UpperMantleLess:
            case InformationsMockup.Mantle:
                Stone3DViewController.Instance.Show("lava2");
                break;
            default:
                Stone3DViewController.Instance.Show("lava2");
                break;

        }
        //rend.material.mainTexture = Resources.Load("glass") as Texture;

        LeanTween.delayedCall(1.7f, () =>
        {
            //SmallInfoViewController.Instance.Show(DataController.Instance.GetDescriptionFor(query));
            SmallInfoViewController.Instance.Show(query);
        });

        /*LeanTween.scale(InterfaceController.Instance.Video3DPanel.gameObject, Vector3.one * 0.26f, 1f).setDelay(3.0f).setOnStart(() =>
        {
            InterfaceController.Instance.ShowSmallInfoPanel(DataController.Instance.GetDescriptionFor(query));
        });*/
    }

    public void HideInfoPanels()
    {
        if (currentLayerSelected != PieLayerSelected.Nothing)
        {
            currentLayerSelected = PieLayerSelected.Nothing;
        }
        //InterfaceController.Instance.HideSmallInfoPanel();
        SmallInfoViewController.Instance.Hide();
        LeanTween.delayedCall(1.3f, () =>
        {
            Stone3DViewController.Instance.Hide();
        }).setOnComplete(() =>
        {
            infoOpened = false;
        });

        /*LeanTween.scale(InterfaceController.Instance.Video3DPanel.gameObject, Vector3.zero, 0.5f).setDelay(2.0f).setOnStart(() =>
        {
            InterfaceController.Instance.Hide3DVideoPanel();
        }).setOnComplete(() =>
        {
            infoOpened = false;
        });*/
    }

    private void Update () {
        if (showingLabels)
        {
            float diff = Mathf.Atan2(transform.position.z - Camera.main.transform.position.z, transform.position.x - Camera.main.transform.position.x);
            //Vector3 diff = transform.position - Camera.main.transform.position;
            if (diff < 0)
            {
                diff = Mathf.PI*2f + diff;
            }

            float val = Mathf.Sqrt(2f) / 2f;

            if (Mathf.Cos(diff) >= val)
            {
                goToSideState(PieSideActive.Back);
            }
            else if (Mathf.Cos(diff) <= val*-1.0f)
            {
                goToSideState(PieSideActive.Front);
            }
            else if (Mathf.Sin(diff) <= val)
            {
                goToSideState(PieSideActive.Right);
            }
            else
            {
                goToSideState(PieSideActive.Left);
            }
        }
	}

    private void goToSideState(PieSideActive newSide)
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
                for (i = 0; i < LeftSideLabels.Count; i++)
                {
                    LeftSideLabels[i].GetComponent<HitechLabelController>().Show();
                }
                break;
            case PieSideActive.Front:
                for (i = 0; i < FrontSideLabels.Count; i++)
                {
                    FrontSideLabels[i].GetComponent<HitechLabelController>().Show();
                }
                break;
            case PieSideActive.Right:
                for (i = 0; i < RightSideLabels.Count; i++)
                {
                    RightSideLabels[i].GetComponent<HitechLabelController>().Show();
                }
                break;
            case PieSideActive.Back:
                for (i = 0; i < BackSideLabels.Count; i++)
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
                for (i = 0; i < LeftSideLabels.Count; i++)
                {
                    LeftSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
            case PieSideActive.Front:
                for (i = 0; i < FrontSideLabels.Count; i++)
                {
                    FrontSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
            case PieSideActive.Right:
                for (i = 0; i < RightSideLabels.Count; i++)
                {
                    RightSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
            case PieSideActive.Back:
                for (i = 0; i < BackSideLabels.Count; i++)
                {
                    BackSideLabels[i].GetComponent<HitechLabelController>().Hide();
                }
                break;
        }
    }
}