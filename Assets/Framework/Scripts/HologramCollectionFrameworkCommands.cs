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

        /*
        Destroy(FindObjectOfType<ProfessorOnPlatform>().gameObject);
        Destroy(FindObjectOfType<HoloStudyDemoGeoMenuController>().gameObject);
        Destroy(FindObjectOfType<SlicedEarthPolygon>().gameObject);
        Destroy(FindObjectOfType<PiePolygon>().gameObject);
        Destroy(FindObjectOfType<BigSimpleInfoPanelController>().gameObject);
        Destroy(FindObjectOfType<PieController>().gameObject);
        Destroy(FindObjectOfType<SceneStateMachine>().gameObject);
        Destroy(FindObjectOfType<EarthController>().gameObject);
        */
    }
}
