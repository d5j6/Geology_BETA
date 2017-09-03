using UnityEngine;
using System.Collections.Generic;

public class CategoryScript : MonoBehaviour {

    bool opened = false;

    public Fader MainButtonFader;
    public List<Fader> childrenFaders = new List<Fader>();
    SubmenuOpener[] submenuOpeners;

    void Awake()
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

            MainButtonFader.Show();
            for (int i = 0; i < childrenFaders.Count; i++)
            {
                childrenFaders[i].HideImmediately();
            }
        }
    }

    public void CloseCategory()
    {
        if (opened)
        {
            for (int i = 0; i < submenuOpeners.Length; i++)
            {
                submenuOpeners[i].Hide();
            }
            opened = false;

            MainButtonFader.Hide();
            for (int i = 0; i < childrenFaders.Count; i++)
            {
                childrenFaders[i].Hide();
            }
        }
    }
}
