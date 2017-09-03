using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DebugFeatures
{
    public class DebugMark : MonoBehaviour
    {
        Renderer rend;

        void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        public DebugMark Blue()
        {
            rend.material.color = Color.blue;
            return this;
        }

        public DebugMark Red()
        {
            rend.material.color = Color.red;
            return this;
        }

        public DebugMark Green()
        {
            rend.material.color = Color.green;
            return this;
        }

        public DebugMark SetColor(Color c)
        {
            rend.material.color = c;
            return this;
        }

        public DebugMark Max()
        {
            transform.localScale = Vector3.one * 0.2f;
            return this;
        }

        public DebugMark Min()
        {
            transform.localScale = Vector3.one * 0.05f;
            return this;
        }

        public DebugMark SetSize(float scale)
        {
            transform.localScale = Vector3.one * scale;
            return this;
        }
    }

    public static class DebugFeaturesExtensions
    {
        static List<GameObject> marks = new List<GameObject>();

        public static DebugMark Mark(this Vector3 worldPoint)
        {
            GameObject newMark = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Debug-Mark"));
            newMark.transform.position = worldPoint;
            DebugMark dm = newMark.AddComponent<DebugMark>();
            marks.Add(newMark);

            return dm;
        }

        public static void CleanAllMarks()
        {
            for (int i = 0; i < marks.Count; i++)
            {
                UnityEngine.Object.Destroy(marks[i]);
            }

            marks.Clear();
        }
    }
}
