using UnityEngine;
using System;
using HoloToolkit.Unity;

public class StartPositioning : Singleton<StartPositioning>
{
    public bool positioned = false;
    public TransparentFader[] HoloFaders;
    public PositioningMenu PositioningMenu;

    //public Transform EarthHolo;
    public Transform InfoHolo;
    public Transform MenuHolo;

    public Vector3 InfoPosAdding;
    public Vector3 MenuPosAdding;
    public Vector3 InfoPosScaling;
    public Vector3 MenuPosScaling;

    public void ShowPositioningHolos(Action callback = null)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        transform.position = BoundaryVolume.Instance.transform.position;
        transform.rotation = BoundaryVolume.Instance.transform.rotation;
        transform.localScale = BoundaryVolume.Instance.transform.localScale;

        /*if ((BoundaryVolume.Instance.MenuVolume.transform.localPosition != Vector3.zero) && (BoundaryVolume.Instance.InfoVolume.transform.localPosition != Vector3.zero))
        {
            MenuHolo.transform.position = BoundaryVolume.Instance.MenuVolume.transform.position;
            MenuHolo.transform.rotation = BoundaryVolume.Instance.MenuVolume.transform.rotation;
            MenuHolo.transform.localScale = BoundaryVolume.Instance.MenuVolume.transform.localScale;
            InfoHolo.transform.position = BoundaryVolume.Instance.InfoVolume.transform.position;
            InfoHolo.transform.rotation = BoundaryVolume.Instance.InfoVolume.transform.rotation;
            InfoHolo.transform.localScale = BoundaryVolume.Instance.InfoVolume.transform.localScale;
        }*/

        for (int i = 1; i < HoloFaders.Length; i++)
        {
            HoloFaders[i].Show();
        }
        if (HoloFaders.Length > 0)
        {
            HoloFaders[0].Show(() =>
            {
                PositioningMenu.ActivateAllButtons(callback);
            });
        }
        else
        {
            Debug.Log("Nothing to Show!");
        }

        AudioSourceController.Instance.StartPlaying(AudioSourceController.Instance.transform.FindChild("Baba_AdjustSizes").gameObject);
    }

    public void HidePositioningHolos(Action callback = null)
    {
        AudioSourceController.Instance.StopPlaying();

        positioned = true;

        BoundaryVolume.Instance.transform.position = transform.position;
        BoundaryVolume.Instance.transform.rotation = transform.rotation;
        BoundaryVolume.Instance.transform.localScale = transform.localScale;

        //InfoPosAdding = transform.position - MenuHolo.transform.position;

        /*BoundaryVolume.Instance.MenuVolume.transform.position = MenuHolo.transform.position;
        BoundaryVolume.Instance.MenuVolume.transform.rotation = MenuHolo.transform.rotation;
        BoundaryVolume.Instance.MenuVolume.transform.localScale = MenuHolo.transform.localScale;
        BoundaryVolume.Instance.InfoVolume.transform.position = InfoHolo.transform.position;
        BoundaryVolume.Instance.InfoVolume.transform.rotation = InfoHolo.transform.rotation;
        BoundaryVolume.Instance.InfoVolume.transform.localScale = InfoHolo.transform.localScale;*/

        PositioningMenu.DeactivateAllButtons(() =>
        {
            for (int i = 1; i < HoloFaders.Length; i++)
            {
                HoloFaders[i].Hide();
            }
            if (HoloFaders.Length > 0)
            {
                HoloFaders[0].Hide(() =>
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    if (callback != null)
                    {
                        callback.Invoke();
                    }
                });
            }
        });
    }
}
