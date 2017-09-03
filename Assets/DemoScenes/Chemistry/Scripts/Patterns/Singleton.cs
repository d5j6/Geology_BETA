using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                T single = FindObjectOfType<T>();
                if(single)
                {
                    instance = single;
                }
                else
                {
                    GameObject go = new GameObject(string.Format("[Singleton] {0}", typeof(T)));
                    T component = go.AddComponent<T>();
                    instance = component;
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.Log("The instance of singleton is already created.");
            return;
        }

        //instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance != null)
            instance = null;
    }
}
