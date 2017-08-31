using UnityEngine;
using System.Collections.Generic;

public class CategoryRingScript : MonoBehaviour {

    bool opened = false;

    public RingsSelectingController MainButtonRings;
    public List<RingsSelectingController> childrenRings = new List<RingsSelectingController>();
    SubmenuOpener[] submenuOpeners;

    /*void Awake()
    {
        submenuOpeners = GetComponents<SubmenuOpener>();
    }*/
    void Start()
    {
        submenuOpeners = GetComponents<SubmenuOpener>();
    }

    public void OpenCategory()
    {
        if (!opened)
        {
            for (int i = 0; i < submenuOpeners.Length; i++)
            {
                submenuOpeners[i].Show();
            }
            opened = true;

            MainButtonRings.Show();
            for (int i = 0; i < childrenRings.Count; i++)
            {
                childrenRings[i].HideImmediately();
            }
        }
    }

    public void CloseCategory(System.Action callback = null)
    {
        if (opened)
        {
            for (int i = 0; i < submenuOpeners.Length; i++)
            {
                submenuOpeners[i].Hide();
            }
            opened = false;

            MainButtonRings.Hide(callback);
            for (int i = 0; i < childrenRings.Count; i++)
            {
                childrenRings[i].Hide();
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

    public void CloseCategoryVoid()
    {
        if (opened)
        {
            for (int i = 0; i < submenuOpeners.Length; i++)
            {
                submenuOpeners[i].Hide();
            }
            opened = false;

            MainButtonRings.Hide();
            for (int i = 0; i < childrenRings.Count; i++)
            {
                childrenRings[i].Hide();
            }
        }
    }
}
