using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersActivationScriptForChemistry : MonoBehaviour {

    public GameObject InteractionManagers;

    public void ActivateInteractionManagersFC()
    {
        InteractionManagers.SetActive(true);
    }

    public void DeactivateInteractionManagersFC()
    {
        InteractionManagers.SetActive(false);
    }
}
