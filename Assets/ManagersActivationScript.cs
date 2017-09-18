using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersActivationScript : Singleton<ManagersActivationScript> {

    public GameObject InteractionManagers;

    public void ActivateInteractionManagers()
    {
        InteractionManagers.SetActive(true);
    }

    public void DeactivateInteractionManagers()
    {
        InteractionManagers.SetActive(false);
    }

}
