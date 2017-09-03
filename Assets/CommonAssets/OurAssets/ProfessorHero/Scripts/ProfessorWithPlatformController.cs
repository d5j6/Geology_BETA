using UnityEngine;
using System.Collections;

public class ProfessorWithPlatformController : MonoBehaviour {

    /*public GameObject ParticlesCircle;
    public PlaygroundParticlesC Particles;

    void Awake()
    {
        Particles.Emit(false);
    }

    public void StartEngine()
    {
        Particles.Emit(true);
    }

    public void StopEngine()
    {
        Particles.Emit(false);
    }*/

    public void SetScale(float newScale)
    {
        transform.localScale = Vector3.one * newScale;
        /*ParticlesCircle.transform.localScale = Vector3.one * 0.06f * newScale;
        Particles.sizeMin = 0.01f * newScale;
        Particles.sizeMax = 0.02f * newScale;*/
    }
}
