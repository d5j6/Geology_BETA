using UnityEngine;
using SpectatorView;
using System.Collections.Generic;

/// <summary>
/// Component that allows add new gameobjects to root at start or at runtime, 
/// and store all models in hologram collection in list
/// </summary>
public class SV_Root : SV_Singleton<SV_Root>
{
    #region Public Fields

    [Tooltip("Add Objects here to add them dynamicly on scene load")]
    public List<GameObject> ObjectsInRoot; // after adding new objects to root, this array represent All objects in root

    #endregion

    #region Public Hidden Fields

    [HideInInspector]
    public Transform Anchor; // public link to this object transform

    #endregion

    #region Main Methods

    private void OnEnable()
    {
        // set current transfrom to Anchor field
        Anchor = transform;
    }

    private void Start()
    {
        // if we have any objects in list add them in root
        if (ObjectsInRoot.Count > 0)
        {
            foreach (var obj in ObjectsInRoot)
            {
                // add objects in root
                AddToRoot(obj);
            }
        }
        
        // after objects added in root, clear list
        ObjectsInRoot.Clear();

        // loop thru all childs of root and add them to list
        foreach (Transform child in Anchor)
        {
            ObjectsInRoot.Add(child.gameObject);
        }
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Add game object to root
    /// </summary>
    /// <param name="obj"></param>
    public void AddToRoot(GameObject obj)
    {
        GameObject model = null;

        if (!obj.activeSelf)
        {
            model = Instantiate(obj); // instaniate model in scene and savel link to it
            model.transform.SetParent(Anchor); // make model child of hologram collection
        }
        else
        {
            model = obj;
            model.transform.SetParent(Anchor); // make model child of hologram collection
        }

        // if list of objects not contains this model, add it
        if (!ObjectsInRoot.Contains(model))
        {
            ObjectsInRoot.Add(model);
        }
    }

    /// <summary>
    /// Add game object to root with position and rotation
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void AddToRoot(GameObject obj, Vector3 position, Quaternion rotation)
    {
        GameObject model = null;

        if (!obj.activeSelf)
        {
            model = Instantiate(obj, position, rotation); // instaniate model in scene and savel link to it
            model.transform.SetParent(Anchor); // make model child of hologram collection
        }
        else
        {
            model = obj;
            model.transform.SetParent(Anchor); // make model child of hologram collection
        }

        // if list of objects not contains this model, add it
        if (!ObjectsInRoot.Contains(model))
        {
            ObjectsInRoot.Add(model);
        }
    }

    #endregion
}
