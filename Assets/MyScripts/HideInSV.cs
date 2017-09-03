using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInSV : MonoBehaviour {
    
	void Start ()
    {
#if UNITY_EDITOR
        if (SV_Sharing.Instance)
        {
            gameObject.SetActive(false);
        }
#endif
    }
}
