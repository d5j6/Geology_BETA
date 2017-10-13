using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramCollectionFrameworkCommands : Singleton<HologramCollectionFrameworkCommands> {

	public void Clear()
    {
        AddToSV_Root[] addToSVRootcomponents = FindObjectsOfType<AddToSV_Root>();

        foreach (AddToSV_Root addToSVRoot in addToSVRootcomponents)
        {
            Destroy(addToSVRoot.gameObject);
        }
    }
}
