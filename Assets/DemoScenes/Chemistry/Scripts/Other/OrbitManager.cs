using UnityEngine;
using System.Collections.Generic;
using Vectrosity;

public class OrbitManager : MonoBehaviour
{
    private List<Orbit> _orbits = new List<Orbit>();
    public List<Orbit> orbits { get { return _orbits; } }

    private bool _isInitialized;

    //private Material _electronMaterial;

    public void Initialize(AtomInformation atomInformation, int[] holes = null, int holesOffset = 0, List<List<float>> overridedPositions = null)
    {
        if(_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        //_electronMaterial = new Material(Shader.Find("Shader Forge/UnlitColorCircleAlphaCutout"));
        //_electronMaterial.color = new Color(0f, 0.5f, 1f);

        for(int i = 0; i < atomInformation.electrons.Length; i++)
        {
            int holesCount = (holes == null) ? 0 : holes[i];
            List<float> positions = null;

            if(overridedPositions != null)
            {
                positions = overridedPositions[i];
            }

            CreateOrbit(i, atomInformation.electrons[i], holesCount, holesOffset, positions);
        }
    }

    public void CreateOrbit(int orbitNumber, int electronsCount, int holesCount, int holesOffset = 0, List<float> overridedPositions = null)
    {
        Orbit orbit = new GameObject(string.Format("OrbitN{0}", orbitNumber)).AddComponent<Orbit>();
        orbit.transform.SetParent(transform);
        orbit.transform.localPosition = Vector3.zero;
        orbit.transform.localRotation = Quaternion.identity;
        orbit.Initialize(orbitNumber, electronsCount, holesCount, holesOffset, overridedPositions);

        _orbits.Add(orbit);
    }

    public List<Transform> GetOrbitHoles(int orbitIndex)
    {
        return _orbits[orbitIndex].GetHoles();
    }

    public void StopRotating(float duration)
    {
        foreach(Orbit orbit in _orbits)
        {
            orbit.StopRotating(duration);
        }
    }

    public void SetRotatingToInitial(float duration)
    {
        foreach(Orbit orbit in _orbits)
        {
            orbit.SetRotatingToInitial(duration);
        }
    }

    public void RotateOrbitBy(int orbitIndex, Vector3 eulers)
    {
        _orbits[orbitIndex].RotateBy(eulers);
    }
}
