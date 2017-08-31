using System;
using UnityEngine;

namespace HoloStudyFramework
{
    public class HoloStudyFrameworkSceneManager : MonoBehaviour, ISceneManager
    {
        public bool StartsAutomatically
        {
            get
            {
                return false;
            }
        }

        public void PrepareToUnload(Action callback)
        {
            callback.Invoke();
        }

        void Awake()
        {
            Loader.Instance.IThinkIWasLoadedCompletelyAndCanStart(this);
        }

        public void StartScene()
        {
            Loader.Instance.LoadScene("EducationDemoStartScene", SceneLoadingMode.Single, false);
        }
    }
}
