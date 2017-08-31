using UnityEngine;
using System.Collections.Generic;
using Vectrosity;
using DG.Tweening;

public class Orbit : MonoBehaviour
{
    private int _orbitNumber;
    private float _orbitRadius;
    private Vector3 _initialDirection;
    public Vector3 initialDirection { get { return _initialDirection; } }

    private VectorLine _orbitLine;
    public VectorLine orbitLine { get { return _orbitLine; } }

    private Transform _electronOrbit;
    public Transform electronOrbit { get { return _electronOrbit; } }

    private float _orbitSpeed;

    private List<Transform> _electrons = new List<Transform>();
    private List<Transform> _holes = new List<Transform>();

    private bool _isInitialize;

    public void Initialize(int orbitNumber, int electronsCount, int holesCount, int holesOffset = 0, List<float> overriderPositions = null)
    {
        _orbitNumber = orbitNumber;

        // Orbit radius and speed determined from orbit number and radius respectively
        _orbitRadius = orbitNumber + 1;
        _orbitSpeed = 180f / _orbitRadius / 3;

        // If orbits rotate in one direction - need to determine positive look-vector
        Vector3 direction = Random.onUnitSphere;
        direction.x = direction.x < 0 ? direction.x * -1f : direction.x;
        direction.y = direction.y < 0 ? direction.y * -1f : direction.y;
        direction.z = direction.z < 0 ? direction.z * -1f : direction.z;

        _initialDirection = direction;
        transform.localRotation = Quaternion.LookRotation(_initialDirection);

        CreateOrbitLine();

        _electronOrbit = new GameObject("ElectronOrbit").transform;
        _electronOrbit.SetParent(transform);
        _electronOrbit.localPosition = Vector3.zero;
        _electronOrbit.localRotation = Quaternion.identity;

        int summaryPoints = electronsCount + holesCount;
        int holePeriod = (holesCount == 0) ? 0 : summaryPoints / holesCount;

        float deltaOffset = 1f / summaryPoints;
        float currentOffset = 0f;

        int holeNumber = 0;

        for(int i = 0; i < summaryPoints; i++)
        {
            if(holesCount > 0 && i % holePeriod == 0 + holesOffset)
            {
                // hole creation
                Transform hole = Instantiate(PrefabManager.Instance.holePrefab).transform;
                hole.name = string.Format("HoleN{0}", holeNumber++);
                hole.SetParent(_electronOrbit);
                hole.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                float offset = currentOffset;
                if(overriderPositions != null)
                {
                    offset = overriderPositions[i];
                }

                Vector3 newPosition = _orbitLine.GetPoint3D01(offset);
                newPosition = newPosition - _electronOrbit.position;
                hole.localPosition = newPosition;

                currentOffset += deltaOffset;

                _holes.Add(hole);
            }
            else
            {
                //electron creation
                Transform electron = Instantiate(PrefabManager.Instance.electronPrefab).transform;
                electron.name = string.Format("ElectronN{0}", i);
                electron.SetParent(_electronOrbit);
                electron.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                float offset = currentOffset;
                if(overriderPositions != null)
                {
                    offset = overriderPositions[i];
                }

                Vector3 newPosition = _orbitLine.GetPoint3D01(offset);
                newPosition = newPosition - _electronOrbit.position;
                electron.localPosition = newPosition;

                currentOffset += deltaOffset;

                _electrons.Add(electron);
            }
        }
    }

    private void CreateOrbitLine()
    {
        _orbitLine = new VectorLine(string.Format("OrbitLineN{0}", _orbitNumber), new List<Vector3>(64), 1f);
        _orbitLine.drawTransform = transform;
        _orbitLine.material = new Material(Shader.Find("Shader Forge/UnlitColor"));
        _orbitLine.material.color = Color.gray;
        _orbitLine.MakeCircle(Vector3.zero, _orbitRadius, 32);
        _orbitLine.Draw3DAuto();
    }

    void Update()
    {
        _electronOrbit.transform.Rotate(new Vector3(0f, 0f, _orbitSpeed) * Time.deltaTime);
    }

    void OnDestroy()
    {
        VectorLine.Destroy(ref _orbitLine);
    }

    public List<Transform> GetElectrons()
    {
        List<Transform> electrons = new List<Transform>();

        foreach(Transform electron in _electrons)
        {
            electrons.Add(electron);
        }

        return electrons;
    }

    public List<Transform> GetHoles()
    {
        List<Transform> holes = new List<Transform>();

        foreach(Transform hole in _holes)
        {
            holes.Add(hole);
        }

        return holes;
    }

    public void StopRotating(float duration)
    {
        if(Mathf.Approximately(duration, 0f))
        {
            _orbitSpeed = 0f;
        }

        DOTween.To(() => { return _orbitSpeed; }, (x) => { _orbitSpeed = x; }, 0f, duration).Play();
    }

    public void SetRotatingToInitial(float duration)
    {
        DOTween.To(() => { return _orbitSpeed; }, (x) => { _orbitSpeed = x; }, 180f / _orbitRadius, duration).Play();
    }

    public void RotateBy(Vector3 eulers)
    {
        _electronOrbit.transform.Rotate(eulers, Space.Self);
    }
}
