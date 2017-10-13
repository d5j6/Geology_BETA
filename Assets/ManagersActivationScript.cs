using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersActivationScript : Singleton<ManagersActivationScript> {

    public GameObject InteractionManagers;

    private void Start()
    {
        if (InteractionManagers == null)
        {
            Debug.Log("Prefabs in " + gameObject.name + " weren't assigned");
        }
    }

    public void ActivateInteractionManagers()
    {
        InteractionManagers.SetActive(true);
    }

    public void DeactivateInteractionManagers()
    {
        InteractionManagers.SetActive(false);
    }

}
