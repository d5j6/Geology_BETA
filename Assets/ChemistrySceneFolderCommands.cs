using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistrySceneFolderCommands : Singleton<ChemistrySceneFolderCommands> {

	public void DeleteChemistryObjects()
    {
        Destroy(this.gameObject);
    }
}
